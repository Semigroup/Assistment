using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Assistment.Extensions;

namespace Assistment.Texts
{
    public class FontGraphicsMeasurer : IFontMeasurer
    {
        //[DllImport("User32.dll")]
        //static extern int SetThreadDpiAwarenessContext(int PROCESS_DPI_AWARENESS);
        [DllImport("User32.dll")]
        static extern int GetDpiForSystem();

        // According to https://msdn.microsoft.com/en-us/library/windows/desktop/dn280512(v=vs.85).aspx
        private enum DpiAwareness
        {
            None = 0,
            SystemAware = 1,
            PerMonitorAware = 2
        }

        private Bitmap Bitmap;
        private Graphics Graphics;
        private Font Font;
        private Font FontBold;
        private Font FontItalic;
        private Font FontBoldAndItalic;

        private float lastUsedScale;
        private Font lastUsedFont;
        private Font lastUsedFontBold;
        private Font lastUsedFontItalic;
        private Font lastUsedFontBoldAndItalic;

        private float Zeilenabstand;
        private float WhiteSpace;
        private float CharToleranz;

        public static readonly StringFormat Format = new StringFormat(StringFormat.GenericTypographic)
        {
            FormatFlags = StringFormatFlags.MeasureTrailingSpaces,
            //Alignment = StringAlignment.Near,
            //HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None,
            //LineAlignment = StringAlignment.Near,
            //Trimming = StringTrimming.None
        };

    public FontGraphicsMeasurer(Font Font)
            : this(Font.Name, Font.Size)
        {

        }
        public FontGraphicsMeasurer(string schrift, float grose)
        {
            //if (Environment.OSVersion.Version >= new Version(6, 2, 9200, 0))
            //{
            //    grose *= 1.25f;// 1.248f;
            //}

            Bitmap = new Bitmap(100, 100);
            Graphics = Bitmap.GetHighGraphics();
            Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            Font = new Font(schrift, grose);
            FontBold = new Font(Font, FontStyle.Bold);
            FontItalic = new Font(Font, FontStyle.Italic);
            FontBoldAndItalic = new Font(Font, FontStyle.Italic | FontStyle.Bold);

            WhiteSpace = Font.Size / 2;//g.MeasureString(" ", Font).Width;//xMass(' ');
            CharToleranz = 0;// Font.Size / 2;
            Zeilenabstand = Font.GetHeight() * 96f / GetDpiForSystem(); //Font.Height depends on current DPI Awareness
            //DetermineLineHeight(schrift, grose);
            //Console.Out.WriteLine(schrift + ", " + grose + " : " + Zeilenabstand + " vs " + Font.Height + " vs " + Font.GetHeight());
            //Console.Out.WriteLine(GetDpiForSystem());
        }

        public Font GetFont(FontStyle style)
            => GetFont((byte)style);
        public Font GetFont(byte style)
        {
            switch (style & 3)
            {
                case 0:
                    return GetFont();
                case 1:
                    return GetFontBold();
                case 2:
                    return GetFontItalic();
                case 3:
                    return GetFontBoldAndItalic();
                default:
                    throw new NotImplementedException();
            }
        }
        public Font GetFont()
        {
            return Font;
        }
        public Font GetFontBold()
        {
            return FontBold;
        }
        public Font GetFontItalic()
        {
            return FontItalic;
        }
        public Font GetFontBoldAndItalic()
        {
            return FontBoldAndItalic;
        }

        private void Cache(float scale)
        {
            if (Math.Abs(scale - lastUsedScale) > 1)
            {
                lastUsedFont = new Font(Font.FontFamily, Font.Size * scale);
                lastUsedFontBold = new Font(Font.FontFamily, Font.Size * scale, FontStyle.Bold);
                lastUsedFontItalic = new Font(Font.FontFamily, Font.Size * scale, FontStyle.Italic);
                lastUsedFontBoldAndItalic = new Font(Font.FontFamily, Font.Size * scale, FontStyle.Bold | FontStyle.Italic);
                lastUsedScale = scale;
            }
        }
        public Font GetFont(float scale)
        {
            Cache(scale);
            return lastUsedFont;
        }
        public Font GetFontBold(float scale)
        {
            Cache(scale);
            return lastUsedFontBold;
        }
        public Font GetFontItalic(float scale)
        {
            Cache(scale);
            return lastUsedFontItalic;
        }
        public Font GetFontBoldAndItalic(float scale)
        {
            Cache(scale);
            return lastUsedFontBoldAndItalic;
        }

        public float XMass(char c, byte style)
            => XMass(c.ToString(), style);
        public float XMass(string s, byte style)
            => Size(s, style).Width;
        public float YMass(char c, byte style)
            => YMass(c.ToString(), style);
        public float YMass(string s, byte style)
            => Size(s, style).Height;
        public SizeF Size(char c, byte style)
            => Size(c.ToString(), style);
        public SizeF Size(string s, byte style)
        {
            Font font = GetFont(style);
            SizeF size = Graphics.MeasureString(s, font, int.MaxValue, Format);
            //if (s.Trim().Length == 0)
            //    size.Width += s.Count(c => c == ' ') * WhiteSpace;
            return size;
        }

        public float GetZeilenabstand()
        {
            return Zeilenabstand;
        }
        public float GetCharToleranz()
        {
            return CharToleranz;
        }
        public float GetWhitespace()
        {
            return WhiteSpace;
        }
        public float GetGeviertgrose()
        {
            return Font.Size;
        }

        public IFontMeasurer GetFontOfSize(float size)
        {
            return new FontGraphicsMeasurer(Font.Name, size);
        }

        public float XMass(char c, FontStyle style)
            => XMass(c, (byte)style);
        public float XMass(string s, FontStyle style)
            => XMass(s, (byte)style);
        public float YMass(char c, FontStyle style)
            => YMass(c, (byte)style);
        public float YMass(string s, FontStyle style)
            => YMass(s, (byte)style);
        public SizeF Size(char c, FontStyle style)
            => Size(c, (byte)style);
        public SizeF Size(string s, FontStyle style)
            => Size(s, (byte)style);

        public static FontGraphicsMeasurer operator *(FontGraphicsMeasurer FontMeasurer, float Scalar)
        {
            return new FontGraphicsMeasurer(FontMeasurer.Font.Name, FontMeasurer.Font.Size * Scalar);
        }
    }
}
