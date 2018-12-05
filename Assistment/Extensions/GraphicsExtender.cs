using System.Drawing;

namespace Assistment.Extensions
{
    public static class GraphicsExtender
    {
        public static void Raise(this Graphics Graphics)
        {
            Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }
        public static void Lower(this Graphics Graphics)
        {
            Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
        }

        public static void DrawFillEllipse(this Graphics g, Pen pen, Brush brush, RectangleF rectangle)
        {
            if (brush != null)
                g.FillEllipse(brush, rectangle);
            if (pen != null)
                g.DrawEllipse(pen, rectangle);
        }
    }
}
