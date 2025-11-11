using System.Collections.Generic;
using UnityEngine;

namespace ST.HUD
{
    public struct HudInstanceData
    {
        public int Visible;
        public Vector3 Position;
        public int NameIndex;
        public float Progress;
        public Vector3 Color;
    }

    internal class HudInstanceDataBuffer
    {
        internal int DrawingCount => _count - _freeIndices.Count;
        internal HudInstanceData this[int index] => _buffer[index];
        internal int Capacity { get;  private set; }
        
        private readonly Stack<int> _freeIndices;
        private readonly HudInstanceData[] _buffer;
        private int _count;
        private bool _dirty;

        internal HudInstanceDataBuffer(int capacity)
        {
            this.Capacity = capacity;
            _freeIndices = new Stack<int>(capacity / 10);   // 10%的空间用来存储空闲索引
            _buffer = new HudInstanceData[capacity];
            this._count = 0;
            _dirty = false;
        }
        
        internal int Insert(HudInstanceData item)
        {
            if (_freeIndices.TryPop(out var index))
            {
                _buffer[index] = item;
                _dirty = true;
                return index;
            }
            if (this.Capacity > this._count)
            {
                index = this._count;
                this._count++;
                _buffer[index] = item;
                _dirty = true;
                return index;
            }
            return -1;
        }
        
        internal void Remove(int index)
        {
            if (index < 0 || index >= this.Capacity) return;
            _buffer[index].Visible = 0;
            _freeIndices.Push(index);
            _dirty = true;
        }
        
        internal void SetData(int index, HudInstanceData data)
        {
            if (index < 0 || index >= this.Capacity) return;
            _buffer[index] = data;
            _dirty = true;
        }
        
        internal void TryAppendData(ComputeBuffer buffer)
        {
            if (_dirty)
            {
                buffer.SetData(_buffer, 0, 0, this._count);
                _dirty = false;
            }
        }
    }

}