using System.Text;
using System.Drawing;

namespace Assistment.Texts
{
    public class ClippedImageBox : DrawBox
    {
        public Image Image { get; set; }
        public RectangleF SourceRegion { get; set; }
        public RectangleF TargetSize { get; set; }

        public ClippedImageBox(Image Image, RectangleF SourceRegion, RectangleF TargetSize)
        {
            this.Box = this.TargetSize = TargetSize;
            this.SourceRegion = SourceRegion;
            this.Image = Image;
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
            con.DrawClippedImage(Image, Box, SourceRegion);
        }

        public override DrawBox Clone()
        {
            return new ClippedImageBox(Image, SourceRegion, TargetSize);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "ClippedImageBox:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            sb.AppendLine(tabs + "\tSourceRegion: " + SourceRegion);
            sb.AppendLine(tabs + "\tTargetSize: " + TargetSize);
            sb.AppendLine(tabs + ".");
        }
        public override void ForceWordStyle(Brush brush, IFontMeasurer font, byte? style, Pen pen)
        {
        }
    }
}
