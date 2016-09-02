using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Texts
{
    public class BackColorBox : WrappingBox
    {
        public Pen RandPen { get; set; }
        public Brush BackColor { get; set; }

        public BackColorBox(Brush BackColor, Pen RandPen, DrawBox DrawBox)
            : base(DrawBox)
        {
            this.BackColor = BackColor;
            this.RandPen = RandPen;
        }

        public override void draw(DrawContext con)
        {
            if (BackColor != null)
                con.fillRectangle(BackColor, DrawBox.box);
            if (RandPen != null)
                con.drawRectangle(RandPen, DrawBox.box);
            base.draw(con);
        }

        public override DrawBox clone()
        {
            return new BackColorBox(BackColor, RandPen, DrawBox);
        }
    }
}
