using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.Geometries.Extensions;
using Assistment.Extensions;
using Assistment.Mathematik;

namespace Assistment.Drawing.Geometries
{
    public class Polygon : Geometrie, IEnumerable<PointF>
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
        /// kopiert die werte
        /// </summary>
        /// <param name="punkte"></param>
        public Polygon(params float[] werte)
        {
            this.punkte = new PointF[werte.Length / 2];
            for (int i = 0; i < punkte.Length; i++)
                punkte[i] = new PointF(werte[2 * i], werte[2 * i + 1]);
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

        public override Geometrie Clone()
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
        /// mit Radius 1
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

        public override IEnumerable<float> Cut(PointF Aufpunkt, PointF Richtungsvektor)
        {
            List<float> ts = new List<float>();
            /*
             * A + t * v = P0 + (P1 - P0) * s
             * m = (P1 - P0 | v)
             * m^-1 * (A - P0) = (s, -t)
             */
            Matrix2 m = new Matrix2(new PointF(), Richtungsvektor);
            for (int i = 0; i < punkte.Length - 1; i++)
            {
                PointF b = Aufpunkt.sub(punkte[i]);
                m.Spalte1 = punkte[i + 1].sub(punkte[i]);
                PointF st = b / m;
                if (0 <= st.X && st.X < 1)
                    ts.Add(-st.Y);
            }
            return ts;
        }

        public override IEnumerable<PointF> Samples(int Number)
        {
            return punkte;
        }

        public override Geometrie ScaleLocal(PointF ScalingFactor)
        {
            punkte.SelfMap(p => p.mul(ScalingFactor));
            return this;
        }

        public override Geometrie TranslateLocal(PointF TranslatingVector)
        {
            punkte.SelfMap(p => p.add(TranslatingVector));
            return this;
        }

        public override Geometrie RotateLocal(double RotatingAngle)
        {
            punkte.SelfMap(p => p.rot(RotatingAngle));
            return this;
        }

        public static implicit operator Polygon(RectangleF Rect)
        {
            return new Polygon(Rect.Left, Rect.Top,
                                Rect.Right, Rect.Top,
                                Rect.Right, Rect.Bottom,
                                Rect.Left, Rect.Bottom,
                                Rect.Left, Rect.Top);
        }

        public override IEnumerable<Gerade> Tangents(PointF Aufpunkt)
        {
            List<Gerade> tangs = new List<Gerade>();
            foreach (var item in punkte)
                tangs.Add(new Gerade(item, item.sub(Aufpunkt)));
            return tangs;
        }

        public override Geometrie MirrorLocal(PointF Aufpunkt, PointF RichtungsVektor)
        {
            throw new NotImplementedException();
        }

        public override PointF Lot(PointF Punkt)
        {
            PointF[] lots = new PointF[punkte.Length - 1];
            for (int i = 0; i < punkte.Length - 1; i++)
            {
                PointF v = punkte[i + 1].sub(punkte[i]);
                PointF d = punkte[i].sub(Punkt);
                float t = v.SKP(d) / v.normSquared();
                t = t.Saturate();
                lots[i] = punkte[i].saxpy(t, d);
            }
            return lots.Optim(p => -p.dist(Punkt));
        }

        public Gerade GetKante(int i)
        {
            return new Gerade(punkte[i], punkte[i + 1].sub(punkte[i]));
        }

        public static Polygon Rechteck(RectangleF rf, PointF SampleRate)
        {
            int NX = (int)(rf.Width * SampleRate.X);
            int NY = (int)(rf.Height * SampleRate.Y);
            NX = Math.Max(NX, 1);
            NY = Math.Max(NY, 1);

            Polygon poly = new Polygon(2 * (NX + NY) + 1);

            int i = 0;
            for (int j = 0; j < NX; j++)
                poly[i++] = new PointF(rf.Left + rf.Width * j / NX, rf.Top);
            for (int j = 0; j < NY; j++)
                poly[i++] = new PointF(rf.Right, rf.Top + rf.Height * j / NY);
            for (int j = 0; j < NX; j++)
                poly[i++] = new PointF(rf.Right - rf.Width * j / NX, rf.Bottom);
            for (int j = 0; j < NY; j++)
                poly[i++] = new PointF(rf.Left, rf.Bottom - rf.Width * j / NY);

            poly.Close();

            return poly;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Polygon{");
            foreach (var item in punkte)
                sb.Append("("+item.X + ";"+item.Y +")-");
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
