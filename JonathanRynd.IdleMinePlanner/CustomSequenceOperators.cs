using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    public static class CustomSequenceOperators
    {
#if UNOPTIMIZED
        private void TripleForEach<T1, T2, T3>(IEnumerable<T1> a1, IEnumerable<T2> a2, IEnumerable<T3> a3, Action<T1, T2, T3> x)
        {
            a3.Zip(a2, (t3, t2) => Tuple.Create(t2, t3)).Zip(a1, (t23, t1) => Tuple.Create(t1, t23.Item1, t23.Item2)).ToList().ForEach(z => x(z.Item1, z.Item2, z.Item3));
        }
#else
        public static void TripleForEach<TFirst, TSecond, TThird>(
        IEnumerable<TFirst> first,
        IEnumerable<TSecond> second,
        IEnumerable<TThird> third,
        Action<TFirst, TSecond, TThird> action)
        {
            using (IEnumerator<TFirst> e1 = first.GetEnumerator())
            using (IEnumerator<TSecond> e2 = second.GetEnumerator())
            using (IEnumerator<TThird> e3 = third.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                {
                    action(e1.Current, e2.Current, e3.Current);
                }
            }
        }
#endif
        public static T2 MaxEx<T1,T2>(
            this IEnumerable<T1> sequence,
            Func<T1,T1,bool> comparer,
            Func<T1,T2> selector)
        {
            IEnumerator<T1> e1 = sequence.GetEnumerator();
            e1.MoveNext();
            T1 maxSoFar = e1.Current;
            while (e1.MoveNext())
            {
                if (comparer(maxSoFar, e1.Current))
                    maxSoFar = e1.Current;
            }
            return selector(maxSoFar);
        }
    }
}
