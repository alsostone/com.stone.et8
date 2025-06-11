using System;

namespace ProcessLog
{
    public class CircleQueue<T>
    {
        public T[] Data { get; private set; }       //队列数组
        public int Length { get; private set; }     //数组长度
        public int Front { get; private set; }      //队列首部索引
        public int Tail { get; private set; }       //队列尾部索引

        public CircleQueue(int capacity)
        {
            //会额外使用一个位置标志队列满
            Length = capacity + 1;
            Data = new T[Length];
            Front = 0;
            Tail = 0;
        }

        /// <summary>
        /// 入队列
        /// </summary>
        /// <param name="value"></param>
        public void Enqueue(T value)
        {
            //队列满
            if (IsFull()) {
                Front = (Front + 1) % Length;
            }

            Data[Tail] = value;
            Tail = (Tail + 1) % Length;
        }

        /// <summary>
        /// 出队列
        /// </summary>
        public T Dequeue()
        {
            if (IsEmpty()) {
                return default(T);
            }

            var result = Data[Front];
            Front = (Front + 1) % Length;

            return result;
        }

        /// <summary>
        /// 队列是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Front == Tail;
        }

        /// <summary>
        /// 队列已满
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return (Tail + 1) % Length == Front;
        }

        /// <summary>
        /// 返回下一个位置的数据
        /// </summary>
        /// <returns></returns>
        public T GetNext()
        {
            if (IsEmpty()) {
                return default;
            }

            if (IsFull()) {
                Front = (Front + 1) % Length;
            }

            var value = Data[Tail];
            if (value != null) {
                Tail = (Tail + 1) % Length;
            }

            return value;
        }

        /// <summary>
        /// 返回队尾往前第N个元素
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public T GetLast(int offset)
        {
            if (IsEmpty()) {
                return default;
            }
            if (offset < 0 || offset >= QueueLength()) {
                return default;
            }

            // 计算实际索引
            int index = (Tail - 1 - offset + Length) % Length;
            return Data[index];
        }
        
        /// <summary>
        /// 返回队列长度
        /// </summary>
        /// <returns></returns>
        public int QueueLength()
        {
            return (Tail + Length - Front) % Length;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            Front = 0;
            Tail = 0;
            Array.Clear(Data, 0, Data.Length);
        }
    }
}