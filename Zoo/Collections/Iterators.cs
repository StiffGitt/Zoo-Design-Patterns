using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Collections
{
    public interface IMyIterator<T>
    {
        T First();
        T Next();
        bool IsCompleted { get; }
    }

    public class HeapForwardIterator<T> : IMyIterator<T>
    {
        private readonly Heap<T> heap;
        private int current = 0;
        private const int step = 1;
        public bool IsCompleted
        {
            get
            {
                return current >= heap.Count;
            }
        }

        public HeapForwardIterator(Heap<T> heap)
        {
            this.heap = heap;
        }
        public T? First()
        {
            current = 0;
            if (heap.Count == 0)
                return default(T);
            return heap[current];
        }
        public T? Next()
        {
            current += step;
            if (!IsCompleted)
            {
                return heap[current];
                //return default(T);
            }
            else
            {
                return default(T);
            }
        }
    }

    public class HeapReverseIterator<T> : IMyIterator<T> 
    {
        private readonly Heap<T> heap;
        private int current = 0;
        private int step = 1;
        public bool IsCompleted
        {
            get
            {
                return current < 0;
            }
        }

        public HeapReverseIterator(Heap<T> heap)
        {
            this.heap = heap;
        }
        public T? First()
        {
            current = heap.Count - 1;
            if (heap.Count == 0)
                return default(T);
            return heap[current];
        }
        public T? Next()
        {
            current -= step;
            if (!IsCompleted)
            {
                return heap[current];
                //return default(T);
            }
            else
            {
                return default(T);
            }
        }
    }
}
