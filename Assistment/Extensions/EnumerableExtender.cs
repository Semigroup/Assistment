﻿using System;
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
    }
}
