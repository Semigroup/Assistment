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
            this.FixSize = Size;
            this.HorizontallyFixed = HorizontallyFixed;
            this.VerticallyFixed = VerticallyFixed;
            this.box.Size = FixSize;
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

        public override void setup(RectangleF box)
        {
            this.box = new RectangleF(box.Location, FixSize);
            RectangleF VirtualBox = box;
            if (HorizontallyFixed)
                VirtualBox.Width = FixSize.Width;
            if (VerticallyFixed)
                VirtualBox.Height = FixSize.Height;

            DrawBox.setup(VirtualBox);
            PointF Rest = new PointF();
            if (HorizontallyFixed)
                Rest.X = VirtualBox.Width - DrawBox.box.Width;
            else
                box.Width = DrawBox.box.Width;

            if (VerticallyFixed)
                Rest.Y = VirtualBox.Height - DrawBox.box.Height;
            else
                box.Height = DrawBox.box.Height;

            DrawBox.Move(Rest.mul(Alignment));
        }

        public override DrawBox clone()
        {
            FixedBox fb = new FixedBox(this.box.Size, DrawBox.clone());
            fb.Alignment = Alignment;
            return fb;
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "FixedBox:");
            sb.AppendLine(tabs + "\tbox: " + box);
            DrawBox.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
    }
}
