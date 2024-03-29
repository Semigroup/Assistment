﻿
using System.Text;


namespace Assistment.Texts
{
    /// <summary>
    /// A Text uses a simple algorithm to order wordblocks in a line, so it assumes that every 
    /// word it contains has a fixed width and a fixed space.
    /// </summary>
    public class Text : PreText
    {
        public Text()
            : base()
        {
        }
        public Text(string Regex, IFontMeasurer Font)
            : this()
        {
            this.PreferedFont = Font;
            this.AddFormat(Regex);
        }
        public Text(PreText PreText)
            : base(PreText)
        {

        }

        protected override void Assigne(Line Line)
        {
            Line.SimpleAssignment();
        }
        public override DrawBox Clone()
        {
            return new Text(this);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "Text:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            foreach (DrawBox item in Words)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
        public static implicit operator Text(string text)
        {
            Text t = new Text();
            t.PreferedFont = StandardFont;
            t.AddFormat(text);
            return t;
        }
        public Text FirstLine()
        {
            Text t = new Text();
            foreach (var item in this)
                if (item.EndsLine)
                    break;
                else
                    t.Add(item);
            return t;
        }
    }
}
