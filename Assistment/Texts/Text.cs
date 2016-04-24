using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Texts
{
    /// <summary>
    /// A Text uses a simple algorithm to order wordblocks in a line, so it assumes that every word it contains has a fixed width and a fixed space.
    /// </summary>
    public class Text : DrawContainer
    {
        private List<DrawBox> words;
        private float min, max, space;
        /// <summary>
        /// Von Rechts nach Links
        /// <para></para>
        /// <para>Von Links nach Rechts, falls false (default Wert)</para>
        /// </summary>
        public bool RightToLeft;

        public Text()
        {
            words = new List<DrawBox>();
            min = max = space = 0;
            RightToLeft = false;
        }
        public Text(string Regex, xFont Font) : this()
        {
            this.preferedFont = Font;
            this.addRegex(Regex);
        }

        public override void add(DrawBox word)
        {
#if CSTRINGDEBUG
            if (word.getMin() != word.getMax())
                throw new NotImplementedException();
#endif
            words.Add(word);
            min = Math.Max(min, word.getMin());
            max += word.getMax();
            space += word.getSpace();
        }
        public override void addRange(DrawContainer container)
        {
            words.AddRange(container);
            this.min = Math.Max(min, container.getMin());
            this.max += container.getMax();
            this.space += container.getSpace();
        }

        public override void insert(int index, DrawBox word)
        {
            words.Insert(index, word);
            this.min = Math.Max(word.getMin(), this.min);
            this.max += word.getMax();
            this.space += word.getSpace();
        }
        public override void remove(int index)
        {
            words.RemoveAt(index);
            this.update();
        }
        public override bool remove(DrawBox word)
        {
            bool result = words.Remove(word);
            this.update();
            return result;
        }

        public override float getSpace()
        {
            return space;
        }
        public override float getMin()
        {
            return min;
        }
        public override float getMax()
        {
            return max;
        }

        public override void update()
        {
            min = max = space = 0;
            foreach (DrawBox word in words)
            {
                min = Math.Max(min, word.getMin());
                max += word.getMax();
                space += word.getSpace();
            }
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            RectangleF subBox = box;
            IEnumerator<DrawBox> word = words.GetEnumerator();
            List<DrawBox> line = new List<DrawBox>();
            float remain = box.Width;
            float height;
            while (word.MoveNext())
            {
                ///Toleranzbit von +1 wurde hinzugefügt (22.08.2014)
                if ((word.Current.getMin() > remain + 1) || word.Current.endsLine)
                {
                    height = 0;
                    if (RightToLeft)
                        subBox.X = box.Right - remain * (1 - alignment);
                    else
                        subBox.X = box.X + remain * alignment;
                    subBox.Width = 0;
                    foreach (DrawBox item in line)
                    {
                        if (RightToLeft)
                            subBox.X -= item.getMin();
                        else
                            subBox.X += subBox.Width;
                        subBox.Width = item.getMin();
                        item.setup(subBox);
                        height = Math.Max(height, item.box.Height);
                    }
                    subBox.Y += height;
                    line.Clear();
                    remain = box.Width;
                }
                line.Add(word.Current);
                remain -= word.Current.getMin();
            }
            if (line.Count > 0)
            {
                height = 0;
                if (RightToLeft)
                    subBox.X = box.Right - remain * (1 - alignment);
                else
                    subBox.X = box.X + remain * alignment;
                subBox.Width = 0;
                foreach (DrawBox item in line)
                {
                    if (RightToLeft)
                        subBox.X -= item.getMin();
                    else
                        subBox.X += subBox.Width;
                    subBox.Width = item.getMin();
                    item.setup(subBox);
                    height = Math.Max(height, item.box.Height);
                }
                subBox.Y += height;
            }
            this.box.Height = subBox.Y - box.Y;
        }
        public override void draw(DrawContext con)
        {
            foreach (DrawBox item in words)
            {
                if (item.box.Y < con.Bildhohe)
                    item.draw(con);
                else break;
            }
        }

        /// <summary>
        /// creates just a flat copy
        /// </summary>
        /// <returns></returns>
        public override DrawBox clone()
        {
            Text te = new Text();
            te.alignment = this.alignment;
            te.preferedFont = preferedFont;
            foreach (var item in words)
                te.words.Add(item.clone());
            te.min = min;
            te.max = max;
            te.space = space;
            te.RightToLeft = RightToLeft;
            return te;
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "Text:");
            sb.AppendLine(tabs + "\tbox: " + box);
            foreach (DrawBox item in words)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }

        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return words.GetEnumerator();
        }

        public override void clear()
        {
            words.Clear();
            min = max = space = 0;
        }

        public static implicit operator Text(string text)
        {
            Text t = new Text();
            t.preferedFont = new FontMeasurer("Calibri", 11);
            t.addRegex(text);
            return t;
        }
    }
}
