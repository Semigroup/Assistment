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
        public static readonly Regex FILENAME_CLEANER_SINGLE_CHARACTER = new Regex(@"\W");

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
            RectangleF rf = new RectangleF(Align(size, layoutRectangle, format), size);

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
            return fileName.Substring(j + 1, i - j - 1);
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
        /// <summary>
        /// Ersetzt alles, was W+ matcht durch ein Whitespace
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToFileName(this string text)
        {
            return FILENAME_CLEANER.Replace(text, " ");
        }
        /// <summary>
        /// Ersetzt alles, was W matcht durch replaceIllegalCharacterByThis
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToFileName(this string text, string replaceIllegalCharacterByThis)
        {
            return FILENAME_CLEANER_SINGLE_CHARACTER.Replace(text, replaceIllegalCharacterByThis);
        }
        /// <summary>
        /// Falls bereits eine Datei mit diesem Path vorhanden ist, wird der FileName angepasst, bis er nicht mehr mit einer anderen Datei kollidiert.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string DecollideFilename(this string filepath)
        {
            if (File.Exists(filepath))
            {
                string directory = Path.GetDirectoryName(filepath);
                string name = Path.GetFileNameWithoutExtension(filepath);
                string exstension = Path.GetExtension(filepath);

                int i = name.Length;
                for (; i >= 0; i--)
                    if (!('0' <= name[i - 1] && name[i - 1] <= '9'))
                        break;
                int number = i < name.Length ? int.Parse(name.Substring(i)) : 0;
                name = name.Substring(0, i);

                while (true)
                {
                    number++;
                    string NewFilePath = Path.Combine(directory, name + number + exstension);
                    if (!File.Exists(NewFilePath))
                        return NewFilePath;
                }
            }
            else
                return filepath;
        }
        /// <summary>
        /// macht slashs zu backslashs
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FlipForwardSlash(this string text)
        {
            return text.Replace('\\', '/');
        }
        /// <summary>
        /// Splittet anhand von Anführungszeichen, vor denen sich direkt keine ungerade Anzahl an Backslashes befindet.
        /// </summary>
        /// <returns></returns>
        public static string[] SplitNotEscapedQuotationMarks(this string text)
        {
            List<string> Parts = new List<string>();
            int start = 0;
            int current = 0;
            int numberOfBackslashes = 0;
            while (current < text.Length)
            {
                switch (text[current])
                {
                    case '\\':
                        numberOfBackslashes++;
                        break;
                    case '"':
                        if (numberOfBackslashes % 2 == 0)
                        {
                            Parts.Add(text.Substring(start, current - start));
                            start = current + 1;
                        }
                        numberOfBackslashes = 0;
                        break;
                    default:
                        numberOfBackslashes = 0;
                        break;
                }
                current++;
            }
            Parts.Add(text.Substring(start, current - start));
            return Parts.ToArray();
        }
        //public static string[] SplitInNotEscapedAreas(this string text, StringSplitOptions Options, params string[] separators)
        //{
        //    string[] exteriorSplits = SplitNotEscapedQuotationMarks(text);
        //    List<string[]> interiorSplits = new List<string[]>();
        //    for (int i = 0; i < exteriorSplits.Length; i += 2)
        //        interiorSplits.Add(exteriorSplits[i].Split(separators, Options));
        //    List<string> result = new List<string>();
        //    int k = 0;
        //    string envelopeQuote = "";
        //    foreach (var item in interiorSplits)
        //    {
        //        if (item.Length == 0 || item.Length == 1)
        //        {
        //            if (exteriorSplits[k].Length > 0 && envelopeQuote.Length > 0)
        //            {
        //                result.Add(envelopeQuote);
        //                envelopeQuote = "";
        //            }
        //            else if(k + 1 < exteriorSplits.Length)
        //                envelopeQuote += "\"" + exteriorSplits[k + 1] + "\"";
        //        }
        //        else
        //        {

        //        }
        //        k += 2;
        //    }
        //    if (envelopeQuote.Length > 0)
        //        result.Add(envelopeQuote);
        //    return result.ToArray();
        //}
        private static PointF Align(SizeF size, RectangleF layout, StringFormat format)
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
