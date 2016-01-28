﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

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
    }
    public class FontMeasurer : xFont
    {
        private Font font;
        private Font fontBold;
        private Font fontItalic;
        private Font fontBoldAndItalic;
        private float[] charTabelle;
        private float chargrose;
        private float Zeilenabstand;
        private float charToleranz;
        private float whitespaceWidth;

        private float lastUsedScale;
        private Font lastUsedFont;
        private Font lastUsedFontBold;
        private Font lastUsedFontItalic;
        private Font lastUsedFontBoldAndItalic;

        private static string Path = Directory.GetCurrentDirectory() + @"\xFonts\";

        public FontMeasurer(string schrift, float grose)
        {
            this.lastUsedScale = 0;
            this.font = new Font(schrift, grose);
            this.fontBold = new Font(schrift, grose, FontStyle.Bold);
            this.fontItalic = new Font(schrift, grose, FontStyle.Italic);
            this.fontBoldAndItalic = new Font(schrift, grose, FontStyle.Bold | FontStyle.Italic);
            this.chargrose = font.Size;
            this.Zeilenabstand = font.Height;
            this.charToleranz = chargrose / 2;
            this.charTabelle = new float[256];
            if (!readXFontX())
                measureFont();
            readXFontX();
            this.whitespaceWidth = xMass(' ');
        }

        public void measureFont()
        {
            Graphics g = Graphics.FromImage(new Bitmap(100, 100));
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            BinaryWriter w = new BinaryWriter(File.Create(Path + font.Name + ".xFontx"));
            for (int i = 0; i < 256; i++)
            {
                byte[] b = { (byte)i };
                string s = System.Text.Encoding.UTF8.GetString(b);
                w.Write(measureTextIterating(g, s[0], 1000));
            }
            w.Close();
        }

        public bool readXFontX()
        {
            try
            {
                BinaryReader r = new BinaryReader(File.OpenRead(Path + font.Name + ".xFontx"));
                for (int i = 0; i < 256; i++)
                    charTabelle[i] = r.ReadSingle();
                r.Close();
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Der Ordner xFonts wurde nicht gefunden!" + Environment.NewLine + "Kein Problem! Ich erschaffe ihn!");
                MessageBox.Show(font.Name + ".xFontx wurde nicht gefunden!" + Environment.NewLine + "Kein Problem! Ich schreib sie neu!");
                return false;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(font.Name + ".xFontx wurde nicht gefunden!" + Environment.NewLine + "Kein Problem! Ich schreib sie neu!");
                return false;
            }
        }
        public float measureTextIterating(Graphics g, char c, int it)
        {
            string s = new string(c, it);
            return ((float)TextRenderer.MeasureText(g, s, font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.Internal).Width) / (it * (chargrose));
        }
        public Font getFont()
        {
            return font;
        }
        public Font getFontBold()
        {
            return fontBold;
        }
        public Font getFontItalic()
        {
            return fontItalic;
        }
        public Font getFontBoldAndItalic()
        {
            return fontBoldAndItalic;
        }
        private void cache(float scale)
        {
            if (scale != lastUsedScale)
            {
                lastUsedFont = new Font(font.FontFamily, font.Size * scale);
                lastUsedFontBold = new Font(font.FontFamily, font.Size * scale, FontStyle.Bold);
                lastUsedFontItalic = new Font(font.FontFamily, font.Size * scale, FontStyle.Italic);
                lastUsedFontBoldAndItalic = new Font(font.FontFamily, font.Size * scale, FontStyle.Bold | FontStyle.Italic);
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
            return charTabelle[(int)c] * chargrose;
        }
        public float xMass(string s)
        {
            float f = 0;
            float fMax = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\n')
                {
                    fMax = Math.Max(f, fMax);
                    f = 0;
                }
                else
                    f += xMass(s[i]);
            }
            fMax = Math.Max(f, fMax);
            return fMax;
        }
        public float yMass(char c)
        {
            string s = "" + c;
            return TextRenderer.MeasureText(s, font).Height;
        }
        public float yMass(string s)
        {
            return TextRenderer.MeasureText(s, font).Height;
        }
        public float getZeilenabstand()
        {
            return Zeilenabstand;
        }
        public float getCharToleranz()
        {
            return charToleranz;
        }
        /// <summary>
        /// returns the width of a ' '
        /// </summary>
        /// <returns></returns>
        public float getWhitespace()
        {
            return whitespaceWidth;
        }
        public float getGeviertgrose()
        {
            return chargrose;
        }
        public override string ToString()
        {
            return font.Name + ", " + font.Size + "pt";
        }


        public SizeF Size(char c)
        {
            return new SizeF(xMass(c), yMass(c));
        }

        public SizeF Size(string s)
        {
            return new SizeF(xMass(s), yMass(s));
        }
    }
}
