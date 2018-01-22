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

        public override float Space => drawBox.Space;

        public override float Min => drawBox.Min;

        public override float Max => drawBox.Max;

        public override void Update()
        {
            drawBox.Update();
        }
        public override void Setup(RectangleF box)
        {
            drawBox.Setup(box);
            this.Box = drawBox.Box;
        }
        public override void Draw(DrawContext con)
        {
            drawBox.Draw(con);
        }

        public override FormBox flone()
        {
            return new WrapForm(drawBox.Clone());
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "WrapForm:");
            sb.AppendLine(tabs + "\tbox: " + Box);
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
