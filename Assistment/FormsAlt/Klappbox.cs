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
            update();
        }

        public void klapp()
        {
            this.aufgeklappt ^= true;
            this.update();
        }

        public override float getSpace()
        {
            return space;
        }
        public override float getMin()
        {
            return min;
        }
        public override float getMax()
        {
            return max;
        }

        public void setBody(FormBox body)
        {
            if (body == this)
                throw new ArgumentException("I see what you did there...");
            this.body = body;
            this.update();
        }

        public override void update()
        {
            min = header.getMin();
            max = header.getMax();
            space = header.getSpace();
            if (aufgeklappt)
            {
                min = Math.Max(min, body.getMin());
                max = Math.Min(max, body.getMax());
                space += body.getSpace();
            }
        }
        public override void setup(RectangleF box)
        {
            this.box = box;

            header.setup(box);
            box.Y += header.box.Height;
            this.box.Width = header.box.Width;
            if (aufgeklappt)
            {
                body.setup(box);
                box.Y += body.box.Height;
                this.box.Width = Math.Max(box.Width, body.box.Width);
            }
            this.box.Height = box.Y - this.box.Y;
        }
        public override void draw(DrawContext con)
        {
            header.draw(con);
            if (aufgeklappt)
                body.draw(con);
        }

        public override void click(PointF point)
        {
            if ((context.button == MouseButtons.Left) && header.check(point))
            {
                klapp();
                context.SetupAll();
            }
            else if (aufgeklappt && body.check(point))
                body.click(point);
        }
        public override void move(PointF point)
        {
            if (aufgeklappt && body.check(point))
                body.move(point);
        }
        public override void release(PointF point)
        {
            if (aufgeklappt && body.check(point))
                body.release(point);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "Klappbox:");
            sb.AppendLine(tabs + "\tbox: " + box);
            sb.AppendLine(tabs + "\taufgeklappt: " + aufgeklappt);
            sb.AppendLine(tabs + "\tHeader: ");
            header.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + "\tBody: ");
            body.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }

        public override FormBox flone()
        {
            Klappbox kb = new Klappbox(header.clone(), body.flone(), aufgeklappt);
            return kb;
        }
        public override void setContext(FormContext context)
        {
            base.setContext(context);
            body.setContext(context);
        }
    }
}
