using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Assistment.Drawing.Geometries;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Mathematik;

namespace Assistment.Drawing.Style
{
    public class FlachenSchema
    {
        public FlachenFunktion<PointF> Flache;
        public FlachenFunktion<Brush> Pinsel;
        public FlachenFunktion<Pen> Stift;

        /// <summary>
        /// (u,v) - Samples
        /// </summary>
        public Point Samples;
        public Point Boxes;
        public Point Thumb;

        public Color? BackColor;
        /// <summary>
        /// Soll der Stift parallel zur U-Achse angewendet werden?
        /// </summary>
        public bool ULinien;
        /// <summary>
        /// Soll der Stift parallel zur V-Achse angewendet werden?
        /// </summary>
        public bool VLinien;

        public void VertikalLineareFarben(params Color[] Farben)
        {
            int n = Farben.Length - 1;
            Pinsel = (u, v) =>
            {
                int i = (int)(Math.Min(n * v, n - 1));
                return new SolidBrush(Farben[i].tween(Farben[i + 1], n * v - i));
            };
            Stift = (u, v) =>
            {
                int i = (int)(Math.Min(n * v, n - 1));
                return new Pen(Farben[i].tween(Farben[i + 1], n * v - i));
            };
        }

        public void VertikalLineareFarben(float alpha, params Color[] Farben)
        {
            VertikalLineareFarben(Farben.Map(color => Color.FromArgb((int)(255 * alpha), color)).ToArray());
        }

        /// <summary>
        /// f(0) = Farben[0]
        /// <para>f(1) = Farben.Last()</para>
        /// </summary>
        /// <param name="Farben"></param>
        /// <returns></returns>
        public static Func<float, Color> RotierFarben(params Color[] Farben)
        {
            int n = Farben.Length;
            return t =>
            {
                float d = n * t;
                int i = d.Floor();
                d -= i;
                if (i >= n - 1)
                    return Farben[n - 1].tween(Farben[0], d);
                else
                    return Farben[i].tween(Farben[i + 1], d);
            };
        }
        /// <summary>
        /// f(0) = Farben[0]
        /// <para>f(1) = Farben.Last()</para>
        /// </summary>
        /// <param name="Farben"></param>
        /// <returns></returns>
        public static Func<float, Color> RotierFarben(float Alpha, params Color[] Farben)
        {
            return RotierFarben(Farben.Flat((Alpha * 255).Floor()));
        }
        /// <summary>
        /// f(0) = Farben[0]
        /// <para>f(1) = Farben.Last()</para>
        /// </summary>
        /// <param name="Farben"></param>
        /// <returns></returns>
        public static Func<float, Color> RotierHartFarben(params Color[] Farben)
        {
            int n = Farben.Length;
            return t =>
            {
                float d = n * t;
                int i = d.Floor() % n;
                return Farben[i];
            };
        }
        /// <summary>
        /// f(0) = Farben[0]
        /// <para>f(1) = Farben.Last()</para>
        /// </summary>
        /// <param name="Farben"></param>
        /// <returns></returns>
        public static Func<float, Color> RotierHartFarben(float Alpha, params Color[] Farben)
        {
            return RotierHartFarben(Farben.Flat((Alpha * 255).Floor()));
        }

        public void Scale(float scale)
        {
            Flache = Flache.mul(scale);
            FlachenFunktion<Pen> AlterStift = Stift;
            if (AlterStift != null)
                Stift = (u, v) =>
                {
                    Pen p = AlterStift(u, v);
                    p.Width *= scale;
                    return p;
                };
        }
        public Polygon Rand()
        {
            int n = 2 * (Samples.X + Samples.Y - 2);
            RandFunktion<PointF> f = Flache.Rand(Samples.X, Samples.Y);
            PointF[] p = f.Samples(n);
            Polygon po = new Polygon(p);
            po.Close();
            return po;
        }
        public static FlachenSchema ChaosRect(RectangleF Rectangle, float xburst, float yburst)
        {
            return ChaosRect(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height, xburst, yburst);
        }
        public static FlachenSchema ChaosRect(float x, float y, float Width, float Height, float xburst, float yburst)
        {
            Random d = new Random();
            FlachenSchema fs = new FlachenSchema();

            fs.Flache = (u, v) => new PointF(x + Width * u + d.NextCenterd() * xburst, y + Height * v + d.NextCenterd() * yburst);

            return fs;
        }
        public static FlachenSchema ChaosKreis(float x, float y, float radius, double bogenmassAnfang, double bogenmassEnde, float xburst, float yburst)
        {
            Random d = new Random();
            FlachenSchema fs = new FlachenSchema();

            fs.Flache = (u, v) => FastMath.Sphere(u * (bogenmassEnde - bogenmassAnfang) + bogenmassAnfang).mul(v * radius).
                add(x + d.NextCenterd() * xburst, y + d.NextCenterd() * yburst);

            return fs;
        }
        public static FlachenSchema ChaosKreis(RectangleF Rectangle, float xburst, float yburst)
        {
            float x = (Rectangle.Right + Rectangle.Left) / 2;
            float y = (Rectangle.Top + Rectangle.Bottom) / 2;
            float radius = FastMath.Sqrt(Rectangle.Width * Rectangle.Width / 4 + Rectangle.Height * Rectangle.Height / 4);
            return ChaosKreis(x, y, radius, 0, 2 * Math.PI, xburst, yburst);
        }
        public static FlachenSchema ChaosKreisPolarerBurst(float x, float y, float radius, float radiusburst, double bogenmassAnfang, double bogenmassEnde, double bogenmassBurst)
        {
            Random d = new Random();
            FlachenSchema fs = new FlachenSchema();

            fs.Flache = (u, v) => FastMath.Sphere(u * (bogenmassEnde - bogenmassAnfang) + bogenmassAnfang + d.NextCenterd() * bogenmassBurst).mul(v * radius + d.NextCenterd() * radiusburst).
                add(x, y);

            return fs;
        }
        public static FlachenSchema ChaosKreisPolarerBurst(RectangleF Rectangle, float radiusburst, double bogenmassBurst)
        {
            float x = (Rectangle.Right + Rectangle.Left) / 2;
            float y = (Rectangle.Top + Rectangle.Bottom) / 2;
            float radius = FastMath.Sqrt(Rectangle.Width * Rectangle.Width / 4 + Rectangle.Height * Rectangle.Height / 4);
            return ChaosKreisPolarerBurst(x, y, radius, radiusburst, 0, 2 * Math.PI, bogenmassBurst);
        }

        public static FlachenSchema BloodSheds(RectangleF Rectangle)
        {
            FlachenSchema fs = FlachenSchema.ChaosRect(Rectangle, 0, 400);
            fs.Thumb = new Point(1, 1);
            fs.Boxes = new Point(1, (int)(Rectangle.Height / 100));
            fs.Samples = new Point(35, 1);
            fs.Pinsel = (u, v) => Color.Red.flat(80).ToBrush();
            fs.BackColor = Color.Black;
            Pen p = new Pen(Color.Black, 1.0f / 5);
            fs.Stift = (u, v) => p;

            return fs;
        }
        public static FlachenSchema UndividedChaos(RectangleF Rectangle)
        {
            Color Tzeentch = Color.Blue;
            Color Khorne = Color.Red;
            Color Slaanesh = Color.DarkViolet;
            Color Nurgle = Color.Green;
            Color Hintergrund = Color.Black;

            Func<float, Color> Farben = RotierFarben(0.5f,
                Hintergrund, Tzeentch, Tzeentch,
                Hintergrund, Khorne, Khorne,
                Hintergrund, Nurgle, Nurgle,
                Hintergrund, Slaanesh, Slaanesh,
                Hintergrund);

            float x = (Rectangle.Right + Rectangle.Left) / 2;
            float y = (Rectangle.Top + Rectangle.Bottom) / 2;
            float radius = FastMath.Sqrt(Rectangle.Width * Rectangle.Width / 4 + Rectangle.Height * Rectangle.Height / 4);

            FlachenSchema fs = ChaosKreis(x, y, radius, Math.PI / 2, 5 * Math.PI / 2, 100, 100);

            fs.Thumb = new Point(1, 1);
            fs.Boxes = new Point(100, (int)(radius / 100));
            fs.Samples = new Point(50, 50);

            fs.Pinsel = (u, v) => new SolidBrush(Farben(u));
            fs.BackColor = Color.Black;
            Pen p = new Pen(Color.White, 1.0f / 5);
            p.LineJoin = LineJoin.Round;
            fs.Stift = (u, v) => p;

            return fs;
        }
        public static FlachenSchema UndividedChaosA0(RectangleF Rectangle)
        {
            Color Tzeentch = Color.Blue;
            Color Khorne = Color.Red;
            Color Slaanesh = Color.DarkViolet;
            Color Nurgle = Color.Green;
            Color Hintergrund = Color.White;
            Color Linien = Color.Black;

            Func<float, Color> Farben = RotierFarben(0.5f,
                Hintergrund, Tzeentch, Tzeentch,
                Hintergrund, Khorne, Khorne,
                Hintergrund, Nurgle, Nurgle,
                Hintergrund, Slaanesh, Slaanesh,
                Hintergrund);

            float x = (Rectangle.Right + Rectangle.Left) / 2;
            float y = (Rectangle.Top + Rectangle.Bottom) / 2;
            float radius = FastMath.Sqrt(Rectangle.Width * Rectangle.Width / 4 + Rectangle.Height * Rectangle.Height / 4);

            FlachenSchema fs = ChaosKreis(x, y, radius, Math.PI / 2, 5 * Math.PI / 2, 100, 100);

            fs.Thumb = new Point(1, 1);
            fs.Boxes = new Point(1000, (int)(radius / 100));
            fs.Samples = new Point(1000, 50);

            fs.Pinsel = (u, v) => new SolidBrush(Farben(u));
            fs.BackColor = Hintergrund;
            Pen p = new Pen(Linien, 1.0f / 5);
            p.LineJoin = LineJoin.Round;
            fs.Stift = (u, v) => p;

            return fs;
        }
        public static FlachenSchema UndividedChaosA0Polar(RectangleF Rectangle)
        {
            Color Tzeentch = Color.Blue;
            Color Khorne = Color.Red;
            Color Slaanesh = Color.DarkViolet;
            Color Nurgle = Color.Green;
            Color Hintergrund = Color.Black;
            Color Linien = Color.White;

            Func<float, Color> Farben = RotierFarben(0.5f,
                 Tzeentch,
                 Khorne,
                 Nurgle,
                 Slaanesh);

            float x = (Rectangle.Right + Rectangle.Left) / 2;
            float y = (Rectangle.Top + Rectangle.Bottom) / 2;
            float radius = FastMath.Sqrt(Rectangle.Width * Rectangle.Width / 4 + Rectangle.Height * Rectangle.Height / 4);

            FlachenSchema fs = ChaosKreis(x, y, radius, Math.PI / 4, 9 * Math.PI / 4, 100, 100);

            fs.Thumb = new Point(1, 1);
            fs.Boxes = new Point(200, (int)(radius / 100));
            fs.Samples = new Point(50, 50);

            fs.Pinsel = (u, v) => new SolidBrush(Farben(u));
            fs.BackColor = Hintergrund;
            Pen p = new Pen(Linien, 1.0f / 5);
            p.LineJoin = LineJoin.Round;
            fs.Stift = (u, v) => p;

            return fs;
        }
    }
}
