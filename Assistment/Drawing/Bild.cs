using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Assistment.Drawing
{
    public class Bild
    {
        public Graphics g;
        public Image b;
        public float scale;
        public PointF offset;
        /// <summary>
        /// nicht aligned!
        /// </summary>
        public float height, width;

        public bool xSpiegel, ySpiegel;

        public Bild(float width, float height)
        {
            this.width = width;
            this.height = height;
            b = new Bitmap((int)width, (int)height);
            g = Graphics.FromImage(b);
            scale = 1;
            offset = new PointF();
            xSpiegel = ySpiegel = false;
        }
        public Bild(float xOffset, float yOffset, float width, float height, float scale)
        {
            this.width = width;
            this.height = height;
            b = new Bitmap((int)(scale * (2 * xOffset + width)), (int)(scale * (yOffset * 2 + height)));
            this.scale = scale;
            this.offset = new PointF(xOffset, yOffset);
            g = Graphics.FromImage(b);
            xSpiegel = ySpiegel = false;
        }
        public Bild(Image b, float width, float height)
        {
            this.width = width;
            this.height = height;
            this.b = b;
            this.scale = Math.Min((b.Width - 1) / width, (b.Height - 1) / height);
            this.offset = new PointF((b.Width / scale - width) / 2, (b.Height / scale - height) / 2);
            g = Graphics.FromImage(b);
            xSpiegel = ySpiegel = false;
        }
        public Bild(Image b, RectangleF drawingRegion, float width, float height)
        {
            this.width = width;
            this.height = height;
            this.b = b;
            this.scale = Math.Min(drawingRegion.Width / width, drawingRegion.Height / height);
            this.offset = new PointF(((drawingRegion.Right + drawingRegion.Left) / scale - width) / 2, ((drawingRegion.Top + drawingRegion.Bottom) / scale - height) / 2);
            g = Graphics.FromImage(b);
            xSpiegel = ySpiegel = false;
        }
        public Bild(Graphics g, RectangleF drawingRegion, float width, float height)
        {
            this.width = width;
            this.height = height;
            this.scale = Math.Min(drawingRegion.Width / width, drawingRegion.Height / height);
            this.offset = new PointF(((drawingRegion.Right + drawingRegion.Left) / scale - width) / 2, ((drawingRegion.Top + drawingRegion.Bottom) / scale - height) / 2);
            this.g = g;
            xSpiegel = ySpiegel = false;
        }
        public Bild(RectangleF drawingRegion, float width, float height)
        {
            this.width = width;
            this.height = height;
            this.scale = Math.Min(drawingRegion.Width / width, drawingRegion.Height / height);
            this.offset = new PointF(((drawingRegion.Right + drawingRegion.Left) / scale - width) / 2, ((drawingRegion.Top + drawingRegion.Bottom) / scale - height) / 2);
            xSpiegel = ySpiegel = false;
        }

        public void clear()
        {
            clear(Color.White);
        }
        public void clear(Color color)
        {
            g.Clear(color);
        }
        public void raiseGraphics()
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }
        public void lowerGraphics()
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
        }
        /// <summary>
        /// hängt automatisch .png an
        /// </summary>
        /// <param name="fileName"></param>
        public void save(string fileName)
        {
            b.Save(fileName + ".png");
        }
        public void save()
        {
            save("test");
        }
        public void DrawString(string s, Font font, Brush brush, PointF P)
        {
            g.DrawString(s, font, brush, align(P));
        }
        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            this.DrawLine(pen, new PointF(x1, y1), new PointF(x2, y2));
        }
        public void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            g.DrawLine(pen, align(pt1), align(pt2));
        }
        public void DrawLines(Pen pen, PointF[] points)
        {
            PointF[] p = new PointF[points.Length];
            for (int i = 0; i < p.Length; i++)
                p[i] = align(points[i]);
            g.DrawLines(pen, p);
        }
        public void DrawPolygon(Pen pen, PointF[] points)
        {
            PointF[] p = new PointF[points.Length];
            for (int i = 0; i < p.Length; i++)
                p[i] = align(points[i]);
            g.DrawPolygon(pen, p);
        }
        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            DrawRectangle(pen, new RectangleF(x, y, width, height));
        }
        public void DrawRectangle(Pen pen, RectangleF re)
        {
            re = align(re);
            g.DrawRectangle(pen, re.X, re.Y, re.Width, re.Height);
        }
        public void DrawEllipse(Pen pen, RectangleF re)
        {
            g.DrawEllipse(pen, scale * (re.X + offset.X), scale * (re.Y + offset.Y), scale * re.Width, scale * re.Height);
        }

        public void FillEllipse(Brush brush, RectangleF re)
        {
            g.FillEllipse(brush, align(re));
        }
        public void FillSphere(Brush brush, PointF mid, float radius)
        {
            FillEllipse(brush, new RectangleF(mid.X - radius, mid.Y - radius, 2 * radius, 2 * radius));
        }
        public void FillRectangle(Brush brush, RectangleF re)
        {
            g.FillRectangle(brush, align(re));
        }

        public RectangleF align(RectangleF R)
        {
            R = new RectangleF(align(R.Location), new SizeF(R.Width * scale, R.Height * scale));
            if (ySpiegel)
                R.Y -= R.Height;
            if (xSpiegel)
                R.X -= R.Width;
            return R;
        }
        public PointF align(PointF P)
        {
            P.X = scale * (P.X + offset.X);
            P.Y = scale * (P.Y + offset.Y);
            if (xSpiegel)
                P.X = b.Width - P.X;
            if (ySpiegel)
                P.Y = b.Height - P.Y;
            return P;
        }
        public PointF[] align(PointF[] polygon)
        {
            PointF[] p = new PointF[polygon.Length];
            for (int i = 0; i < polygon.Length; i++)
                p[i] = align(polygon[i]);
            return p;
        }
        public PointF invAlign(PointF P)
        {
            if (xSpiegel)
                if (ySpiegel)
                    P = new PointF(b.Width - P.X, b.Height - P.Y);
                else
                    P = new PointF(b.Width - P.X, P.Y);
            else
                if (ySpiegel)
                    P = new PointF(P.X, b.Height - P.Y);

            return new PointF(P.X / scale - offset.X, P.Y / scale - offset.Y);
        }

        public RectangleF getDrawingRegion()
        {
            return align(new RectangleF(0, 0, width, height));
        }

        public void drawGrid(int zeilen, int spalten, float breite, float hohe, Pen stift)
        {
            for (int i = 1; i < zeilen; i++)
                DrawLine(stift, 0, hohe * i, width, hohe * i);
            for (int i = 1; i < spalten; i++)
                DrawLine(stift, breite * i, 0, breite * i, height);
        }
        public void drawRahmen(Pen stift)
        {
            DrawRectangle(stift, 0, 0, width, height);
        }
    }
}