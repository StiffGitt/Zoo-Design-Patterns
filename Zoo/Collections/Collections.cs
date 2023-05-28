using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Collections
{
    public interface IMyCollection<T>
    {
        IMyIterator<T> GetForwardIterator();
        IMyIterator<T> GetReverseIterator();
        public void Insert(T item);
        public void Remove();
        public void RemoveItem(T item);
    }

    public class Heap<T> : IMyCollection<T>
    {
        private List<T> L;
        private readonly IMyComparator comparator;
        public int Count { get { return L.Count; } }
        public T this[int i]
        {
            get => L[i];
        }
        public Heap(/*List<T> list,*/IMyComparator comparator)
        {
            this.comparator = comparator;
            L = new List<T>();
        }

        public void Insert(T item)
        {
            L.Add(item);
            UpHeap(L.Count - 1);
        }
        public void Remove()
        {
            if (L.Count == 0)
                return;
            L[0] = L[L.Count - 1];
            L.RemoveAt(L.Count - 1);
            DownHeap(0);
        }
        public IMyIterator<T> GetForwardIterator()
        {
            return new HeapForwardIterator<T>(this);
        }
        public IMyIterator<T> GetReverseIterator()
        {
            return new HeapReverseIterator<T>(this);
        }
        public void RemoveItem(T item)
        {
            int idx = FindItemIdx(item);
            if (idx < 0)
                return;
            Swap(idx, L.Count - 1);
            L.RemoveAt(L.Count - 1);
            if (idx != L.Count)
            {
                DownHeap(idx);
                UpHeap(idx);
            }
        }
        private void UpHeap(int idx)
        {
            int parent = (idx - 1) / 2;
            if (idx <= 0)
                return;

            if (comparator.Compare(L[idx], L[parent]) > 0)
            {
                Swap(idx, parent);
            }
            UpHeap(parent);
        }

        private void DownHeap(int idx)
        {
            int left = idx * 2 + 1;
            int right = idx * 2 + 2;
            int biggerChild = 0;
            if (L.Count <= left)
                return;
            else if (L.Count - 1 == left)
            {
                if (comparator.Compare(L[idx], L[left]) < 0)
                {
                    Swap(idx, left);
                }
                return;
            }
            else
            {
                biggerChild = comparator.Compare(L[left], L[right]) > 0 ? left : right;
                if (comparator.Compare(L[idx], L[biggerChild]) < 0)
                {
                    Swap(idx, biggerChild);
                }
            }
            DownHeap(biggerChild);
        }
        private void Swap(int i, int j)
        {
            T tmp = L[i];
            L[i] = L[j];
            L[j] = tmp;
        }
        private int FindItemIdx(T item)
        {
            for (int i = 0; i < L.Count; i++)
            {
                if (L[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }
        
    }
}
