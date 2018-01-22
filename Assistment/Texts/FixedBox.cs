using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Texts
{
    public class FixedBox : WrappingBox
    {
        public SizeF FixSize { get; set; }
        public SizeF Alignment { get; set; }
        public bool HorizontallyFixed { get; set; }
        public bool VerticallyFixed { get; set; }

        public FixedBox(SizeF Size, DrawBox DrawBox)
            : this(Size, true, true, DrawBox)
        {
        }
        public FixedBox(SizeF Size, bool HorizontallyFixed, bool VerticallyFixed, DrawBox DrawBox)
            : base(DrawBox)
        {
            this.HorizontallyFixed = HorizontallyFixed;
            this.VerticallyFixed = VerticallyFixed;
            this.Box.Size = FixSize = Size;
        }

        public override float Space => Box.Width * Box.Height;
        public override float Min => Box.Width;
        public override float Max => Box.Height;
        public override void Setup(RectangleF box)
        {
            this.Box = new RectangleF(box.Location, FixSize);
            RectangleF VirtualBox = box;
            if (HorizontallyFixed)
                VirtualBox.Width = FixSize.Width;
            if (VerticallyFixed)
                VirtualBox.Height = FixSize.Height;

            DrawBox.Setup(VirtualBox);
            PointF Rest = new PointF();
            if (HorizontallyFixed)
                Rest.X = VirtualBox.Width - DrawBox.Box.Width;
            else
                box.Width = DrawBox.Box.Width;

            if (VerticallyFixed)
                Rest.Y = VirtualBox.Height - DrawBox.Box.Height;
            else
                box.Height = DrawBox.Box.Height;

            DrawBox.Move(Rest.mul(Alignment));
        }

        public override DrawBox Clone()
        {
            FixedBox fb = new FixedBox(this.Box.Size, DrawBox.Clone());
            fb.Alignment = Alignment;
            return fb;
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "FixedBox:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            DrawBox.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
    }
}
