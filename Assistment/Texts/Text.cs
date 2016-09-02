using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Mathematik;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;
using System.Diagnostics;

namespace Assistment.Texts
{
    /// <summary>
    /// A Text uses a simple algorithm to order wordblocks in a line, so it assumes that every word it contains has a fixed width and a fixed space.
    /// </summary>
    public class Text : PreText
    {
        public Text()
            : base()
        {
        }
        public Text(string Regex, xFont Font)
            : this()
        {
            this.preferedFont = Font;
            this.addRegex(Regex);
        }
        public Text(PreText PreText)
            : base(PreText)
        {

        }
       
        protected override void setupLines(RectangleF box, List<PreText.Line> Lines)
        {
            PointF Location = box.Location;

            Lines.First().Setup(Location, alignment, RightToLeft);
            Location.Y += Lines.First().Box.Height;
            this.box = Lines.First().Box;
            foreach (var line in Lines.Skip(1))
            {
                line.Setup(Location, alignment, RightToLeft);
                Location.Y += line.Box.Height;
                this.box = this.box.Extend(line.Box);
            }
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
            t.preferedFont = StandardFont;
            t.addRegex(text);
            return t;
        }
    }
}
