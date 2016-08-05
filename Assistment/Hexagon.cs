using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;
using Assistment.Drawing;
using Assistment.Drawing.Graph;
using Assistment.Drawing.Style;
using Assistment.Drawing.Geometries;

namespace SpielplanErsteller
{
    public class Hexagon : IEnumerable<Polygon>
    {
        /// <summary>
        /// Defaultmäßig Din A3 Größe Hochformat in Millimetern.
        /// </summary>
        public SizeF BrettSize = new SizeF(297, 420);
        /// <summary>
        /// Größe eines Feldes
        /// </summary>
        public SizeF FeldSize = new SizeF(30f, 30f);//new SizeF(24.75f, 24.75f);//new SizeF(37.125f, 37.125f);//new SizeF(22.5f, 22.5f);

        public int Rows
        {
            get
            {
                return (int)Math.Ceiling(BrettSize.Height / FeldSize.Height * 2 / Math.Sqrt(3)) + 2;
            }
        }
        public int Columns
        {
            get
            {
                return (int)Math.Ceiling(BrettSize.Width / FeldSize.Width * 4 / 7) + 2;
            }
        }

        public Polygon[,] GetAll()
        {
            int x = Columns;
            int y = Rows;
            Polygon[,] result = new Polygon[x, y];

            float h = (float)Math.Sqrt(0.75);
            PointF off = new PointF();
            for (int i = 0; i < x; i++)
            {
                Polygon P = Polygon.RegelPoly(6);
                P += off;
                for (int j = 0; j < y; j++)
                {
                    result[i, j] = P * FeldSize;
                    P += new PointF(0, 2 * h);
                }
                off = off.add(new PointF(1.5f, i % 2 == 1 ? -h : h));
            }
            return result;
        }

        public void Scale(float scale)
        {
            BrettSize = BrettSize.mul(scale);
            FeldSize = FeldSize.mul(scale);
        }

        public static Hexagon operator *(Hexagon Hexagon, float Scale)
        {
            Hexagon h = new Hexagon();
            h.BrettSize = Hexagon.BrettSize.mul(Scale);
            h.FeldSize = Hexagon.FeldSize.mul(Scale);

            return h;
        }

        public Bitmap GetBoden()
        {
            return new Bitmap((int)BrettSize.Width, (int)BrettSize.Height);
        }

        public IEnumerator<Polygon> GetEnumerator()
        {
            return (EnumerableExtender.Enumerate(GetAll())).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void printDinA3Pdf(Schema schema)
        {
            float burst = 5 * schema.scale;
            float breite = 12 * schema.scale;
            int lines = 10;
            Pen pen = schema.stift;
            Pen fett = (Pen)schema.stift.Clone();
            fett.Width *= 3;

            this.Scale(schema.scale);

            Bitmap b = GetBoden();
            Graphics g = Graphics.FromImage(b);
            g.Clear(schema.background);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Shadex.chaosRect(g, new RectangleF(-10, -10, b.Width + 10, b.Height + 10), schema);
            //Shadex.zitterQuadrate(g, new RectangleF(-10, -10, b.Width + 10, b.Height + 10), 100, 100, schema);

            foreach (Polygon poly in this)
            {
                Knoten k = Shadex.getCyberPunkDraht(poly.punkte, burst, 0, lines, breite, pen);
                k.drawGraph(g);
                g.DrawPolygon(fett, poly);
            }

            b.saveDinA3Pdf("./Hex/Hex" + schema.name);
        }
        public void printDinA3Pdf(FlachenSchema schema, float Scale, string Name)
        {
            float burst = 5 * Scale;
            float breite = 12 * Scale;
            int lines = 10;
            Pen pen = (Pen)schema.Stift(0, 0).Clone();
            pen.Width *= Scale;
            Pen fett = (Pen)pen.Clone();
            fett.Width *= 3;

            this.Scale(Scale);

            Bitmap b = GetBoden();
            Graphics g = b.GetHighGraphics();
            g.Clear(schema.BackColor.Value);
            //Shadex.ChaosFlache(g, schema);
            //Shadex.Chaos2Flache(g, schema);

            foreach (Polygon poly in this)
            {
                Knoten k = Shadex.getCyberPunkDraht(poly.punkte, burst, 0, lines, breite, pen);
                k.drawGraph(g);
                g.DrawPolygon(fett, poly);
            }

            b.saveDinA3Pdf("./Hex/Hex" + Name);
        }
    }

    public static class HexagonErweiterer
    {
        public static void DrawPolygon(this Graphics g, Pen Pen, Hexagon Hexagon)
        {
            foreach (Polygon item in Hexagon)
                g.DrawPolygon(Pen, item);
        }

        public static void FillPolygon(this Graphics g, Brush Brush, Hexagon Hexagon)
        {
            foreach (Polygon item in Hexagon)
                g.FillPolygon(Brush, item);
        }
    }
}
