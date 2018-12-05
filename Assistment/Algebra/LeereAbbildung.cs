using System.Collections.Generic;

namespace Assistment.Algebra
{
    public class LeereAbbildung<A, B> : Abbildung<A, B>
    {
        public override B Get(A Preimage)
        {
            return default(B);
        }

        public override IEnumerable<A> Support()
        {
            return new A[0];
        }
    }
}
