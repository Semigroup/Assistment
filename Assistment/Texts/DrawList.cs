using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Texts
{
    public class DrawList : DrawContainer
    {
        private List<DrawBox> list = new List<DrawBox>();
        private float min, max, space = 0;

        public DrawList()
        {

        }
        public DrawList(xFont preferedFont)
        {
            this.preferedFont = preferedFont;
        }
        public DrawList(xFont preferedFont, IEnumerable<string> Zeilen)
        {
            this.preferedFont = preferedFont;

            foreach (var item in Zeilen)
                this.addWort(item);
        }
        public DrawList(xFont preferedFont, params string[] Zeilen)
            : this(preferedFont, (IEnumerable<string>)Zeilen)
        {
        }

        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        public override void add(DrawBox word)
        {
            list.Add(word);
            this.min = Math.Max(this.min, word.getMin());
            this.max = Math.Max(this.max, word.getMax());
            this.space += word.getSpace();
        }
        //public override void addRange(DrawContainer container)
        //{
        //    list.AddRange(container);
        //    this.min = Math.Max(this.min, container.getMin());
        //    this.max = Math.Min(this.max, container.getMax());
        //    this.space += container.getSpace();
        //}
        public override void insert(int index, DrawBox word)
        {
            list.Insert(index, word);
        }
        public override bool remove(DrawBox word)
        {
            return list.Remove(word);
        }
        public override void remove(int index)
        {
            list.RemoveAt(index);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = tabs + "\t";
            sb.AppendLine(tabs + "Festes Mosaik:");
            sb.AppendLine(tabs + "\tbox: " + box);
            sb.AppendLine(tabs + "\tElemente:");
            foreach (DrawBox item in list)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }

        public override DrawBox clone()
        {
            DrawList l = new DrawList();
            foreach (var item in list)
                l.list.Add(item.clone());
            l.min = this.min;
            l.max = this.max;
            l.space = this.space;
            return l;
        }
        public override void draw(DrawContext con)
        {
            foreach (var item in list)
                item.draw(con);
        }

        public override float getMax()
        {
            return max;
        }
        public override float getMin()
        {
            return min;
        }
        public override float getSpace()
        {
            return space;
        }

        public override void setup(RectangleF box)
        {
            this.box.Location = box.Location;
            foreach (var item in list)
            {
                item.setup(box);
                this.box.Width = Math.Max(this.box.Width, item.box.Width);
                box.Offset(0, item.box.Height);
            }
            this.box.Height = box.Y - this.box.Y;
        }
        public override void update()
        {
            min = max = space = 0;
            foreach (var word in list)
            {
                this.min = Math.Max(this.min, word.getMin());
                this.max = Math.Min(this.max, word.getMax());
                this.space += word.getSpace();
            }
        }

        public override void clear()
        {
            list.Clear();
            min = max = space = 0;
        }
    }
}
