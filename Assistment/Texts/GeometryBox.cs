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
        public float TopSpace { get; set; }
        public float BottomSpace { get; set; }
        public float RightSpace { get; set; }
        public float LeftSpace { get; set; }

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
            this.LeftSpace = left;
            this.BottomSpace = bottom;
            this.RightSpace = right;
            this.TopSpace = top;
        }

        public override float getSpace()
        {
            float A = InnerDrawBox.getSpace();
            float a = FastMath.Sqrt(A);

            return (TopSpace + a + BottomSpace) * (RightSpace + a + LeftSpace);
        }

        public override float getMin()
        {
            return InnerDrawBox.getMin() + LeftSpace + RightSpace;
        }

        public override float getMax()
        {
            return InnerDrawBox.getMax() + LeftSpace + RightSpace;
        }

        public override void update()
        {
            InnerDrawBox.update();
        }

        public override void setup(RectangleF box)
        {
            this.box = box;
            RectangleF innerBox = new RectangleF(box.X + LeftSpace, box.Y + TopSpace, box.Width - LeftSpace - RightSpace, box.Height - TopSpace - BottomSpace);
            InnerDrawBox.setup(innerBox);
            this.box.Height = TopSpace + BottomSpace + InnerDrawBox.box.Height;
        }

        public override void draw(DrawContext con)
        {
            InnerDrawBox.draw(con);
        }

        public override DrawBox clone()
        {
            return new GeometryBox(InnerDrawBox.clone(), TopSpace, BottomSpace, RightSpace, LeftSpace);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "GeometryBox:");
            sb.AppendLine(ttabs + "box: " + box);
            sb.AppendLine(ttabs + "Left: " + LeftSpace);
            sb.AppendLine(ttabs + "Right: " + RightSpace);
            sb.AppendLine(ttabs + "Top: " + TopSpace);
            sb.AppendLine(ttabs + "Bottom: " + BottomSpace);
            InnerDrawBox.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
    }
}
