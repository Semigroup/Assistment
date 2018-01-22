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

        public override void Add(DrawBox word)
        {
            Layers.Add(word);
            min = Math.Max(word.Min, min);
            max = Math.Max(word.Max, max);
            space = Math.Max(word.Space, space);
        }

        public override void Insert(int index, DrawBox word)
        {
            Layers.Insert(index, word);
            min = Math.Max(word.Min, min);
            max = Math.Max(word.Max, max);
            space = Math.Max(word.Space, space);
        }

        public override void Remove(int index)
        {
            Layers.RemoveAt(index);
            Update();
        }

        public override bool Remove(DrawBox word)
        {
           bool b = Layers.Remove(word);
            Update();
            return b;
        }

        public override void Clear()
        {
            Layers.Clear();
            min = max = space = 0;
        }

        public override void Update()
        {
            foreach (var word in Layers)
            {
                   min = Math.Max(word.Min, min);
            max = Math.Max(word.Max, max);
            space = Math.Max(word.Space, space);
            }
        }

        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return Layers.GetEnumerator();
        }

        public override float Space => space;

        public override float Min => min;

        public override float Max => max;

        public override void Setup(RectangleF box)
        {
            this.Box = box;
            foreach (var item in Layers)
            {
                item.Setup(box);
                this.Box.Height = Math.Max(item.Box.Height, this.Box.Height);         
            }
        }

        public override void Draw(DrawContext con)
        {
            foreach (var item in Layers)
                item.Draw(con);
        }

        public override DrawBox Clone()
        {
            LayerBox lb = new LayerBox();
            lb.min = min;
            lb.max = max;
            lb.space = space;
            foreach (var item in Layers)
                lb.Layers.Add(item.Clone());
            return lb;
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "LayerBox:");
            sb.AppendLine(tabs + "\tbox: " + Box);
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
