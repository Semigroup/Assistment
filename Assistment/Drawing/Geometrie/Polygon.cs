using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;

namespace Assistment.Drawing.Geometrie
{
    public class Polygon : IEnumerable<PointF>
    {
        public PointF[] punkte;

        public Polygon(int n)
        {
            this.punkte = new PointF[n];
        }
        /// <summary>
        /// setzt einen Pointer auf das übergebene array
        /// </summary>
        /// <param name="punkte"></param>
        public Polygon(PointF[] punkte)
        {
            this.punkte = punkte;
        }
        /// <summary>
        /// setzt einen Pointer auf das übergebene array
        /// </summary>
        /// <param name="punkte"></param>
        public Polygon(IEnumerable<PointF> punkte)
        {
            this.punkte = punkte.ToArray();
        }

        public PointF this[int index]
        {
            get { return punkte[index]; }
            set { punkte[index] = value; }
        }

        /// <summary>
        /// Schließt dieses Polygon im Sinne von
        /// <para>
        /// punkte[punkte.Length - 1] = punkte[0];
        /// </para>
        /// </summary>
        public void Close()
        {
            punkte[punkte.Length - 1] = punkte[0];
        }

        public Polygon Map(FlachenFunktion<PointF> Flache)
        {
            PointF[] P = punkte.Map(p => Flache(p.X, p.Y)).ToArray();
            return new Polygon(P);
        }

        public Polygon Clone()
        {
            return new Polygon((PointF[])punkte.Clone());
        }

        /// <summary>
        /// R^2 operiert auf der Menge aller Polygone durch Translation.
        /// </summary>
        /// <param name="Polygon"></param>
        /// <param name="Vektor"></param>
        /// <returns></returns>
        public static Polygon operator +(Polygon Polygon, PointF Vektor)
        {
            return new Polygon(Polygon.Map(P => P.add(Vektor)));
        }

        /// <summary>
        /// R^2 operiert auf der Menge aller Polygone auch durch Skalierungen.
        /// </summary>
        /// <param name="Polygon"></param>
        /// <param name="Scale"></param>
        /// <returns></returns>
        public static Polygon operator *(Polygon Polygon, PointF Scale)
        {
            return new Polygon(Polygon.Map(P => P.mul(Scale)));
        }

        /// <summary>
        /// R^2 operiert auf der Menge aller Polygone auch durch Skalierungen.
        /// </summary>
        /// <param name="Polygon"></param>
        /// <param name="Scale"></param>
        /// <returns></returns>
        public static Polygon operator *(Polygon Polygon, SizeF Scale)
        {
            return new Polygon(Polygon.Map(P => P.mul(Scale)));
        }

        /// <summary>
        /// R operiert auf der Menge aller Polygone durch Skalierungen.
        /// </summary>
        /// <param name="Polygon"></param>
        /// <param name="Scale"></param>
        /// <returns></returns>
        public static Polygon operator *(Polygon Polygon, float Scale)
        {
            return new Polygon(Polygon.Map(P => P.mul(Scale)));
        }

        /// <summary>
        /// S^1 operiert auf der Menge aller Polygone durch Drehungen gegen den Uhrzeigersinn
        /// </summary>
        /// <param name="Polygon"></param>
        /// <param name="Winkel">beliebige Zahl (normalerweise zwischen 0 und 2Pi)</param>
        /// <returns></returns>
        public static Polygon operator ^(Polygon Polygon, float Winkel)
        {
            return new Polygon(Polygon.Map(P => P.rot(Winkel)));
        }

        /// <summary>
        /// Erzeugt ein regelmäßiges n-Eck
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Polygon RegelPoly(int n)
        {
            Polygon p = new Polygon(n + 1);
            for (int i = 0; i < n; i++)
            {
                double t = i * Math.PI * 2 / n;
                p.punkte[i] = new PointF((float)Math.Cos(t), (float)Math.Sin(t));
            }
            p.Close();
            return p;
        }

        public IEnumerator<PointF> GetEnumerator()
        {
            return ((IEnumerable<PointF>)punkte).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
