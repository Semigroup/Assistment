using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Extensions
{
    public static class EnumerableExtender
    {
        public class LambdaEnumerator<T> : IEnumerator<T>
        {
            private Func<T> next;
            private Func<bool> valid;
            private Action reset;
            private T current = default(T);

            /// <summary>
            /// while(valid())
            /// <para> return next()</para>
            /// </summary>
            /// <param name="next">gibt bei jedem aufruf das nächste element der sequenz an</param>
            /// <param name="valid">gibt bei jedem aufruf an, ob die sequenz noch valid ist</param>
            public LambdaEnumerator(Func<T> next, Func<bool> valid)
            {
                this.next = next;
                this.valid = valid;
                this.reset = null;
            }
            /// <summary>
            /// while(valid())
            /// <para> return next()</para>
            /// </summary>
            /// <param name="next">gibt bei jedem aufruf das nächste element der sequenz an</param>
            /// <param name="valid">gibt bei jedem aufruf an, ob die sequenz noch valid ist</param>
            public LambdaEnumerator(Func<T> next, Func<bool> valid, Action reset)
            {
                this.next = next;
                this.valid = valid;
                this.reset = reset;
            }

            public T Current
            {
                get { return current; }
            }
            public void Dispose()
            {
            }
            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }
            public bool MoveNext()
            {
                if (valid())
                {
                    current = next();
                    return true;
                }
                else
                    return false;
            }
            public void Reset()
            {
                reset();
            }
        }

        public class DependingEnumerable<T> : IEnumerable<T>
        {
            private IEnumerator<T> Enumerator;

            public DependingEnumerable(IEnumerator<T> Enumerator)
            {
                this.Enumerator = Enumerator;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return Enumerator;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return Enumerator;
            }
        }

        public class IEnumerableMap<A, B> : IEnumerable<B>
        {
            public EnumeratorExtender.Function<A, B> Function;
            public IEnumerable<A> Domain;

            public IEnumerableMap(EnumeratorExtender.Function<A, B> Function, IEnumerable<A> Domain)
            {
                this.Function = Function;
                this.Domain = Domain;
            }

            public IEnumerator<B> GetEnumerator()
            {
                return Domain.GetEnumerator().Map<A, B>(Function);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        public static IEnumerable<B> Map<A, B>(this IEnumerable<A> Domain, EnumeratorExtender.Function<A, B> Function)
        {
            return new IEnumerableMap<A, B>(Function, Domain);
        }
        public static bool Equals<T>(this IEnumerable<T> Enumerable1, IEnumerable<T> Enumerable2)
        {
            return Enumerable1.GetEnumerator().Equals<T>(Enumerable2.GetEnumerator());
        }

        public static IEnumerable<T> Enumerate<T>(T[,] Array)
        {
            return new Array2Enumerable<T>(Array);
        }

        private class Array2Enumerable<T> : IEnumerable<T>
        {
            T[,] Array;

            public Array2Enumerable(T[,] Array)
            {
                this.Array = Array;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return EnumeratorExtender.Enumerate(Array);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static string SumText(this IEnumerable<object> text)
        {
            StringBuilder sb = new StringBuilder();
            bool neu = true;
            foreach (var item in text)
            {
                if (neu)
                    neu = false;
                else
                    sb.AppendLine();
                sb.Append(item);
            }
            return sb.ToString();
        }

        public static string SumText(this IEnumerable<object> text, string Separator)
        {
            StringBuilder sb = new StringBuilder();
            bool neu = true;
            foreach (var item in text)
            {
                if (neu)
                    neu = false;
                else
                    sb.Append(Separator);
                sb.Append(item);
            }
            return sb.ToString();
        }
        /// <summary>
        /// gibt das kleinste float zurück, das größer gleich minimum ist.
        /// <para>falls ein solches nicht existiert, wird float.MaxValue zurückgegeben.</para>
        /// </summary>
        /// <param name="floats"></param>
        /// <returns></returns>
        public static float Min(this IEnumerable<float> floats, float minimum)
        {
            float f = float.MaxValue;
            foreach (var item in floats)
                if (item >= minimum && item < f)
                    f = item;
            return f;
        }

        public static string Print<T>(this IEnumerable<T> Liste)
        {
            StringBuilder sb = new StringBuilder();
            bool neu = true;
            foreach (var item in Liste)
            {
                if (neu)
                    neu = false;
                else
                    sb.AppendLine();
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }
        public static string Print<T>(this T[] Array)
        {
            StringBuilder sb = new StringBuilder();
            bool neu = true;
            foreach (var item in Array)
            {
                if (neu)
                    neu = false;
                else
                    sb.AppendLine();
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }
        public static string Print<T>(this T[,] Array)
        {
            StringBuilder sb = new StringBuilder();
            bool neueZeile = false;
            bool neueSpalte = false;
            for (int i = 0; i < Array.GetLength(0); i++)
            {
                if (neueZeile)
                    sb.AppendLine();
                else
                    neueZeile = true;
                for (int j = 0; j < Array.GetLength(1); j++)
                {
                    if (neueSpalte)
                        sb.Append("\t    ");
                    else
                        neueSpalte = true;
                    sb.Append(Array[i, j].ToString());
                }
                neueSpalte = false;
            }
            return sb.ToString();
        }

        public static void SelfMap<T>(this T[] array, EnumeratorExtender.Function<T, T> function)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = function(array[i]);
        }
        public static void CountMap<T>(this T[] array, EnumeratorExtender.Function<int, T> function)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = function(i);
        }

        public static bool Empty<T>(this IEnumerable<T> Enumerable)
        {
            return !Enumerable.GetEnumerator().MoveNext();
        }
        /// <summary>
        /// Maximiert die gegebene OptimiertFunktion.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Enumerable"></param>
        /// <param name="OptimierFunktion"></param>
        /// <returns></returns>
        public static T Optim<T>(this IEnumerable<T> Enumerable, Func<T, float> OptimierFunktion)
        {
            T m = default(T);
            float f = float.MinValue;
            foreach (var item in Enumerable)
            {
                float o = OptimierFunktion(item);
                if (o > f)
                {
                    f = o;
                    m = item;
                }
            }
            return m;
        }

        public static IEnumerable<T> Drop<T>(this IEnumerable<T> Enumerable, int number)
        {
            IEnumerator<T> t = Enumerable.GetEnumerator();
            for (int i = 0; i < number; i++)
                t.MoveNext();
            return new DependingEnumerable<T>(t);
        }

        /// <summary>
        /// Nimmt { Enumerable[from], ..., Enumerable[to - 1] }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Array"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static IEnumerable<T> FromTo<T>(this IEnumerable<T> Enumerable, int from, int to)
        {
            return Enumerable.Skip(from).Take(to - from);
        }

        /// <summary>
        /// Nimmt { Enumerable[from], ..., Enumerable[to - 1] }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Array"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static IEnumerable<T> FromTo<T>(this T[] Array, int from, int to)
        {
            if (from < to)
                return Array.SubSequence(from, to - 1, 1);
            else
                return Array.SubSequence(from, to - 1, -1);
        }
        /// <summary>
        /// gibt Array[start], Array[start + 1 * speed], Array[start + 2 * speed], ... wieder
        /// <para>
        /// bis start + i * speed > min(end, Array.Length), falls speed positiv
        /// </para>
        /// <para>
        /// oder bis start + i * speed echt kleiner max(end, 0), falls speed negativ
        /// </para>
        /// </summary>
        /// <param name="Array"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static IEnumerable<T> SubSequence<T>(this T[] Array, int start, int end, int speed)
        {
            if (speed > 0)
                end = Math.Min(end, Array.Length - 1);
            else if (speed < 0)
                end = Math.Max(end, 0);

            int i = start;

            Func<bool> While;
            if (speed > 0)
                While = () => i <= end;
            else if (speed < 0)
                While = () => i >= end;
            else
            {
                bool b = 0 <= start && start < Array.Length;
                While = () => b;
            }

            return new DependingEnumerable<T>(new LambdaEnumerator<T>(
                () =>
                {
                    T t = Array[i];
                    i += speed;
                    return t;
                },
                While,
                () => { i = start; }
                ));
        }
        /// <summary>
        /// Point = (x,y)
        /// zählt auf: (0,0), ... (x-1,0), (0,1), ... (x-1,1), ... (0, y-1), ... (x-1, y-1)
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        public static IEnumerable<Point> Enumerate(this Point Point)
        {
            Point p = new Point();
            return
                new DependingEnumerable<Point>(
                new LambdaEnumerator<Point>(() =>
            {
                Point q = p;
                p.X++;
                if (p.X == Point.X)
                {
                    p.X = 0;
                    p.Y++;
                }
                return q;
            }, () => p.Y < Point.Y, () => p = new Point()));
        }
    }
}
