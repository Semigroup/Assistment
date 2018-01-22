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
                this.drawOb.Setup(getAbsBox(mainBox));
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
            this.min = Math.Max(drawOb.Min/ relBox.Width, this.min);
            this.max = Math.Max(drawOb.Max/ relBox.Width, this.max);
        }
        /// <summary>
        /// entfernt alle Kacheln mit diesem drawObject
        /// </summary>
        /// <param name="drawOb"></param>
        public void removeKachel(FormBox drawOb)
        {
            kacheln.RemoveAll(X => X.drawOb == drawOb);
        }

        public override float Max => min;
        public override float Min => max;
        public override float Space => 0;

        public override void Setup(RectangleF box)
        {
            this.Box = box;
            foreach (kachel item in kacheln)
                item.setup(this.Box);
        }
        public override void Update()
        {
            this.min = this.max = 0;
            foreach (kachel item in kacheln)
            {
                item.drawOb.Update();
                this.min = Math.Max(item.drawOb.Min/ item.relativBox.Width, this.min);
                this.max = Math.Max(item.drawOb.Max/ item.relativBox.Width, this.max);
            }
        }
        public override void Draw(Texts.DrawContext con)
        {
            foreach (kachel item in kacheln)
                item.drawOb.Draw(con);
        }

        public override void click(PointF point)
        {
            foreach (var item in kacheln)
                if (item.drawOb.Check(point))
                    item.drawOb.click(point);
        }
        public override void move(PointF point)
        {
            foreach (var item in kacheln)
                if (item.drawOb.Check(point))
                    item.drawOb.move(point);
        }
        public override void release(PointF point)
        {
            foreach (var item in kacheln)
                if (item.drawOb.Check(point))
                    item.drawOb.release(point);
        }

        public override FormBox flone()
        {
            Mosaik m = new Mosaik();
            foreach (var item in kacheln)
                m.kacheln.Add(new kachel(item.drawOb.flone(), item.relativBox));
            m.Box = Box;
            m.max = this.max;
            m.min = this.min;
            return m;
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "Festes Mosaik:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            foreach (kachel item in kacheln)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs);
        }
    }
}
