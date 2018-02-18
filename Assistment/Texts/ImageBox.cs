using System.Text;
using System.Drawing;

namespace Assistment.Texts
{
    public class ImageBox : DrawBox
    {
        private Image image;
        public Image Image { get { return image; } set { image = value; } }

        public ImageBox(Image Image)
        {
            this.Box.Size = Image.Size;
            this.Image = Image;
        }
        public ImageBox(float width, Image Image)
        {
            this.Box.Width = width;
            this.Box.Height = width * Image.Height / Image.Width;
            this.Image = Image;
        }
        public ImageBox(SizeF size, Image Image)
        {
            this.Box.Size = size;
            this.Image = Image;
        }
        public ImageBox(float width, float height, Image Image) : this(new SizeF(width, height), Image)
        {
        }
        public override float Space => Box.Width * Box.Height;
        public override float Min => Box.Width;
        public override float Max => Box.Width;

        public override void Update()
        {
        }
        public override void Setup(RectangleF box)
        {
            this.Box.Location = box.Location;
        }
        public override void Draw(DrawContext con)
        {
            con.DrawImage(Image, Box);
        }

        public override DrawBox Clone()
        {
            return new ImageBox(Box.Size, Image);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "ImageBox:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            sb.AppendLine(tabs + ".");
        }
        public override void ForceWordStyle(Brush brush, xFont font, byte? style, Pen pen)
        {
        }
    }
}
