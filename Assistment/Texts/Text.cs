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
    public class Text : PreText
    {
        public Text() : base()
        {
        }
        public Text(string Regex, xFont Font) : this()
        {
            this.preferedFont = Font;
            this.addRegex(Regex);
        }
        public Text(PreText PreText) : base(PreText)
        {

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
        public override DrawBox clone()
        {
            return new Text(this);
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
        public static implicit operator Text(string text)
        {
            Text t = new Text();
            t.preferedFont = new FontMeasurer("Calibri", 11);
            t.addRegex(text);
            return t;
        }
    }
}
