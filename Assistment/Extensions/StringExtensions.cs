using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Assistment.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Speichert den String unter name.txt
        /// </summary>
        /// <param name="text"></param>
        /// <param name="name"></param>
        public static void Save(this string text, string name)
        {
            File.WriteAllText(name + ".txt", text);
        }
        /// <summary>
        /// Speichert den String unter test.txt
        /// </summary>
        /// <param name="text"></param>
        public static void Save(this string text)
        {
            text.Save("test");
        }
        public static void DrawAlphaString(this Graphics g, string s, Font font, Brush brush, PointF point, int alpha)
        {
            g.DrawAlphaString(s, font, brush, new RectangleF(point, new SizeF()), new StringFormat(), alpha);
        }
        public static void DrawAlphaString(this Graphics g, string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format, int alpha)
        {
            SizeF size = g.MeasureString(s, font, layoutRectangle.Size, format);
            RectangleF rf = new RectangleF(align(size, layoutRectangle, format), size);

            g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, Color.White)), rf);
            g.DrawString(s, font, brush, layoutRectangle, format);
        }

        private static PointF align(SizeF size, RectangleF layout, StringFormat format)
        {
            PointF p = layout.Location;
            switch (format.Alignment)
            {
                case StringAlignment.Center:
                    p.X += (layout.Width - size.Width) / 2;
                    break;
                case StringAlignment.Far:
                    p.X += (layout.Width - size.Width);
                    break;
            }
            switch (format.LineAlignment)
            {
                case StringAlignment.Center:
                    p.Y += (layout.Height - size.Height) / 2;
                    break;
                case StringAlignment.Far:
                    p.Y += (layout.Height - size.Height);
                    break;
            }
            return p;
        }
    }
}
