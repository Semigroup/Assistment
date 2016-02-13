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
        public float Min { get; private set; }

        public DynamicImageBox(Image Image) : this(Image, 0)
        {

        }
        public DynamicImageBox(Image Image, float Min)
        {
            this.Image = Image;
            this.Min = Min;
            this.update();
        }

        public override float getSpace()
        {
            return Image.Width * Image.Height;
        }
        public override float getMin()
        {
            return Min;
        }
        public override float getMax()
        {
            return Image.Width;
        }

        public override void update()
        {
            this.box.Width = Image.Width;
            this.box.Height = Image.Height;
        }

        public override void setup(System.Drawing.RectangleF box)
        {
            this.box = box;
            this.box.Height = Image.Height * box.Width / Image.Width;
        }

        public override void draw(DrawContext con)
        {
            con.drawImage(Image, box);
        }

        public override DrawBox clone()
        {
            return new DynamicImageBox(Image, Min);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.Append(tabs + "DynamicImageBox");
            sb.Append(tabs + "\tMin: " + Min);
            sb.Append(tabs + "\tbox" + box); 
        }
    }
}
