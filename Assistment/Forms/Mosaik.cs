using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Assistment.Forms
{
    public class Mosaik : FormBox
    {
        private struct kachel
        {
            public RectangleF relativBox;
            public FormBox drawOb;

            public kachel(FormBox drawOb, RectangleF relativBox)
            {
                this.relativBox = relativBox;
                this.drawOb = drawOb;
            }

            public RectangleF getAbsBox(RectangleF mainBox)
            {
                return new RectangleF(mainBox.X + relativBox.X * mainBox.Width, mainBox.Y + relativBox.Y * mainBox.Height,
                    relativBox.Width * mainBox.Width, relativBox.Height * mainBox.Height);
            }

            public void InStringBuilder(StringBuilder sb, string tabs)
            {
                sb.AppendLine(tabs + "Kachel:");
                sb.AppendLine(tabs + "\trelativBox: " + relativBox);
                sb.AppendLine(tabs + "\tdrawOb:");
                drawOb.InStringBuilder(sb, tabs + "\t");
                sb.AppendLine(tabs);
            }

            public void setup(RectangleF mainBox)
            {
                this.drawOb.setup(getAbsBox(mainBox));
            }
        }

        private List<kachel> kacheln = new List<kachel>();
        private float min, max;

        public Mosaik()
        {
            this.min = this.max = 0;
        }

        public override void setContext(FormContext context)
        {
            base.setContext(context);
            foreach (var item in kacheln)
                item.drawOb.setContext(context);
        }

        /// <summary>
        /// relBox besteht aus relativen Koordinaten zwischen 0 und 1
        /// </summary>
        /// <param name="drawOb"></param>
        /// <param name="relBox"></param>
        public void addKachel(FormBox drawOb, RectangleF relBox)
        {
            kacheln.Add(new kachel(drawOb, relBox));
            this.min = Math.Max(drawOb.getMin() / relBox.Width, this.min);
            this.max = Math.Max(drawOb.getMax() / relBox.Width, this.max);
        }
        /// <summary>
        /// entfernt alle Kacheln mit diesem drawObject
        /// </summary>
        /// <param name="drawOb"></param>
        public void removeKachel(FormBox drawOb)
        {
            kacheln.RemoveAll(X => X.drawOb == drawOb);
        }

        public override float getMax()
        {
            return min;
        }
        public override float getMin()
        {
            return max;
        }
        public override float getSpace()
        {
            return 0;
        }

        public override void setup(RectangleF box)
        {
            this.box = box;
            foreach (kachel item in kacheln)
                item.setup(this.box);
        }
        public override void update()
        {
            this.min = this.max = 0;
            foreach (kachel item in kacheln)
            {
                item.drawOb.update();
                this.min = Math.Max(item.drawOb.getMin() / item.relativBox.Width, this.min);
                this.max = Math.Max(item.drawOb.getMax() / item.relativBox.Width, this.max);
            }
        }
        public override void draw(Texts.DrawContext con)
        {
            foreach (kachel item in kacheln)
                item.drawOb.draw(con);
        }

        public override void click(PointF point)
        {
            foreach (var item in kacheln)
                if (item.drawOb.check(point))
                    item.drawOb.click(point);
        }
        public override void move(PointF point)
        {
            foreach (var item in kacheln)
                if (item.drawOb.check(point))
                    item.drawOb.move(point);
        }
        public override void release(PointF point)
        {
            foreach (var item in kacheln)
                if (item.drawOb.check(point))
                    item.drawOb.release(point);
        }

        public override FormBox flone()
        {
            Mosaik m = new Mosaik();
            foreach (var item in kacheln)
                m.kacheln.Add(new kachel(item.drawOb.flone(), item.relativBox));
            m.box = box;
            m.max = this.max;
            m.min = this.min;
            return m;
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "Festes Mosaik:");
            sb.AppendLine(tabs + "\tbox: " + box);
            foreach (kachel item in kacheln)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs);
        }
    }
}
