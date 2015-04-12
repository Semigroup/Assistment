using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Assistment.Texts
{
    public class FestesMosaik : DrawBox
    {
        private struct kachel
        {
            public RectangleF relativBox;
            public DrawBox drawOb;

            public kachel(DrawBox drawOb, RectangleF relativBox)
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

        public Brush backColor;
        private List<kachel> kacheln = new List<kachel>();
        //private float min, max;

        public FestesMosaik(RectangleF box)
            : this(box, Brushes.White)
        {

        }
        public FestesMosaik(RectangleF box, Brush backColor)
        {
            this.box = box;
            this.backColor = backColor;
            //this.min = this.max = 0;
        }

        /// <summary>
        /// relBox besteht aus relativen Koordinaten zwischen 0 und 1
        /// </summary>
        /// <param name="drawOb"></param>
        /// <param name="relBox"></param>
        public void addKachel(DrawBox drawOb, RectangleF relBox)
        {
            kacheln.Add(new kachel(drawOb, relBox));
            //this.min = Math.Max(drawOb.getMin() / relBox.Width, this.min);
            //this.max = Math.Min(drawOb.getMax() / relBox.Width, this.max);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "Festes Mosaik:");
            sb.AppendLine(tabs + "\tbox: " + box);
            sb.AppendLine(tabs + "\tbackColor: " + backColor);
            foreach (kachel item in kacheln)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs);
        }
        public override DrawBox clone()
        {
            FestesMosaik m = new FestesMosaik(box, backColor);
            foreach (var item in kacheln)
                m.kacheln.Add(new kachel(item.drawOb, item.relativBox));
            return m;
        }
        public override void draw(DrawContext con)
        {
            con.fillRectangle(backColor, box);
            foreach (kachel item in kacheln)
                item.drawOb.draw(con);
        }

        public override float getMax()
        {
            return box.Width;
        }
        public override float getMin()
        {
            return box.Width;
        }
        public override float getSpace()
        {
            return this.box.Width * this.box.Height;
        }

        public override void setup(RectangleF box)
        {
            this.box.Location = box.Location;
            foreach (kachel item in kacheln)
                item.setup(this.box);
        }
        public override void update()
        {
            foreach (kachel item in kacheln)
                item.drawOb.update();
        }

        //public override void click(MouseEventArgs args)
        //{
        //    foreach (var item in kacheln)
        //        if (item.drawOb.check(args.Location))
        //            item.drawOb.click(args);
        //    //mehrere kacheln können überlappen
        //}
    }
}
