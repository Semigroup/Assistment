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

        public override float Space
        {
            get
            {
                float A = DrawBox.Space;
                float a = FastMath.Sqrt(A);

                return (TopSpace + a + BottomSpace) * (RightSpace + a + LeftSpace);
            }
        }

        public override float Min => DrawBox.Min + LeftSpace + RightSpace;
        public override float Max => DrawBox.Max + LeftSpace + RightSpace;

        public override void Update()
        {
            DrawBox.Update();
        }
        public override void Setup(RectangleF box)
        {
            RectangleF innerBox = new RectangleF(box.X + LeftSpace, box.Y + TopSpace, box.Width - LeftSpace - RightSpace, box.Height - TopSpace - BottomSpace);
            DrawBox.Setup(innerBox);
            this.Box = new RectangleF(DrawBox.Box.Left - LeftSpace,
                DrawBox.Box.Top - TopSpace,
                DrawBox.Box.Width + RightSpace + LeftSpace,
                DrawBox.Box.Height + TopSpace + BottomSpace);
        }
        public override void Draw(DrawContext con)
        {
            DrawBox.Draw(con);
        }

        public override DrawBox Clone()
        {
            return new GeometryBox(DrawBox.Clone(), TopSpace, BottomSpace, RightSpace, LeftSpace);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "GeometryBox:");
            sb.AppendLine(ttabs + "box: " + Box);
            sb.AppendLine(ttabs + "Left: " + LeftSpace);
            sb.AppendLine(ttabs + "Right: " + RightSpace);
            sb.AppendLine(ttabs + "Top: " + TopSpace);
            sb.AppendLine(ttabs + "Bottom: " + BottomSpace);
            DrawBox.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
    }
}
