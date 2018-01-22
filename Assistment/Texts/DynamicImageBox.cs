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
        public SizeF MinSize { get; private set; }
        public SizeF MaxSize { get; private set; }

        public DynamicImageBox(Image Image)
            : this(Image, SizeF.Empty, Image.Size)
        {

        }
        public DynamicImageBox(Image Image, float MinX, float MinY, float MaxX, float MaxY)
            : this(Image, new SizeF(MinX, MinY), new SizeF(MaxX, MaxY))
        {
        }
        public DynamicImageBox(Image Image, SizeF MinSize, SizeF MaxSize)
        {
            this.Image = Image;
            this.MinSize = MinSize;
            this.MaxSize = MaxSize;
            this.Update();
        }

        public override float Space => Image.Width * Image.Height;
        public override float Min => MinSize.Width;
        public override float Max => MaxSize.Width;

        public override void Update()
        {
            float rel = Image.Height * 1f / Image.Width;

            SizeF newMin = new SizeF();
            newMin.Width = Math.Max(MinSize.Width, MinSize.Height / rel);
            newMin.Height = Math.Max(MinSize.Height, MinSize.Width * rel);
            MinSize = newMin;

            SizeF newMax = new SizeF();
            newMax.Width = Math.Min(Math.Min(MaxSize.Width, MaxSize.Height / rel), Image.Width);
            newMax.Height = Math.Min(Math.Min(MaxSize.Height, MaxSize.Width * rel), Image.Height);
            MaxSize = newMax;

            this.Box.Width = Image.Width;
            this.Box.Height = Image.Height;
        }

        public override void Setup(RectangleF box)
        {
            this.Box = box;
            this.Box.Width = Math.Min(box.Width, MaxSize.Width);
            this.Box.Height = Image.Height * this.Box.Width / Image.Width;
        }

        public override void Draw(DrawContext con)
        {
            con.drawImage(Image, Box);
        }

        public override DrawBox Clone()
        {
            return new DynamicImageBox(Image, MinSize, MaxSize);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "DynamicImageBox");
            sb.AppendLine(tabs + "\tMinSize: " + Min);
            sb.AppendLine(tabs + "\tMaxSize: " + Max);
            sb.AppendLine(tabs + "\tbox" + Box);
            sb.AppendLine(tabs + ".");
        }
    }
}
