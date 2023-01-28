using System.Drawing;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Drawing.Geometries
{
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
        public static float Length(this Weg weg, int approxPoints)
        {
            PointF[] points = new PointF[approxPoints];
            for (int i = 0; i < approxPoints; i++)
                points[i] = weg(i / (approxPoints - 1f));
            float length = 0;
            for (int i = 0; i < approxPoints - 1; i++)
                length += points[i].dist(points[i + 1]);
            return length;
        }
        public static Weg Normalize(this Weg Weg)
            => t => Weg(t).normalize();
        public static Weg FlipSign(this Weg Weg)
           => t =>
           {
               PointF p = Weg(t);
               return new PointF(-p.X, -p.Y);
           };
        public static Weg LinksOrtho(this Weg Weg)
          => t =>
          {
              PointF p = Weg(t);
              return new PointF(p.Y, -p.X);
          };
    }
}
