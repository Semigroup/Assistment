using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Drawing.LinearAlgebra
{
    public static class PointFErweiterer
    {
        /// <summary>
        /// erstellt einen neuen Vektor, der Summe der beiden Summanden ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF saxpy(this PointF a, float c, PointF b)
        {
            return new PointF(a.X + c * b.X, a.Y + c * b.Y);
        }
        /// <summary>
        /// erstellt einen neuen Vektor, der Summe der beiden Summanden ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF add(this PointF a, PointF b)
        {
            return new PointF(a.X + b.X, a.Y + b.Y);
        }
        /// <summary>
        /// Erstellt einen neuen Vektor, indem von diesem b abgezogen wird
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF sub(this PointF a, PointF b)
        {
            return new PointF(a.X - b.X, a.Y - b.Y);
        }
        /// <summary>
        /// erstellt einen neuen Vektor, der Produkt der beiden Faktoren ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF mul(this PointF a, float c)
        {
            return new PointF(a.X * c, a.Y * c);
        }
        /// <summary>
        /// gibt das SKP dieses Vektors mit sich selber zurück
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float normSquared(this PointF a)
        {
            return a.X * a.X + a.Y * a.Y;
        }
        /// <summary>
        /// gibt die Norm dieses Vektors zurück
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float norm(this PointF a)
        {
            return (float)Math.Sqrt(a.normSquared());
        }
        /// <summary>
        /// gibt das SKP dieses Punktes mit b zurück
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float SKP(this PointF a, PointF b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        /// <summary>
        /// gibt this * (1-t) + t * b zurück
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static PointF tween(this PointF a, PointF b, float t)
        {
            return new PointF(a.X * (1 - t) + t * b.X,
                                a.Y * (1 - t) + t * b.Y);
        }
        /// <summary>
        /// gibt this * (1-t) - t * b zurück
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static PointF negTween(this PointF a, PointF b, float t)
        {
            return new PointF(a.X * (1 - t) - t * b.X,
                                a.Y * (1 - t) - t * b.Y);
        }
        /// <summary>
        /// (Y, -X)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static PointF linksOrtho(this PointF a)
        {
            return new PointF(a.Y, -a.X);
        }
        public static PointF normalize(this PointF a)
        {
            return a.mul(1 / a.norm());
        }
    }
    public static class ColorErweiterer
    {
        /// <summary>
        /// erzeugt this * (1 - t) + t * b 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Color tween(this Color a, Color b, float t)
        {
            return Color.FromArgb((int)(a.A * (1 - t) + t * b.A),
                                (int)(a.R * (1 - t) + t * b.R),
                                (int)(a.G * (1 - t) + t * b.G),
                                (int)(a.B * (1 - t) + t * b.B));
        }
    }
}
