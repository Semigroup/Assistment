using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Mathematik;

namespace Assistment.Texts
{
    public class GeometryBox : WrappingBox
    {
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
            this.DrawBox = InnerDrawBox;
            this.LeftSpace = left;
            this.BottomSpace = bottom;
            this.RightSpace = right;
            this.TopSpace = top;
        }

        public override float getSpace()
        {
            float A = DrawBox.getSpace();
            float a = FastMath.Sqrt(A);

            return (TopSpace + a + BottomSpace) * (RightSpace + a + LeftSpace);
        }
        public override float getMin()
        {
            return DrawBox.getMin() + LeftSpace + RightSpace;
        }
        public override float getMax()
        {
            return DrawBox.getMax() + LeftSpace + RightSpace;
        }

        public override void update()
        {
            DrawBox.update();
        }
        public override void setup(RectangleF box)
        {
            RectangleF innerBox = new RectangleF(box.X + LeftSpace, box.Y + TopSpace, box.Width - LeftSpace - RightSpace, box.Height - TopSpace - BottomSpace);
            DrawBox.setup(innerBox);
            this.box = new RectangleF(DrawBox.box.Left - LeftSpace,
                DrawBox.box.Top - TopSpace,
                DrawBox.box.Width + RightSpace + LeftSpace,
                DrawBox.box.Height + TopSpace + BottomSpace);
        }
        public override void draw(DrawContext con)
        {
            DrawBox.draw(con);
        }

        public override DrawBox clone()
        {
            return new GeometryBox(DrawBox.clone(), TopSpace, BottomSpace, RightSpace, LeftSpace);
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
            DrawBox.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
    }
}
