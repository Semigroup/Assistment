using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Texts
{
    /// <summary>
    /// Layers werden in der Reihenfolge ihres Einfügens gemalt
    /// </summary>
    public class LayerBox : DrawContainer
    {
        private List<DrawBox> Layers = new List<DrawBox>();
        private float min = 0, max = 0, space = 0;

        public override int Count => Layers.Count;

        public override bool IsReadOnly => false;

        public override void add(DrawBox word)
        {
            Layers.Add(word);
            min = Math.Max(word.getMin(), min);
            max = Math.Max(word.getMax(), max);
            space = Math.Max(word.getSpace(), space);
        }

        public override void insert(int index, DrawBox word)
        {
            Layers.Insert(index, word);
            min = Math.Max(word.getMin(), min);
            max = Math.Max(word.getMax(), max);
            space = Math.Max(word.getSpace(), space);
        }

        public override void remove(int index)
        {
            Layers.RemoveAt(index);
            update();
        }

        public override bool remove(DrawBox word)
        {
           bool b = Layers.Remove(word);
            update();
            return b;
        }

        public override void clear()
        {
            Layers.Clear();
            min = max = space = 0;
        }

        public override void update()
        {
            foreach (var word in Layers)
            {
                   min = Math.Max(word.getMin(), min);
            max = Math.Max(word.getMax(), max);
            space = Math.Max(word.getSpace(), space);
            }
        }

        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return Layers.GetEnumerator();
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

        public override void setup(RectangleF box)
        {
            this.box = box;
            foreach (var item in Layers)
            {
                item.setup(box);
                this.box.Height = Math.Max(item.box.Height, this.box.Height);         
            }
        }

        public override void draw(DrawContext con)
        {
            foreach (var item in Layers)
                item.draw(con);
        }

        public override DrawBox clone()
        {
            LayerBox lb = new LayerBox();
            lb.min = min;
            lb.max = max;
            lb.space = space;
            foreach (var item in Layers)
                lb.Layers.Add(item.clone());
            return lb;
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "LayerBox:");
            sb.AppendLine(tabs + "\tbox: " + box);
            foreach (DrawBox item in Layers)
                item.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }

        public override bool Contains(DrawBox item)
        {
            return Layers.Contains(item);
        }
        public override void CopyTo(DrawBox[] array, int arrayIndex)
        {
            Layers.CopyTo(array, arrayIndex);
        }
    }
}
