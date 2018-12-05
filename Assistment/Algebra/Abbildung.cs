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
}
