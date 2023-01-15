using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using System.IO;

using Assistment.Drawing;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Texts
{
    public abstract class DrawContainer : DrawBox, ICollection<DrawBox>
    {
        public float Alignment = 0;
        public IFontMeasurer PreferedFont = StandardFont;

        public abstract int Count { get; }

        public abstract bool IsReadOnly { get; }

        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            foreach (var item in this)
                item.Move(ToMove);
        }

        public abstract void Add(DrawBox word);
        public virtual void AddRange(IEnumerable<DrawBox> container)
        {
            foreach (var word in container)
                Add(word);
        }
        #region AddWort Überladungen
        public void AddWort(object text)
        {
            this.AddWort(text.ToString(), Brushes.Black, PreferedFont, 0, Pens.Black);
        }
        public void AddWort(object text, Pen pen)
        {
            this.AddWort(text.ToString(), Brushes.Black, PreferedFont, 0, pen);
        }
        public void AddWort(object text, byte style)
        {
            this.AddWort(text.ToString(), Brushes.Black, PreferedFont, style, Pens.Black);
        }
        public void AddWort(object text, byte style, Pen pen)
        {
            this.AddWort(text.ToString(), Brushes.Black, PreferedFont, style, pen);
        }
        public void AddWort(object text, IFontMeasurer font)
        {
            this.AddWort(text.ToString(), Brushes.Black, font, 0, Pens.Black);
        }
        public void AddWort(object text, IFontMeasurer font, Pen pen)
        {
            this.AddWort(text.ToString(), Brushes.Black, font, 0, pen);
        }
        public void AddWort(object text, IFontMeasurer font, byte style)
        {
            this.AddWort(text.ToString(), Brushes.Black, font, style, Pens.Black);
        }
        public void AddWort(object text, IFontMeasurer font, byte style, Pen pen)
        {
            this.AddWort(text.ToString(), Brushes.Black, font, style, pen);
        }
        public void AddWort(object text, Brush brush)
        {
            this.AddWort(text.ToString(), brush, PreferedFont, 0, Pens.Black);
        }
        public void AddWort(object text, Brush brush, Pen pen)
        {
            this.AddWort(text.ToString(), brush, PreferedFont, 0, pen);
        }
        public void AddWort(object text, Brush brush, byte style)
        {
            this.AddWort(text.ToString(), brush, PreferedFont, style, Pens.Black);
        }
        public void AddWort(object text, Brush brush, byte style, Pen pen)
        {
            this.AddWort(text.ToString(), brush, PreferedFont, style, pen);
        }
        public void AddWort(object text, Brush brush, IFontMeasurer font)
        {
            this.AddWort(text.ToString(), brush, font, 0, Pens.Black);
        }
        public void AddWort(object text, Brush brush, IFontMeasurer font, Pen pen)
        {
            this.AddWort(text.ToString(), brush, font, 0, pen);
        }
        public void AddWort(object text, Brush brush, IFontMeasurer font, byte style)
        {
            this.AddWort(text.ToString(), brush, font, style, Pens.Black);
        }
        public void AddWort(object text, Brush brush, IFontMeasurer font, byte style, Pen pen)
        {
            this.Add(new Word(text.ToString(), brush, font, style, pen));
        }
        public void AddWort(string text)
        {
            this.AddWort(text, Brushes.Black, PreferedFont, 0, Pens.Black);
        }
        public void AddWort(string text, Pen pen)
        {
            this.AddWort(text, Brushes.Black, PreferedFont, 0, pen);
        }
        public void AddWort(string text, byte style)
        {
            this.AddWort(text, Brushes.Black, PreferedFont, style, Pens.Black);
        }
        public void AddWort(string text, byte style, Pen pen)
        {
            this.AddWort(text, Brushes.Black, PreferedFont, style, pen);
        }
        public void AddWort(string text, IFontMeasurer font)
        {
            this.AddWort(text, Brushes.Black, font, 0, Pens.Black);
        }
        public void AddWort(string text, IFontMeasurer font, Pen pen)
        {
            this.AddWort(text, Brushes.Black, font, 0, pen);
        }
        public void AddWort(string text, IFontMeasurer font, byte style)
        {
            this.AddWort(text, Brushes.Black, font, style, Pens.Black);
        }
        public void AddWort(string text, IFontMeasurer font, byte style, Pen pen)
        {
            this.AddWort(text, Brushes.Black, font, style, pen);
        }
        public void AddWort(string text, Brush brush)
        {
            this.AddWort(text, brush, PreferedFont, 0, Pens.Black);
        }
        public void AddWort(string text, Brush brush, Pen pen)
        {
            this.AddWort(text, brush, PreferedFont, 0, pen);
        }
        public void AddWort(string text, Brush brush, byte style)
        {
            this.AddWort(text, brush, PreferedFont, style, Pens.Black);
        }
        public void AddWort(string text, Brush brush, byte style, Pen pen)
        {
            this.AddWort(text, brush, PreferedFont, style, pen);
        }
        public void AddWort(string text, Brush brush, IFontMeasurer font)
        {
            this.AddWort(text, brush, font, 0, Pens.Black);
        }
        public void AddWort(string text, Brush brush, IFontMeasurer font, Pen pen)
        {
            this.AddWort(text, brush, font, 0, pen);
        }
        public void AddWort(string text, Brush brush, IFontMeasurer font, byte style)
        {
            this.AddWort(text, brush, font, style, Pens.Black);
        }
        public void AddWort(string text, Brush brush, IFontMeasurer font, byte style, Pen pen)
        {
            this.Add(new Word(text, brush, font, style, pen));
        }
        #endregion
        #region AddImage Überladungen
        public void AddImage(Image img)
        {
            AddImage(img, img.Width, img.Height);
        }
        public void AddImage(Image img, float width)
        {
            AddImage(img, width, width * img.Height / img.Width);
        }
        public void AddImage(Image img, float width, float height)
        {
            Add(new ImageBox(width, height, img));
        }
        #endregion
        #region addWhitespace und addAbsatz Überladungen
        /// <summary>
        /// added n Leerzeichen in prefferdFont
        /// </summary>
        /// <param name="n"></param>
        public void AddWhitespace(int n)
        {
            this.AddWhitespace(n * PreferedFont.GetWhitespace(), PreferedFont.GetZeilenabstand());
        }
        public void AddWhitespace(IFontMeasurer font)
        {
            this.Add(new Whitespace(font.GetWhitespace(), font.GetZeilenabstand(), false));
        }
        public void AddWhitespace(float width)
        {
            this.AddWhitespace(width, 0, false);
        }
        public void AddWhitespace(float width, float height)
        {
            this.AddWhitespace(width, height, false);
        }
        public void AddWhitespace(float width, float height, bool endsLine)
        {
            this.Add(new Whitespace(width, height, endsLine));
        }
        public void AddAbsatz()
        {
            this.AddWhitespace(0, 0, true);
        }
        #endregion
        #region AddFormat Überladungen
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
        /// <para>\mAf : Alignment auf A setzen</para>
        /// <para></para>
        /// <para>\{...} : CString, der ... als formattedString added</para>
        /// <para>\[...] : Text, der ... als formattedString added</para>
        /// <para>\"...\" : interpretiert ... nicht und added es als Wort</para>
        /// <para></para>
        /// <para> \\ : \</para>
        /// </summary>
        /// <param name="formattedString"></param>
        /// <param name="font"></param>
        public void AddFormat(string formattedString, IFontMeasurer font)
        {
            int i = 0;
            ///anfang substring
            int a = 0;
            Pen p = Pens.Black;
            Brush br = Brushes.Black;
            byte m = 0;
            while (i < formattedString.Length)
            {
                switch (formattedString[i])
                {
                    case '\\':
                        if (i != a)
                            this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        i++;
                        if (formattedString.Length == i)
                            this.AddWort(@"\?", br, font, m, p);
                        else
                        {
                            switch (formattedString[i])
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
                                    if (formattedString.Length - i - 8 > 0)
                                    {
                                        Color c = formattedString.Substring(i + 1, 8).ToColor();
                                        br = new SolidBrush(c);
                                        p = new Pen(c);
                                    }
                                    else
                                        this.AddWort(formattedString.Substring(i - 1, formattedString.Length - i + 1) + new string('?', 9 - formattedString.Length + i), br, font, m, p);
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
                                    this.AddAbsatz();
                                    break;
                                case 't':
                                    int b = -1;
                                    a = i + 1;
                                    while ((i < formattedString.Length) && (formattedString[i] != 'f'))
                                    {
                                        if (formattedString[i] == 'x')
                                            b = i;
                                        i++;
                                    }
                                    float width, height;
                                    try
                                    {
                                        if (b == -1)
                                        {
                                            width = Convert.ToSingle(formattedString.Substring(a, i - a));
                                            height = 0;
                                        }
                                        else
                                        {
                                            width = Convert.ToSingle(formattedString.Substring(a, b - a));
                                            height = Convert.ToSingle(formattedString.Substring(b + 1, i - b - 1));
                                        }
                                        AddWhitespace(width * font.GetWhitespace(), height * font.GetZeilenabstand());
                                    }
                                    catch (FormatException)
                                    {
                                        this.AddWort("\\t" + formattedString.Substring(a, i - a) + "f?", br, font, m, p);
                                    }
                                    i++;
                                    break;
                                case 'm':
                                    a = i + 1;
                                    while ((i < formattedString.Length) && (formattedString[i] != 'f')) i++;
                                    try
                                    {
                                        this.Alignment = Convert.ToSingle(formattedString.Substring(a, i - a));
                                    }
                                    catch (FormatException)
                                    {
                                        this.AddWort("\\m" + formattedString.Substring(a, i - a) + "f?", br, font, m, p);
                                    }
                                    i++;
                                    break;
                                #endregion
                                #region Sonderzeichen
                                case '{':
                                    i++;
                                    this.AddWort("{", br, font, m, p);
                                    break;
                                case '}':
                                    i++;
                                    this.AddWort("}", br, font, m, p);
                                    break;
                                case '[':
                                    i++;
                                    this.AddWort("[", br, font, m, p);
                                    break;
                                case ']':
                                    i++;
                                    this.AddWort("]", br, font, m, p);
                                    break;
                                case '\"':
                                    i++;
                                    this.AddWort("\"", br, font, m, p);
                                    break;
                                case '\\':
                                    i++;
                                    this.AddWort("\\", br, font, m, p);
                                    break;
                                #endregion
                                default:
                                    this.AddWort("\\?", br, font, m, p);
                                    break;
                            }
                        }
                        a = i;
                        break;
                    #region Interpunktion und Plus und Bindestrich
                    case ':':
                    case '.':
                    case ',':
                    case ';':
                        i++;
                        this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        a = i;
                        break;

                    case '-':
                        if (i != a)
                            this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        a = i;
                        i++;
                        if (formattedString.Length == i)
                            this.AddWort(@"-", br, font, m, p);
                        else if (formattedString[i] == '-')
                        {
                            this.AddWort(@"−", br, font, m, p);
                            i++;
                            a = i;
                        }
                        break;

                    case '−':
                    case '+':
                        if (i != a)
                            this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        a = i;
                        i++;
                        break;
                    #endregion
                    #region Container
                    case '{':
                        if (i != a)
                            this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        i = SuchGeschweifteKlammerZu(i, formattedString);
                        CString cs = new CString();
                        if (i != a)
                            cs.AddFormat(formattedString.Substring(a, i - a), font);
                        this.Add(cs);
                        i++;
                        a = i;
                        break;
                    case '[':
                        if (i != a)
                            this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        i = SuchEckigeKlammerZu(i, formattedString);
                        Text te = new Text();
                        if (i != a)
                            te.AddFormat(formattedString.Substring(a, i - a), font);
                        this.Add(te);
                        i++;
                        a = i;
                        break;
                    case '\"':
                        if (i != a)
                            this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        i = SuchEndeAnfuhrungsZeichen(i, formattedString);
                        if (i != a)
                            AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        break;
                    #endregion
                    #region Whitespace
                    case ' ':
                        if (i != a)
                            this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        this.AddWhitespace(font);
                        //this.addWhitespace(font.getWhitespace());
                        i++;
                        a = i;
                        break;
                    case '\n':
                        if (i != a)
                            this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        this.AddAbsatz();
                        i++;
                        a = i;
                        break;
                    case '\r':
                        if (i != a)
                            this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
                        i++;
                        a = i;
                        break;
                    #endregion
                    default:
                        i++;
                        break;
                }
            }
            if ((a < formattedString.Length) && (i != a))
                this.AddWort(formattedString.Substring(a, i - a), br, font, m, p);
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
        /// <para>\mAf : Alignment auf A setzen</para>
        /// <para></para>
        /// <para>\{...} : CString, der ... als formattedString added</para>
        /// <para>\[...] : Text, der ... als formattedString added</para>
        /// <para>\"...\" : interpretiert ... nicht und added es als Wort</para>
        /// <para></para>
        /// <para> \\ : \</para>
        /// </summary>
        /// <param name="formattedString"></param>
        /// <param name="font"></param>
        public void AddFormat(string formattedString)
        {
            this.AddFormat(formattedString, PreferedFont);
        }
        /// <summary>
        /// geht davon aus, dass formattedString[i - 1] = '['
        /// <para>gibt j zurück, sodass formattedString[j] = ']'</para>
        /// <para>und die klammerzahl = 0 ist</para>
        /// <para>und dinge in "..." ignoriert wurden.</para>
        /// <para>Ignoriert zeichen die direkt hinter einem '\\' stehen.</para>
        /// <para></para>
        /// <para>Falls ein solches j nicht existiert, wird formattedString.length zurückgegeben</para>
        /// <para>(i darf formattedString.length sein)</para>
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int SuchGeschweifteKlammerZu(int i, string formattedString)
        {
            int klammer = 1;
            while (i < formattedString.Length)
            {
                if (formattedString[i] == '{')
                {
                    klammer++;
                    i++;
                }
                else if ((formattedString[i] == '}') && (--klammer == 0))
                    break;
                else if (formattedString[i] == '\"')
                    i = Math.Min(SuchEndeAnfuhrungsZeichen(i + 1, formattedString) + 1, formattedString.Length);
                else if (formattedString[i] == '\\')
                    i = Math.Min(i + 2, formattedString.Length);
                else i++;
            }
            return i;
        }
        /// <summary>
        /// geht davon aus, dass formattedString[i - 1] = '['
        /// <para>gibt j zurück, sodass formattedString[j] = ']'</para>
        /// <para>und die klammerzahl = 0 ist</para>
        /// <para>und dinge in "..." ignoriert wurden.</para>
        /// <para>Ignoriert zeichen die direkt hinter einem '\\' stehen.</para>
        /// <para></para>
        /// <para>Falls ein solches j nicht existiert, wird formattedString.length zurückgegeben</para>
        /// <para>(i darf formattedString.length sein)</para>
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int SuchEckigeKlammerZu(int i, string formattedString)
        {
            int klammer = 1;
            while (i < formattedString.Length)
            {
                if (formattedString[i] == '[')
                {
                    klammer++;
                    i++;
                }
                else if ((formattedString[i] == ']') && (--klammer == 0))
                    break;
                else if (formattedString[i] == '\"')
                    i = Math.Min(SuchEndeAnfuhrungsZeichen(i + 1, formattedString) + 1, formattedString.Length);
                else if (formattedString[i] == '\\')
                    i = Math.Min(i + 2, formattedString.Length);
                else i++;
            }
            return i;
        }
        /// <summary>
        /// geht davon aus, dass formattedString[i - 1] = '\"'
        /// <para>gibt j zurück, sodass formattedString[j] = '"\'</para>
        /// <para>ignoriert '"', falls direkt davor ein '\\' steht.</para>
        /// <para></para>
        /// <para>Falls ein solches j nicht existiert, wird formattedString.length zurückgegeben</para>
        /// <para>(i darf formattedString.length sein)</para>
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int SuchEndeAnfuhrungsZeichen(int i, string formattedString)
        {
            while (i < formattedString.Length)
            {
                if (formattedString[i] == '\"')
                    break;
                else if (formattedString[i] == '\\')
                    i = Math.Min(i + 2, formattedString.Length);
                else i++;
            }
            return i;
        }
        #endregion

        public void AddZoomedImage(Image Image, float DesiredHeight)
        {
            this.AddImage(Image, Image.Width * DesiredHeight / Image.Height, DesiredHeight);
        }
        public void AddZoomedImage(Image Image)
        {
            this.AddZoomedImage(Image, PreferedFont.GetZeilenabstand());
        }

        /// <summary>
        /// true iff this container doesnt contain any DrawBoxes
        /// </summary>
        /// <returns></returns>
        public virtual bool Empty()
        {
            return !this.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// fügt word in Stelle index ein
        /// </summary>
        /// <param name="word"></param>
        /// <param name="index"></param>
        public abstract void Insert(int index, DrawBox word);
        /// <summary>
        /// entfernt ein Wort bei Stelle index ein
        /// <para>Durch das Entfernen von Objekten wird automatisch update() ausgelöst.</para>
        /// </summary>
        /// <param name="index"></param>
        public abstract void Remove(int index);
        /// <summary>
        /// entfernt höchstens ein Vorkommen von word
        /// <para>Durch das Entfernen von Objekten wird automatisch update() ausgelöst.</para>
        /// </summary>
        /// <param name="word"></param>
        /// <returns>true iff genau eines entfernt wurde</returns>
        public abstract bool Remove(DrawBox word);
        public int RemoveWhitespaces()
        {
            List<DrawBox> ToRemove = new List<DrawBox>();
            foreach (var item in this)
                if (item is Whitespace)
                    ToRemove.Add(item);
            foreach (var item in ToRemove)
                this.Remove(item);
            return ToRemove.Count;
        }
        /// <summary>
        /// entfernt alle Elemente
        /// </summary>
        public abstract void Clear();

        public abstract IEnumerator<DrawBox> GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this)
                sb.Append(item.ToString());
            return sb.ToString();
        }

        public abstract bool Contains(DrawBox item);
        public abstract void CopyTo(DrawBox[] array, int arrayIndex);

        public static implicit operator DrawContainer(string text)
        {
            return (Text)text;
        }
        public override void ForceWordStyle(Brush brush = null, IFontMeasurer font = null, byte? style = null, Pen pen = null)
        {
            foreach (var item in this)
                item.ForceWordStyle(brush, font, style, pen);
        }
    }
}
