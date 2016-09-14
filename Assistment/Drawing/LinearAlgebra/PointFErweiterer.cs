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
        /// erstellt einen neuen Vektor, der Die Komponenten des ersten Vektors durch die jeweiligen Komponenten des zweiten teilt
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF div(this PointF a, float x, float y)
        {
            return new PointF(a.X / x, a.Y / y);
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

        /// <summary>
        /// klemmt beide Koordinaten zwischen 0 und 1 ein
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static PointF sat(this PointF a)
        {
            return new PointF(a.X.Saturate(), a.Y.Saturate());
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
        public static Pen ToPen(this Color Color, float width)
        {
            return new Pen(Color, width);
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
        /// Sorgt dafür, dass Neu.Width = Constraintment.Width oder Neu.Height = Constraintment.Height
        /// <para>und Alt.Ratio = Neu.Ratio</para>
        /// <para>Nimmt an, dass Alt, Constraintment > (0,0) </para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="Constraintment"></param>
        /// <returns></returns>
        public static SizeF Constraint(this SizeF Alt, SizeF Constraintment)
        {
            float r = Alt.ratio();
            float w = Constraintment.Width / Alt.Width;
            float h = Constraintment.Height / Alt.Height;
            if (w > h)
            {
                Alt.Height = Constraintment.Height;
                Alt.Width = r * Alt.Height;
            }
            else
            {
                Alt.Width = Constraintment.Width;
                Alt.Height = Alt.Width / r;
            }
            return Alt;
        }

        public static SizeF add(this SizeF a, SizeF c)
        {
            return new SizeF(a.Width + c.Width, a.Height + c.Height);
        }
        public static SizeF add(this SizeF a, float x, float y)
        {
            return new SizeF(a.Width + x, a.Height + y);
        }

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
        /// Size.Width / Size.Height
        /// </summary>
        /// <param name="Size"></param>
        /// <returns></returns>
        public static float ratio(this SizeF Size)
        {
            return Size.Width / Size.Height;
        }

        public static SizeF sub(this SizeF a, SizeF c)
        {
            return new SizeF(a.Width - c.Width, a.Height - c.Height);
        }
        public static SizeF sub(this SizeF a, float x, float y)
        {
            return new SizeF(a.Width - x, a.Height - y);
        }

        public static float norm(this SizeF a)
        {
            return a.Width * a.Width + a.Height * a.Height;
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
        /// <summary>
        /// Width * Height
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float Inhalt(this SizeF a)
        {
            return a.Width * a.Height;
        }
        /// <summary>
        /// min(Width, Height)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static float Min(this SizeF a)
        {
            return Math.Min(a.Width, a.Height);
        }
        /// <summary>
        /// (min(a.X, b.X), min(a.Y, b.Y))
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static SizeF Min(this SizeF a, SizeF b)
        {
            return new SizeF(Math.Min(a.Width, b.Width), Math.Min(a.Height, b.Height));
        }
        /// <summary>
        /// (min(a.X, b.X), min(a.Y, b.Y))
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static SizeF Min(this SizeF a, float x, float y)
        {
            return a.Min(new SizeF(x, y));
        }
        /// <summary>
        /// (max(a.X, b.X), max(a.Y, b.Y))
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static SizeF Max(this SizeF a, SizeF b)
        {
            return new SizeF(Math.Max(a.Width, b.Width), Math.Max(a.Height, b.Height));
        }
        /// <summary>
        /// (Max(a.X, b.X), Max(a.Y, b.Y))
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static SizeF Max(this SizeF a, float x, float y)
        {
            return a.Max(new SizeF(x, y));
        }
        /// <summary>
        /// erstellt einen neuen Vektor, der Produkt der beiden Faktoren ist
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static SizeF mul(this SizeF a, SizeF d)
        {
            return new SizeF(a.Width * d.Width, a.Height * d.Height);
        }
        public static SizeF div(this SizeF a, SizeF d)
        {
            return new SizeF(a.Width / d.Width, a.Height / d.Height);
        }
        public static SizeF div(this SizeF a, float d)
        {
            return new SizeF(a.Width / d, a.Height / d);
        }
        public static SizeF div(this SizeF a, float x, float y)
        {
            return new SizeF(a.Width / x, a.Height / y);
        }

        public static bool Equal(this SizeF a, SizeF b)
        {
            return a.ToPointF().Equal(b.ToPointF());
        }

        /// <summary>
        /// DinA Größe in Millimeter
        /// </summary>
        /// <param name="DinANumber"></param>
        /// <returns></returns>
        public static Size DinA(this int DinANumber, bool Hoch)
        {
            int[] ls = { 1189, 841, 594, 420, 297, 210, 148, 105, 74, 52, 37, 26 };
            if (Hoch)
                return new Size(ls[DinANumber + 1], ls[DinANumber]);
            else
                return new Size(ls[DinANumber], ls[DinANumber + 1]);
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
        /// Erstellt einen neuen Vektor, indem zu diesem b addiert wird
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point add(this Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        /// <summary>
        /// Erstellt einen neuen Vektor, indem zu diesem c*b addiert wird
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point saxpy(this Point a, int c, Point b)
        {
            return new Point(a.X + c * b.X, a.Y + c * b.Y);
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
        public static Point mul(this Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }
        public static Point mul(this Point a, float x, float y)
        {
            return new Point((int)(a.X * x), (int)(a.Y * y));
        }
        public static Point mul(this Point a, float c)
        {
            return a.mul(c, c);
        }
        public static Point mulCeil(this Point a, SizeF b)
        {
            return new Point((a.X * b.Width).Ceil(), (a.Y * b.Height).Ceil());
        }
        public static Point divCeil(this Point a, Point b)
        {
            return new Point((a.X * 1f / b.X).Ceil(), (a.Y * 1f / b.Y).Ceil());
        }
        public static Point divFloor(this Point a, Point b)
        {
            return new Point((a.X * 1f / b.X).Floor(), (a.Y * 1f / b.Y).Floor());
        }

        public static bool Equal(this PointF a, PointF b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) < 0.00001f;
        }
    }
    public static class RectangleFErweiterer
    {
        public static PointF Center(this RectangleF Rectangle)
        {
            return new PointF(Rectangle.Left + Rectangle.Right, Rectangle.Top + Rectangle.Bottom).div(2);
        }

        public static RectangleF mul(this RectangleF Rectangle, float c)
        {
            return new RectangleF(Rectangle.Location.mul(c), Rectangle.Size.mul(c));
        }
        public static RectangleF mul(this RectangleF Rectangle, float x, float y)
        {
            return new RectangleF(Rectangle.Location.mul(x, y), Rectangle.Size.mul(x, y));
        }
        public static RectangleF mul(this RectangleF Rectangle, SizeF Size)
        {
            return Rectangle.mul(Size.Width, Size.Height);
        }
        public static RectangleF mul(this RectangleF Rectangle, PointF Point)
        {
            return Rectangle.mul(Point.X, Point.Y);
        }
        public static RectangleF div(this RectangleF Rectangle, float c)
        {
            return new RectangleF(Rectangle.Location.div(c), Rectangle.Size.div(c));
        }
        public static RectangleF div(this RectangleF Rectangle, float x, float y)
        {
            return new RectangleF(Rectangle.Location.div(x, y), Rectangle.Size.div(x, y));
        }
        public static RectangleF div(this RectangleF Rectangle, SizeF Size)
        {
            return Rectangle.div(Size.Width, Size.Height);
        }
        public static RectangleF div(this RectangleF Rectangle, PointF Point)
        {
            return Rectangle.div(Point.X, Point.Y);
        }
        public static RectangleF move(this RectangleF Rectangle, PointF MoveBy)
        {
            return new RectangleF(Rectangle.Location.add(MoveBy), Rectangle.Size);
        }
        public static RectangleF move(this RectangleF Rectangle, float x, float y)
        {
            return new RectangleF(Rectangle.Location.add(x, y), Rectangle.Size);
        }
        /// <summary>
        /// this.Location += (x,y)
        /// <para>this.Size -= 2 * (x,y)</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectangleF Inner(this RectangleF Rectangle, float x, float y)
        {
            return new RectangleF(Rectangle.Location.add(x, y), Rectangle.Size.sub(2 * x, 2 * y));
        }
        /// <summary>
        /// this.Location += (x,y)
        /// <para>this.Size -= 2 * (x,y)</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectangleF Inner(this RectangleF Rectangle, PointF Point)
        {
            return Rectangle.Inner(Point.X, Point.Y);
        }
        /// <summary>
        /// this.Location += (x,y)
        /// <para>this.Size -= 2 * (x,y)</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static RectangleF Inner(this RectangleF Rectangle, SizeF Size)
        {
            return Rectangle.Inner(Size.Width, Size.Height);
        }

        public static bool Equal(this RectangleF Rectangle1, RectangleF Rectangle2)
        {
            return Rectangle1.Location.Equal(Rectangle2.Location) && Rectangle1.Size.Equal(Rectangle2.Size);
        }

        /// <summary>
        /// Destination.Loc = Source.Loc * T.Size + T.Loc
        /// <para>Destination.Size = Source.Size * T.Size</para>
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Transformation"></param>
        /// <returns></returns>
        public static RectangleF Transform(this RectangleF Source, RectangleF Transformation)
        {
            Source = Source.mul(Transformation.Size);
            Source = Source.move(Transformation.Location);
            return Source;
        }
        /// <summary>
        /// Source.Size = Destination.Size / T.Size
        /// <para> Source.Loc = (Destination.Loc - T.Loc) / T.Size</para>
        /// Destination.Loc = Source.Loc * T.Size + T.Loc
        /// <para>Destination.Size = Source.Size * T.Size</para>
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Transformation"></param>
        /// <returns></returns>
        public static RectangleF InvertTransform(this RectangleF Destination, RectangleF Transformation)
        {
            Destination = Destination.move(Transformation.Location.mul(-1));
            Destination = Destination.div(Transformation.Size);
            return Destination;
        }
        /// <summary>
        /// T.Size = D.Size / S.Size
        /// <para> T.Loc = D.Loc - S.Loc * T.Size </para>
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Destination"></param>
        /// <returns></returns>
        public static RectangleF GetTransformation(this RectangleF Source, RectangleF Destination)
        {
            RectangleF T = new RectangleF();
            T.Size = Destination.Size.div(Source.Size);
            T.Location = Destination.Location.sub(Source.Location.mul(T.Size));
            return T;
        }
    }
}
