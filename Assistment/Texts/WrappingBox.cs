using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Texts
{
    public abstract class WrappingBox : DrawBox
    {
        protected DrawBox drawBox;
        public DrawBox DrawBox { get { return GetDrawBox(); } set { SetDrawBox(value); } }

        public WrappingBox()
        {

        }
        public WrappingBox(DrawBox DrawBox)
        {
            this.DrawBox = DrawBox;
        }

        protected virtual DrawBox GetDrawBox()
        {
            return drawBox;
        }
        protected virtual void SetDrawBox(DrawBox Value)
        {
            this.drawBox = Value;
        }

        public override float getSpace()
        {
            return DrawBox.getSpace();
        }
        public override float getMin()
        {
            return DrawBox.getMin();
        }
        public override float getMax()
        {
            return DrawBox.getMax();
        }
        public override void Move(System.Drawing.PointF ToMove)
        {
            base.Move(ToMove);
            drawBox.Move(ToMove);
        }

        public override void update()
        {
            drawBox.update();
        }
        public override void setup(System.Drawing.RectangleF box)
        {
            drawBox.setup(box);
            this.box = drawBox.box;
        }
        public override void draw(DrawContext con)
        {
            drawBox.draw(con);
        }
    }
}
