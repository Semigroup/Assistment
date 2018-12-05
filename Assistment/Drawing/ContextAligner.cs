using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Texts;
using System.Drawing;

namespace Assistment.Drawing
{
    public class ContextAligner
    {
        public DrawContext context;
        public float scale;
        public RectangleF drawingRegion;

        public bool xSpiegel, ySpiegel;

        /// <summary>
        /// bestimmt scale so, dass innerRelativeSize maximal zentriert in outerBox liegt
        /// </summary>
        /// <param name="outerBox"></param>
        /// <param name="innerRelativeSize"></param>
        public ContextAligner(RectangleF outerBox, SizeF innerRelativeSize)
        {
            scale = Math.Min(outerBox.Width / innerRelativeSize.Width, outerBox.Height / innerRelativeSize.Height);
            drawingRegion = new RectangleF();
            drawingRegion.Width = scale * innerRelativeSize.Width;
            drawingRegion.Height = scale * innerRelativeSize.Height;
            drawingRegion.X = outerBox.X + (outerBox.Width - drawingRegion.Width) / 2;
            drawingRegion.Y = outerBox.Y + (outerBox.Height - drawingRegion.Height) / 2;

            this.ySpiegel = false;
            this.xSpiegel = false;
        }
        public ContextAligner(RectangleF outerBox, IDrawable drawable) : this(outerBox, drawable.getRelativeSize())
        {
            this.xSpiegel = drawable.getXSpiegel();
            this.ySpiegel = drawable.getYSpiegel();
        }


        public void clear()
        {
            clear(Brushes.White);
        }
        public void clear(Brush brush)
        {
            context.FillRectangle(brush, drawingRegion);
        }
        public void DrawString(string s, Font font, Brush brush, PointF P, float height)
        {
            context.DrawString(s, font, brush, align(P), height);
        }
        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            this.DrawLine(pen, new PointF(x1, y1), new PointF(x2, y2));
        }
        public void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            context.DrawLine(pen, align(pt1), align(pt2));
        }
        public void DrawPolygon(Pen pen, PointF[] points)
        {
            PointF[] p = new PointF[points.Length];
            for (int i = 0; i < p.Length; i++)
                p[i] = align(points[i]);
            context.DrawPolygon(pen, p);
        }
        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            DrawRectangle(pen, new RectangleF(x, y, width, height));
        }
        public void DrawRectangle(Pen pen, RectangleF re)
        {
            re = align(re);
            context.DrawRectangle(pen, re.X, re.Y, re.Width, re.Height);
        }
        public void DrawEllipse(Pen pen, RectangleF re)
        {
            re = align(re);
            context.DrawEllipse(pen, re);
        }

        public void FillEllipse(Brush brush, RectangleF re)
        {
            context.FillEllipse(brush, align(re));
        }
        public void FillSphere(Brush brush, PointF mid, float radius)
        {
            FillEllipse(brush, new RectangleF(mid.X - radius, mid.Y - radius, 2 * radius, 2 * radius));
        }
        public void FillRectangle(Brush brush, RectangleF re)
        {
            context.FillRectangle(brush, align(re));
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
            P.X = scale * P.X;
            P.Y = scale * P.Y;
            if (xSpiegel)
                P.X = drawingRegion.Width - P.X;
            if (ySpiegel)
                P.Y = drawingRegion.Height - P.Y;
            P.X += drawingRegion.X;
            P.Y += drawingRegion.Y;
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
            P.X -= drawingRegion.X;
            P.Y -= drawingRegion.Y;

            if (xSpiegel)
                P.X = drawingRegion.Width - P.X;
            if (ySpiegel)
                P.Y = drawingRegion.Height - P.Y;

            P.X = P.X / scale;
            P.Y = P.Y / scale;
            return P;
        }

        public void drawRahmen(Pen stift)
        {
            context.DrawRectangle(stift, drawingRegion);
        }
        public void drawGrid(Pen stift, int spalten, int zeilen)
        {
            float f;
            float w = drawingRegion.Width / spalten;
            for (int i = 1; i < spalten; i++)
            {
                f = i * w + drawingRegion.X;
                context.DrawLine(stift, f, drawingRegion.Top, f, drawingRegion.Bottom);
            }
            w = drawingRegion.Height / zeilen;
            for (int i = 1; i < zeilen; i++)
            {
                f = i * w + drawingRegion.Y;
                context.DrawLine(stift, drawingRegion.Right, f, drawingRegion.Left, f);
            }
        }
    }
}
