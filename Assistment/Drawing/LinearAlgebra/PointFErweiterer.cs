using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Mathematik;

namespace Assistment.Drawing.LinearAlgebra
{
    public static class PointFErweiterer
    {
        /// <summary>
        /// gibt this + c * b zurück
        /// </summary>
        /// <param name="a"></param>
        /// <param name="c"></param>
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
        /// erstellt einen neuen Vektor, der Summe der beiden Summanden ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF add(this PointF a, float x, float y)
        {
            return new PointF(a.X + x, a.Y + y);
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
        /// Erstellt einen neuen Vektor, indem von diesem b abgezogen wird
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF sub(this PointF a, float x, float y)
        {
            return new PointF(a.X - x, a.Y - y);
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
        /// erstellt einen neuen Vektor, der Produkt der beiden Faktoren ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF mul(this PointF a, PointF c)
        {
            return new PointF(a.X * c.X, a.Y * c.Y);
        }
        /// <summary>
        /// erstellt einen neuen Vektor, der Produkt der beiden Faktoren ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF mul(this PointF a, float x, float y)
        {
            return new PointF(a.X * x, a.Y * y);
        }
        /// <summary>
        /// erstellt einen neuen Vektor, der Produkt der beiden Faktoren ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF mul(this PointF a, SizeF c)
        {
            return new PointF(a.X * c.Width, a.Y * c.Height);
        }
        /// <summary>
        /// erstellt einen neuen Vektor, der Die Komponenten des ersten Vektors durch die Zahl teilt
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF div(this PointF a, float b)
        {
            return new PointF(a.X / b, a.Y / b);
        }
        /// <summary>
        /// erstellt einen neuen Vektor, der Die Komponenten des ersten Vektors durch die jeweiligen Komponenten des zweiten teilt
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF div(this PointF a, PointF b)
        {
            return new PointF(a.X / b.X, a.Y / b.Y);
        }
        /// <summary>
        /// rotiert diesen Vektor um w Grad (in Bogenmaß) gegen den Uhrzeigersinn
        /// </summary>
        /// <param name="a"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static PointF rot(this PointF a, double w)
        {
            float c = (float)Math.Cos(w);
            float s = (float)Math.Sin(w);

            return new PointF(c * a.X + s * a.Y,
                                c * a.Y - s * a.X);
        }
        public static SizeF ToSize(this PointF a)
        {
            return new SizeF(a.X, a.Y);
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
        /// <summary>
        /// rundet die beiden Komponenten des Punktes ab
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static PointF Floor(this PointF a)
        {
            return new PointF((float)Math.Floor(a.X), (float)Math.Floor(a.Y));
        }
        /// <summary>
        /// rundet die beiden Komponenten des Punktes auf
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static PointF Ceil(this PointF a)
        {
            return new PointF((float)Math.Ceiling(a.X), (float)Math.Ceiling(a.Y));
        }
        /// <summary>
        /// Lesser or equal than
        /// <para>
        /// true iff beide Komponenten dieses Punktes sind kleiner gleich denen von b
        /// </para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool lqt(this PointF a, PointF b)
        {
            return a.X <= b.X && a.Y <= b.Y;
        }
        /// <summary>
        /// Greater or equal than
        /// <para>
        /// true iff beide Komponenten dieses Punktes sind größer gleich denen von b
        /// </para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool gqt(this PointF a, PointF b)
        {
            return a.X >= b.X && a.Y >= b.Y;
        }
        /// <summary>
        /// gibt zwei Vektoren n1, n2 zurück, sodass
        /// <para>
        /// ||n1|| = 1 = ||n2||
        /// </para>
        /// <para>
        /// SKP(this, n1) = a = SKP(this, n2)
        /// </para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="skp"></param>
        /// <returns></returns>
        public static PointF[] findLeftRight(this PointF a, float skp)
        {
            PointF[] result = new PointF[2];
            float norm = a.norm();
            //c = (a, b)
            PointF c = a.div(norm);
            float d = skp / norm;
            //finde: <n,c> = skp

            float disk = 1 - d * d;
            if (disk >= 0)
            {
                if (Math.Abs(c.Y) > Math.Abs(c.X))
                {
                    float ad = c.X * d;
                    float bd = c.Y * FastMath.Sqrt(disk);
                    result[0].X = ad + bd;
                    result[1].X = ad - bd;
                    for (int i = 0; i < result.Length; i++)
                        result[i].Y = (d - c.X * result[i].X) / c.Y;
                }
                else if (!c.X.isZero())
                {
                    float bd = c.Y * d;
                    float ad = c.X * FastMath.Sqrt(disk);
                    result[0].Y = bd + ad;
                    result[1].Y = bd - ad;
                    for (int i = 0; i < result.Length; i++)
                        result[i].X = (d - c.Y * result[i].Y) / c.X;
                }
            }
            return result;
        }
        /// <summary>
        /// Gibt zurück, ob a und b parallel sind.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Parallel(this PointF a, PointF b)
        {
            return (a.X * b.Y - a.Y * b.X).isZero();
        }
        /// <summary>
        /// Atan2(Y, X)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double atan(this PointF a)
        {
            return Math.Atan2(a.Y, a.X);
        }
        /// <summary>
        /// gibt den abstand zwischen a und b wieder
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float dist(this PointF a, PointF b)
        {
            return a.sub(b).norm();
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

        public static Color flat(this Color color)
        {
            return Color.FromArgb(128, color);
        }
        public static Color flat(this Color color, int Alpha)
        {
            return Color.FromArgb(Alpha, color);
        }

        public static SolidBrush ToBrush(this Color Color)
        {
            return new SolidBrush(Color);
        }

        public static Color[] Flat(this Color[] Colors, int Alpha)
        {
            Color[] c = new Color[Colors.Length];
            for (int i = 0; i < Colors.Length; i++)
                c[i] = Color.FromArgb(Alpha, Colors[i]);
            return c;
        }
    }
    public static class SizeFErweiterer
    {
        /// <summary>
        /// erstellt einen neuen Vektor, der Produkt der beiden Faktoren ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SizeF mul(this SizeF a, float c)
        {
            return new SizeF(a.Width * c, a.Height * c);
        }
        /// <summary>
        /// erstellt einen neuen Vektor, der Produkt der beiden Faktoren ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SizeF mul(this SizeF a, float c, float d)
        {
            return new SizeF(a.Width * c, a.Height * d);
        }
    }
    public static class PointErweiterer
    {
        /// <summary>
        /// Erstellt einen neuen Vektor, indem von diesem b abgezogen wird
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point sub(this Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
        /// <summary>
        /// gibt das SKP dieses Vektors mit sich selber zurück
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int normSquared(this Point a)
        {
            return a.X * a.X + a.Y * a.Y;
        }
        /// <summary>
        /// gibt die Norm dieses Vektors zurück
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float norm(this Point a)
        {
            return (float)Math.Sqrt(a.normSquared());
        }
        /// <summary>
        /// gibt den abstand zwischen a und b wieder
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float dist(this Point a, Point b)
        {
            return a.sub(b).norm();
        }
    }

    public static class RectangleFErweiterer
    {
        public static PointF Center(this RectangleF Rectangle)
        {
            return new PointF(Rectangle.Left + Rectangle.Right, Rectangle.Top + Rectangle.Bottom).div(2);
        }
    }
}
