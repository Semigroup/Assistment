using System.Text;
using System.Drawing;
using System;

namespace Assistment.Texts
{
    public class Whitespace : DrawBox
    {
        public RectangleF OriginalBox { get; set; }

        public Whitespace(float width, float height, bool endsLine)
        {
            this.OriginalBox = new RectangleF(0, 0, width, height);
            this.EndsLine = endsLine;
        }

        public override float Space => this.OriginalBox.Width * this.OriginalBox.Height;
        public override float Min => this.OriginalBox.Width;
        public override float Max => this.OriginalBox.Width;

        public override void Setup(RectangleF box, bool isFirstInLine)
        {
            this.Box = OriginalBox;
            this.Box.Location = box.Location;
            if (isFirstInLine)
                this.Box.Width = 0;
        }
        public override void Setup(RectangleF box)
        {
            this.Box = OriginalBox;
            this.Box.Location = box.Location;
        }
        public override void Draw(DrawContext con)
        {
        }
        public override void Update()
        {
        }
        public override DrawBox Clone()
        {
            return new Whitespace(OriginalBox.Width, OriginalBox.Height, EndsLine);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "Whitespace:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            sb.AppendLine(tabs + ".");
        }
        public override string ToString()
        {
            return (OriginalBox.Width > 0 ? " " : "") + (EndsLine ? "\r\n" : "");
        }
    }
}
