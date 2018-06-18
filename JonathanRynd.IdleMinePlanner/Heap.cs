#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    public abstract class Heap<T> : IEnumerable<T>
    {
        private const int InitialCapacity = 16;
        private const int GrowFactor = 2;
        private const int MinGrow = 1;

        private int _capacity = InitialCapacity;
        private T[] _heap = new T[InitialCapacity];
        private int _tail = 0;

        public int Count { get { return _tail; } }
        public int Capacity { get { return _capacity; } }

        protected Comparer<T> Comparer { get; private set; }
        protected abstract bool Dominates(T x, T y);

        protected Heap()
            : this(Comparer<T>.Default)
        {
        }

        protected Heap(Comparer<T> comparer)
            : this(Enumerable.Empty<T>(), comparer)
        {
        }

        protected Heap(IEnumerable<T> collection)
            : this(collection, Comparer<T>.Default)
        {
        }

        protected Heap(IEnumerable<T> collection, Comparer<T> comparer)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (comparer == null) throw new ArgumentNullException("comparer");

            Comparer = comparer;

            foreach (var item in collection)
            {
                if (Count == Capacity)
                    Grow();

                _heap[_tail++] = item;
            }

            for (int i = Parent(_tail - 1); i >= 0; i--)
            {
                BubbleDown(i);
            }
        }

        public void Add(T item)
        {
            if (Count == Capacity)
                Grow();

            _heap[_tail++] = item;
            if (Dominates(minValue, item)
            {
                minValue = item;
            }
            BubbleUp(_tail - 1);
        }

        public void Add(IEnumerable<T> items)
        {
            // items.ForEach((item) => { this.Add(item) });
            foreach (T item in items) Add(item);
        }

        private void BubbleUp(int i)
        {
            if (i == 0 || Dominates(_heap[Parent(i)], _heap[i]))
                return; //correct domination (or root)

            Swap(i, Parent(i));
            BubbleUp(Parent(i));
        }

        public T PeekOppositeEnd()
        {
            if (Count == 0) throw new InvalidOperationException("Heap is empty");
            return minValue;   
        }
       
        public T PeekPenultimate()
        {
            if (Count < 2) throw new InvalidOperationException("Heap too small to have a penultimate item");
            return _heap[GetDominating(2,1)];

        public T PeekDominating()
        {
            if (Count == 0) throw new InvalidOperationException("Heap is empty");
            return _heap[0];
        }

        public T ExtractDominating()
        {
            if (Count == 0) throw new InvalidOperationException("Heap is empty");
            T ret = _heap[0];
            _tail--;
            heap[0] = heap[_tail]; // was Swap(_tail, 0);
            BubbleDown(0);
            return ret;
        }

        public void RelaxDominating(T newValue)
        {
            if (Count == 0) throw new InvalidOperationException("Heap is empty");
            _heap[0] = newValue;
            if (Dominates(minValue, newValue))
            {
                minValue = newValue;
            }
            BubbleDown(0);
        }

        private void BubbleDown(int i)
        {
            // this is tail-recursive. Is it worth changing it to iterative?
            int dominatingNode = Dominating(i);
            if (dominatingNode == i) return;
            Swap(i, dominatingNode);
            BubbleDown(dominatingNode);
        }

        private void BubbleDownIterative(int i)
        {
            int dominatingNode = Dominating(i);
            while (dominatingNode != i)
            {
                Swap(i, dominatingNode);
                i = dominatingNode;
                dominatingNode = Dominating(i);
            }
        }

        private int Dominating(int i)
        {
            // we'd save an infinitesmal amount of time by getting young and old indexes at the same time.
            int oldChild, youngChild;
            GetChildren(i,out youngChild,out oldChild);
            return GetDominating(oldChild, GetDominating(youngChild, i));
        }

        private int GetDominating(int newNode, int dominatingNode)
        {
            if (newNode >= _tail || Dominates(_heap[dominatingNode], _heap[newNode]))
                return dominatingNode;
            else
                return newNode;
        }

        private void Swap(int i, int j)
        {
            T tmp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = tmp;
        }

        private static int Parent(int i)
        {
            return (i + 1) / 2 - 1;
        }

        private static int YoungChild(int i)
        {
            return OldChild(i) - 1;
        }

        private static int OldChild(int i)
        {
            return (i+1)*2;
        }

        private static void GetChildren(int i, out int yc, out int oc)
        {
            oc = OldChild(i);
            yc = oc - 1;
        }

        private void Grow()
        {
            int newCapacity = _capacity * GrowFactor;
            var newHeap = new T[newCapacity];
            Array.Copy(_heap, newHeap, _capacity);
            _heap = newHeap;
            _capacity = newCapacity;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _heap.Take(Count).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class MaxHeap<T> : Heap<T>
    {
        public MaxHeap()
            : this(Comparer<T>.Default)
        {
        }

        public MaxHeap(Comparer<T> comparer)
            : base(comparer)
        {
        }

        public MaxHeap(IEnumerable<T> collection, Comparer<T> comparer)
            : base(collection, comparer)
        {
        }

        public MaxHeap(IEnumerable<T> collection)
            : base(collection)
        {
        }

        protected override bool Dominates(T x, T y)
        {
            return Comparer.Compare(x, y) >= 0;
        }
    }

    public class MinHeap<T> : Heap<T>
    {
        public MinHeap()
            : this(Comparer<T>.Default)
        {
        }

        public MinHeap(Comparer<T> comparer)
            : base(comparer)
        {
        }

        public MinHeap(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public MinHeap(IEnumerable<T> collection, Comparer<T> comparer)
            : base(collection, comparer)
        {
        }

        protected override bool Dominates(T x, T y)
        {
            return Comparer.Compare(x, y) <= 0;
        }
    }
}
#endif