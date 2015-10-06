using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Drawing.Geometrie
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
    }

    public static class WegErweiterer
    {
        /// <summary>
        /// Generiert einen neuen Weg, der den alten im Bogenmaß gegen den Uhrzeigersinn dreht.
        /// </summary>
        /// <param name="weg"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static Weg rot(this Weg weg, float w)
        {
            return t => weg(t).rot(w);
        }

        public static Weg scale(this Weg weg, float c)
        {
            return t => weg(t).mul(c);
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
            float u2v = uv+ u;
            
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

        public static T[] Samples<T>(this RandFunktion<T> rand, int samples)
        {
            T[] array = new T[samples];
            for (int i = 0; i < samples; i++)
                array[i] = rand(i / (samples - 1f));
            return array;
        }
    }
}
