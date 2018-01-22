using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Mathematik;

namespace Assistment.Drawing.Geometries
{
    public static class Erweiterer
    {
        public static void DrawPolygon(this Graphics Graphics, Pen Pen, Polygon Polygon)
        {
            Graphics.DrawPolygon(Pen, Polygon.punkte);
        }
        public static void FillPolygon(this Graphics Graphics, Brush Brush, Polygon Polygon)
        {
            Graphics.FillPolygon(Brush, Polygon.punkte);
        }
        public static void DrawLines(this Graphics Graphics, Pen Pen, IEnumerable<PointF> Points)
        {
            IEnumerator<PointF> en = Points.GetEnumerator();
            en.MoveNext();
            PointF old = en.Current;
            while (en.MoveNext())
            {
                Graphics.DrawLine(Pen, old, en.Current);
                old = en.Current;
            }
        }

        public static void DrawRectangle(this Graphics g, Pen Pen, RectangleF RectangleF)
        {
            g.DrawRectangle(Pen, RectangleF.X, RectangleF.Y, RectangleF.Width, RectangleF.Height);
        }

        public static void FillDrawWegAufOrientierbarerWeg(this Graphics g, Brush Brush, Pen Pen, Weg y, OrientierbarerWeg oy, int samples)
        {
            PointF[] Samples = new PointF[samples];
            float L = y(1).X - y(0).X;

            if (Math.Abs(L) <= float.Epsilon)
                throw new NotImplementedException();

            for (int i = 0; i < samples - 1; i++)
            {
                float t = i / (samples - 1f);
                PointF v = y(t);
                float x = v.X / L;
                Samples[i] = oy.weg(x).saxpy(v.Y, oy.normale(x));
                //Samples[i] = oy.weg(t).add(-v.X * n.Y + v.Y * n.X, v.X * n.X + v.Y * n.Y); n = oy.normale(t)
            }
            Samples[samples - 1] = Samples[0];
            if (Brush != null)
                g.FillPolygon(Brush, Samples);
            if (Pen != null)
                g.DrawPolygon(Pen, Samples);
        }
    }

    public static class WegErweiterer
    {
        /// <summary>
        /// Generiert einen neuen Weg, der den alten im Bogenmaß gegen den Uhrzeigersinn dreht.
        /// </summary>
        /// <param name="weg"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Weg rot(this Weg weg, double w)
        {
            return t => weg(t).rot(w);
        }
        public static Weg scale(this Weg weg, float c)
        {
            return t => weg(t).mul(c);
        }
        public static Weg Concat(this Weg Weg1, Weg Weg2)
        {
            PointF d = Weg1(1).sub(Weg2(0));
            return t => (t <= 0.5f) ? Weg1(2 * t) : Weg2(2 * t - 1).add(d);
        }
        public static Weg Frequent(this Weg Weg, int numberOfRepetitions)
            => Frequent(Weg, numberOfRepetitions, 1f / numberOfRepetitions);
        public static Weg Frequent(this Weg Weg, int numberOfRepetitions, float lengthOfUnit)
        {
            return t =>
            {
                int i = (int)(t * numberOfRepetitions);
                if (i == numberOfRepetitions)
                    i--;
                float t0 = t * numberOfRepetitions - i;
                PointF P = Weg(t0);
                return new PointF(P.X / numberOfRepetitions + lengthOfUnit * i, P.Y);
            };
        }
    }

    public static class FlacheErweiterer
    {
        public static FlachenFunktion<PointF> mul(this FlachenFunktion<PointF> flache, float c)
        {
            return (u, v) => flache(u, v).mul(c);
        }

        public static RandFunktion<T> Rand<T>(this FlachenFunktion<T> flache, float u, float v)
        {
            float uv = (u + v);
            float uv2 = 2 * uv;
            float u2v = uv + u;

            float u1 = u / uv / 2;
            float v1 = v / uv / 2;
            float u2 = u1 + 0.5f;
            float v2 = v1 + 0.5f;

            return t =>
            {
                float f = uv2 * t;
                if (f < u)
                    return flache(f / u, 0);
                else if (f < uv)
                    return flache(1, (f - u) / v);
                else if (t < u2v)
                    return flache((u2v - f) / u, 1);
                else
                    return flache(0, (uv2 - f) / v);
            };
        }
        /// <summary>
        /// Erzeugt die Gerade: flache(cU * t / norm, cV * t / norm) wobei t von 0 nach 1 laufen soll
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flache"></param>
        /// <param name="cU"></param>
        /// <param name="cV"></param>
        /// <returns></returns>
        public static RandFunktion<T> Gerade<T>(this FlachenFunktion<T> flache, float cU, float cV)
        {
            float norm = FastMath.Sqrt(cU * cU + cV * cV);
            norm /= cU < cV ? cV : cU;
            cU /= norm;
            cV /= norm;

            return t => flache(cU * t, cV * t);
        }

        public static T[] Samples<T>(this RandFunktion<T> rand, int samples)
        {
            T[] array = new T[samples];
            for (int i = 0; i < samples; i++)
                array[i] = rand(i / (samples - 1f));
            return array;
        }
    }
}
