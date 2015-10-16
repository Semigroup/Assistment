using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Mathematik;
using Assistment.Extensions;

namespace Assistment.Drawing.Geometrie
{
    public class Gerade : Geometrie, IComparable<Gerade>
    {
        public PointF Aufpunkt;
        public PointF Richtungsvektor;

        public Gerade(PointF Aufpunkt, PointF Richtungsvektor)
            : base()
        {
            this.Aufpunkt = Aufpunkt;
            this.Richtungsvektor = Richtungsvektor;
        }

        public Gerade(float x, float y, float dx, float dy)
        {
            this.Aufpunkt = new PointF(x, y);
            this.Richtungsvektor = new PointF(dx, dy);
        }
        /// <summary>
        /// gibt Aufpunkt + t * Richtungsvektor zurück
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public PointF Stelle(float t)
        {
            return Aufpunkt.saxpy(t, Richtungsvektor);
        }

        public override IEnumerable<float> Cut(PointF Aufpunkt, PointF Richtungsvektor)
        {
            //(A,B) = this, (C,D) = params
            //A + B * t = C + D * s
            //A - C = (D|B) * (s, -t)
            //(s, -t) = (D|B)^-1 (A-C)

            Matrix2 m = new Matrix2(Richtungsvektor, this.Richtungsvektor);
            if (m.Determinante().isZero())
                return new float[0];

            PointF st = this.Aufpunkt.sub(Aufpunkt) / m;
            return new float[] { st.X };
        }

        public override IEnumerable<PointF> Samples(int Number)
        {
            PointF[] S = new PointF[Number];
            for (int i = 0; i < Number; i++)
                S[i] = Aufpunkt.add(Richtungsvektor.mul(i));
            return S;
        }

        public override Geometrie Clone()
        {
            return new Gerade(Aufpunkt, Richtungsvektor);
        }

        public override Geometrie ScaleLocal(PointF ScalingFactor)
        {
            Aufpunkt = Aufpunkt.mul(ScalingFactor);
            Richtungsvektor = Richtungsvektor.mul(ScalingFactor);
            return this;
        }

        public override Geometrie TranslateLocal(PointF TranslatingVector)
        {
            Aufpunkt = Aufpunkt.add(TranslatingVector);
            return this;
        }

        public override Geometrie RotateLocal(double RotatingAngle)
        {
            Aufpunkt = Aufpunkt.rot(RotatingAngle);
            Richtungsvektor = Richtungsvektor.rot(RotatingAngle);
            return this;
        }

        public override Geometrie MirroLocal(PointF MirroringAxis)
        {
            throw new NotImplementedException();
        }

        public bool Parallel(Gerade Gerade)
        {
            return Gerade.Richtungsvektor.Parallel(Richtungsvektor);
        }
        /// <summary>
        /// true iff Punkt liegt auf der Gerade
        /// </summary>
        /// <param name="Punkt"></param>
        /// <returns></returns>
        public bool Hat(PointF Punkt)
        {
            return Aufpunkt.sub(Punkt).Parallel(Richtungsvektor);
        }

        public override IEnumerable<Gerade> Tangents(PointF Aufpunkt)
        {
            if (Hat(Aufpunkt))
                return new Gerade[] { (Gerade)Clone() };
            else
                return new Gerade[0];
        }

        public int CompareTo(Gerade other)
        {
            double d = this.Richtungsvektor.atan() - other.Richtungsvektor.atan();
            if (d > 0)
                return 1;
            else if (d < 0)
                return -1;
            else
                return 0;
        }
        public override string ToString()
        {
            return Aufpunkt + " + t * " + Richtungsvektor;
        }
    }
    public static class GeradenErweiterer
    {
        public static void DrawGerade(this Graphics g, Pen Pen, Gerade Gerade, float t1, float t2)
        {
            g.DrawLine(Pen, Gerade.Stelle(t1), Gerade.Stelle(t2));
        }

        public static void DrawGerade(this Graphics g, Pen Pen, Gerade Gerade, Polygon Grenze)
        {
            IEnumerable<float> ts = Grenze.Cut(Gerade);
            IEnumerator<float> en = ts.GetEnumerator();
            if (en.MoveNext())
            {
                float alt = en.Current;
                while (en.MoveNext())
                {
                    float neu = en.Current;
                    g.DrawGerade(Pen, Gerade, alt, neu);
                    alt = neu;
                }
            }
        }
        public static void DrawStrahl(this Graphics g, Pen Pen, Gerade Gerade, Polygon Grenze)
        {
            float t = Grenze.Cut(Gerade).Min(float.Epsilon);
            g.DrawGerade(Pen, Gerade, 0, t);
        }

        public static void DrawString(this Graphics g, string s, Font font, Brush brush, Gerade Gerade, float stelle)
        {
            float f = FastMath.Grad(Gerade.Richtungsvektor.atan());
            PointF P = Gerade.Stelle(stelle);
            g.TranslateTransform(P.X, P.Y);
            g.RotateTransform(f);
            g.DrawString(s, font, brush, new PointF());
            g.RotateTransform(-f);
            g.TranslateTransform(-P.X, -P.Y);
        }
        public static void DrawAlphaString(this Graphics g, string s, Font font, Brush brush, Gerade Gerade, float stelle, int alpha)
        {
            float f = FastMath.Grad(Gerade.Richtungsvektor.atan());
            PointF P = Gerade.Stelle(stelle);
            g.TranslateTransform(P.X, P.Y);
            g.RotateTransform(f);
            g.DrawAlphaString(s, font, brush, new PointF(), alpha);
            g.RotateTransform(-f);
            g.TranslateTransform(-P.X, -P.Y);
        }
        /// <summary>
        /// Malt den Kegel, der sich links von A befindet.
        /// <para>Ursprung ist Schnittpunkt von A und B.</para>
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public static void FillDrawCone(this Graphics g, Brush Brush, Pen Pen, Gerade A, Gerade B, PointF Radius)
        {
            IEnumerable<float> ts = A.Cut(B);
            if (ts.Empty())
                return;
            PointF Schnitt = B.Stelle(ts.First());
            RectangleF rf = new RectangleF(Schnitt.sub(Radius), Radius.mul(2).ToSize());
            float startAngle = FastMath.Grad(A.Richtungsvektor.atan());
            float sweepingAngle = FastMath.Grad(B.Richtungsvektor.atan()) - startAngle;
            if (Brush != null)
                g.FillPie(Brush, rf.X, rf.Y, rf.Width, rf.Height, startAngle, sweepingAngle);
            if (Pen != null)
                g.DrawPie(Pen, rf, startAngle, sweepingAngle);
        }
        /// <summary>
        /// Malt den Kegel, der sich links von A befindet.
        /// <para>Ursprung ist Schnittpunkt von A und B.</para>
        /// </summary>
        /// <param name="g"></param>
        /// <param name="Brush"></param>
        /// <param name="Pen"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="Radius"></param>
        public static void FillDrawCone(this Graphics g, Brush Brush, Pen Pen, Gerade A, Gerade B, float Radius)
        {
            FillDrawCone(g, Brush, Pen, A, B, new PointF(Radius, Radius));
        }
    }
}
