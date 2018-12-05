using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Drawing.Geometries
{
    public static class GeometrieErweiterer
    {
        public static float Distanz(this Geometrie Geometrie, PointF P)
        {
            return Geometrie.Lot(P).dist(P);
        }

        public static bool HasCutMatching(this Geometrie Geometrie, Gerade Gerade, Predicate<float> match)
        {
            IEnumerable<float> ts = Geometrie.Cut(Gerade);
            foreach (var item in ts)
                if (match(item))
                    return true;
            return false;
        }
        public static bool HasNotNegativeCut(this Geometrie Geometrie, Gerade Gerade)
        {
            IEnumerable<float> ts = Geometrie.Cut(Gerade);
            foreach (var item in ts)
                if (item >= 0)
                    return true;
            return false;
        }

        public static PointF NotNegativeCut(this Geometrie Geometrie, Gerade Gerade)
        {
            List<float> ts = new List<float>();
            foreach (var item in Geometrie.Cut(Gerade))
                if (item > 0)
                    ts.Add(item);
            if (ts.Count == 0)
                return new PointF();
            else
                return Gerade.Stelle(ts.Min());
        }

        public static void DrawGeometry(this Graphics g, Pen Pen, Geometrie Geometrie, int Samples)
        {
            g.DrawPolygon(Pen, Geometrie.Samples(Samples).ToArray());
        }
        public static void FillGeometry(this Graphics g, Brush Brush, Geometrie Geometrie, int Samples, FillMode FillMode)
        {
            g.FillPolygon(Brush, Geometrie.Samples(Samples).ToArray(), FillMode);
        }
        public static void FillDrawGeometry(this Graphics g, Brush Brush, Pen Pen, Geometrie Geometrie, int Samples, FillMode FillMode)
        {
            PointF[] Array = Geometrie.Samples(Samples).ToArray();
            g.FillPolygon(Brush, Array, FillMode);
            g.DrawPolygon(Pen, Array);
        }

        /// <summary>
        /// Bestimmt ob die Geometrie den Point enthält
        /// <para>indem die Anzahl der Schnitte mit einer Random Gerade (Point + t * (1, 0)) geprüft</para>
        /// <para>geht von der Annahme aus, dass es sich um eine offene Umgebung handelt</para>
        /// </summary>
        /// <param name="Geometrie"></param>
        /// <param name="Point"></param>
        /// <returns></returns>
        public static bool ContainsPoint(this Geometrie Geometrie, PointF Point)
        {
            Gerade g = new Gerade(Point, new PointF(1, 0));
            bool drin = false;
            foreach (float cut in Geometrie.Cut(g))
                if (cut < 0)
                    drin = !drin;
            return drin;
        }
    }
}
