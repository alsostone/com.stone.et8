using System;
using System.Diagnostics;
using System.Collections.Generic;
using TrueSync;

namespace ST.Collision
{
    public class DynamicTree<T>
    {
        private TreeNode[] _treeNodes;
        private int _nodeCount;
        private int _nodeCapacity;

        private int _root;
        private int _freeList;

        private readonly Stack<int> _queryStack = new Stack<int>(256);

        public DynamicTree() : this(Settings.DEFAULT_CAPACITY) { }

        public DynamicTree(int capacity)
        {
            _root = Settings.NULL_NODE;
            _nodeCapacity = capacity;
            _nodeCount = 0;

            _treeNodes = new TreeNode[_nodeCapacity];

            // Build a linked list for the free list.
            for (int i = 0; i < _nodeCapacity - 1; i++)
            {
                _treeNodes[i].Next = i + 1;
                _treeNodes[i].Height = -1;
            }

            _treeNodes[_nodeCapacity - 1].Next = Settings.NULL_NODE;
            _treeNodes[_nodeCapacity - 1].Height = -1;
            _freeList = 0;
        }
        
        /// <summary>
        /// Allocate a node from the pool. Grow the pool if necessary.
        /// </summary>
        private int AllocateNode()
        {
            // Expand the node pool as needed.
            if (_freeList == Settings.NULL_NODE)
            {
                Debug.Assert(_nodeCount == _nodeCapacity);

                // The free list is empty. Rebuild a bigger pool.
                TreeNode[] oldNodes = _treeNodes;
                _nodeCapacity *= 2;

                _treeNodes = new TreeNode[_nodeCapacity];

                for (int i = 0; i < oldNodes.Length; i++)
                    _treeNodes[i] = oldNodes[i];

                // Build a linked list for the free list.
                for (int i = _nodeCount; i < _nodeCapacity - 1; i++)
                {
                    _treeNodes[i].Next = i + 1;
                    _treeNodes[i].Height = -1;
                }

                _treeNodes[_nodeCapacity - 1].Next = Settings.NULL_NODE;
                _treeNodes[_nodeCapacity - 1].Height = -1;
                _freeList = _nodeCount;
            }

            // Peel a node off the free list.
            int nodeId = _freeList;
            _freeList = _treeNodes[nodeId].Next;
            ref var newNode = ref _treeNodes[nodeId];
            newNode.Parent = Settings.NULL_NODE;
            newNode.Child1 = Settings.NULL_NODE;
            newNode.Child2 = Settings.NULL_NODE;
            newNode.Height = 0;
            newNode.Next = 0;
            newNode.UserData = default;

            ++_nodeCount;

            return nodeId;
        }

        /// <summary>
        /// Return a node to the pool.
        /// </summary>
        private void FreeNode(int nodeId)
        {
            Debug.Assert(0 <= nodeId && nodeId < _nodeCapacity);
            Debug.Assert(0 < _nodeCount);

            ref var freeNode = ref _treeNodes[nodeId];
            freeNode.Next = _freeList;
            freeNode.Height = -1;
            _freeList = nodeId;
            --_nodeCount;
        }

        /// Create a proxy. Provide a tight fitting AABB and a userData pointer.
        public int CreateProxy(in AABB aabb, T userData)
        {
            int proxyId = AllocateNode();
            ref var proxyNode = ref _treeNodes[proxyId];
            
            // Fatten the aabb.
            var r = new TSVector(Settings.AABB_EXTENSION, Settings.AABB_EXTENSION, Settings.AABB_EXTENSION);
            proxyNode.AABB.LowerBound = aabb.LowerBound - r;
            proxyNode.AABB.UpperBound = aabb.UpperBound + r;
            proxyNode.UserData = userData;
            proxyNode.Height = 0;

            InsertLeaf(proxyId);

            return proxyId;
        }
        
        /// <summary>
        /// Destroy a proxy. This asserts if the id is invalid.
        /// </summary>
        public void DestroyProxy(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);
            Debug.Assert(_treeNodes[proxyId].IsLeaf);

            RemoveLeaf(proxyId);
            FreeNode(proxyId);
        }

        /// <summary>
        /// Move a proxy with a swept bounds. If the proxy has moved outside of its padded bounds,
        /// then the proxy is removed from the tree and re-inserted. Otherwise
        /// the function returns immediately.
        /// </summary>
        /// <returns>true if the object was re-inserted.</returns>
        public bool MoveProxy(int proxyId, in AABB aabb, in TSVector displacement)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);
            Debug.Assert(_treeNodes[proxyId].IsLeaf);

            // Extend AABB
            var r = new TSVector(Settings.AABB_EXTENSION, Settings.AABB_EXTENSION, Settings.AABB_EXTENSION);
            var fatAABB = new AABB
            {
                LowerBound = aabb.LowerBound - r,
                UpperBound = aabb.UpperBound + r
            };
            
            // Predict AABB movement
            TSVector d = Settings.AABB_MULTIPLIER * displacement;

            if (d.x < FP.Zero)
                fatAABB.LowerBound.x += d.x;
            else
                fatAABB.UpperBound.x += d.x;

            if (d.y < FP.Zero)
                fatAABB.LowerBound.y += d.y;
            else
                fatAABB.UpperBound.y += d.y;

            if (d.z < FP.Zero)
                fatAABB.LowerBound.z += d.z;
            else
                fatAABB.UpperBound.z += d.z;

            ref var proxyNode = ref _treeNodes[proxyId];
            ref var treeAABB = ref proxyNode.AABB;
            if (treeAABB.Contains(aabb))
            {
                // The tree AABB still contains the object, but it might be too large.
                // Perhaps the object was moving fast but has since gone to sleep.
                // The huge AABB is larger than the new fat AABB.
                var hugeAABB = new AABB
                {
                    LowerBound = fatAABB.LowerBound - 4 * r,
                    UpperBound = fatAABB.UpperBound + 4 * r
                };

                if (hugeAABB.Contains(treeAABB))
                {
                    // The tree AABB contains the object AABB and the tree AABB is
                    // not too large. No tree update needed.
                    return false;
                }

                // Otherwise the tree AABB is huge and needs to be shrunk
            }

            RemoveLeaf(proxyId);

            proxyNode.AABB = fatAABB;

            InsertLeaf(proxyId);

            return true;
        }

        /// Get proxy user data.
        /// @return the proxy user data or 0 if the id is invalid.
        public T GetUserData(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);
            return _treeNodes[proxyId].UserData;
        }
        
        /// Get the fat AABB for a proxy.
        public AABB GetFatAABB(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < _nodeCapacity);
            return _treeNodes[proxyId].AABB;
        }
        
        /// <summary>
        /// Enumerate over all leaf nodes which are fully or partially within the given bounds.
        /// </summary>
        public IEnumerable<TreeNode> Query(AABB aabb)
        {
            _queryStack.Clear();
            _queryStack.Push(_root);

            while (_queryStack.Count > 0)
            {
                int nodeId = _queryStack.Pop();

                if (nodeId == Settings.NULL_NODE)
                    continue;

                if (_treeNodes[nodeId].AABB.Intersects(aabb))
                {
                    if (_treeNodes[nodeId].IsLeaf)
                    {
                        yield return _treeNodes[nodeId];
                    }
                    else
                    {
                        _queryStack.Push(_treeNodes[nodeId].Child1);
                        _queryStack.Push(_treeNodes[nodeId].Child2);
                    }
                }
            }
        }
        
        
        /// <summary>
        /// Enumerate all nodes, both leaf and internal, and return both the nodes themselves and their depth in the tree.
        /// </summary>
        public IEnumerable<(TreeNode, int)> EnumerateNodes()
        {
            foreach ((TreeNode, int) node in EnumerateNodes(_root, 0))
            {
                yield return node;
            }
        }

        private IEnumerable<(TreeNode, int)> EnumerateNodes(int startPoint, int currentDepth)
        {
            Debug.Assert(0 <= startPoint && startPoint < _nodeCapacity);
            Debug.Assert(startPoint != Settings.NULL_NODE);

            TreeNode treeNode = _treeNodes[startPoint];

            yield return (treeNode, currentDepth);

            if (!treeNode.IsLeaf)
            {
                int child1 = treeNode.Child1;

                if (child1 != Settings.NULL_NODE)
                {
                    foreach ((TreeNode, int) child1Node in EnumerateNodes(child1, currentDepth + 1))
                    {
                        yield return child1Node;
                    }
                }

                int child2 = treeNode.Child2;

                if (child2 != Settings.NULL_NODE)
                {
                    foreach ((TreeNode, int) child2Node in EnumerateNodes(child2, currentDepth + 1))
                    {
                        yield return child2Node;
                    }
                }
            }
        }
        
        /// <summary>
        /// Validate this tree. For testing.
        /// </summary>
        public void Validate()
        {
            ValidateStructure(_root);
            ValidateMetrics(_root);

            int freeCount = 0;
            int freeIndex = _freeList;
            while (freeIndex != Settings.NULL_NODE)
            {
                Debug.Assert(0 <= freeIndex && freeIndex < _nodeCapacity);
                freeIndex = _treeNodes[freeIndex].Next;
                ++freeCount;
            }

            Debug.Assert(GetHeight() == ComputeHeight());

            Debug.Assert(_nodeCount + freeCount == _nodeCapacity);
        }

        /// <summary>
        /// Compute the height of the binary tree in O(N) time. Should not be
        /// called often.
        /// </summary>
        public int GetHeight() => _root == Settings.NULL_NODE ? 0 : _treeNodes[_root].Height;

        /// <summary>
        /// Get the maximum balance of an node in the tree. The balance is the difference
        /// in height of the two children of a node.
        /// </summary>
        public int GetMaxBalance()
        {
            int maxBalance = 0;
            for (int i = 0; i < _nodeCapacity; i++)
            {
                ref readonly var node = ref _treeNodes[i];
                if (node.Height <= 1)
                    continue;

                Debug.Assert(!node.IsLeaf);

                int child1 = node.Child1;
                int child2 = node.Child2;
                int balance = Math.Abs(_treeNodes[child2].Height - _treeNodes[child1].Height);
                maxBalance = Math.Max(maxBalance, balance);
            }

            return maxBalance;
        }

        /// <summary>
        /// Get the ratio of the sum of the node areas to the root area.
        /// </summary>
        public FP GetSurfaceAreaRatio()
        {
            if (_root == Settings.NULL_NODE)
                return FP.Zero;

            FP rootArea = _treeNodes[_root].AABB.GetSurfaceArea();

            FP totalArea = FP.Zero;
            for (int i = 0; i < _nodeCapacity; i++)
            {
                ref readonly var node = ref _treeNodes[i];
                if (node.Height < 0)
                    continue;

                totalArea += node.AABB.GetSurfaceArea();
            }

            return totalArea / rootArea;
        }

        /// <summary>
        /// Build an optimal tree. Very expensive. For testing.
        /// </summary>
        public void RebuildBottomUp()
        {
            int[] nodes = new int[_nodeCount];
            int count = 0;

            // Build array of leaves. Free the rest.
            for (int i = 0; i < _nodeCapacity; i++)
            {
                ref var nodeI = ref _treeNodes[i];
                
                // free node in pool, ignore
                if (nodeI.Height < 0)
                    continue;

                if (nodeI.IsLeaf)
                {
                    nodeI.Parent = Settings.NULL_NODE;
                    nodes[count] = i;
                    ++count;
                }
                else
                {
                    FreeNode(i);
                }
            }

            while (count > 1)
            {
                FP minCost = FP.MaxValue;
                int iMin = -1, jMin = -1;
                for (int i = 0; i < count; ++i)
                {
                    AABB aabbi = _treeNodes[nodes[i]].AABB;

                    for (int j = i + 1; j < count; ++j)
                    {
                        AABB aabbj = _treeNodes[nodes[j]].AABB;
                        AABB.Combine(aabbi, aabbj, out var b);
                        
                        FP cost = b.GetSurfaceArea();
                        if (cost < minCost)
                        {
                            iMin = i;
                            jMin = j;
                            minCost = cost;
                        }
                    }
                }

                int index1 = nodes[iMin];
                int index2 = nodes[jMin];
                ref var child1 = ref _treeNodes[index1];
                ref var child2 = ref _treeNodes[index2];

                int parentIndex = AllocateNode();
                ref var parent = ref _treeNodes[parentIndex];
                parent.Child1 = index1;
                parent.Child2 = index2;
                parent.Height = 1 + Math.Max(child1.Height, child2.Height);
                
                parent.AABB.Combine(child1.AABB, child2.AABB);
                parent.Parent = Settings.NULL_NODE;

                child1.Parent = parentIndex;
                child2.Parent = parentIndex;

                nodes[jMin] = nodes[count - 1];
                nodes[iMin] = parentIndex;
                --count;
            }

            _root = nodes[0];

            Validate();
        }

        /// <summary>
        /// Shift the world origin. Useful for large worlds.
        /// The shift formula is: position -= newOrigin
        /// </summary>
        /// <param name="newOrigin">The new origin with respect to the old origin</param>
        public void ShiftOrigin(TSVector newOrigin)
        {
            for (int i = 0; i < _nodeCapacity; i++)
            {
                _treeNodes[i].AABB.LowerBound -= newOrigin;
                _treeNodes[i].AABB.UpperBound -= newOrigin;
            }
        }

        private void InsertLeaf(int leaf)
        {
            if (_root == Settings.NULL_NODE)
            {
                _root = leaf;
                _treeNodes[_root].Parent = Settings.NULL_NODE;
                return;
            }

            // Find the best sibling for this node
            AABB leafAABB = _treeNodes[leaf].AABB;
            int index = _root;
            while (!_treeNodes[index].IsLeaf)
            {
                ref var indexNode = ref _treeNodes[index];
                int child1 = indexNode.Child1;
                int child2 = indexNode.Child2;

                FP area = indexNode.AABB.GetSurfaceArea();
                
                AABB.Combine(indexNode.AABB, leafAABB, out var combinedAABB);
                FP combinedArea = combinedAABB.GetSurfaceArea();

                // Cost of creating a new parent for this node and the new leaf
                FP cost = FP.Two * combinedArea;

                // Minimum cost of pushing the leaf further down the tree
                FP inheritanceCost = FP.Two * (combinedArea - area);

                // Cost of descending into child1
                FP cost1;
                if (_treeNodes[child1].IsLeaf)
                {
                    AABB.Combine(leafAABB, _treeNodes[child1].AABB, out var aabb);
                    cost1 = aabb.GetSurfaceArea() + inheritanceCost;
                }
                else
                {
                    AABB.Combine(leafAABB, _treeNodes[child1].AABB, out var aabb);
                    FP oldArea = _treeNodes[child1].AABB.GetSurfaceArea();
                    FP newArea = aabb.GetSurfaceArea();
                    cost1 = (newArea - oldArea) + inheritanceCost;
                }

                // Cost of descending into child2
                FP cost2;
                if (_treeNodes[child2].IsLeaf)
                {
                    AABB.Combine(leafAABB, _treeNodes[child2].AABB, out var aabb);
                    cost2 = aabb.GetSurfaceArea() + inheritanceCost;
                }
                else
                {
                    AABB.Combine(leafAABB, _treeNodes[child2].AABB, out var aabb);
                    FP oldArea = _treeNodes[child2].AABB.GetSurfaceArea();
                    FP newArea = aabb.GetSurfaceArea();
                    cost2 = newArea - oldArea + inheritanceCost;
                }

                // Descend according to the minimum cost.
                if (cost < cost1 && cost < cost2)
                    break;

                // Descend
                if (cost1 < cost2)
                    index = child1;
                else
                    index = child2;
            }

            int sibling = index;

            // Create a new parent.
            ref readonly var oldNode = ref _treeNodes[sibling];
            int oldParent = oldNode.Parent;
            int newParent = AllocateNode();
            ref var newParentNode = ref _treeNodes[newParent];
            newParentNode.Parent = oldParent;
            newParentNode.UserData = default;

            newParentNode.AABB.Combine(leafAABB, oldNode.AABB);
            newParentNode.Height = oldNode.Height + 1;

            if (oldParent != Settings.NULL_NODE)
            {
                ref var oldParentNode = ref _treeNodes[oldParent];

                // The sibling was not the root.
                if (oldParentNode.Child1 == sibling)
                {
                    oldParentNode.Child1 = newParent;
                }
                else
                {
                    oldParentNode.Child2 = newParent;
                }

                newParentNode.Child1 = sibling;
                newParentNode.Child2 = leaf;
                _treeNodes[sibling].Parent = newParent;
                _treeNodes[leaf].Parent = newParent;
            }
            else
            {
                // The sibling was the root.
                newParentNode.Child1 = sibling;
                newParentNode.Child2 = leaf;
                _treeNodes[sibling].Parent = newParent;
                _treeNodes[leaf].Parent = newParent;
                _root = newParent;
            }

            // Walk back up the tree fixing heights and AABBs
            index = _treeNodes[leaf].Parent;
            while (index != Settings.NULL_NODE)
            {
                index = Balance(index);
                ref var indexNode = ref _treeNodes[index];
                Debug.Assert(indexNode.Child1 != Settings.NULL_NODE);
                Debug.Assert(indexNode.Child2 != Settings.NULL_NODE);
                
                ref var child1 = ref _treeNodes[indexNode.Child1];
                ref var child2 = ref _treeNodes[indexNode.Child2];
                
                indexNode.Height = 1 + Math.Max(child1.Height, child2.Height);
                indexNode.AABB.Combine(child1.AABB, child2.AABB);

                index = indexNode.Parent;
            }

            //Validate();
        }

        private void RemoveLeaf(int leaf)
        {
            if (leaf == _root)
            {
                _root = Settings.NULL_NODE;
                return;
            }

            int parent = _treeNodes[leaf].Parent;
            ref var parentNode = ref _treeNodes[parent];
            int grandParent = parentNode.Parent;

            int sibling = parentNode.Child1 == leaf ? parentNode.Child2 : parentNode.Child1;

            if (grandParent != Settings.NULL_NODE)
            {
                ref var grandParentNode = ref _treeNodes[grandParent];
                
                // Destroy parent and connect sibling to grandParent.
                if (grandParentNode.Child1 == parent)
                    grandParentNode.Child1 = sibling;
                else
                    grandParentNode.Child2 = sibling;

                _treeNodes[sibling].Parent = grandParent;
                FreeNode(parent);

                // Adjust ancestor bounds.
                int index = grandParent;
                while (index != Settings.NULL_NODE)
                {
                    index = Balance(index);

                    ref var indexNode = ref _treeNodes[index];
                    ref var child1 = ref _treeNodes[indexNode.Child1];
                    ref var child2 = ref _treeNodes[indexNode.Child2];
                    
                    indexNode.AABB.Combine(child1.AABB, child2.AABB);
                    indexNode.Height = 1 + Math.Max(child1.Height, child2.Height);

                    index = indexNode.Parent;
                }
            }
            else
            {
                _root = sibling;
                _treeNodes[sibling].Parent = Settings.NULL_NODE;
                FreeNode(parent);
            }

            //Validate();
        }

        private int Balance(int iA)
        {
            Debug.Assert(iA != Settings.NULL_NODE);

            ref var A = ref _treeNodes[iA];
            if (A.IsLeaf || A.Height < 2)
                return iA;

            int iB = A.Child1;
            int iC = A.Child2;

            Debug.Assert(0 <= iB && iB < _nodeCapacity);
            Debug.Assert(0 <= iC && iC < _nodeCapacity);
            ref var B = ref _treeNodes[iB];
            ref var C = ref _treeNodes[iC];

            int balance = C.Height - B.Height;

            // Rotate C up
            if (balance > 1)
            {
                int iF = C.Child1;
                int iG = C.Child2;

                Debug.Assert(0 <= iF && iF < _nodeCapacity);
                Debug.Assert(0 <= iG && iG < _nodeCapacity);
                ref var F = ref _treeNodes[iF];
                ref var G = ref _treeNodes[iG];

                // Swap A and C
                C.Child1 = iA;
                C.Parent = A.Parent;
                A.Parent = iC;

                // A's old parent should point to C
                if (C.Parent != Settings.NULL_NODE)
                {
                    ref var cParentNode = ref _treeNodes[C.Parent];
                    if (cParentNode.Child1 == iA)
                    {
                        cParentNode.Child1 = iC;
                    }
                    else
                    {
                        Debug.Assert(cParentNode.Child2 == iA);
                        cParentNode.Child2 = iC;
                    }
                }
                else
                {
                    _root = iC;
                }

                // Rotate
                if (F.Height > G.Height)
                {
                    C.Child2 = iF;
                    A.Child2 = iG;
                    G.Parent = iA;
                    A.AABB.Combine(B.AABB, G.AABB);
                    C.AABB.Combine(A.AABB, F.AABB);

                    A.Height = 1 + Math.Max(B.Height, G.Height);
                    C.Height = 1 + Math.Max(A.Height, F.Height);
                }
                else
                {
                    C.Child2 = iG;
                    A.Child2 = iF;
                    F.Parent = iA;
                    A.AABB.Combine(B.AABB, F.AABB);
                    C.AABB.Combine(A.AABB, G.AABB);

                    A.Height = 1 + Math.Max(B.Height, F.Height);
                    C.Height = 1 + Math.Max(A.Height, G.Height);
                }

                return iC;
            }

            // Rotate B up
            if (balance < -1)
            {
                int iD = B.Child1;
                int iE = B.Child2;

                Debug.Assert(0 <= iD && iD < _nodeCapacity);
                Debug.Assert(0 <= iE && iE < _nodeCapacity);
                ref var D = ref _treeNodes[iD];
                ref var E = ref _treeNodes[iE];

                // Swap A and B
                B.Child1 = iA;
                B.Parent = A.Parent;
                A.Parent = iB;

                // A's old parent should point to B
                if (B.Parent != Settings.NULL_NODE)
                {
                    ref var bParentNode = ref _treeNodes[B.Parent];
                    if (bParentNode.Child1 == iA)
                    {
                        bParentNode.Child1 = iB;
                    }
                    else
                    {
                        Debug.Assert(bParentNode.Child2 == iA);
                        bParentNode.Child2 = iB;
                    }
                }
                else
                {
                    _root = iB;
                }

                // Rotate
                if (D.Height > E.Height)
                {
                    B.Child2 = iD;
                    A.Child1 = iE;
                    E.Parent = iA;
                    A.AABB.Combine(C.AABB, E.AABB);
                    B.AABB.Combine(A.AABB, D.AABB);

                    A.Height = 1 + Math.Max(C.Height, E.Height);
                    B.Height = 1 + Math.Max(A.Height, D.Height);
                }
                else
                {
                    B.Child2 = iE;
                    A.Child1 = iD;
                    D.Parent = iA;
                    A.AABB.Combine(C.AABB, D.AABB);
                    B.AABB.Combine(A.AABB, E.AABB);

                    A.Height = 1 + Math.Max(C.Height, D.Height);
                    B.Height = 1 + Math.Max(A.Height, E.Height);
                }

                return iB;
            }

            return iA;
        }

        private int ComputeHeight()
        {
            int height = ComputeHeight(_root);
            return height;
        }

        private int ComputeHeight(int nodeId)
        {
            Debug.Assert(0 <= nodeId && nodeId < _nodeCapacity);
            ref readonly var node = ref _treeNodes[nodeId];
            
            if (node.IsLeaf)
                return 0;

            int height1 = ComputeHeight(node.Child1);
            int height2 = ComputeHeight(node.Child2);
            return 1 + Math.Max(height1, height2);
        }

        private void ValidateStructure(int index)
        {
            if (index == Settings.NULL_NODE)
                return;

            if (index == _root)
                Debug.Assert(_treeNodes[index].Parent == Settings.NULL_NODE);

            ref readonly var node = ref _treeNodes[index];
            
            int child1 = node.Child1;
            int child2 = node.Child2;

            if (node.IsLeaf)
            {
                Debug.Assert(child1 == Settings.NULL_NODE);
                Debug.Assert(child2 == Settings.NULL_NODE);
                Debug.Assert(node.Height == 0);
                return;
            }

            Debug.Assert(0 <= child1 && child1 < _nodeCapacity);
            Debug.Assert(0 <= child2 && child2 < _nodeCapacity);

            Debug.Assert(_treeNodes[child1].Parent == index);
            Debug.Assert(_treeNodes[child2].Parent == index);

            ValidateStructure(child1);
            ValidateStructure(child2);
        }

        private void ValidateMetrics(int index)
        {
            if (index == Settings.NULL_NODE)
                return;

            ref readonly var node = ref _treeNodes[index];
            
            int child1 = node.Child1;
            int child2 = node.Child2;

            if (node.IsLeaf)
            {
                Debug.Assert(child1 == Settings.NULL_NODE);
                Debug.Assert(child2 == Settings.NULL_NODE);
                Debug.Assert(node.Height == 0);
                return;
            }

            Debug.Assert(0 <= child1 && child1 < _nodeCapacity);
            Debug.Assert(0 <= child2 && child2 < _nodeCapacity);

            int height1 = _treeNodes[child1].Height;
            int height2 = _treeNodes[child2].Height;
            int height = 1 + Math.Max(height1, height2);
            Debug.Assert(node.Height == height);

            AABB.Combine(_treeNodes[child1].AABB, _treeNodes[child2].AABB, out var aabb);
            Debug.Assert(aabb.LowerBound == node.AABB.LowerBound);
            Debug.Assert(aabb.UpperBound == node.AABB.UpperBound);

            ValidateMetrics(child1);
            ValidateMetrics(child2);
        }

        public struct TreeNode
        {
            public bool IsLeaf => Child1 == Settings.NULL_NODE;

            public int Child1;
            public int Child2;
            public int Height;
            public int Parent;
            public int Next;
            public AABB AABB;
            public T UserData;
        }

    }
}



