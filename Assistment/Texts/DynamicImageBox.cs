using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Texts
{
    public class DynamicImageBox : DrawBox
    {
        public Image Image { get; private set; }
        public SizeF Min { get; private set; }
        public SizeF Max { get; private set; }

        public DynamicImageBox(Image Image)
            : this(Image, SizeF.Empty, Image.Size)
        {

        }
        public DynamicImageBox(Image Image, float MinX, float MinY, float MaxX, float MaxY)
            : this(Image, new SizeF(MinX, MinY), new SizeF(MaxX, MaxY))
        {
        }
        public DynamicImageBox(Image Image, SizeF Min, SizeF Max)
        {
            this.Image = Image;
            this.Min = Min;
            this.Max = Max;
            this.update();
        }

        public override float getSpace()
        {
            return Image.Width * Image.Height;
        }
        public override float getMin()
        {
            return Min.Width;
        }
        public override float getMax()
        {
            return Max.Width;
        }

        public override void update()
        {
            float rel = Image.Height * 1f / Image.Width;

            SizeF newMin = new SizeF();
            newMin.Width = Math.Max(Min.Width, Min.Height / rel);
            newMin.Height = Math.Max(Min.Height, Min.Width * rel);
            Min = newMin;

            SizeF newMax = new SizeF();
            newMax.Width = Math.Min(Math.Min(Max.Width, Max.Height / rel), Image.Width);
            newMax.Height = Math.Min(Math.Min(Max.Height, Max.Width * rel), Image.Height);
            Max = newMax;

            this.box.Width = Image.Width;
            this.box.Height = Image.Height;
        }

        public override void setup(RectangleF box)
        {
            this.box = box;
            this.box.Width = Math.Min(box.Width, Max.Width);
            this.box.Height = Image.Height * this.box.Width / Image.Width;
        }

        public override void draw(DrawContext con)
        {
            con.drawImage(Image, box);
        }

        public override DrawBox clone()
        {
            return new DynamicImageBox(Image, Min, Max);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "DynamicImageBox");
            sb.AppendLine(tabs + "\tMin: " + Min);
            sb.AppendLine(tabs + "\tMax: " + Max);
            sb.AppendLine(tabs + "\tbox" + box);
            sb.AppendLine(tabs + ".");
        }
    }
}
