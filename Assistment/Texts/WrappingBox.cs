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

        public override float Space => DrawBox.Space;
        public override float Min => DrawBox.Min;
        public override float Max => DrawBox.Max;
        public override void Move(System.Drawing.PointF ToMove)
        {
            base.Move(ToMove);
            drawBox.Move(ToMove);
        }

        public override void Update()
        {
            drawBox.Update();
        }
        public override void Setup(System.Drawing.RectangleF box)
        {
            drawBox.Setup(box);
            this.Box = drawBox.Box;
        }
        public override void Draw(DrawContext con)
        {
            drawBox.Draw(con);
        }
    }
}
