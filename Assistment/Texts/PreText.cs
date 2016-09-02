using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assistment.Extensions;

namespace Assistment.Texts
{
    /// <summary>
    /// A PreText implements the methods which are common for Texts and CStrings.
    /// </summary>
    public abstract class PreText : DrawContainer
    {
        protected class Line
        {
            public List<DrawBox> List = new List<DrawBox>();
            public RectangleF Box = new RectangleF();
            public bool LineEnds = false;
            public float MaxWidth = 0;

            private DrawBox[] DrawBoxs;
            private float[] AssignedWidths;

            public Line(float MaxWidth)
            {
                this.MaxWidth = MaxWidth;
            }

            public void Add(DrawBox DrawBox)
            {
                List.Add(DrawBox);
                if (DrawBox.getMin() > 0.01f)
                {
                    Box.Width += DrawBox.getMin();
                    Box.Height = Math.Max(Box.Height, DrawBox.getSpace() / DrawBox.getMin());
                }
                LineEnds = LineEnds || Box.Width >= MaxWidth || DrawBox.endsLine;
            }
            public bool CanAdd(DrawBox DrawBox)
            {
                return !LineEnds && Box.Width + DrawBox.getMin() <= MaxWidth;
            }
            public void Setup(PointF Location, float Alignment, bool RightToLeft)
            {
                if (!RightToLeft)
                {
                    Location.X += (MaxWidth - Box.Width) * Alignment;
                    Box.Location = Location;
                    foreach (var item in List)
                    {
                        item.setup(Location);
                        Location.X = item.Right;
                    }
                }
                else
                {
                    Location.X += MaxWidth - (MaxWidth - Box.Width) * Alignment;
                    foreach (var item in List)
                    {
                        Location.X -= item.getMin();
                        item.setup(Location);
                    }
                    Box.Location = Location;
                }
            }
            public void ComplexSetup(PointF Location, float Alignment, bool RightToLeft)
            {
                DrawBoxs = List.ToArray();
                AssignedWidths = DrawBoxs.Map(x => x.getMin()).ToArray();

                int i = DrawBoxs.IndexOfMaxim(x => x.getSpace() / x.getMax());
                float h = DrawBoxs[i].getSpace() / DrawBoxs[i].getMax(); // Mindesthöhe = min{ A_i / max_i | i }
                
                float Remainder = MaxWidth - Box.Width;

                while (Remainder > 0)
                {
                    
                }
            }

            public static List<Line> BreakDown(IEnumerable<DrawBox> DrawBoxs, float Width)
            {
                List<Line> lines = new List<Line>();
                Line line = new Line(Width);
                lines.Add(line);
                foreach (var item in DrawBoxs)
                    if (line.CanAdd(item))
                        line.Add(item);
                    else
                    {
                        line = new Line(Width);
                        lines.Add(line);
                        line.Add(item);
                    }
                return lines;
            }
        }

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
                word.update();
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
        public override void setup(RectangleF box)
        {
            if (words.Count == 0)
            {
                this.box = new RectangleF(box.Location, new SizeF());
                return;
            }

            List<Line> Lines = Line.BreakDown(words, box.Width);
            setupLines(box, Lines);
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

        protected abstract void setupLines(RectangleF box, List<Line> Lines);
    }
}
