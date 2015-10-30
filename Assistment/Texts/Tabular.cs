using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Texts
{
    public class Tabular : DrawBox
    {
        private class row
        {
            public DrawBox[] drawBoxes;
            public float height;
            /// <summary>
            /// pen draws a line below the row iff pen is not null
            /// </summary>
            public Pen pen;
        }

        /// <summary>
        /// Number of Columns.
        /// <para>This field is final.</para>
        /// </summary>
        public int Columns { get; protected set; }
        /// <summary>
        /// Number of Rows.
        /// <para>This field will be computed.</para>
        /// </summary>
        public int Rows
        {
            get
            {
                return rows.Count;
            }
        }
        private List<row> rows;
        /// <summary>
        /// contains Columns + 1 Pens for each Line between two Columns
        /// </summary>
        public Pen[] columnPens { get; protected set; }

        private float space = 0;
        private float min = 0;
        private float max = 0;
        private float[] mins, maxs;

        public Tabular(int Columns)
        {
            this.columnPens = new Pen[Columns + 1];
            this.Columns = Columns;
            this.rows = new List<row>();
            this.endsLine = false;
            this.box = new RectangleF();
            this.mins = new float[Columns];
            this.maxs = new float[Columns];
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
        public override void update()
        {
            DrawBox drawBox;
            float height;
            float absHeight = 0;

            for (int i = 0; i < Columns; i++)
                this.mins[i] = this.maxs[i] = 0;
            foreach (row item in rows)
            {
                for (int i = 0; i < Columns; i++)
                {
                    drawBox = item.drawBoxes[i];
                    if (drawBox != null)
                    {
                        drawBox.update();
                        this.mins[i] = Math.Max(this.mins[i], drawBox.getMin());
                        this.maxs[i] = Math.Max(this.maxs[i], drawBox.getMax());
                    }
                }
            }
            foreach (row item in rows)
            {
                height = 0;
                for (int i = 0; i < Columns; i++)
                {
                    drawBox = item.drawBoxes[i];
                    if (drawBox != null && mins[i] > 0)
                        height = Math.Max(height, drawBox.getSpace() / this.mins[i]);
                }
                item.height = height;
                absHeight += height;
            }
            this.min = this.max = 0;
            for (int i = 0; i < Columns; i++)
            {
                this.min += this.mins[i];
                this.max += this.maxs[i];
            }
            this.space = this.min * absHeight;
        }

        public void addRow()
        {
            this.addRow(null);
        }
        /// <summary>
        /// adds n rows
        /// </summary>
        /// <param name="n"></param>
        public void addRow(int n)
        {
            this.addRow(n, null);
        }
        public void addRow(Pen line)
        {
            row r = new row();
            r.height = 0;
            r.drawBoxes = new DrawBox[Columns];
            r.pen = line;
            rows.Add(r);
        }
        /// <summary>
        /// adds n rows
        /// </summary>
        /// <param name="n"></param>
        public void addRow(int n, Pen line)
        {
            for (int i = 0; i < n; i++)
                addRow(line);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowNumber">nullbasiert!</param>
        public void removeRow(int rowNumber)
        {
            rows.RemoveAt(rowNumber);
        }

        public void setRowPen(int rowNumber,Pen pen)
        {
            rows[rowNumber].pen = pen;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">nullbasiert!</param>
        /// <param name="column">nullbasiert!</param>
        /// <returns></returns>
        public DrawBox this[int row, int column]
        {
            get { return rows[row].drawBoxes[column]; }
            set { rows[row].drawBoxes[column] = value; }
        }

        public override void setup(RectangleF box)
        {
            this.update();
            RectangleF[] cell = new RectangleF[Columns];
            DrawBox drawBox;
            this.box = box;
            this.box.Height = 0;
            float height;
            float width = 0;
            float diff = 0;
            float t;
            for (int i = 0; i < Columns; i++)
                diff += maxs[i] - mins[i];
            if (diff > 0)
                t = (box.Width - this.min) / diff;
            else
                t = 0;
            for (int i = 0; i < Columns; i++)
            {
                cell[i].X = width + box.X;
                cell[i].Width = mins[i] + (maxs[i] - mins[i]) * t;
                width += cell[i].Width;
            }
            this.box.Width = width;

            foreach (row item in rows)
            {
                height = 0;
                for (int i = 0; i < Columns; i++)
                {
                    cell[i].Y = this.box.Bottom;
                    drawBox = item.drawBoxes[i];
                    if (drawBox != null)
                    {
                        drawBox.setup(cell[i]);
                        height = Math.Max(drawBox.box.Height, height);
                    }
                }
                item.height = height;
                this.box.Height += height;
            }
        }
        public override void draw(DrawContext con)
        {
            DrawBox drawBox;
            float height = box.Y;
            float[] widths = new float[Columns + 1];
            for (int i = 0; i < Columns; i++)
                widths[i] = box.X;
            widths[Columns] = box.Right;

            foreach (row item in rows)
            {
                height += item.height;
                if (height > con.Bildhohe)
                    return;
                if (item.pen != null)
                    con.drawLine(item.pen, box.X, height, box.Right, height);
                for (int i = 0; i < Columns; i++)
                {
                    drawBox = item.drawBoxes[i];
                    if (drawBox != null)
                    {
                        drawBox.draw(con);
                        widths[i] = drawBox.box.X;
                    }
                }
            }
            for (int i = 0; i < Columns + 1; i++)
                if (columnPens[i] != null)
                    con.drawLine(columnPens[i], widths[i], box.Top, widths[i], box.Bottom);
        }
        public override DrawBox clone()
        {
            Tabular tab = new Tabular(this.Columns);
            tab.space = this.space;
            tab.min = this.min;
            tab.max = this.max;
            for (int i = 0; i < Columns; i++)
            {
                tab.mins[i] = this.mins[i];
                tab.maxs[i] = this.maxs[i];
            }
            row nRow;
            foreach (row item in rows)
            {
                nRow = new row();
                nRow.pen = item.pen;
                nRow.drawBoxes = new DrawBox[Columns];
                for (int i = 0; i < Columns; i++)
                    nRow.drawBoxes[i] = item.drawBoxes[i].clone();
                nRow.height = item.height;
                tab.rows.Add(nRow);
            }
            return tab;
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            int j = 0;
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "Tabular:");
            sb.AppendLine(tabs + "\tbox: " + box);
            sb.AppendLine(tabs + "\tColumns: " + Columns);
            foreach (row item in rows)
            {
                j++;
                sb.AppendLine(tabs + "Zeile: " + j);
                sb.AppendLine(tabs + "Höhe: " + item.height);
                for (int i = 0; i < Columns; i++)
                {
                    sb.AppendLine(tabs + "Spalte: " + i);
                    if (item.drawBoxes[i] == null)
                        sb.AppendLine(ttabs + "null");
                    else
                        item.drawBoxes[i].InStringBuilder(sb, ttabs);
                }
            }
            sb.AppendLine(tabs);
        }
    }
}
