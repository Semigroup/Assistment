using System;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Microsoft.Win32;
using System.Drawing.Imaging;

namespace Assistment.Texts
{
    public abstract class DrawContext : IDisposable
    {
        public Brush Backcolor { get; protected set; }
        /// <summary>
        /// gibt an, dass ab Bildhohe nicht mehr gezeichnet werden soll
        /// </summary>
        public float Bildhohe { get; protected set; }

        public void DrawRectangle(Pen pen, RectangleF box)
        {
            DrawRectangle(pen, box.X, box.Y, box.Width, box.Height);
        }
        public abstract void DrawRectangle(Pen pen, float x, float y, float width, float height);
        public void DrawEllipse(Pen pen, RectangleF box)
        {
            DrawEllipse(pen, box.X, box.Y, box.Width, box.Height);
        }
        public abstract void DrawEllipse(Pen pen, float x, float y, float width, float height);
        public void FillEllipse(Brush brush, RectangleF box)
        {
            FillEllipse(brush, box.X, box.Y, box.Width, box.Height);
        }
        public abstract void FillEllipse(Brush brush, float x, float y, float width, float height);
        public void FillRectangle(Brush brush, RectangleF box)
        {
            this.FillRectangle(brush, box.X, box.Y, box.Width, box.Height);
        }
        public abstract void FillRectangle(Brush brush, float x, float y, float width, float height);
        public void DrawLine(Pen pen, PointF A, PointF B)
        {
            this.DrawLine(pen, A.X, A.Y, B.X, B.Y);
        }
        public abstract void DrawLine(Pen pen, float x1, float y1, float x2, float y2);
        public abstract void DrawPolygon(Pen pen, PointF[] polygon);
        public abstract void FillPolygon(Brush Brush, PointF[] polygon);
        public void DrawString(string text, Font font, Brush brush, PointF point, float height)
        {
            this.DrawString(text, font, brush, point.X, point.Y, height);
        }
        public abstract void DrawString(string text, Font font, Brush brush, float x, float y, float height);
        public void DrawImage(Image img, PointF point)
        {
            this.DrawImage(img, point.X, point.Y);
        }
        public abstract void DrawImage(Image img, float x, float y);
        public void DrawImage(Image img, RectangleF box)
        {
            this.DrawImage(img, box.X, box.Y, box.Width, box.Height);
        }
        public void DrawImage(Image img, RectangleF box, ImageAttributes attributes)
        {
            this.DrawImage(img, box.X, box.Y, box.Width, box.Height, attributes);
        }
        public void DrawImage(Image img, float x, float y, float width, float height)
            => DrawImage(img, x, y, width, height, new ImageAttributes());
        public abstract void DrawImage(Image img, float x, float y, float width, float height, ImageAttributes attributes);
        public abstract void DrawClippedImage(Image img, float x, float y, RectangleF source);
        public abstract void DrawClippedImage(Image img, RectangleF destination, RectangleF source);
        public void DrawClippedImage(RectangleF DrawingRegion, Image img, RectangleF Destination)
        {
            SizeF Faktor = ((SizeF)img.Size).div(Destination.Size);
            RectangleF source = DrawingRegion;
            source = source.move(Destination.Location.mul(-1));
            source = source.mul(Faktor);
            source.Intersect(new RectangleF(new PointF(), img.Size));
            Destination.Intersect(DrawingRegion);
            DrawClippedImage(img, Destination, source);
        }

        public abstract void NewPage();

        public abstract void Dispose();
    }
}
