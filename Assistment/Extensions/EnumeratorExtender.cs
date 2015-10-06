using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Extensions
{
    public static class EnumeratorExtender
    {
        public delegate B Function<A, B>(A X);

        public class IEnumeratorMap<A, B> : IEnumerator<B>
        {
            public Function<A, B> Function;
            public IEnumerator<A> Domain;

            public IEnumeratorMap(Function<A, B> Function, IEnumerator<A> Domain)
            {
                this.Function = Function;
                this.Domain = Domain;
            }

            public B Current
            {
                get { return Function(Domain.Current); }
            }

            public void Dispose()
            {
                Domain.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return Domain.MoveNext();
            }

            public void Reset()
            {
                Domain.Reset();
            }
        }

        public static IEnumerator<B> Map<A, B>(this IEnumerator<A> Domain, Function<A, B> Function)
        {
            return new IEnumeratorMap<A, B>(Function, Domain);
        }
        public static bool Equals<T>(this IEnumerator<T> Enumerator1, IEnumerator<T> Enumerator2)
        {
            bool equals = true;
            while (Enumerator1.MoveNext() && Enumerator2.MoveNext())
            {
                if ((Enumerator1.Current != null && !Enumerator1.Current.Equals(Enumerator2.Current))
                    || (Enumerator1.Current == null && Enumerator2.Current != null))
                {
                    equals = false;
                    break;
                }
            }
            return equals;
        }

        public static IEnumerator<T> Enumerate<T>(T[,] Array)
        {
            return new Array2Enumerator<T>(Array);
        }

        private class Array2Enumerator<T> : IEnumerator<T>
        {
            int i, j;

            T current;

            T[,] Array;

            public Array2Enumerator(T[,] Array)
            {
                this.Array = Array;
                this.Reset();
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
                get { return current; }
            }

            public bool MoveNext()
            {
                if (i < Array.GetLength(0))
                {
                    if (j < Array.GetLength(1))
                    {
                        current = Array[i, j++];
                        return true;
                    }
                    else
                    {
                        j = 0;
                        i++;
                        return MoveNext();
                    }
                }
                else
                {
                    current = default(T);
                    return false;
                }
            }

            public void Reset()
            {
                i = 0;
                j = 0;
                current = default(T);
            }
        }
    }
}
