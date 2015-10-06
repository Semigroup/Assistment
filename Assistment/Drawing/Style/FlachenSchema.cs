using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using Assistment.Drawing.Geometrie;
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

        public Color BackColor;

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

        public static FlachenSchema ChaosRect(float x, float y, float Width, float Height, float xburst, float yburst)
        {
            Random d = new Random();
            FlachenSchema fs = new FlachenSchema();

            fs.Flache = (u, v) => new PointF(x + Width * u +d.NextCenterd() * xburst, y + Height * v + d.NextCenterd() * yburst);

            return fs;
        }
    }
}
