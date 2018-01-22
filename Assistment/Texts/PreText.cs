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
                if (DrawBox.Min> 0.01f)
                {
                    Box.Width += DrawBox.Min;
                    Box.Height = Math.Max(Box.Height, DrawBox.Space/ DrawBox.Min);
                }
                LineEnds = LineEnds || Box.Width >= MaxWidth || DrawBox.EndsLine;
            }
            public bool CanAdd(DrawBox DrawBox)
            {
                return !LineEnds && Box.Width + DrawBox.Min<= MaxWidth;
            }
            public void SimpleAssignment()
            {
                DrawBoxs = List.ToArray();
                int n = DrawBoxs.Length;
                AssignedWidths = new float[n];
                for (int i = 0; i < n; i++)
                    AssignedWidths[i] = DrawBoxs[i].Min;
                Box.Width = AssignedWidths.Sum();
            }
            public void ComplexAssignment()
            {
                SimpleAssignment();
                int n = DrawBoxs.Length;
                if (n == 0)
                    return;

                int I = DrawBoxs.IndexOfMaxim(x => x.Max> 1 ? x.Space/ x.Max: 0);
                if (I < 0)
                    return;
                float h = DrawBoxs[I].Space/ DrawBoxs[I].Max; // Mindesthöhe = min{ A_i / max_i | i }
                h = Math.Min(1, h);

                float Space = 0;
                float Width = MaxWidth;

                List<int> ToChange = new List<int>();
                for (int i = 0; i < n; i++)
                {
                    DrawBox item = DrawBoxs[i];
                    if (item.Min< 0.01f)
                        break;
                    float maxHeight = item.Space/ item.Min;
                    if (maxHeight > h)
                    {
                        ToChange.Add(i);
                        Space += item.Space;
                    }
                    else
                        Width -= item.Min;
                }
                foreach (var i in ToChange)
                    AssignedWidths[i] = Width * DrawBoxs[i].Space/ Space;
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
                        RectangleF itemBox = new RectangleF(Location, new SizeF(AssignedWidths[i], 0));
                        DrawBoxs[i].Setup(itemBox, i == 0);
                        Box.Height = Math.Max(Box.Height, DrawBoxs[i].Box.Height);
                        Location.X = DrawBoxs[i].Right;
                    }
                }
                else
                {
                    Location.X += MaxWidth - (MaxWidth - Box.Width) * Alignment;
                    for (int i = 0; i < n; i++)
                    {
                        Location.X -= AssignedWidths[i];

                        RectangleF itemBox = new RectangleF(Location, new SizeF(AssignedWidths[i], 0));
                        DrawBoxs[i].Setup(itemBox, i == 0);
                        Box.Height = Math.Max(Box.Height, DrawBoxs[i].Box.Height);
                        Location.X = DrawBoxs[i].Left;
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

        protected List<DrawBox> Words { get; private set; }
        private float min, max, space;
        private float currentMax;
        /// <summary>
        /// Von Rechts nach Links
        /// <para></para>
        /// <para>Von Links nach Rechts, falls false (default Wert)</para>
        /// </summary>
        public bool RightToLeft;

        public override int Count => Words.Count;
        public override bool IsReadOnly => false;

        public PreText()
        {
            Words = new List<DrawBox>();
            min = max = currentMax = space = 0;
            RightToLeft = false;
        }
        /// <summary>
        /// creates a deep copy
        /// </summary>
        /// <param name="PreText"></param>
        public PreText(PreText PreText)
        {
            Words = new List<DrawBox>();
            foreach (var item in PreText.Words)
                Words.Add(item.Clone());
            this.min = PreText.min;
            this.max = PreText.max;
            this.currentMax = PreText.currentMax;
            this.space = PreText.space;
            this.RightToLeft = PreText.RightToLeft;
            this.PreferedFont = PreText.PreferedFont;
            this.Alignment = PreText.Alignment;
        }

        public override void Add(DrawBox word)
        {
            Words.Add(word);
            min = Math.Max(min, word.Min);
            currentMax += word.Max;
            space += word.Space;
            if (word.EndsLine)
            {
                max = Math.Max(max, currentMax);
                currentMax = 0;
            }
        }
        public override void Insert(int index, DrawBox word)
        {
            Words.Insert(index, word);
            this.Update();
        }
        public override void Remove(int index)
        {
            Words.RemoveAt(index);
            this.Update();
        }
        public override bool Remove(DrawBox word)
        {
            bool result = Words.Remove(word);
            if (result)
                this.Update();
            return result;
        }

        public override float Space => space;
        public override float Min => min;
        public override float Max => Math.Max(max, currentMax);

        public override void Update()
        {
            min = max = currentMax = space = 0;
            foreach (DrawBox word in Words)
            {
                word.Update();
                min = Math.Max(min, word.Min);
                currentMax += word.Max;
                space += word.Space;
                if (word.EndsLine)
                {
                    max = Math.Max(max, currentMax);
                    currentMax = 0;
                }
            }
        }
        public override void Setup(RectangleF box)
        {
            this.Box = new RectangleF(box.Location, new SizeF());
            if (Words.Count == 0)
                return;

            List<Line> Lines = Line.BreakDown(Words, box.Width);
            PointF Location = box.Location;

            foreach (var line in Lines)
            {
                Assigne(line);
                line.Setup(Location, Alignment, RightToLeft);
                Location.Y += line.Box.Height;
                if (this.Box.Size.Inhalt() > 0)
                    this.Box = this.Box.Extend(line.Box);
                else
                    this.Box = line.Box;
            }
        }
        public override void Draw(DrawContext con)
        {
            foreach (DrawBox item in Words)
            {
                if (item.Box.Y < con.Bildhohe)
                    item.Draw(con);
                else break;
            }
        }

        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return Words.GetEnumerator();
        }
        public override void Clear()
        {
            Words.Clear();
            min = max = currentMax = space = 0;
        }

        public override bool Contains(DrawBox item)
        {
            return Words.Contains(item);
        }
        public override void CopyTo(DrawBox[] array, int arrayIndex)
        {
             Words.CopyTo(array, arrayIndex);
        }

        protected abstract void Assigne(Line Line);
    }
}
