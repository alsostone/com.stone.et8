using System.Collections;
using System.Collections.Generic;

namespace RVO
{
    public class PoolLinkedNode<TData>
    {
        public TData Data { get; set; }
        public PoolLinkedNode<TData> Previous { get; set; }
        public PoolLinkedNode<TData> Next { get; set; }

        public PoolLinkedNode(TData data)
        {
            Data = data;
        }
    }
    
    public class PoolLinkedList<K, T> : IEnumerable where T : class, new()
    {
        public int Count => _idNodeMapping.Count;
        public PoolLinkedNode<T> First => _head;
        public PoolLinkedNode<T> Last => _tail;

        private readonly Dictionary<K, PoolLinkedNode<T>> _idNodeMapping;
        private PoolLinkedNode<T> _head;
        private PoolLinkedNode<T> _tail;
        
        private static readonly Stack<PoolLinkedNode<T>> _pool = new ();
        private static PoolLinkedNode<T> GetNodeFromPool()
        {
            if (_pool.Count > 0)
            {
                return _pool.Pop();
            }
            return new PoolLinkedNode<T>(new T());
        }

        public PoolLinkedList()
        {
            _idNodeMapping = new Dictionary<K, PoolLinkedNode<T>>();
        }
        
        public T Add(K key)
        {
            var node = GetNodeFromPool();

            if (_tail == null)
            {
                _head = _tail = node;
            }
            else
            {
                node.Previous = _tail;
                _tail.Next = node;
                _tail = node;
            }

            _idNodeMapping[key] = node;
            return node.Data;
        }
        
        public bool Remove(K key)
        {
            if (!_idNodeMapping.TryGetValue(key, out var node))
                return false;

            RemoveNode(node);
            _idNodeMapping.Remove(key);

            return true;
        }
        
        public T Get(K key)
        {
            if (_idNodeMapping.TryGetValue(key, out var node))
            {
                return node.Data;
            }
            return null;
        }
        
        public void Clear()
        {
            PoolLinkedNode<T> current = _head;
            while (current != null)
            {
                var next = current.Next;
                current.Previous = null;
                current.Next = null;
                _pool.Push(current);
                current = next;
            }

            _head = null;
            _tail = null;
            _idNodeMapping.Clear();
        }
        
        private void RemoveNode(PoolLinkedNode<T> node)
        {
            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            else
            {
                _head = node.Next;
            }
            
            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }
            else
            {
                _tail = node.Previous;
            }

            node.Previous = null;
            node.Next = null;
            _pool.Push(node);
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        private struct Enumerator : IEnumerator<T>
        {
            private readonly PoolLinkedList<K, T> _list;
            private PoolLinkedNode<T> _current;
            private T _currentItem;

            public Enumerator(PoolLinkedList<K, T> list)
            {
                _list = list;
                _current = _list._head;
                _currentItem = default;
            }

            public T Current => _currentItem;

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_current == null)
                {
                    _currentItem = default;
                    return false;
                }

                _currentItem = _current.Data;
                _current = _current.Next;
                return true;
            }

            public void Reset()
            {
                _current = _list._head;
                _currentItem = default;
            }

            public void Dispose()
            {
            }
        }
    }
}