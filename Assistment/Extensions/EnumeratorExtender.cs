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
    }
}
