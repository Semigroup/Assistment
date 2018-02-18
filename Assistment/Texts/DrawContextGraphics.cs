using System.Drawing;
using System.Drawing.Imaging;

namespace Assistment.Texts
{
    public class DrawContextGraphics : DrawContext
    {
        public Graphics g;
        public DrawContextGraphics(Graphics g)
        {
            this.g = g;
            this.Backcolor = Brushes.White;
            this.Bildhohe = float.MaxValue;
        }
        public DrawContextGraphics(Graphics g, Brush backcolor)
        {
            this.g = g;
            this.Backcolor = backcolor;
            this.Bildhohe = float.MaxValue;
        }
        public DrawContextGraphics(Graphics g, Brush backcolor, float Bildhohe)
        {
            this.g = g;
            this.Backcolor = backcolor;
            this.Bildhohe = Bildhohe;
        }

        public override void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            g.DrawRectangle(pen, x, y, width, height);
        }
        public override void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            g.FillRectangle(brush, x, y, width, height);
        }
        public override void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            g.DrawLine(pen, x1, y1, x2, y2);
        }
        public override void DrawString(string text, Font font, Brush brush, float x, float y, float height)
        {
            g.DrawString(text, font, brush, x - font.SizeInPoints * 0.2f, y);
        }
        public override void DrawImage(Image img, float x, float y)
        {
            g.DrawImage(img, x, y);
        }
        public override void DrawImage(Image img, float x, float y, float width, float height, ImageAttributes imageAttributes)
        {
            g.DrawImage(img, 
                new PointF[] { new PointF(x,y), new PointF(x+width, y), new PointF(x, y+ height) },
                new RectangleF(0,0, img.Width, img.Height),
                GraphicsUnit.Pixel, 
                imageAttributes);
        }
        public override void DrawClippedImage(Image img, float x, float y, RectangleF source)
        {
            g.DrawImage(img, x, y, source, GraphicsUnit.Pixel);
        }
        public override void DrawClippedImage(Image img, RectangleF destination, RectangleF source)
        {
            g.DrawImage(img, destination, source, GraphicsUnit.Pixel);
        }
        public override void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            g.DrawEllipse(pen, x, y, width, height);
        }
        public override void FillEllipse(Brush brush, float x, float y, float width, float height)
        {
            g.FillEllipse(brush, x, y, width, height);
        }
        public override void DrawPolygon(Pen pen, PointF[] polygon)
        {
            g.DrawPolygon(pen, polygon);
        }

        public void RaiseGraphics()
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }
        public void LowerGraphics()
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
        }

        public override void Dispose()
        {
            g.Dispose();
        }

        public override void NewPage()
        {
        }

        public override void FillPolygon(System.Drawing.Brush Brush, System.Drawing.PointF[] polygon)
        {
            g.FillPolygon(Brush, polygon);
        }
    }
}
