﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using System.IO;

namespace Assistment.Texts
{
    public abstract class DrawBox
    {
        public RectangleF box;
        public bool endsLine;

        /// <summary>
        /// gibt einen Wert zurück, der ungefähr breite*höhe entsprechen soll
        /// </summary>
        /// <returns></returns>
        public abstract float getSpace();
        /// <summary>
        /// this method may only return zero iff space is zero, too
        /// </summary>
        /// <returns></returns>
        public abstract float getMin();
        public abstract float getMax();

        /// <summary>
        /// updates the values of min, max and space
        /// </summary>
        public abstract void update();
        /// <summary>
        /// computes the box of this and the boxes of any subdrawitems
        /// </summary>
        /// <param name="box"></param>
        public abstract void setup(RectangleF box);
        /// <summary>
        /// draws the components of this item
        /// <para>use this after setup()</para>
        /// </summary>
        /// <param name="con"></param>
        public abstract void draw(DrawContext con);
        /// <summary>
        /// gibt möglichst tiefe Kopien zurück
        /// </summary>
        /// <returns></returns>
        public abstract DrawBox clone();
        public abstract void InStringBuilder(StringBuilder sb, string tabs);
        public virtual bool check(PointF punkt)
        {
            return box.Contains(punkt);
        }
        public bool check(float x, float y)
        {
            return check(new PointF(x, y));
        }

        protected static void show(params object[] objekte)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in objekte)
                sb.AppendLine(item.ToString());
            System.Windows.Forms.MessageBox.Show(sb.ToString());
        }
        protected static Color HexToColor(string hex)
        {
            int c = 0;
            int j;
            for (int i = 7; i >= 0; i--)
            {
                c = c << 4;
                j = (hex[7 - i] - '0');
                if (j > 9)
                    j -= 7;
                if (j > 16)
                    j -= 32;
                c += j;
            }
            return Color.FromArgb(c);
        }

        public void createImage(string name)
        {
            const float ab = 10;
            this.setup(new RectangleF(ab, ab, this.getMin(), 0));
            Bitmap b = new Bitmap((int)(box.Width + 2 * ab), (int)(box.Height + 2 * ab));
            Graphics g = Graphics.FromImage(b);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            g.Clear(Color.White);
            DrawContextGraphics dcg = new DrawContextGraphics(g, Brushes.White, box.Height + ab);
            this.draw(dcg);
            b.Save(name + ".png");
            dcg.Dispose();
            b.Dispose();
        }
        public void createImage(string name, float width, float height)
        {
            const float ab = 10;
            this.setup(new RectangleF(ab, ab, width, 0));
            Bitmap b = new Bitmap((int)(box.Width + 2 * ab), (int)(box.Height + 2 * ab));
            Graphics g = Graphics.FromImage(b);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            g.Clear(Color.White);
            DrawContextGraphics dcg = new DrawContextGraphics(g, Brushes.White, height + ab);
            this.draw(dcg);
            b.Save(name + ".png");
            dcg.Dispose();
            b.Dispose();
        }
        //public void createPDF(string name)
        //{
        //    const float ab = 00;
        //    this.setup(new RectangleF(ab, ab, this.getMin(), 0));

        //    iTextSharp.text.Document doc = new iTextSharp.text.Document();
        //    PdfWriter writer = PdfWriter.GetInstance(doc, System.IO.File.Create(name + ".pdf"));

        //    doc.Open();
        //    PdfContentByte pCon = writer.DirectContent;
        //    pCon.SetLineWidth(0.3f);
        //    DrawContextDocument dcd = new DrawContextDocument(pCon, box.Height + ab);
        //    this.draw(dcd);
        //    doc.Close();
        //    dcd.Dispose();
        //    writer.Dispose();
        //}
        public void createDinA3PDF(string name)
        {
            const float ab = 00;
            this.update();
            this.setup(new RectangleF(ab, ab, this.getMin(), 0));

            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetPageSize(iTextSharp.text.PageSize.A3);
            PdfWriter writer = PdfWriter.GetInstance(doc, System.IO.File.Create(name + ".pdf"));

            doc.Open();
            PdfContentByte pCon = writer.DirectContent;
            pCon.SetLineWidth(0.3f);
            DrawContextDocument dcd = new DrawContextDocument(pCon, box.Height + ab);
            this.draw(dcd);
            doc.Close();
            dcd.Dispose();
            writer.Dispose();
        }
        public void createPDF(string name)
        {
            this.createPDF(name, 1000, float.MaxValue);
        }
        public void createPDF(string name, float width, float height)
        {
            const float ab = 00;
            this.update();
            this.setup(new RectangleF(ab, ab, width, 0));

            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, System.IO.File.Create(name + ".pdf"));

            doc.Open();
            PdfContentByte pCon = writer.DirectContent;
            pCon.SetLineWidth(0.3f);
            DrawContextDocument dcd = new DrawContextDocument(pCon, height + ab);
            this.draw(dcd);
            doc.Close();
            dcd.Dispose();
            writer.Dispose();
        }
        public void createLog(string name, float width)
        {
            const float ab = 00;
            this.update();
            this.setup(new RectangleF(ab, ab, width, 0));

            StringBuilder sb = new StringBuilder();
            this.InStringBuilder(sb, "");
            StreamWriter f = File.CreateText(Directory.GetCurrentDirectory() + @"\" + name + ".txt");
            f.Write(sb.ToString());
            f.Close();
            f.Dispose();
        }

        public static CString operator +(DrawBox box1, DrawBox box2)
        {
            CString t = new CString();
            t.add(box1);
            t.add(box2);
            return t;
        }
        public static DrawContainer operator +(DrawContainer box1, DrawBox box2)
        {
            DrawContainer t = (DrawContainer)box1.clone();
            t.add(box2);
            return t;
        }
    }
    public abstract class DrawContainer : DrawBox, IEnumerable<DrawBox>
    {
        public float alignment = 0;
        public xFont preferedFont;

        public abstract void add(DrawBox word);
        public abstract void addRange(DrawContainer container);
        #region addWort Überladungen
        public void addWort(string text)
        {
            this.addWort(text, Brushes.Black, preferedFont, 0, Pens.Black);
        }
        public void addWort(string text, Pen pen)
        {
            this.addWort(text, Brushes.Black, preferedFont, 0, pen);
        }
        public void addWort(string text, byte style)
        {
            this.addWort(text, Brushes.Black, preferedFont, style, Pens.Black);
        }
        public void addWort(string text, byte style, Pen pen)
        {
            this.addWort(text, Brushes.Black, preferedFont, style, pen);
        }
        public void addWort(string text, xFont font)
        {
            this.addWort(text, Brushes.Black, font, 0, Pens.Black);
        }
        public void addWort(string text, xFont font, Pen pen)
        {
            this.addWort(text, Brushes.Black, font, 0, pen);
        }
        public void addWort(string text, xFont font, byte style)
        {
            this.addWort(text, Brushes.Black, font, style, Pens.Black);
        }
        public void addWort(string text, xFont font, byte style, Pen pen)
        {
            this.addWort(text, Brushes.Black, font, style, pen);
        }
        public void addWort(string text, Brush brush)
        {
            this.addWort(text, brush, preferedFont, 0, Pens.Black);
        }
        public void addWort(string text, Brush brush, Pen pen)
        {
            this.addWort(text, brush, preferedFont, 0, pen);
        }
        public void addWort(string text, Brush brush, byte style)
        {
            this.addWort(text, brush, preferedFont, style, Pens.Black);
        }
        public void addWort(string text, Brush brush, byte style, Pen pen)
        {
            this.addWort(text, brush, preferedFont, style, pen);
        }
        public void addWort(string text, Brush brush, xFont font)
        {
            this.addWort(text, brush, font, 0, Pens.Black);
        }
        public void addWort(string text, Brush brush, xFont font, Pen pen)
        {
            this.addWort(text, brush, font, 0, pen);
        }
        public void addWort(string text, Brush brush, xFont font, byte style)
        {
            this.addWort(text, brush, font, style, Pens.Black);
        }
        public void addWort(string text, Brush brush, xFont font, byte style, Pen pen)
        {
            this.add(new Word(text, brush, font, style, pen));
        }
        #endregion
        #region addImage Überladungen
        public void addImage(Image img)
        {
            addImage(img, img.Width, img.Height);
        }
        public void addImage(Image img, float width)
        {
            addImage(img, width, width * img.Height / img.Width);
        }
        public void addImage(Image img, float width, float height)
        {
            add(new ImageBox(width, height, img));
        }
        #endregion
        #region addWhitespace und addAbsatz Überladungen
        /// <summary>
        /// added n Leerzeichen in prefferdFont
        /// </summary>
        /// <param name="n"></param>
        public void addWhitespace(int n)
        {
            this.addWhitespace(n * preferedFont.getWhitespace(), preferedFont.getZeilenabstand());
        }
        public void addWhitespace(xFont font)
        {
            this.add(new Whitespace(font.getWhitespace(), font.getZeilenabstand(), false));
        }
        public void addWhitespace(float width)
        {
            this.addWhitespace(width, 0, false);
        }
        public void addWhitespace(float width, float height)
        {
            this.addWhitespace(width, height, false);
        }
        public void addWhitespace(float width, float height, bool endsLine)
        {
            this.add(new Whitespace(width, height, endsLine));
        }
        public void addAbsatz()
        {
            this.addWhitespace(0, 0, true);
        }
        #endregion
        #region addRegex Überladungen
        /// <summary>
        /// <para>\n : Absatz</para>
        /// <para></para>
        /// <para>\r, \b, \y : rot, blau, gelb</para>
        /// <para>\g, \v, \o : grün, dunkelviolett, orange</para>
        /// <para>\l : sattelbraun</para>
        /// <para>\s, \e, \w : schwarz, dunkelgrau, weiß</para>
        /// <para>\cAARRGGBB : Color(AA,RR,GG,BB)</para>
        /// <para></para>
        /// <para>\d, \i : fett, kursiv</para>
        /// <para>\u, \a, \f, \j : Wort ist unter-, ober-, links-, rechts-strichen</para>
        /// <para>\x : Wort ist horizontal durchgestrichen</para>
        /// <para></para>
        /// <para>\tXf, \tXxYf : Whitespace mit Breite X * Leerzeichen und Höhe Y * Zeilenabstand</para>
        /// <para>\{...} : CString, der ... als regex added</para>
        /// <para>\[...] : Text, der ... als regex added</para>
        /// <para>\"...\" : interpretiert ... nicht und added es als Wort</para>
        /// <para></para>
        /// <para> \\ : \</para>
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="font"></param>
        public void addRegex(string regex, xFont font)
        {
            int i = 0;
            ///anfang substring
            int a = 0;
            Pen p = Pens.Black;
            Brush br = Brushes.Black;
            byte m = 0;
            while (i < regex.Length)
            {
                switch (regex[i])
                {
                    case '\\':
                        if (i != a)
                            this.addWort(regex.Substring(a, i - a), br, font, m, p);
                        i++;
                        if (regex.Length == i)
                            this.addWort(@"\?", br, font, m, p);
                        else
                        {
                            switch (regex[i])
                            {
                                #region Farben
                                case 'r':
                                    i++;
                                    br = Brushes.Red;
                                    p = Pens.Red;
                                    break;
                                case 'b':
                                    i++;
                                    br = Brushes.Blue;
                                    p = Pens.Blue;
                                    break;
                                case 'y':
                                    i++;
                                    br = Brushes.Yellow;
                                    p = Pens.Yellow;
                                    break;
                                case 'g':
                                    i++;
                                    br = Brushes.Green;
                                    p = Pens.Green;
                                    break;
                                case 'v':
                                    i++;
                                    br = Brushes.DarkViolet;
                                    p = Pens.DarkViolet;
                                    break;
                                case 'o':
                                    i++;
                                    br = Brushes.Orange;
                                    p = Pens.Orange;
                                    break;
                                case 'l':
                                    i++;
                                    br = Brushes.SaddleBrown;
                                    p = Pens.SaddleBrown;
                                    break;
                                case 's':
                                    i++;
                                    br = Brushes.Black;
                                    p = Pens.Black;
                                    break;
                                case 'e':
                                    i++;
                                    br = Brushes.DarkGray;
                                    p = Pens.DarkGray;
                                    break;
                                case 'w':
                                    i++;
                                    br = Brushes.White;
                                    p = Pens.White;
                                    break;
                                case 'c':
                                    if (regex.Length - i - 8 > 0)
                                    {
                                        Color c = HexToColor(regex.Substring(i + 1, 8));
                                        br = new SolidBrush(c);
                                        p = new Pen(c);
                                    }
                                    else
                                        this.addWort(regex.Substring(i - 1, regex.Length - i + 1) + new string('?', 9 - regex.Length + i), br, font, m, p);
                                    i += 9;
                                    break;
                                #endregion
                                #region style
                                case 'd':
                                    i++;
                                    m ^= Word.FONTSTYLE_BOLD;
                                    break;
                                case 'i':
                                    i++;
                                    m ^= Word.FONTSTYLE_ITALIC;
                                    break;
                                case 'u':
                                    i++;
                                    m ^= Word.FONTSTYLE_UNDERLINED;
                                    break;
                                case 'x':
                                    i++;
                                    m ^= Word.FONTSTYLE_CROSSEDOUT;
                                    break;
                                case 'a':
                                    i++;
                                    m ^= Word.FONTSTYLE_OVERLINED;
                                    break;
                                case 'f':
                                    i++;
                                    m ^= Word.FONTSTYLE_LEFTLINED;
                                    break;
                                case 'j':
                                    i++;
                                    m ^= Word.FONTSTYLE_RIGHTLINED;
                                    break;
                                #endregion
                                #region Format
                                case 'n':
                                    i++;
                                    this.addAbsatz();
                                    break;
                                case 't':
                                    int b = -1;
                                    a = i + 1;
                                    while ((i < regex.Length) && (regex[i] != 'f'))
                                    {
                                        if (regex[i] == 'x')
                                            b = i;
                                        i++;
                                    }
                                    float width, height;
                                    try
                                    {
                                        if (b == -1)
                                        {
                                            width = Convert.ToSingle(regex.Substring(a, i - a));
                                            height = 0;
                                        }
                                        else
                                        {
                                            width = Convert.ToSingle(regex.Substring(a, b - a));
                                            height = Convert.ToSingle(regex.Substring(b + 1, i - b - 1));
                                        }
                                        addWhitespace(width * font.getWhitespace(), height * font.getZeilenabstand());
                                    }
                                    catch (FormatException)
                                    {
                                        this.addWort("\\t" + regex.Substring(a, i - a) + "f?", br, font, m, p);
                                    }
                                    i++;
                                    break;
                                case 'm':
                                    a = i + 1;
                                    while ((i < regex.Length) && (regex[i] != 'f')) i++;
                                    try
                                    {
                                        this.alignment = Convert.ToSingle(regex.Substring(a, i - a));
                                    }
                                    catch (FormatException)
                                    {
                                        this.addWort("\\m" + regex.Substring(a, i - a) + "f?", br, font, m, p);
                                    }
                                    i++;
                                    break;
                                #endregion
                                #region Sonderzeichen
                                case '{':
                                    i++;
                                    this.addWort("{", br, font, m, p);
                                    break;
                                case '}':
                                    i++;
                                    this.addWort("}", br, font, m, p);
                                    break;
                                case '[':
                                    i++;
                                    this.addWort("[", br, font, m, p);
                                    break;
                                case ']':
                                    i++;
                                    this.addWort("]", br, font, m, p);
                                    break;
                                case '\"':
                                    i++;
                                    this.addWort("\"", br, font, m, p);
                                    break;
                                case '\\':
                                    i++;
                                    this.addWort("\\", br, font, m, p);
                                    break;
                                #endregion
                                default:
                                    this.addWort("\\?", br, font, m, p);
                                    break;
                            }
                        }
                        a = i;
                        break;
                    #region Interpunktion
                    case ':':
                    case '.':
                    case ',':
                    case ';':
                        i++;
                        this.addWort(regex.Substring(a, i - a), br, font, m, p);
                        a = i;
                        break;

                    case '+':
                    case '-':
                        if (i != a)
                            this.addWort(regex.Substring(a, i - a), br, font, m, p);
                        a = i;
                        i++;
                        break;
                    #endregion
                    #region Container
                    case '{':
                        if (i != a)
                            this.addWort(regex.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        i = suchGeschweifteKlammerZu(i, regex);
                        CString cs = new CString();
                        if (i != a)
                            cs.addRegex(regex.Substring(a, i - a), font);
                        this.add(cs);
                        i++;
                        a = i;
                        break;
                    case '[':
                        if (i != a)
                            this.addWort(regex.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        i = suchEckigeKlammerZu(i, regex);
                        Text te = new Text();
                        if (i != a)
                            te.addRegex(regex.Substring(a, i - a), font);
                        this.add(te);
                        i++;
                        a = i;
                        break;
                    case '\"':
                        if (i != a)
                            this.addWort(regex.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        i = suchEndeAnfuhrungsZeichen(i, regex);
                        if (i != a)
                            addWort(regex.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        break;
                    #endregion
                    #region Whitespace
                    case ' ':
                        if (i != a)
                            this.addWort(regex.Substring(a, i - a), br, font, m, p);
                        this.addWhitespace(font);
                        //this.addWhitespace(font.getWhitespace());
                        i++;
                        a = i;
                        break;

                    case '\n':
                        if (i != a)
                            this.addWort(regex.Substring(a, i - a), br, font, m, p);
                        this.addAbsatz();
                        i++;
                        a = i;
                        break;
                    case '\r':
                        if (i != a)
                            this.addWort(regex.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        break;
                    #endregion
                    default:
                        i++;
                        break;
                }
            }
            if ((a < regex.Length) && (i != a))
                this.addWort(regex.Substring(a, i - a), br, font, m, p);
        }
        /// <summary>
        /// <para>\n : Absatz</para>
        /// <para></para>
        /// <para>\r, \b, \y : rot, blau, gelb</para>
        /// <para>\g, \v, \o : grün, dunkelviolett, orange</para>
        /// <para>\l : sattelbraun</para>
        /// <para>\s, \e, \w : schwarz, dunkelgrau, weiß</para>
        /// <para>\cAARRGGBB : Color(AA,RR,GG,BB)</para>
        /// <para></para>
        /// <para>\d, \i : fett, kursiv</para>
        /// <para>\u, \a, \f, \j : Wort ist unter-, ober-, links-, rechts-strichen</para>
        /// <para>\x : Wort ist horizontal durchgestrichen</para>
        /// <para></para>
        /// <para>\tXf, \tXxYf : Whitespace mit Breite X * Leerzeichen und Höhe Y * Zeilenabstand</para>
        /// <para>\{...} : CString, der ... als regex added</para>
        /// <para>\[...] : Text, der ... als regex added</para>
        /// <para>\"...\" : interpretiert ... nicht und added es als Wort</para>
        /// <para></para>
        /// <para> \\ : \</para>
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="font"></param>
        public void addRegex(string regex)
        {
            this.addRegex(regex, preferedFont);
        }
        /// <summary>
        /// geht davon aus, dass regex[i - 1] = '['
        /// <para>gibt j zurück, sodass regex[j] = ']'</para>
        /// <para>und die klammerzahl = 0 ist</para>
        /// <para>und dinge in "..." ignoriert wurden.</para>
        /// <para>Ignoriert zeichen die direkt hinter einem '\\' stehen.</para>
        /// <para></para>
        /// <para>Falls ein solches j nicht existiert, wird regex.length zurückgegeben</para>
        /// <para>(i darf regex.length sein)</para>
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int suchGeschweifteKlammerZu(int i, string regex)
        {
            int klammer = 1;
            while (i < regex.Length)
            {
                if (regex[i] == '{')
                {
                    klammer++;
                    i++;
                }
                else if ((regex[i] == '}') && (--klammer == 0))
                    break;
                else if (regex[i] == '\"')
                    i = Math.Min(suchEndeAnfuhrungsZeichen(i + 1, regex) + 1, regex.Length);
                else if (regex[i] == '\\')
                    i = Math.Min(i + 2, regex.Length);
                else i++;
            }
            return i;
        }
        /// <summary>
        /// geht davon aus, dass regex[i - 1] = '['
        /// <para>gibt j zurück, sodass regex[j] = ']'</para>
        /// <para>und die klammerzahl = 0 ist</para>
        /// <para>und dinge in "..." ignoriert wurden.</para>
        /// <para>Ignoriert zeichen die direkt hinter einem '\\' stehen.</para>
        /// <para></para>
        /// <para>Falls ein solches j nicht existiert, wird regex.length zurückgegeben</para>
        /// <para>(i darf regex.length sein)</para>
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int suchEckigeKlammerZu(int i, string regex)
        {
            int klammer = 1;
            while (i < regex.Length)
            {
                if (regex[i] == '[')
                {
                    klammer++;
                    i++;
                }
                else if ((regex[i] == ']') && (--klammer == 0))
                    break;
                else if (regex[i] == '\"')
                    i = Math.Min(suchEndeAnfuhrungsZeichen(i + 1, regex) + 1, regex.Length);
                else if (regex[i] == '\\')
                    i = Math.Min(i + 2, regex.Length);
                else i++;
            }
            return i;
        }
        /// <summary>
        /// geht davon aus, dass regex[i - 1] = '\"'
        /// <para>gibt j zurück, sodass regex[j] = '"\'</para>
        /// <para>ignoriert '"', falls direkt davor ein '\\' steht.</para>
        /// <para></para>
        /// <para>Falls ein solches j nicht existiert, wird regex.length zurückgegeben</para>
        /// <para>(i darf regex.length sein)</para>
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int suchEndeAnfuhrungsZeichen(int i, string regex)
        {
            while (i < regex.Length)
            {
                if (regex[i] == '\"')
                    break;
                else if (regex[i] == '\\')
                    i = Math.Min(i + 2, regex.Length);
                else i++;
            }
            return i;
        }
        #endregion

        //public override void click(MouseEventArgs args)
        //{
        //    foreach (DrawBox item in this)
        //        if (item.check(args.Location))
        //        {
        //            //item.click(args);
        //            return;
        //        }
        //}

        /// <summary>
        /// fügt word in Stelle index ein
        /// </summary>
        /// <param name="word"></param>
        /// <param name="index"></param>
        public abstract void insert(int index, DrawBox word);
        /// <summary>
        /// entfernt ein Wort bei Stelle index ein
        /// <para>Durch das Entfernen von Objekten wird automatisch update() ausgelöst.</para>
        /// </summary>
        /// <param name="index"></param>
        public abstract void remove(int index);
        /// <summary>
        /// entfernt höchstens ein Vorkommen von word
        /// <para>Durch das Entfernen von Objekten wird automatisch update() ausgelöst.</para>
        /// </summary>
        /// <param name="word"></param>
        /// <returns>true iff genau eines entfernt wurde</returns>
        public abstract bool remove(DrawBox word);
        /// <summary>
        /// entfernt alle Elemente
        /// </summary>
        public abstract void clear();

        public abstract IEnumerator<DrawBox> GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
