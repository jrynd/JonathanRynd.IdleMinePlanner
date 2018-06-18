using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    class MemoizeEnumerable<T> : IList<T>, IDisposable
    {
        private readonly IEnumerable<T> _source;
        private IList<T> _data;
        private IEnumerator<T> _enumSource;
        private bool _disposed;
        private bool _reachedEnd;

        #region Fake functions to fake being an IList
        public bool IsReadOnly { get {return true;} set {throw new NotSupportedException(); } }
        public void Insert(int index, T item) { throw new NotImplementedException("readonly"); } // readonly
        public void RemoveAt(int index) { throw new NotImplementedException("readonly"); } // readonly
        public bool Remove(T item) { throw new NotImplementedException("readonly"); } // readonly
        public void Add(T item) { throw new NotImplementedException("readonly"); } // readonly
        public void Clear() { throw new NotImplementedException("readonly"); } // readonly
        public int Count { get { throw new NotSupportedException(); } set { throw new NotSupportedException("don't know if it's infinite"); } }
        public void CopyTo(T[] item, int count) { throw new NotSupportedException("don't know if it's infinite"); }
        #endregion
        public T this[int index] { get { return _data[index]; } set { throw new NotImplementedException(); } } // readonly
        public int IndexOf(T item) { return _data.IndexOf(item); }
        public bool Contains(T item) { return _data.Contains(item); }



        public MemoizeEnumerable(IEnumerable<T> source)
        {
            _source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_disposed)
                throw new ObjectDisposedException("LetIterator");

            if (_enumSource == null && !_reachedEnd)
            {
                _enumSource = _source.GetEnumerator();
                _data = new List<T>();
            }

            return new MemoizeEnumerator<T>(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool TryMoveNext(int cursor, out T value)
        {
            //Debug.Assert(cursor <= _data.Count);

            if (_disposed)
                throw new ObjectDisposedException("LetIterator");

            if (cursor < _data.Count)
            {
                value = _data[cursor];
                return true;
            }
            else
            {
                if (_reachedEnd)
                {
                    value = default(T);
                    return false;
                }

                if (_enumSource.MoveNext())
                {
                    value = _enumSource.Current;
                    _data.Add(value);
                    return true;
                }
                else
                {
                    // When we reach the end, no reason to delay the disposal of the source
                    // any longer. Now we should avoid calls to MoveNext further on, so we
                    // set a _reachedEnd flag to detect this case.
                    _enumSource.Dispose();
                    _enumSource = null;
                    _reachedEnd = true;

                    value = default(T);
                    return false;
                }
            }
        }

        public void Dispose()
        {
            // Facilitates deterministic disposal of the underlying store. We can't rely
            // on Dispose calls on the enumerator object as we don't know the number of
            // consumers we're dealing with.
            if (!_reachedEnd)
            {
                _enumSource.Dispose();
                _enumSource = null;
            }

            _disposed = true;
            _data.Clear();
        }

        class MemoizeEnumerator<T_> : IEnumerator<T_>
        {
            private readonly MemoizeEnumerable<T_> _source;
            private int _cursor;
            private T_ _lastValue;

            public MemoizeEnumerator(MemoizeEnumerable<T_> source)
            {
                _source = source;
                _cursor = 0;
            }

            public T_ Current
            {
                get { return _lastValue; }
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return _source.TryMoveNext(_cursor++, out _lastValue);
            }

            public void Reset()
            {
                _cursor = 0;
            }

            public void Dispose()
            {
                // No disposal here - the enumerable object does the bookkeeping of the
                // underlying enumerator object. Here we're essentially just dealing with
                // a cursor into the underlying sequence.
            }
        }
    }
}
