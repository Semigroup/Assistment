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
    }
}
