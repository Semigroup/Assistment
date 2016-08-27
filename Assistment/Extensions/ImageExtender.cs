using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Extensions
{
    public static class ImageExtender
    {
        public static void saveDinA3Pdf(this Image Image, string name)
        {
            ImageBox ib = new ImageBox(1403, Image);
            ib.createDinA3PDF(name);
        }
        public static void savePdf(this Image Image, string name)
        {
            ImageBox ib = new ImageBox(701, Image);
            ib.createPDF(name);
        }
        public static void Raise(this Graphics Graphics)
        {
            Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }

        public static Graphics GetHighGraphics(this Image Image)
        {
            Graphics g = Graphics.FromImage(Image);
            g.Raise();
            return g;
        }
        public static Graphics GetHighGraphics(this Image Image, float Scaling)
        {
            Graphics g = Graphics.FromImage(Image);
            g.Raise();
            g.ScaleTransform(Scaling, Scaling);
            return g;
        }
        public static Graphics GetHighGraphics(this Image Image, Color BackgroundColor)
        {
            Graphics g = Image.GetHighGraphics();
            g.Clear(BackgroundColor);
            return g;
        }

        public static Bitmap Beschrifte(this Image Image, RectangleF mass, PointF rate, string einheit, Font Font, Pen Pen, Brush Brush, bool unten, bool links)
        {
            PointF zusatz = new PointF(2, 1).mul(Font.Height);
            Bitmap b = new Bitmap((int)(zusatz.X + Image.Width), (int)(zusatz.Y + Image.Height));
            Graphics g = b.GetHighGraphics(Color.White);
            g.DrawImage(Image, links ? zusatz.X : 0, unten ? 0 : zusatz.Y);

            PointF p = new PointF(0, unten ? Image.Height : 0);
            for (int i = (int)Math.Ceiling(mass.Left / rate.X); i < (int)Math.Ceiling(mass.Right / rate.X); i++)
            {
                float t = i * rate.X;
                p.X = (t - mass.Left) / mass.Width * Image.Width;
                if (links)
                    p.X += zusatz.X;
                g.DrawLine(Pen, p, p.add(0, zusatz.Y));
                g.DrawString(t.ToString("F1") + " " + einheit, Font, Brush, p);
            }

            p = new PointF(links ? 0 : Image.Width, 0);
            for (int i = (int)Math.Ceiling(mass.Top / rate.Y); i < (int)Math.Ceiling(mass.Bottom / rate.Y); i++)
            {
                float t = -i * rate.X;
                p.Y = (t - mass.Top) / mass.Height * Image.Height;
                if (!unten)
                    p.Y += zusatz.Y;
                g.DrawLine(Pen, p, p.add(zusatz.X, 0));
                g.DrawString(t.ToString("F1") + " " + einheit, Font, Brush, p);
            }
            return b;
        }
    }
}
