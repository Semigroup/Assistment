using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Assistment.Texts
{
    public class DrawContextGraphicsFlip : DrawContextGraphics
    {
        public bool FlippedX { get; private set; }
        public RectangleF Frame { get; private set; }
        public float Scaling { get; private set; }

        public DrawContextGraphicsFlip(Graphics g, RectangleF Frame, float Scaling)
          : base(g)
        {
            this.Frame = Frame;
            this.Scaling = Scaling;
        }
        public DrawContextGraphicsFlip(Graphics g, RectangleF Frame, float Scaling, Brush backcolor, float Bildhohe)
            : base(g, backcolor, Bildhohe)
        {
            this.Frame = Frame;
            this.Scaling = Scaling;
        }

        public void FlipX()
        {
            if (FlippedX)
            {
                g.ResetTransform();
                g.ScaleTransform(Scaling, Scaling);
                g.TranslateTransform(this.Frame.Left, this.Frame.Top);
                FlippedX = false;
            }
            else
            {
                g.ResetTransform();
                g.ScaleTransform(-Scaling, Scaling);
                g.TranslateTransform(-this.Frame.Right, this.Frame.Top);
                FlippedX = true;
            }
        }
        public override void DrawString(string text, Font font, Brush brush, float x, float y, float height)
        {
            if (FlippedX)
            {
                FlipX();
                SizeF size = g.MeasureString(text, font, int.MaxValue, FontGraphicsMeasurer.Format);
                x = Frame.Right - x - size.Width;
                base.DrawString(text, font, brush, x, y, height);

                FlipX();
            }
            else
                base.DrawString(text, font, brush, x, y, height);
        }
    }
}
