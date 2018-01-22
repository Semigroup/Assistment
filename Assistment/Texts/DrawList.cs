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

        public override int Count => list.Count;
        public override bool IsReadOnly => false;

        public DrawList()
        {

        }
        public DrawList(xFont preferedFont)
        {
            this.PreferedFont = preferedFont;
        }
        public DrawList(xFont preferedFont, IEnumerable<string> Zeilen)
        {
            this.PreferedFont = preferedFont;

            foreach (var item in Zeilen)
                this.AddWort(item);
        }
        public DrawList(xFont preferedFont, params string[] Zeilen)
            : this(preferedFont, (IEnumerable<string>)Zeilen)
        {
        }

        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        public override void Add(DrawBox word)
        {
            list.Add(word);
            this.min = Math.Max(this.min, word.Min);
            this.max = Math.Max(this.max, word.Max);
            this.space += word.Space;
        }
        //public override void addRange(DrawContainer container)
        //{
        //    list.AddRange(container);
        //    this.min = Math.Max(this.min, container.getMin());
        //    this.max = Math.Min(this.max, container.getMax());
        //    this.space += container.getSpace();
        //}
        public override void Insert(int index, DrawBox word)
        {
            list.Insert(index, word);
        }
        public override bool Remove(DrawBox word)
        {
            return list.Remove(word);
        }
        public override void Remove(int index)
        {
            list.RemoveAt(index);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = tabs + "\t";
            sb.AppendLine(tabs + "Festes Mosaik:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            sb.AppendLine(tabs + "\tElemente:");
            foreach (DrawBox item in list)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }

        public override DrawBox Clone()
        {
            DrawList l = new DrawList();
            foreach (var item in list)
                l.list.Add(item.Clone());
            l.min = this.min;
            l.max = this.max;
            l.space = this.space;
            return l;
        }
        public override void Draw(DrawContext con)
        {
            foreach (var item in list)
                item.Draw(con);
        }

        public override float Max => max;
        public override float Min => min;
        public override float Space => space;

        public override void Setup(RectangleF box)
        {
            this.Box.Location = box.Location;
            foreach (var item in list)
            {
                item.Setup(box);
                this.Box.Width = Math.Max(this.Box.Width, item.Box.Width);
                box.Offset(0, item.Box.Height);
            }
            this.Box.Height = box.Y - this.Box.Y;
        }
        public override void Update()
        {
            min = max = space = 0;
            foreach (var word in list)
            {
                this.min = Math.Max(this.min, word.Min);
                this.max = Math.Min(this.max, word.Max);
                this.space += word.Space;
            }
        }

        public override void Clear()
        {
            list.Clear();
            min = max = space = 0;
        }

        public override bool Contains(DrawBox item)
        {
            return list.Contains(item);
        }
        public override void CopyTo(DrawBox[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }
    }
}
