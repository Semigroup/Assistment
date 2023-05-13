using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using iTextSharp.text.pdf;
using System.Drawing.Imaging;
using Assistment.Drawing.Geometries.Extensions;
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
            if (!bilder.TryGetValue(img, out iTextSharp.text.Image result))
            {
                if (ImageFormatTestEquality(img.RawFormat, ImageFormat.Jpeg))
                    result = iTextSharp.text.Image.GetInstance(img, ImageFormat.Jpeg);
                else
                    result = iTextSharp.text.Image.GetInstance(img, color: null);

                bilder.Add(img, result);
            }
            return result;
        }
        private static bool ImageFormatTestEquality(ImageFormat format1, ImageFormat format2)
                => format1.Guid == format2.Guid;
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
