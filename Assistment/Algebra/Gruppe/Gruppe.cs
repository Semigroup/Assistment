using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Algebra.Gruppe
{
    public abstract class Gruppe
    {
        /// <summary>
        /// Multipliziert Faktor an diese Gruppe.
        /// </summary>
        /// <param name="Faktor"></param>
        /// <returns></returns>
        public abstract Gruppe MultLocal(Gruppe Faktor);
        /// <summary>
        /// Invertiert diese Gruppe und gibt sie zurück.
        /// </summary>
        /// <returns></returns>
        public abstract Gruppe InvertLocal();
        public abstract Gruppe Clone();
        /// <summary>
        /// Macht diese Gruppe zum Neutralelement und gibt es zurück.
        /// </summary>
        /// <returns></returns>
        public abstract Gruppe NeutralLocal();
        /// <summary>
        /// Produziert eine neue Gruppe, die this * Faktor ist, und gibt diese zurück
        /// </summary>
        /// <param name="Faktor"></param>
        /// <returns></returns>
        public virtual Gruppe Mult(Gruppe Faktor)
        {
            return Clone().MultLocal(Faktor);
        }
        /// <summary>
        /// Klont diese Gruppe; invertiert den Klon und gibt ihn zurück.
        /// </summary>
        /// <returns></returns>
        public virtual Gruppe Invert()
        {
            return Clone().Invert();
        }
        /// <summary>
        /// Gibt das Neutralelement zurück, ohne diese Gruppe zu ändern.
        /// </summary>
        /// <returns></returns>
        public virtual Gruppe Neutral()
        {
            return Clone().Neutral();
        }
        public static Gruppe operator *(Gruppe A, Gruppe B)
        {
            return A.Mult(B);
        }

        public static Gruppe operator ^(Gruppe A, int n)
        {
            if (n == 0)
            {
                return A.Neutral();
            }
            else
            {
                Gruppe G;
                if (n < 0)
                {
                    n *= -1;
                    G = A.Invert();
                }
                else
                {
                    G = A.Clone();
                }
                n--;
                while (n > 0)
                {
                    if (n % 2 == 0)
                    {

                    }
                    else
                    {

                    }
                }
            }
        }
    }

    public struct GruppenSymbol<X>
    {
        public bool Invertiert;
        public X Erzeuger;

        public GruppenSymbol(X Erzeuger, bool Invertiert)
        {
            this.Erzeuger = Erzeuger;
            this.Invertiert = Invertiert;
        }
    }

    /// <summary>
    /// Die Gruppe, die in X frei erzeugt wird.
    /// </summary>
    /// <typeparam name="X"></typeparam>
    public class FreieGruppe<X> : ICollection<GruppenSymbol<X>>, Gruppe
    {
        private List<GruppenSymbol<X>> list = new List<GruppenSymbol<X>>();

        public void Add(X Erzeuger)
        {
            list.Add(new GruppenSymbol<X>(Erzeuger, false));
        }

        public void Add(GruppenSymbol<X> item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(GruppenSymbol<X> item)
        {
            return list.Contains(item);
        }

        public void CopyTo(GruppenSymbol<X>[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(GruppenSymbol<X> item)
        {
            return list.Remove(item);
        }

        public IEnumerator<GruppenSymbol<X>> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
