using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Extensions
{
    public static class EnumerableExtender
    {
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
                return Domain.GetEnumerator().Map<A,B>(Function);
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

        public static string Sum(this IEnumerable<string> text)
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

        public static string Sum(this IEnumerable<string> text, string Separator)
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

        public static void SelfMap<T>(this T[] array, EnumeratorExtender.Function<T,T> function)
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
    }
}
