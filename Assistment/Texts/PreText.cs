using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;

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
            public void SimpleAssignment()
            {
                DrawBoxs = List.ToArray();
                int n = DrawBoxs.Length;
                AssignedWidths = new float[n];
                for (int i = 0; i < n; i++)
                    AssignedWidths[i] = DrawBoxs[i].getMin();
                Box.Width = AssignedWidths.Sum();
            }
            public void ComplexAssignment()
            {
                SimpleAssignment();
                int n = DrawBoxs.Length;
                if (n == 0)
                    return;

                int I = DrawBoxs.IndexOfMaxim(x => x.getMax() > 1 ? x.getSpace() / x.getMax() : 0);
                if (I < 0)
                    return;
                float h = DrawBoxs[I].getSpace() / DrawBoxs[I].getMax(); // Mindesthöhe = min{ A_i / max_i | i }
                h = Math.Min(1, h);

                float Space = 0;
                float Width = MaxWidth;

                List<int> ToChange = new List<int>();
                for (int i = 0; i < n; i++)
                {
                    DrawBox item = DrawBoxs[i];
                    if (item.getMin() < 0.01f)
                        break;
                    float maxHeight = item.getSpace() / item.getMin();
                    if (maxHeight > h)
                    {
                        ToChange.Add(i);
                        Space += item.getSpace();
                    }
                    else
                        Width -= item.getMin();
                }
                foreach (var i in ToChange)
                    AssignedWidths[i] = Width * DrawBoxs[i].getSpace() / Space;
                Box.Width = AssignedWidths.Sum();
            }
            public void Setup(PointF Location, float Alignment, bool RightToLeft)
            {
                int n = DrawBoxs.Length;
                Box.Height = 0;
                if (!RightToLeft)
                {
                    Location.X += (MaxWidth - Box.Width) * Alignment;
                    Box.Location = Location;
                    for (int i = 0; i < n; i++)
                    {
                        DrawBoxs[i].setup(Location, AssignedWidths[i]);
                        Box.Height = Math.Max(Box.Height, DrawBoxs[i].box.Height);
                        Location.X = DrawBoxs[i].Right;
                    }
                }
                else
                {
                    Location.X += MaxWidth - (MaxWidth - Box.Width) * Alignment;
                    for (int i = 0; i < n; i++)
                    {
                        Location.X -= AssignedWidths[i];
                        Box.Height = Math.Max(Box.Height, DrawBoxs[i].box.Height);
                        DrawBoxs[i].setup(Location, AssignedWidths[i]);
                    }
                    Box.Location = Location;
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

        public override int Count => words.Count;
        public override bool IsReadOnly => false;

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
            this.box = new RectangleF(box.Location, new SizeF());
            if (words.Count == 0)
                return;

            List<Line> Lines = Line.BreakDown(words, box.Width);
            PointF Location = box.Location;

            foreach (var line in Lines)
            {
                Assigne(line);
                line.Setup(Location, alignment, RightToLeft);
                Location.Y += line.Box.Height;
                if (this.box.Size.Inhalt() > 0)
                    this.box = this.box.Extend(line.Box);
                else
                    this.box = line.Box;
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

        public override bool Contains(DrawBox item)
        {
            return words.Contains(item);
        }
        public override void CopyTo(DrawBox[] array, int arrayIndex)
        {
             words.CopyTo(array, arrayIndex);
        }

        protected abstract void Assigne(Line Line);
    }
}
