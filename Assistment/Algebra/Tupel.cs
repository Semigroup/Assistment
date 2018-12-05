using System.Collections.Generic;

namespace Assistment.Algebra
{
    public class Tupel<A, B> : Abbildung<A, B>
    {
        public A Preimage;
        public B Image;

        public Tupel(A Preimage, B Image)
        {
            this.Preimage = Preimage;
            this.Image = Image;
        }
        public Tupel()
        {

        }

        public override B Get(A Preimage)
        {
            if (this.Preimage.Equals(Preimage))
                return Image;
            else
                return default(B);
        }

        public override IEnumerable<A> Support()
        {
            return new A[] { Preimage };
        }
    }
}
