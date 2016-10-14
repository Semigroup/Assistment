using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Extensions;

namespace Assistment.Algebra
{
    public abstract class Abbildung<A, B> : IEnumerable<B>
    {
        public B this[A index]
        {
            get { return Get(index); }
        }

        public abstract B Get(A Preimage);
        public abstract IEnumerable<A> Support();

        public IEnumerator<B> GetEnumerator()
        {
            return Support().Map(x => Get(x)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            string s = "";
            foreach (var item in Support())
            {
                s += item + " -> " + Get(item) + "\r\n";
            }
            return s;
        }

        public string ToDataString()
        {
            string s = "";
            foreach (var item in Support())
            {
                B b = Get(item);
                if (!b.Equals(default(B)))
                    s += b + " " + item + " ";
            }
            return s;
        }
        public string ToShortString()
        {
            string s = "";
            foreach (var item in Support())
            {
                B b = Get(item);
                if (!b.Equals(default(B)))
                    s += b + " " + item + ", ";
            }
            return s;
        }

        private class Konkatenation<C> : Abbildung<A, C>
        {
            private Abbildung<A, B> Abbildung;
            private Func<B, C> Funktion;

            public Konkatenation(Abbildung<A, B> Abbildung, Func<B, C> Funktion)
            {
                this.Abbildung = Abbildung;
                this.Funktion = Funktion;
            }

            public override C Get(A Preimage)
            {
                return Funktion(Abbildung.Get(Preimage));
            }

            public override IEnumerable<A> Support()
            {
                return Abbildung.Support();
            }
        }
        public Abbildung<A, C> Map<C>(Func<B,C> Funktion) 
        {
            return new Konkatenation<C>(this, Funktion);
        }
        private class Konkatenation2<C> : Abbildung<A, C>
        {
            private Abbildung<A, B> Abbildung;
            private Func<A, B, C> Funktion;

            public Konkatenation2(Abbildung<A, B> Abbildung, Func<A,B, C> Funktion)
            {
                this.Abbildung = Abbildung;
                this.Funktion = Funktion;
            }

            public override C Get(A Preimage)
            {
                return Funktion(Preimage, Abbildung.Get(Preimage));
            }

            public override IEnumerable<A> Support()
            {
                return Abbildung.Support();
            }
        }
        public Abbildung<A, C> Map<C>(Func<A, B, C> Funktion)
        {
            return new Konkatenation2<C>(this, Funktion);
        }
    }
    public interface Operiert<in A, in B>
    {
        void Add(A Preimage, B Wert);
    }

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
