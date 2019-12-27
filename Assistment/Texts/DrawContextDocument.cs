using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using iTextSharp.text.pdf;
using System.Drawing.Imaging;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;

namespace Assistment.Texts
{
    public class DrawContextDocument : DrawContext
    {
        /// <summary>
        /// Standard-Größenangaben müssen mit diesem Wert multipliziert werden, bevor sie an die itext-Api übergeben werden.
        /// </summary>
        public const float factor = 0.6f;
        public const float FontFactor = 1.0f;
        public const float FontOffset = 2.7f;

        private iTextSharp.text.pdf.PdfContentByte pCon;
        private float yOff;
        private SortedDictionary<Image, iTextSharp.text.Image> bilder;
        private SortedDictionary<Color, iTextSharp.text.BaseColor> farben;
        //private SortedDictionary<Font, BaseFont> fonts;
        public readonly iTextSharp.text.BaseColor STANDARD = iTextSharp.text.BaseColor.BLACK;
        private static string FontPath = Directory.GetCurrentDirectory() + @"\Fonts\";
        private static bool madeBaseFonts = false;
        /// <summary>
        /// 0: Calibri
        /// <para>1: Calibri fett</para>
        /// <para>2: Calibri italy</para>
        /// <para>3: Calibri fett+italy</para>
        /// <para>4: Exocet</para>
        /// <para>5: Exocet fett</para>
        /// <para>6: Exocet italy</para>
        /// <para>7: Exocet fett+italy</para>
        /// </summary>
        private static BaseFont[] fonts;

        //public struct SeitennummerOptionen
        //{
        //    public SeitennummerOptionen(float x, float y)
        //    {
        //        this.ort = new PointF(x, y);
        //        this.font = new FontGraphicsMeasurer("Calibri", 11);
        //        this.backGerade = Brushes.White;
        //        this.backUngerade = Brushes.White;
        //        this.fontfarbeGerade = Brushes.Black;
        //        this.fontfarbeUngerade = Brushes.Black;
        //        this.text = "";

        //        this.aktiv = false;
        //    }

        //    /// <summary>
        //    /// ignoriert den einzug/margin
        //    /// </summary>
        //    public PointF ort;
        //    public xFont font;
        //    public Brush backGerade;
        //    public Brush backUngerade;
        //    public Brush fontfarbeGerade;
        //    public Brush fontfarbeUngerade;

        //    public string text;

        //    public bool aktiv;
        //}
        //public static SeitennummerOptionen STANDARD_SN_OPTION = new SeitennummerOptionen(10, 10);
        //public SeitennummerOptionen seitennummer = STANDARD_SN_OPTION;

        private class Sortierer : IComparer<Image>, IComparer<Color>, IComparer<Font>
        {
            public int Compare(Image x, Image y)
            {
                return x.GetHashCode() - y.GetHashCode();
            }

            public int Compare(Color x, Color y)
            {
                return x.ToArgb() - y.ToArgb();
            }

            public int Compare(System.Drawing.Font x, System.Drawing.Font y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }

        public DrawContextDocument(iTextSharp.text.pdf.PdfContentByte pCon)
            : this(pCon, float.MaxValue)
        {
        }
        public DrawContextDocument(iTextSharp.text.pdf.PdfContentByte pCon, float Bildhohe)
        {
            this.pCon = pCon;
            this.yOff = pCon.PdfDocument.PageSize.Height;
            this.bilder = new SortedDictionary<Image, iTextSharp.text.Image>(new Sortierer());
            this.farben = new SortedDictionary<Color, iTextSharp.text.BaseColor>(new Sortierer());
            //this.fonts = new SortedDictionary<Font, BaseFont>(new sortierer());
            this.Backcolor = Brushes.White;
            this.Bildhohe = Bildhohe;

            if (!madeBaseFonts)
            {
                madeBaseFonts = true;
                fonts = new BaseFont[12];
                fonts[0] = BaseFont.CreateFont(FontPath + "calibri.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                fonts[1] = BaseFont.CreateFont(FontPath + "calibrib.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                fonts[2] = BaseFont.CreateFont(FontPath + "calibrii.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                fonts[3] = BaseFont.CreateFont(FontPath + "calibriz.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);

                fonts[4] = BaseFont.CreateFont(FontPath + "Exocet2.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                fonts[5] = BaseFont.CreateFont(FontPath + "Exocet1.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                fonts[6] = BaseFont.CreateFont(FontPath + "Exocet2.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                fonts[7] = BaseFont.CreateFont(FontPath + "Exocet1.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);

                fonts[8] = BaseFont.CreateFont(FontPath + "Plain Germanica.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                fonts[9] = BaseFont.CreateFont(FontPath + "Plain Germanica.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                fonts[10] = BaseFont.CreateFont(FontPath + "Plain Germanica.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                fonts[11] = BaseFont.CreateFont(FontPath + "Plain Germanica.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            }
        }

        //private string getFontFile(Font Font)
        //{
        //    Console.WriteLine(Font.Name + " -> " + Font.OriginalFontName + " -> " + Font.SystemFontName);
        //    RegistryKey keys = null;
        //    try
        //    {
        //        keys = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Fonts", false);
        //        if (keys == null)
        //            keys = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Fonts", false);
        //        if (keys == null)
        //            throw new Exception("Can't find font registry database.");
        //        StringBuilder name = new StringBuilder(keys.Name);
        //        if (Font.Bold)
        //            name.Append(" Bold");
        //        if (Font.Italic)
        //            name.Append(" Italic");
        //        name.Append(" (TrueType)");
        //        string fullname = name.ToString();
        //        string basename = keys.Name + " (TrueType)";
        //        Console.WriteLine(fullname + ",  " + basename);
        //        object file = keys.GetValue(fullname);
        //        if (file == null)
        //            file = keys.GetValue(basename);
        //        if (file != null)
        //            return file.ToString();
        //        return null;
        //    }
        //    finally
        //    {
        //        if (keys != null)
        //            keys.Dispose();
        //    }
        //}
        private BaseFont getFont(Font f)
        {
            int i;
            switch (f.Name[0])
            {
                default:
                case 'C':
                case 'c':
                    i = 0;
                    break;
                case 'E':
                case 'e':
                    i = 4;
                    break;
                case 'p':
                case 'P':
                    i = 8;
                    break;
            }
            if ((f.Style & FontStyle.Bold) != 0)
                i += 1;
            if ((f.Style & FontStyle.Italic) != 0)
                i += 2;
            return fonts[i];
            //if (fonts.ContainsKey(f))
            //    return fonts[f];
            //else
            //{
            //    string fontFile = getFontFile(f);
            //    Console.WriteLine(fontFile + " von " + f.Name + ", " + f.Size);
            //    BaseFont bf = BaseFont.CreateFont(fontFile, BaseFont.CP1252, BaseFont.EMBEDDED);
            //    fonts.Add(f, bf);
            //    return bf;
            //}
        }
        private iTextSharp.text.BaseColor getColor(Color c)
        {
            iTextSharp.text.BaseColor result;
            if (!farben.TryGetValue(c, out result))
            {
                result = new iTextSharp.text.BaseColor(c);
                farben.Add(c, result);
            }
            return result;
        }
        private iTextSharp.text.BaseColor getColor(Brush b)
        {
            if (b is SolidBrush)
                return getColor(((SolidBrush)b).Color);
            else return STANDARD;
        }
        private iTextSharp.text.BaseColor getColor(Pen p)
        {
            return getColor(p.Color);
        }
        private iTextSharp.text.Image getImage(Image img)
        {
            iTextSharp.text.Image result;
            if (!bilder.TryGetValue(img, out result))
            {
                byte[] b1 = img.RawFormat.Guid.ToByteArray();
                byte[] b2 = ImageFormat.Jpeg.Guid.ToByteArray();
                bool equals = true;
                for (int i = 0; i < b1.Length; i++)
                    if (b1[i] != b2[i])
                        equals = false;
                if (equals)
                    result = iTextSharp.text.Image.GetInstance(img, img.RawFormat);
                else
                    result = iTextSharp.text.Image.GetInstance(img, color: null);

                bilder.Add(img, result);
            }
            return result;
        }
        private void adjustPageNumber(float y)
        {
            //Console.WriteLine("adjustPageNumber: " + y);
            //Console.WriteLine("yOff: " + yOff);
            const float HOHEN_TOLERANZ = 1;
            y -= HOHEN_TOLERANZ;
            if (pCon.PdfDocument.PageNumber * pCon.PdfDocument.PageSize.Height < factor * y)
            {
                for (int i = pCon.PdfDocument.PageNumber; i < (int)Math.Ceiling(factor * y / pCon.PdfDocument.PageSize.Height); i++)
                {
                    //schreibSeitennummer();
                    yOff += pCon.PdfDocument.PageSize.Height;
                    //Console.WriteLine("yOff: " + yOff);
                    pCon.PdfDocument.NewPage();
                }
            }
        }
        public override void NewPage()
        {
            float y = pCon.PdfDocument.PageNumber * pCon.PdfDocument.PageSize.Height / factor;
            adjustPageNumber(y + 2);
        }

        //private void schreibSeitennummer()
        //{
        //    if (seitennummer.aktiv)
        //    {
        //        string s = pCon.PdfDocument.PageNumber.ToString() + " " + seitennummer.text;

        //        RectangleF r = new RectangleF();
        //        r.Location = seitennummer.ort;
        //        r.Y += (pCon.PdfDocument.PageNumber - 1) * pCon.PdfDocument.PageSize.Height / factor;
        //        r.Height = seitennummer.font.yMass(s);
        //        r.Width = seitennummer.font.xMass(s);

        //        if (pCon.PdfDocument.PageNumber % 2 == 0)
        //        {
        //            fillRectangle(seitennummer.backGerade, r);
        //            drawString(s, seitennummer.font.getFont(), seitennummer.fontfarbeGerade, r.Location, r.Height);
        //        }
        //        else
        //        {
        //            fillRectangle(seitennummer.backUngerade, r);
        //            drawString(s, seitennummer.font.getFont(), seitennummer.fontfarbeUngerade, r.Location, r.Height);
        //        }
        //    }
        //}

        private void SetPen(Pen Pen)
        {
            pCon.SetColorStroke(getColor(Pen));
            pCon.SetLineWidth(Pen.Width * factor);
        }

        public override void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            adjustPageNumber(y + height);
            pCon.Rectangle(factor * x, yOff - factor * y, factor * width, -factor * height);
            SetPen(pen);
            pCon.Stroke();
        }
        public override void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            adjustPageNumber(y + height);
            pCon.SaveState();

            iTextSharp.text.BaseColor Color = getColor(brush);

            PdfGState PdfGState = new PdfGState();
            PdfGState.FillOpacity = Color.A / 255f;
            PdfGState.StrokeOpacity = 0;
            pCon.SetGState(PdfGState);

            pCon.Rectangle(factor * x, yOff - factor * y, factor * width, -factor * height);
            pCon.SetColorFill(Color);
            pCon.FillStroke();

            pCon.RestoreState();
        }
        public override void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            adjustPageNumber(Math.Max(y1, y2));
            pCon.MoveTo(factor * x1, (yOff - factor * y1));
            pCon.LineTo(factor * x2, (yOff - factor * y2));
            SetPen(pen);
            pCon.Stroke();
        }
        public override void DrawString(string text, Font font, Brush brush, float x, float y, float height)
        {
            adjustPageNumber(y + height);
            pCon.SetColorFill(getColor(brush));
            pCon.SetFontAndSize(getFont(font), font.Size * FontFactor);
            pCon.BeginText();
            pCon.SetTextMatrix(factor * x, yOff - factor * (y + height) + FontOffset);//+ 2
            pCon.ShowText(text);
            pCon.EndText();
            pCon.Stroke();
        }
        public override void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            adjustPageNumber(y + height);
            pCon.Ellipse(factor * x, yOff - factor * y, factor * width, -factor * height);
            SetPen(pen);
            pCon.Stroke();
        }
        public override void FillEllipse(Brush brush, float x, float y, float width, float height)
        {
            adjustPageNumber(y + height);
            pCon.Ellipse(factor * x, yOff - factor * y, factor * width, -factor * height);
            pCon.SetColorFill(getColor(brush));
            pCon.FillStroke();
        }
        public override void DrawPolygon(Pen pen, PointF[] polygon)
        {
            adjustPageNumber(polygon[0].Y);
            pCon.MoveTo(factor * polygon[0].X, (yOff - factor * polygon[0].Y));
            for (int i = 1; i < polygon.Length; i++)
                pCon.LineTo(factor * polygon[i].X, (yOff - factor * polygon[i].Y));
            SetPen(pen);
            pCon.Stroke();
        }
        public override void DrawImage(Image img, float x, float y)
        {
            DrawImage(img, x, y, img.Width, img.Height);
        }
        public override void DrawImage(Image img, float x, float y, float width, float height, ImageAttributes attributes)
        {
            adjustPageNumber(y);
            //iTextSharp.text.Image i = getImage(img);
            //i.SetAbsolutePosition(factor * x, yOff - factor * (y + height));
            //i.ScaleAbsolute(factor * width, factor * height);
            //pCon.AddImage(i);
            pCon.AddImage(getImage(img), factor * width, 0, 0, factor * height, factor * x, yOff - factor * (y + height));
        }

        public override void Dispose()
        {
            //schreibSeitennummer();
            pCon = null;
            bilder = null;
            farben = null;
        }

        public override void DrawClippedImage(Image img, float x, float y, RectangleF source)
        {
            DrawClippedImage(img, new RectangleF(new PointF(x, y), source.Size), source);
        }
        public override void DrawClippedImage(Image img, RectangleF destination, RectangleF source)
        {
            RectangleF T = source.GetTransformation(destination);
            RectangleF Pic = new RectangleF(new PointF(), img.Size);
            RectangleF Dest = Pic.Transform(T);
            adjustPageNumber(Dest.Y);

            pCon.SaveState();
            pCon.Rectangle(factor * destination.X,
                yOff - factor * destination.Y - factor * destination.Height,
                factor * destination.Width,
                factor * destination.Height);
            pCon.Clip();
            pCon.NewPath();

            DrawImage(img, Dest);

            pCon.RestoreState();
        }

        public override void FillPolygon(System.Drawing.Brush Brush, System.Drawing.PointF[] polygon)
        {
            float bottom = polygon.Map(p => p.Y).Max();
            adjustPageNumber(bottom);
            pCon.MoveTo(factor * polygon[0].X, (yOff - factor * polygon[0].Y));
            for (int i = 1; i < polygon.Length; i++)
                pCon.LineTo(factor * polygon[i].X, (yOff - factor * polygon[i].Y));
            pCon.SetColorFill(getColor(Brush));
            pCon.Fill();
        }
    }
}
