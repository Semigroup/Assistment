//#define CSTRINGDEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Texts
{
    /// <summary>
    /// Now here's the real deal.
    /// </summary>
    public class CString : DrawContainer
    {
        private List<DrawBox> list;

        private float space;
        private float min;
        private float max;

        private class chunk : IComparable<chunk>
        {
            public DrawBox box;
            public float maxHeight, minHeight;
            public float assignedWidth;
            public int place;

            public chunk(DrawBox box, int place)
            {
                this.box = box;
                this.maxHeight = box.getSpace() / box.getMin();
                this.minHeight = box.getSpace() / box.getMax();
                if (minHeight.Equals(float.NaN))
                    minHeight = 0;
                this.place = place;
#if CSTRINGDEBUG
                if (minHeight > maxHeight)
                    throw new NotImplementedException();
#endif
            }

            public float getWidth(float height)
            {
#if CSTRINGDEBUG
                if ((minHeight > height) || (height > maxHeight))
                    throw new NotImplementedException();
#endif
                return box.getSpace() / height;
            }
            public float assigneWidth(float height)
            {
                this.assignedWidth = Math.Max(box.getMin(), box.getSpace() / height);
                if (assignedWidth.Equals(float.NaN))
                    assignedWidth = 0;
                return this.assignedWidth;
            }
            public int CompareTo(chunk other)
            {
                return (int)(100f * (other.maxHeight - this.maxHeight));
            }
            public override string ToString()
            {
                return place + ": " + assignedWidth;
            }
        }

        public CString()
        {
            this.list = new List<DrawBox>();
            this.space = 0;
            this.min = this.max = 0;
        }

        public override void add(DrawBox word)
        {
            list.Add(word);
            this.min = Math.Max(word.getMin(), this.min);
            this.max += word.getMax();
            this.space += word.getSpace();
        }
        public override void addRange(DrawContainer container)
        {
            list.AddRange(container);
            this.min = Math.Max(min, container.getMin());
            this.max += container.getMax();
            this.space += container.getSpace();
        }

        public override void insert(int index, DrawBox word)
        {
            list.Insert(index, word);
            this.min = Math.Max(word.getMin(), this.min);
            this.max += word.getMax();
            this.space += word.getSpace();
        }
        public override void remove(int index)
        {
            list.RemoveAt(index);
            this.update();
        }
        public override bool remove(DrawBox word)
        {
            bool result = list.Remove(word);
            this.update();
            return result;
        }

        public override void setup(RectangleF box)
        {
            this.box = box;
            RectangleF subBox = box;
            IEnumerator<DrawBox> en = list.GetEnumerator();
            chunk[] chunks;
            float height;
            float width;
            en.MoveNext();
            while (en.Current != null)
            {
                height = 0;
                subBox.Width = 0;
                chunks = getLine(en, out width);
                subBox.X = box.X + (box.Width - width) * alignment;
                foreach (chunk item in chunks)
                {
                    subBox.X += subBox.Width;
                    subBox.Width = item.assignedWidth;
                    item.box.setup(subBox);
                    height = Math.Max(height, item.box.box.Height);
                }
                subBox.Y += height;
            }
            this.box = box;
            this.box.Height = subBox.Y - box.Y;
        }
        public override void draw(DrawContext con)
        {
            //System.Windows.Forms.MessageBox.Show(box + "");
            foreach (DrawBox item in list)
            {
                if (item.box.Y < con.Bildhohe)
                    item.draw(con);
                else break;
            }
            //con.drawRectangle(Pens.Red, box);
        }
        /// <summary>
        /// ab einschl. en.current
        /// </summary>
        /// <param name="en"></param>
        private chunk[] getLine(IEnumerator<DrawBox> en, out float usedWidth)
        {
#if CSTRINGDEBUG
            string s = "Debug:";
#endif
            List<chunk> line = new List<chunk>();
            float width = en.Current.getMin();
            int place = 0;
            chunk ch = new chunk(en.Current, place);
            /// inf = max{ mindestHöhe }
            float inf = ch.minHeight;
            #region Sammeln und Sortieren
            bool end = en.Current.endsLine;
            line.Add(ch);
            ///width + en.Current.getMin() <= con.box.Width  ==> width + en.Current.getMin() <= con.box.Width +1
            ///Toleranzbit musste aufgrund von floatfehlern hnzugefügt werden. (22.08.2014)
            while (en.MoveNext() && !end && (width + en.Current.getMin() <= box.Width + 1))
            {
                end = en.Current.endsLine;
                width += en.Current.getMin();
                place++;
                ch = new chunk(en.Current, place);
                inf = Math.Max(inf, ch.minHeight);
                line.Add(ch);
            }
            line.Sort();
#if CSTRINGDEBUG
            foreach (var item in line)
                s += "\n" + item.maxHeight + " bis " + item.minHeight;
#endif
            #endregion
            #region Höhe suchen
            int n = 1;
            IEnumerator<chunk> chEn = line.GetEnumerator();
            chEn.MoveNext();
            float maxWidth = chEn.Current.box.getMax();
            float height = chEn.Current.box.getSpace();
            while ((chEn.MoveNext()) && (chEn.Current.maxHeight > inf) && (box.Width >= getWidth(line, n, chEn.Current.maxHeight)))
            {
                n++;
                maxWidth += chEn.Current.box.getMax();
                height += chEn.Current.box.getSpace();
            }
            //Have to minimize the biggest n chunks
            float remainWidth = box.Width;
            while (chEn.Current != null)
            {
                remainWidth -= chEn.Current.box.getMin();
                chEn.MoveNext();
            }
            // b_i = A_i/h
            // Sum_i=1^n A_i/h = min(remainWidth, maxWidth) =:B
            // h = Sum_i=1^n A_i / B
#if CSTRINGDEBUG
            s += "\n" + "n: " + n + "; Space: " + height;
#endif
            height = Math.Max(height / Math.Min(remainWidth, maxWidth), inf);
#if CSTRINGDEBUG
            s += "\n" + height + ": " + Math.Min(remainWidth, maxWidth) + " = " + remainWidth + ", " + maxWidth;
#endif
            #endregion
            #region Weise Breiten zu
            chEn.Reset();
            chunk[] widths = new chunk[place + 1];
            usedWidth = 0;
            while (chEn.MoveNext())
            {
                widths[chEn.Current.place] = chEn.Current;
                usedWidth += chEn.Current.assigneWidth(height);
            }
            #endregion
#if CSTRINGDEBUG
            con.drawLine(Pens.Purple, new PointF(con.box.X, height + con.box.Y), new PointF(con.box.X + con.box.Width, height + con.box.Y));
            con.drawString(s, new Font("Calibri", 11), Brushes.Black, new PointF(con.box.X, height + con.box.Y));
#endif
            return widths;
        }
        private float getWidth(List<chunk> chunks, int i, float height)
        {
            float width = 0;
            IEnumerator<chunk> en = chunks.GetEnumerator();
            for (int j = 0; j < i; j++)
            {
                en.MoveNext();
                width += en.Current.getWidth(height);
            }
            return width;
        }

        public override float getSpace()
        {
            return this.space;
        }
        public override float getMin()
        {
            return this.min;
        }
        public override float getMax()
        {
            return this.max;
        }

        public override void update()
        {
            this.min = this.max = this.space = 0;
            foreach (DrawBox box in list)
            {
                box.update();
                this.min = Math.Max(this.min, box.getMin());
                this.max += box.getMax();
                this.space += box.getSpace();
            }
        }
        public override DrawBox clone()
        {
            CString cs = new CString();
            cs.alignment = this.alignment;
            cs.preferedFont = this.preferedFont;
            foreach (var item in list)
                cs.list.Add(item.clone());
            cs.min = this.min;
            cs.max = this.max;
            cs.space = this.space;
            return cs;
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "CString:");
            sb.AppendLine(tabs + "\tbox: " + box);
            foreach (DrawBox item in list)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public override void clear()
        {
            list.Clear();
            min = max = space = 0;
        }
    }
}
