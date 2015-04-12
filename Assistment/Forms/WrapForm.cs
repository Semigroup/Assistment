using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Texts;
using System.Drawing;

namespace Assistment.Forms
{
    public class WrapForm : FormBox
    {
        public DrawBox drawBox { get; private set; }

        public WrapForm(DrawBox drawBox)
        {
            this.drawBox = drawBox;
        }

        public override void click(PointF point)
        {
        }
        public override void move(PointF point)
        {
        }
        public override void release(PointF point)
        {
        }

        public override float getSpace()
        {
            return drawBox.getSpace();
        }

        public override float getMin()
        {
            return drawBox.getMin();
        }

        public override float getMax()
        {
            return drawBox.getMax();
        }

        public override void update()
        {
            drawBox.update();
        }
        public override void setup(RectangleF box)
        {
            drawBox.setup(box);
            this.box = drawBox.box;
        }
        public override void draw(DrawContext con)
        {
            drawBox.draw(con);
        }

        public override FormBox flone()
        {
            return new WrapForm(drawBox.clone());
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "WrapForm:");
            sb.AppendLine(tabs + "\tbox: " + box);
            sb.AppendLine(tabs + "\tdrawBox: ");
            drawBox.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
    }
    public static class WrapFormErweiterung
    {
        public static WrapForm ToForm(this DrawBox drawbox)
        {
            return new WrapForm(drawbox);
        }
    }
}
