using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Assistment.Texts;

namespace Assistment.Forms
{
    /// <summary>
    /// klappt einen text (body) auf und zu, wenn mit links auf header geklickt wird.
    /// </summary>
    public class Klappbox : FormBox
    {
        public DrawBox header { get; private set; }
        public FormBox body { get; private set; }
        public bool aufgeklappt { get; set; }

        private float min, max, space;

        public Klappbox(DrawBox header, FormBox body, bool aufgeklappt)
        {
            this.header = header;
            this.body = body;
            this.aufgeklappt = aufgeklappt;
            Update();
        }

        public void klapp()
        {
            this.aufgeklappt ^= true;
            this.Update();
        }

        public override float Space => space;
        public override float Min => min;
        public override float Max => max;

        public void setBody(FormBox body)
        {
            if (body == this)
                throw new ArgumentException("I see what you did there...");
            this.body = body;
            this.Update();
        }

        public override void Update()
        {
            min = header.Min;
            max = header.Max;
            space = header.Space;
            if (aufgeklappt)
            {
                min = Math.Max(min, body.Min);
                max = Math.Min(max, body.Max);
                space += body.Space;
            }
        }
        public override void Setup(RectangleF box)
        {
            this.Box = box;

            header.Setup(box);
            box.Y += header.Box.Height;
            this.Box.Width = header.Box.Width;
            if (aufgeklappt)
            {
                body.Setup(box);
                box.Y += body.Box.Height;
                this.Box.Width = Math.Max(box.Width, body.Box.Width);
            }
            this.Box.Height = box.Y - this.Box.Y;
        }
        public override void Draw(DrawContext con)
        {
            header.Draw(con);
            if (aufgeklappt)
                body.Draw(con);
        }

        public override void click(PointF point)
        {
            if ((context.button == MouseButtons.Left) && header.Check(point))
            {
                klapp();
                context.SetupAll();
            }
            else if (aufgeklappt && body.Check(point))
                body.click(point);
        }
        public override void move(PointF point)
        {
            if (aufgeklappt && body.Check(point))
                body.move(point);
        }
        public override void release(PointF point)
        {
            if (aufgeklappt && body.Check(point))
                body.release(point);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "Klappbox:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            sb.AppendLine(tabs + "\taufgeklappt: " + aufgeklappt);
            sb.AppendLine(tabs + "\tHeader: ");
            header.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + "\tBody: ");
            body.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }

        public override FormBox flone()
        {
            Klappbox kb = new Klappbox(header.Clone(), body.flone(), aufgeklappt);
            return kb;
        }
        public override void setContext(FormContext context)
        {
            base.setContext(context);
            body.setContext(context);
        }
    }
}
