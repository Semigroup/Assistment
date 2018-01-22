using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Drawing;

namespace Assistment.Forms
{
    public class AlignedDrawForm : FormBox
    {
        public IDrawable drawable { get; private set; }
        private ContextAligner contextAligner;

        public AlignedDrawForm(IDrawable drawable)
        {
            this.drawable = drawable;
        }
        public override FormBox flone()
        {
            return new AlignedDrawForm(drawable);
        }

        public override float Space => 0;
        public override float Min => 0;
        public override float Max => 0;

        public override void Update()
        {
        }

        public override void Setup(System.Drawing.RectangleF box)
        {
            this.Box = box;
            contextAligner = new ContextAligner(box, drawable);
        }
        public override void Draw(Texts.DrawContext con)
        {
            contextAligner.context = con;
            drawable.draw(contextAligner);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "AlignedDrawForm:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            sb.AppendLine(tabs + "\tdrawable.RelativeSize: " + drawable.getRelativeSize());
            sb.AppendLine(tabs + "\tdrawable: "+drawable);
            sb.AppendLine(tabs + ".");
        }
    }
}
