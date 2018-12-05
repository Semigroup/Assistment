using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Mathematik;
using Assistment.Extensions;

namespace Assistment.Drawing.Geometries
{
    public static class GeradenErweiterer
    {
        public static void DrawGerade(this Graphics g, Pen Pen, Gerade Gerade, float t1, float t2)
        {
            g.DrawLine(Pen, Gerade.Stelle(t1), Gerade.Stelle(t2));
        }

        public static void DrawGerade(this Graphics g, Pen Pen, Gerade Gerade, Geometrie Grenze)
        {
            IEnumerable<float> ts = Grenze.Cut(Gerade);
            if (!ts.Empty())
            {
                float minT = ts.Min();
                float maxT = ts.Max();
                if (minT < maxT)
                    g.DrawGerade(Pen, Gerade, minT, maxT);
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
