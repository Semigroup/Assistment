using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Texts
{
    /// <summary>
    /// A PreText implements the methods which are common for Texts and CStrings.
    /// </summary>
    public abstract class PreText : DrawContainer
    {
        protected List<DrawBox> words { get; private set; }
        private float min, max, space;
        private float currentMax;
        /// <summary>
        /// Von Rechts nach Links
        /// <para></para>
        /// <para>Von Links nach Rechts, falls false (default Wert)</para>
        /// </summary>
        public bool RightToLeft;

        public PreText()
        {
            words = new List<DrawBox>();
            min = max = currentMax = space = 0;
            RightToLeft = false;
        }
        /// <summary>
        /// creates a deep copy
        /// </summary>
        /// <param name="PreText"></param>
        public PreText(PreText PreText)
        {
            words = new List<DrawBox>();
            foreach (var item in PreText.words)
                words.Add(item.clone());
            this.min = PreText.min;
            this.max = PreText.max;
            this.currentMax = PreText.currentMax;
            this.space = PreText.space;
            this.RightToLeft = PreText.RightToLeft;
            this.preferedFont = PreText.preferedFont;
            this.alignment = PreText.alignment;
        }

        public override void add(DrawBox word)
        {
            words.Add(word);
            min = Math.Max(min, word.getMin());
            currentMax += word.getMax();
            space += word.getSpace();
            if (word.endsLine)
            {
                max = Math.Max(max, currentMax);
                currentMax = 0;
            }
        }
        public override void insert(int index, DrawBox word)
        {
            words.Insert(index, word);
            this.update();
        }
        public override void remove(int index)
        {
            words.RemoveAt(index);
            this.update();
        }
        public override bool remove(DrawBox word)
        {
            bool result = words.Remove(word);
            if (result)
                this.update();
            return result;
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
            return Math.Max(max, currentMax);
        }

        public override void update()
        {
            min = max = currentMax = space = 0;
            foreach (DrawBox word in words)
            {
                min = Math.Max(min, word.getMin());
                currentMax += word.getMax();
                space += word.getSpace();
                if (word.endsLine)
                {
                    max = Math.Max(max, currentMax);
                    currentMax = 0;
                }
            }
        }
        
        public override void draw(DrawContext con)
        {
            foreach (DrawBox item in words)
            {
                if (item.box.Y < con.Bildhohe)
                    item.draw(con);
                else break;
            }
        }

        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return words.GetEnumerator();
        }
        public override void clear()
        {
            words.Clear();
            min = max = currentMax = space = 0;
        }
    }
}
