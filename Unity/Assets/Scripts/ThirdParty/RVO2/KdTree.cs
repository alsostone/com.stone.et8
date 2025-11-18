/*
 * KdTree.cs
 * RVO2 Library C#
 *
 * SPDX-FileCopyrightText: 2008 University of North Carolina at Chapel Hill
 * SPDX-License-Identifier: Apache-2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Please send all bug reports to <geom@cs.unc.edu>.
 *
 * The authors may be contacted via:
 *
 * Jur van den Berg, Stephen J. Guy, Jamie Snape, Ming C. Lin, Dinesh Manocha
 * Dept. of Computer Science
 * 201 S. Columbia St.
 * Frederick P. Brooks, Jr. Computer Science Bldg.
 * Chapel Hill, N.C. 27599-3175
 * United States of America
 *
 * <http://gamma.cs.unc.edu/RVO2/>
 */

using System;
using System.Collections.Generic;
using TrueSync;

namespace RVO
{
    /**
     * <summary>Defines k-D trees for agents and static obstacles in the
     * simulation.</summary>
     */
    internal class KdTree
    {
        /**
         * <summary>Defines a node of an agent k-D tree.</summary>
         */
        private struct AgentTreeNode
        {
            internal int begin_;
            internal int end_;
            internal int left_;
            internal int right_;
            internal FP maxX_;
            internal FP maxY_;
            internal FP minX_;
            internal FP minY_;
        }

        /**
         * <summary>Defines a pair of scalar values.</summary>
         */
        private struct FloatPair
        {
            private readonly FP a_;
            private readonly FP b_;

            /**
             * <summary>Constructs and initializes a pair of scalar
             * values.</summary>
             *
             * <param name="a">The first scalar value.</param>
             * <param name="b">The second scalar value.</param>
             */
            internal FloatPair(FP a, FP b)
            {
                a_ = a;
                b_ = b;
            }

            /**
             * <summary>Returns true if the first pair of scalar values is less
             * than the second pair of scalar values.</summary>
             *
             * <returns>True if the first pair of scalar values is less than the
             * second pair of scalar values.</returns>
             *
             * <param name="pair1">The first pair of scalar values.</param>
             * <param name="pair2">The second pair of scalar values.</param>
             */
            public static bool operator <(FloatPair pair1, FloatPair pair2)
            {
                return pair1.a_ < pair2.a_ || !(pair2.a_ < pair1.a_) && pair1.b_ < pair2.b_;
            }

            /**
             * <summary>Returns true if the first pair of scalar values is less
             * than or equal to the second pair of scalar values.</summary>
             *
             * <returns>True if the first pair of scalar values is less than or
             * equal to the second pair of scalar values.</returns>
             *
             * <param name="pair1">The first pair of scalar values.</param>
             * <param name="pair2">The second pair of scalar values.</param>
             */
            public static bool operator <=(FloatPair pair1, FloatPair pair2)
            {
                return (pair1.a_ == pair2.a_ && pair1.b_ == pair2.b_) || pair1 < pair2;
            }

            /**
             * <summary>Returns true if the first pair of scalar values is
             * greater than the second pair of scalar values.</summary>
             *
             * <returns>True if the first pair of scalar values is greater than
             * the second pair of scalar values.</returns>
             *
             * <param name="pair1">The first pair of scalar values.</param>
             * <param name="pair2">The second pair of scalar values.</param>
             */
            public static bool operator >(FloatPair pair1, FloatPair pair2)
            {
                return !(pair1 <= pair2);
            }

            /**
             * <summary>Returns true if the first pair of scalar values is
             * greater than or equal to the second pair of scalar values.
             * </summary>
             *
             * <returns>True if the first pair of scalar values is greater than
             * or equal to the second pair of scalar values.</returns>
             *
             * <param name="pair1">The first pair of scalar values.</param>
             * <param name="pair2">The second pair of scalar values.</param>
             */
            public static bool operator >=(FloatPair pair1, FloatPair pair2)
            {
                return !(pair1 < pair2);
            }
        }

        /**
         * <summary>Defines a node of an obstacle k-D tree.</summary>
         */
        private class ObstacleTreeNode
        {
            internal Obstacle obstacle_;
            internal ObstacleTreeNode left_;
            internal ObstacleTreeNode right_;
        };

        /**
         * <summary>The maximum size of an agent k-D tree leaf.</summary>
         */
        private const int MAX_LEAF_SIZE = 10;

        private readonly List<Agent> agents_ = new (128);
        private readonly List<Obstacle> obstacles_ = new (64);
        private AgentTreeNode[] agentTree_ = new AgentTreeNode[256];
        private ObstacleTreeNode obstacleTree_;
        
        internal void SetAgents(PoolLinkedList<long, Agent> agents)
        {
            int index = 0;
            foreach (Agent agent in agents) {
                agents_[index++] = agent;
            }
            for (int i = agents_.Count - 1; i >= index; i--) {
                agents_.RemoveAt(i);
            }
            if (agents.Count * 2 > agentTree_.Length) {
                agentTree_ = new AgentTreeNode[agents.Count * 2];
            }
        }
        
        internal void SetObstacles(PoolLinkedList<(long, int), Obstacle> obstacles)
        {
            int index = 0;
            foreach (Obstacle obstacle in obstacles) {
                obstacles_[index] = obstacle;
            }
            for (int i = obstacles_.Count - 1; i >= index; i--) {
                obstacles_.RemoveAt(i);
            }
        }

        /**
         * <summary>Builds an agent k-D tree.</summary>
         */
        internal void buildAgentTree()
        {
            if (agents_.Count != 0)
            {
                buildAgentTreeRecursive(0, agents_.Count, 0);
            }
        }

        /**
         * <summary>Builds an obstacle k-D tree.</summary>
         */
        internal void buildObstacleTree()
        {
            obstacleTree_ = buildObstacleTreeRecursive(obstacles_);
        }

        /**
         * <summary>Computes the agent neighbors of the specified agent.
         * </summary>
         *
         * <param name="agent">The agent for which agent neighbors are to be
         * computed.</param>
         * <param name="rangeSq">The squared range around the agent.</param>
         */
        internal void computeAgentNeighbors(Agent agent, ref FP rangeSq)
        {
            queryAgentTreeRecursive(agent, ref rangeSq, 0);
        }

        /**
         * <summary>Computes the obstacle neighbors of the specified agent.
         * </summary>
         *
         * <param name="agent">The agent for which obstacle neighbors are to be
         * computed.</param>
         * <param name="rangeSq">The squared range around the agent.</param>
         */
        internal void computeObstacleNeighbors(Agent agent, FP rangeSq)
        {
            queryObstacleTreeRecursive(agent, rangeSq, obstacleTree_);
        }

        /**
         * <summary>Queries the visibility between two points within a specified
         * radius.</summary>
         *
         * <returns>True if q1 and q2 are mutually visible within the radius;
         * false otherwise.</returns>
         *
         * <param name="q1">The first point between which visibility is to be
         * tested.</param>
         * <param name="q2">The second point between which visibility is to be
         * tested.</param>
         * <param name="radius">The radius within which visibility is to be
         * tested.</param>
         */
        internal bool queryVisibility(TSVector2 q1, TSVector2 q2, FP radius)
        {
            return queryVisibilityRecursive(q1, q2, radius, obstacleTree_);
        }

        /**
         * <summary>Recursive method for building an agent k-D tree.</summary>
         *
         * <param name="begin">The beginning agent k-D tree node node index.
         * </param>
         * <param name="end">The ending agent k-D tree node index.</param>
         * <param name="node">The current agent k-D tree node index.</param>
         */
        private void buildAgentTreeRecursive(int begin, int end, int node)
        {
            agentTree_[node].begin_ = begin;
            agentTree_[node].end_ = end;
            agentTree_[node].minX_ = agentTree_[node].maxX_ = agents_[begin].position.x;
            agentTree_[node].minY_ = agentTree_[node].maxY_ = agents_[begin].position.y;

            for (int i = begin + 1; i < end; ++i)
            {
                agentTree_[node].maxX_ = TSMath.Max(agentTree_[node].maxX_, agents_[i].position.x);
                agentTree_[node].minX_ = TSMath.Min(agentTree_[node].minX_, agents_[i].position.x);
                agentTree_[node].maxY_ = TSMath.Max(agentTree_[node].maxY_, agents_[i].position.y);
                agentTree_[node].minY_ = TSMath.Min(agentTree_[node].minY_, agents_[i].position.y);
            }

            if (end - begin > MAX_LEAF_SIZE)
            {
                /* No leaf node. */
                bool isVertical = agentTree_[node].maxX_ - agentTree_[node].minX_ > agentTree_[node].maxY_ - agentTree_[node].minY_;
                FP splitValue = FP.Half * (isVertical ? agentTree_[node].maxX_ + agentTree_[node].minX_ : agentTree_[node].maxY_ + agentTree_[node].minY_);

                int left = begin;
                int right = end;

                while (left < right)
                {
                    while (left < right && (isVertical ? agents_[left].position.x : agents_[left].position.y) < splitValue)
                    {
                        ++left;
                    }

                    while (right > left && (isVertical ? agents_[right - 1].position.x : agents_[right - 1].position.y) >= splitValue)
                    {
                        --right;
                    }

                    if (left < right)
                    {
                        (agents_[left], agents_[right - 1]) = (agents_[right - 1], agents_[left]);
                        ++left;
                        --right;
                    }
                }

                int leftSize = left - begin;

                if (leftSize == 0)
                {
                    ++leftSize;
                    ++left;
                }

                agentTree_[node].left_ = node + 1;
                agentTree_[node].right_ = node + 2 * leftSize;

                buildAgentTreeRecursive(begin, left, agentTree_[node].left_);
                buildAgentTreeRecursive(left, end, agentTree_[node].right_);
            }
        }

        /**
         * <summary>Recursive method for building an obstacle k-D tree.
         * </summary>
         *
         * <returns>An obstacle k-D tree node.</returns>
         *
         * <param name="obstacles">A list of obstacles.</param>
         */
        private ObstacleTreeNode buildObstacleTreeRecursive(IList<Obstacle> obstacles)
        {
            if (obstacles.Count == 0)
            {
                return null;
            }

            ObstacleTreeNode node = new();

            int optimalSplit = 0;
            int minLeft = obstacles.Count;
            int minRight = obstacles.Count;

            for (int i = 0; i < obstacles.Count; ++i)
            {
                int leftSize = 0;
                int rightSize = 0;

                Obstacle obstacleI1 = obstacles[i];
                Obstacle obstacleI2 = obstacleI1.next_;

                /* Compute optimal split node. */
                for (int j = 0; j < obstacles.Count; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    Obstacle obstacleJ1 = obstacles[j];
                    Obstacle obstacleJ2 = obstacleJ1.next_;

                    FP j1LeftOfI = RVOMath.leftOf(obstacleI1.point_, obstacleI2.point_, obstacleJ1.point_);
                    FP j2LeftOfI = RVOMath.leftOf(obstacleI1.point_, obstacleI2.point_, obstacleJ2.point_);

                    if (j1LeftOfI >= -FP.EN5 && j2LeftOfI >= -FP.EN5)
                    {
                        ++leftSize;
                    }
                    else if (j1LeftOfI <= FP.EN5 && j2LeftOfI <= FP.EN5)
                    {
                        ++rightSize;
                    }
                    else
                    {
                        ++leftSize;
                        ++rightSize;
                    }

                    if (new FloatPair(Math.Max(leftSize, rightSize), Math.Min(leftSize, rightSize)) >= new FloatPair(Math.Max(minLeft, minRight), Math.Min(minLeft, minRight)))
                    {
                        break;
                    }
                }

                if (new FloatPair(Math.Max(leftSize, rightSize), Math.Min(leftSize, rightSize)) < new FloatPair(Math.Max(minLeft, minRight), Math.Min(minLeft, minRight)))
                {
                    minLeft = leftSize;
                    minRight = rightSize;
                    optimalSplit = i;
                }
            }

            {
                /* Build split node. */
                IList<Obstacle> leftObstacles = new List<Obstacle>(minLeft);

                for (int n = 0; n < minLeft; ++n)
                {
                    leftObstacles.Add(null);
                }

                IList<Obstacle> rightObstacles = new List<Obstacle>(minRight);

                for (int n = 0; n < minRight; ++n)
                {
                    rightObstacles.Add(null);
                }

                int leftCounter = 0;
                int rightCounter = 0;
                int i = optimalSplit;

                Obstacle obstacleI1 = obstacles[i];
                Obstacle obstacleI2 = obstacleI1.next_;

                for (int j = 0; j < obstacles.Count; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    Obstacle obstacleJ1 = obstacles[j];
                    Obstacle obstacleJ2 = obstacleJ1.next_;

                    FP j1LeftOfI = RVOMath.leftOf(obstacleI1.point_, obstacleI2.point_, obstacleJ1.point_);
                    FP j2LeftOfI = RVOMath.leftOf(obstacleI1.point_, obstacleI2.point_, obstacleJ2.point_);

                    if (j1LeftOfI >= -FP.EN5 && j2LeftOfI >= -FP.EN5)
                    {
                        leftObstacles[leftCounter++] = obstacles[j];
                    }
                    else if (j1LeftOfI <= FP.EN5 && j2LeftOfI <= FP.EN5)
                    {
                        rightObstacles[rightCounter++] = obstacles[j];
                    }
                    else
                    {
                        /* Split obstacle j. */
                        FP t = RVOMath.det(obstacleI2.point_ - obstacleI1.point_, obstacleJ1.point_ - obstacleI1.point_) / RVOMath.det(obstacleI2.point_ - obstacleI1.point_, obstacleJ1.point_ - obstacleJ2.point_);

                        TSVector2 splitPoint = obstacleJ1.point_ + t * (obstacleJ2.point_ - obstacleJ1.point_);

                        Obstacle newObstacle = new();
                        newObstacle.point_ = splitPoint;
                        newObstacle.previous_ = obstacleJ1;
                        newObstacle.next_ = obstacleJ2;
                        newObstacle.convex_ = true;
                        newObstacle.direction_ = obstacleJ1.direction_;

                        obstacleJ1.next_ = newObstacle;
                        obstacleJ2.previous_ = newObstacle;

                        if (j1LeftOfI > FP.Zero)
                        {
                            leftObstacles[leftCounter++] = obstacleJ1;
                            rightObstacles[rightCounter++] = newObstacle;
                        }
                        else
                        {
                            rightObstacles[rightCounter++] = obstacleJ1;
                            leftObstacles[leftCounter++] = newObstacle;
                        }
                    }
                }

                node.obstacle_ = obstacleI1;
                node.left_ = buildObstacleTreeRecursive(leftObstacles);
                node.right_ = buildObstacleTreeRecursive(rightObstacles);

                return node;
            }
        }

        /**
         * <summary>Recursive method for computing the agent neighbors of the
         * specified agent.</summary>
         *
         * <param name="agent">The agent for which agent neighbors are to be
         * computed.</param>
         * <param name="rangeSq">The squared range around the agent.</param>
         * <param name="node">The current agent k-D tree node index.</param>
         */
        private void queryAgentTreeRecursive(Agent agent, ref FP rangeSq, int node)
        {
            if (agentTree_[node].end_ - agentTree_[node].begin_ <= MAX_LEAF_SIZE)
            {
                for (int i = agentTree_[node].begin_; i < agentTree_[node].end_; ++i)
                {
                    agent.insertAgentNeighbor(agents_[i], ref rangeSq);
                }
            }
            else
            {
                FP distSqLeft = RVOMath.sqr(TSMath.Max(FP.Zero, agentTree_[agentTree_[node].left_].minX_ - agent.position.x)) + RVOMath.sqr(TSMath.Max(FP.Zero, agent.position.x - agentTree_[agentTree_[node].left_].maxX_)) + RVOMath.sqr(TSMath.Max(FP.Zero, agentTree_[agentTree_[node].left_].minY_ - agent.position.y)) + RVOMath.sqr(TSMath.Max(FP.Zero, agent.position.y - agentTree_[agentTree_[node].left_].maxY_));
                FP distSqRight = RVOMath.sqr(TSMath.Max(FP.Zero, agentTree_[agentTree_[node].right_].minX_ - agent.position.x)) + RVOMath.sqr(TSMath.Max(FP.Zero, agent.position.x - agentTree_[agentTree_[node].right_].maxX_)) + RVOMath.sqr(TSMath.Max(FP.Zero, agentTree_[agentTree_[node].right_].minY_ - agent.position.y)) + RVOMath.sqr(TSMath.Max(FP.Zero, agent.position.y - agentTree_[agentTree_[node].right_].maxY_));

                if (distSqLeft < distSqRight)
                {
                    if (distSqLeft < rangeSq)
                    {
                        queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].left_);

                        if (distSqRight < rangeSq)
                        {
                            queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].right_);
                        }
                    }
                }
                else
                {
                    if (distSqRight < rangeSq)
                    {
                        queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].right_);

                        if (distSqLeft < rangeSq)
                        {
                            queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].left_);
                        }
                    }
                }

            }
        }

        /**
         * <summary>Recursive method for computing the obstacle neighbors of the
         * specified agent.</summary>
         *
         * <param name="agent">The agent for which obstacle neighbors are to be
         * computed.</param>
         * <param name="rangeSq">The squared range around the agent.</param>
         * <param name="node">The current obstacle k-D node.</param>
         */
        private void queryObstacleTreeRecursive(Agent agent, FP rangeSq, ObstacleTreeNode node)
        {
            if (node != null)
            {
                Obstacle obstacle1 = node.obstacle_;
                Obstacle obstacle2 = obstacle1.next_;

                FP agentLeftOfLine = RVOMath.leftOf(obstacle1.point_, obstacle2.point_, agent.position);

                queryObstacleTreeRecursive(agent, rangeSq, agentLeftOfLine >= FP.Zero ? node.left_ : node.right_);

                FP distSqLine = RVOMath.sqr(agentLeftOfLine) / RVOMath.absSq(obstacle2.point_ - obstacle1.point_);

                if (distSqLine < rangeSq)
                {
                    if (agentLeftOfLine < FP.Zero)
                    {
                        /*
                         * Try obstacle at this node only if agent is on right side of
                         * obstacle (and can see obstacle).
                         */
                        agent.insertObstacleNeighbor(node.obstacle_, rangeSq);
                    }

                    /* Try other side of line. */
                    queryObstacleTreeRecursive(agent, rangeSq, agentLeftOfLine >= FP.Zero ? node.right_ : node.left_);
                }
            }
        }

        /**
         * <summary>Recursive method for querying the visibility between two
         * points within a specified radius.</summary>
         *
         * <returns>True if q1 and q2 are mutually visible within the radius;
         * false otherwise.</returns>
         *
         * <param name="q1">The first point between which visibility is to be
         * tested.</param>
         * <param name="q2">The second point between which visibility is to be
         * tested.</param>
         * <param name="radius">The radius within which visibility is to be
         * tested.</param>
         * <param name="node">The current obstacle k-D node.</param>
         */
        private bool queryVisibilityRecursive(TSVector2 q1, TSVector2 q2, FP radius, ObstacleTreeNode node)
        {
            if (node == null)
            {
                return true;
            }

            Obstacle obstacle1 = node.obstacle_;
            Obstacle obstacle2 = obstacle1.next_;

            FP q1LeftOfI = RVOMath.leftOf(obstacle1.point_, obstacle2.point_, q1);
            FP q2LeftOfI = RVOMath.leftOf(obstacle1.point_, obstacle2.point_, q2);
            FP invLengthI = FP.One / RVOMath.absSq(obstacle2.point_ - obstacle1.point_);

            if (q1LeftOfI >= FP.Zero && q2LeftOfI >= FP.Zero)
            {
                return queryVisibilityRecursive(q1, q2, radius, node.left_) && ((RVOMath.sqr(q1LeftOfI) * invLengthI >= RVOMath.sqr(radius) && RVOMath.sqr(q2LeftOfI) * invLengthI >= RVOMath.sqr(radius)) || queryVisibilityRecursive(q1, q2, radius, node.right_));
            }

            if (q1LeftOfI <= FP.Zero && q2LeftOfI <= FP.Zero)
            {
                return queryVisibilityRecursive(q1, q2, radius, node.right_) && ((RVOMath.sqr(q1LeftOfI) * invLengthI >= RVOMath.sqr(radius) && RVOMath.sqr(q2LeftOfI) * invLengthI >= RVOMath.sqr(radius)) || queryVisibilityRecursive(q1, q2, radius, node.left_));
            }

            if (q1LeftOfI >= FP.Zero && q2LeftOfI <= FP.Zero)
            {
                /* One can see through obstacle from left to right. */
                return queryVisibilityRecursive(q1, q2, radius, node.left_) && queryVisibilityRecursive(q1, q2, radius, node.right_);
            }

            FP point1LeftOfQ = RVOMath.leftOf(q1, q2, obstacle1.point_);
            FP point2LeftOfQ = RVOMath.leftOf(q1, q2, obstacle2.point_);
            FP invLengthQ = FP.One / RVOMath.absSq(q2 - q1);

            return point1LeftOfQ * point2LeftOfQ >= FP.Zero && RVOMath.sqr(point1LeftOfQ) * invLengthQ > RVOMath.sqr(radius) && RVOMath.sqr(point2LeftOfQ) * invLengthQ > RVOMath.sqr(radius) && queryVisibilityRecursive(q1, q2, radius, node.left_) && queryVisibilityRecursive(q1, q2, radius, node.right_);
        }
    }
}
