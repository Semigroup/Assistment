using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Mathematik;

namespace Assistment.Texts
{
    public class GeometryBox : DrawBox
    {
        public DrawBox InnerDrawBox { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Right { get; set; }
        public float Left { get; set; }

        public GeometryBox()
            : this(null, 30)
        {

        }
        public GeometryBox(DrawBox InnerDrawBox, float abstand)
            : this(InnerDrawBox, abstand, abstand, abstand, abstand)
        {

        }
        public GeometryBox(DrawBox InnerDrawBox, float top, float bottom, float right, float left)
        {
            this.InnerDrawBox = InnerDrawBox;
            this.Left = left;
            this.Bottom = bottom;
            this.Right = right;
            this.Top = top;
        }

        public override float getSpace()
        {
            float A = InnerDrawBox.getSpace();
            float a = FastMath.Sqrt(A);

            return (Top + a + Bottom) * (Right + a + Left);
        }

        public override float getMin()
        {
            return InnerDrawBox.getMin() + Left + Right;
        }

        public override float getMax()
        {
            return InnerDrawBox.getMax() + Left + Right;
        }

        public override void update()
        {
            InnerDrawBox.update();
        }

        public override void setup(RectangleF box)
        {
            this.box = box;
            RectangleF innerBox = new RectangleF(box.X + Left, box.Y + Top, box.Width - Left - Right, box.Height - Top - Bottom);
            InnerDrawBox.setup(innerBox);
            this.box.Height = Top + Bottom + InnerDrawBox.box.Height;
        }

        public override void draw(DrawContext con)
        {
            InnerDrawBox.draw(con);
        }

        public override DrawBox clone()
        {
            return new GeometryBox(InnerDrawBox.clone(), Top, Bottom, Right, Left);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "GeometryBox:");
            sb.AppendLine(ttabs + "box: " + box);
            sb.AppendLine(ttabs + "Left: " + Left);
            sb.AppendLine(ttabs + "Right: " + Right);
            sb.AppendLine(ttabs + "Top: " + Top);
            sb.AppendLine(ttabs + "Bottom: " + Bottom);
            InnerDrawBox.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
    }
}
