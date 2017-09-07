using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Assistment.Extensions;

namespace Assistment.Texts
{
    public interface xFont
    {
        Font getFont();
        Font getFontBold();
        Font getFontItalic();
        Font getFontBoldAndItalic();
        Font getFont(float scale);
        Font getFontBold(float scale);
        Font getFontItalic(float scale);
        Font getFontBoldAndItalic(float scale);
        float xMass(char c);
        float xMass(string s);
        float yMass(char c);
        float yMass(string s);
        SizeF Size(char c);
        SizeF Size(string s);
        float getZeilenabstand();
        float getCharToleranz();
        float getWhitespace();
        float getGeviertgrose();
        xFont getFontOfSize(float size);
    }
    public class DummyFont : xFont
    {
        public static readonly Font STANDARD_FONT = new Font("Calibri", 11);
        public static readonly xFont DUMMY_FONT = new DummyFont();


        public Font getFont()
        {
            return STANDARD_FONT;
        }
        public Font getFontBold()
        {
            return STANDARD_FONT;
        }
        public Font getFontItalic()
        {
            return STANDARD_FONT;
        }
        public Font getFontBoldAndItalic()
        {
            return STANDARD_FONT;
        }
        public Font getFont(float scale)
        {
            return STANDARD_FONT;
        }
        public Font getFontBold(float scale)
        {
            return STANDARD_FONT;
        }
        public Font getFontItalic(float scale)
        {
            return STANDARD_FONT;
        }
        public Font getFontBoldAndItalic(float scale)
        {
            return STANDARD_FONT;
        }
        public float xMass(char c)
        {
            return 0;
        }

        public float xMass(string s)
        {
            return 0;
        }

        public float yMass(char c)
        {
            return 0;
        }

        public float yMass(string s)
        {
            return 0;
        }

        public SizeF Size(char c)
        {
            return SizeF.Empty;
        }

        public SizeF Size(string s)
        {
            return SizeF.Empty;
        }

        public float getZeilenabstand()
        {
            return 0;
        }

        public float getCharToleranz()
        {
            return 0;
        }

        public float getWhitespace()
        {
            return 0;
        }
        public float getGeviertgrose()
        {
            return 0;
        }

        public xFont getFontOfSize(float size)
        {
            return new DummyFont();
        }
    }
    //public class FontMeasurer : xFont
    //{
    //    private Font font;
    //    private Font fontBold;
    //    private Font fontItalic;
    //    private Font fontBoldAndItalic;
    //    private float[] charTabelle;
    //    private float chargrose;
    //    private float Zeilenabstand;
    //    private float charToleranz;
    //    private float whitespaceWidth;

    //    private float lastUsedScale;
    //    private Font lastUsedFont;
    //    private Font lastUsedFontBold;
    //    private Font lastUsedFontItalic;
    //    private Font lastUsedFontBoldAndItalic;

    //    private static string Path = Directory.GetCurrentDirectory() + @"\xFonts\";

    //    public FontMeasurer(string schrift, float grose)
    //    {
    //        this.lastUsedScale = 0;
    //        this.font = new Font(schrift, grose);
    //        this.fontBold = new Font(schrift, grose, FontStyle.Bold);
    //        this.fontItalic = new Font(schrift, grose, FontStyle.Italic);
    //        this.fontBoldAndItalic = new Font(schrift, grose, FontStyle.Bold | FontStyle.Italic);
    //        this.chargrose = font.Size;
    //        this.Zeilenabstand = font.Height;
    //        this.charToleranz = chargrose / 2;
    //        this.charTabelle = new float[256];
    //        if (!readXFontX())
    //            measureFont();
    //        readXFontX();
    //        this.whitespaceWidth = xMass(' ');
    //    }

    //    public void measureFont()
    //    {
    //        Graphics g = Graphics.FromImage(new Bitmap(100, 100));
    //        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
    //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
    //        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
    //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    //        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

    //        if (!Directory.Exists(Path))
    //            Directory.CreateDirectory(Path);
    //        BinaryWriter w = new BinaryWriter(File.Create(Path + font.Name + ".xFontx"));
    //        for (int i = 0; i < 256; i++)
    //        {
    //            byte[] b = { (byte)i };
    //            string s = System.Text.Encoding.UTF8.GetString(b);
    //            w.Write(measureTextIterating(g, s[0], 1000));
    //        }
    //        w.Close();
    //    }

    //    public bool readXFontX()
    //    {
    //        try
    //        {
    //            BinaryReader r = new BinaryReader(File.OpenRead(Path + font.Name + ".xFontx"));
    //            for (int i = 0; i < 256; i++)
    //                charTabelle[i] = r.ReadSingle();
    //            r.Close();
    //            return true;
    //        }
    //        catch (DirectoryNotFoundException)
    //        {
    //            MessageBox.Show("Der Ordner xFonts wurde nicht gefunden!" + Environment.NewLine + "Kein Problem! Ich erschaffe ihn!");
    //            MessageBox.Show(font.Name + ".xFontx wurde nicht gefunden!" + Environment.NewLine + "Kein Problem! Ich schreib sie neu!");
    //            return false;
    //        }
    //        catch (FileNotFoundException)
    //        {
    //            MessageBox.Show(font.Name + ".xFontx wurde nicht gefunden!" + Environment.NewLine + "Kein Problem! Ich schreib sie neu!");
    //            return false;
    //        }
    //    }
    //    public float measureTextIterating(Graphics g, char c, int it)
    //    {
    //        string s = new string(c, it);
    //        return ((float)TextRenderer.MeasureText(g, s, font,
    //            new Size(int.MaxValue, int.MaxValue),
    //            TextFormatFlags.Internal).Width) / (it * (chargrose));
    //    }
    //    public Font getFont()
    //    {
    //        return font;
    //    }
    //    public Font getFontBold()
    //    {
    //        return fontBold;
    //    }
    //    public Font getFontItalic()
    //    {
    //        return fontItalic;
    //    }
    //    public Font getFontBoldAndItalic()
    //    {
    //        return fontBoldAndItalic;
    //    }
    //    private void cache(float scale)
    //    {
    //        if (Math.Abs(scale - lastUsedScale) > 1)
    //        {
    //            lastUsedFont = new Font(font.FontFamily, font.Size * scale);
    //            lastUsedFontBold = new Font(font.FontFamily, font.Size * scale, FontStyle.Bold);
    //            lastUsedFontItalic = new Font(font.FontFamily, font.Size * scale, FontStyle.Italic);
    //            lastUsedFontBoldAndItalic = new Font(font.FontFamily, font.Size * scale, FontStyle.Bold | FontStyle.Italic);
    //            lastUsedScale = scale;
    //        }
    //    }
    //    public Font getFont(float scale)
    //    {
    //        cache(scale);
    //        return lastUsedFont;
    //    }
    //    public Font getFontBold(float scale)
    //    {
    //        cache(scale);
    //        return lastUsedFontBold;
    //    }
    //    public Font getFontItalic(float scale)
    //    {
    //        cache(scale);
    //        return lastUsedFontItalic;
    //    }
    //    public Font getFontBoldAndItalic(float scale)
    //    {
    //        cache(scale);
    //        return lastUsedFontBoldAndItalic;
    //    }
    //    public float xMass(char c)
    //    {
    //        int i = (int)c;
    //        if (i >= 256)
    //            return charTabelle[(int)' '] * chargrose;
    //        else
    //            return charTabelle[i] * chargrose;
    //    }
    //    public float xMass(string s)
    //    {
    //        float f = 0;
    //        float fMax = 0;
    //        for (int i = 0; i < s.Length; i++)
    //        {
    //            if (s[i] == '\n')
    //            {
    //                fMax = Math.Max(f, fMax);
    //                f = 0;
    //            }
    //            else
    //                f += xMass(s[i]);
    //        }
    //        fMax = Math.Max(f, fMax);
    //        return fMax;
    //    }
    //    public float yMass(char c)
    //    {
    //        string s = "" + c;
    //        return TextRenderer.MeasureText(s, font).Height;
    //    }
    //    public float yMass(string s)
    //    {
    //        return TextRenderer.MeasureText(s, font).Height;
    //    }
    //    public float getZeilenabstand()
    //    {
    //        return Zeilenabstand;
    //    }
    //    public float getCharToleranz()
    //    {
    //        return charToleranz;
    //    }
    //    /// <summary>
    //    /// returns the width of a ' '
    //    /// </summary>
    //    /// <returns></returns>
    //    public float getWhitespace()
    //    {
    //        return whitespaceWidth;
    //    }
    //    public float getGeviertgrose()
    //    {
    //        return chargrose;
    //    }
    //    public override string ToString()
    //    {
    //        return font.Name + ", " + font.Size + "pt";
    //    }

    //    public SizeF Size(char c)
    //    {
    //        return new SizeF(xMass(c), yMass(c));
    //    }
    //    public SizeF Size(string s)
    //    {
    //        return new SizeF(xMass(s), yMass(s));
    //    }
    //}
    public class FontGraphicsMeasurer : xFont
    {
        private Bitmap b;
        private Graphics g;
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

            b = new Bitmap(100, 100);
            g = b.GetHighGraphics();
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            Font = new Font(schrift, grose);
            FontBold = new Font(Font, FontStyle.Bold);
            FontItalic = new Font(Font, FontStyle.Italic);
            FontBoldAndItalic = new Font(Font, FontStyle.Italic | FontStyle.Bold);

            Zeilenabstand = Font.Height;
            WhiteSpace = Font.Size / 2;//g.MeasureString(" ", Font).Width;//xMass(' ');
            CharToleranz = 0;// Font.Size / 2;
        }

        public Font getFont()
        {
            return Font;
        }
        public Font getFontBold()
        {
            return FontBold;
        }
        public Font getFontItalic()
        {
            return FontItalic;
        }
        public Font getFontBoldAndItalic()
        {
            return FontBoldAndItalic;
        }

        private void cache(float scale)
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
        public Font getFont(float scale)
        {
            cache(scale);
            return lastUsedFont;
        }
        public Font getFontBold(float scale)
        {
            cache(scale);
            return lastUsedFontBold;
        }
        public Font getFontItalic(float scale)
        {
            cache(scale);
            return lastUsedFontItalic;
        }
        public Font getFontBoldAndItalic(float scale)
        {
            cache(scale);
            return lastUsedFontBoldAndItalic;
        }

        public float xMass(char c)
        {
            return Size(c).Width;
        }
        public float xMass(string s)
        {
            return Size(s).Width;
        }
        public float yMass(char c)
        {
            return Size(c).Height;
        }
        public float yMass(string s)
        {
            return Size(s).Height;
        }
        public SizeF Size(char c)
        {
            return Size(c + "");
        }
        public SizeF Size(string s)
        {
            SizeF size = g.MeasureString(s, Font, int.MaxValue, StringFormat.GenericTypographic);
            size.Width += s.Count(c => c == ' ') * WhiteSpace;
            return size;
        }

        public float getZeilenabstand()
        {
            return Zeilenabstand;
        }
        public float getCharToleranz()
        {
            return CharToleranz;
        }
        public float getWhitespace()
        {
            return WhiteSpace;
        }
        public float getGeviertgrose()
        {
            return Font.Size;
        }

        public xFont getFontOfSize(float size)
        {
            return new FontGraphicsMeasurer(Font.Name, size);
        }

        public static FontGraphicsMeasurer operator *(FontGraphicsMeasurer FontMeasurer, float Scalar)
        {
            return new FontGraphicsMeasurer(FontMeasurer.Font.Name, FontMeasurer.Font.Size * Scalar);
        }
    }

    public static class FontErweiterer
    {
        public static FontGraphicsMeasurer GetMeasurer(this Font Font)
        {
            return new FontGraphicsMeasurer(Font);
        }
    }
}
