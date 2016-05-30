using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Assistment.Extensions
{
    public static class StringExtensions
    {
        public static readonly Regex FILENAME_CLEANER = new Regex(@"\W+");

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

        /// <summary>
        /// Gibt die letzte Dateiendung mit Punkt
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Endung(this string fileName)
        {
            int i = fileName.LastIndexOf(".");
            if (i < 0)
                return "";
            else
                return fileName.Substring(i, fileName.Length - i);
        }
        /// <summary>
        /// Gibt den Dateinamen ohne .Endung an
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FileName(this string fileName)
        {
            int i = fileName.LastIndexOf(".");
            if (i < 0)
                return "";
            int j = Math.Max(fileName.LastIndexOf("\\", i - 1), fileName.LastIndexOf("/", i - 1));
            return fileName.Substring(j + 1, i - j -1);
        }
        /// <summary>
        /// Gibt alles vor dem letzten \ oder / wieder (inklusive dem \ bzw. /)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Verzeichnis(this string path)
        {
            int i = Math.Max(path.LastIndexOf("\\"), path.LastIndexOf("/"));
            if (i < 0)
                return "";
            else
                return path.Substring(0, i + 1);
        }
        /// <summary>
        /// Gibt alles vor dem letzten \ oder / wieder und nach dem vorletzten \ oder / wieder (ohne irgendwelche \- bzw. /-Zeichen)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Ordner(this string path)
        {
            int b = Math.Max(path.LastIndexOf("\\"), path.LastIndexOf("/"));
            if (b < 0)
                return "";
            int a = Math.Max(path.LastIndexOf("\\", b - 1), path.LastIndexOf("/", b - 1));

            return path.Substring(a + 1, b - a - 1);
        }
        /// <summary>
        /// Tauscht den Dateinamen aus, behält Verzeichnis und Endung
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public static string ExchangeFileName(this string path, string newName)
        {
            return path.Verzeichnis() + newName + path.Endung();
        }

        public static string ToFileName(this string text)
        {
            return FILENAME_CLEANER.Replace(text, " ");
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
