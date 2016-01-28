using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Assistment.Drawing.Geometries;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;

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


        public void Scale(float scale)
        {
            Flache = Flache.mul(scale);
            if (Stift != null)
                Stift = (u, v) =>
                {
                    Pen p = Stift(u, v);
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

            fs.Flache = (u, v) => new PointF(x + Width * u +d.NextCenterd() * xburst, y + Height * v + d.NextCenterd() * yburst);

            return fs;
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
            fs.Stift = (u, v) =>  p;

            return fs;
        }
    }
}
