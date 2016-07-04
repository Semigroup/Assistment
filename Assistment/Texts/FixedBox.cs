using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Texts
{
    public class FixedBox : DrawBox
    {
        public DrawBox Inhalt { get; set; }
        public SizeF Alignment { get; set; }

        public FixedBox(SizeF Size, DrawBox Inhalt)
        {
            this.box.Size = Size;
            this.Inhalt = Inhalt;
        }

        public override float getSpace()
        {
            return box.Width * box.Height;
        }

        public override float getMin()
        {
            return box.Width;
        }

        public override float getMax()
        {
            return box.Height;
        }

        public override void update()
        {
            Inhalt.update();
        }

        public override void setup(RectangleF box)
        {
            this.box.Location = box.Location;
            Inhalt.setup(this.box);
            SizeF Rest = this.box.Size.sub(Inhalt.box.Size);
            Rest.Width = this.box.Width - Inhalt.getMax();
            RectangleF InnerBox = new RectangleF(box.Location.add(Rest.mul(Alignment).ToPointF()), Inhalt.box.Size);
            Inhalt.setup(InnerBox);
        }

        public override void draw(DrawContext con)
        {
            Inhalt.draw(con);
        }

        public override DrawBox clone()
        {
            return new FixedBox(this.box.Size, Inhalt.clone());
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "FixedBox:");
            sb.AppendLine(tabs + "\tbox: " + box);
            Inhalt.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
    }
}
