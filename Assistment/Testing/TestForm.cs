using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Assistment.Texts;

namespace Assistment.Testing
{
    public partial class TestForm : System.Windows.Forms.Form
    {
        CString cs;
        float textWidth;
        bool trace;
        Graphics g;
        xFont font;
        public TestForm(CString cs)
        {
            InitializeComponent();
            this.cs = cs;
            this.textWidth = this.Width;
            this.trace = false;
            this.BackgroundImage = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(this.BackgroundImage);
            font = new FontMeasurer("Calibri", 22);
            mal();
        }

        public void mal()
        {
            if (textWidth >= cs.getMin())
            {
                g.Clear(Color.White);
                DrawContextGraphics con = new DrawContextGraphics(g);//new RectangleF(0, 0, textWidth, 0),
                cs.setup(new RectangleF(0, 0, textWidth, 0));
                cs.draw(con);
            }
            else
                g.Clear(Color.Red);
            g.DrawLine(Pens.Black, textWidth, 0, textWidth, this.Height);
            this.Refresh();

        }

        public static void startDialog(CString cs)
        {
            TestForm tf = new TestForm(cs);
            tf.ShowDialog();
        }
        public static void startDialog()
        {
            TestForm tf = new TestForm(new CString());
            tf.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            mal();
        }

        private void TestForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.trace = true;
        }
        private void TestForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.trace = false;
        }
        private void TestForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (trace)
            {
                textWidth = e.X;
                mal();
            }
        }

        private void TestForm_SizeChanged(object sender, EventArgs e)
        {
            this.BackgroundImage = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(this.BackgroundImage);
            this.mal();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            cs = new CString();
            cs.addRegex(textBox1.Text, font);
            this.mal();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(1024);
            cs.InStringBuilder(sb, "");
            File.WriteAllText("test.txt", sb.ToString());
        }
    }
    public class testBox : DrawBox
    {
        float space;
        float min;
        float max;
        Pen pen;
        Brush brush;
        static Random rand = new Random();

        public testBox(float space, float min, float max, Pen pen, Brush brush)
        {
            this.max = max;
            this.min = min;
            this.space = space;
            this.pen = pen;
            this.brush = brush;
        }
        public testBox(float width, float height)
        {
            this.min = this.max = width;
            this.space = width * height;
            this.pen = Pens.Black;
            this.brush = new SolidBrush(Color.FromArgb(rand.Next() | (255 << 24)));
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

        public override void draw(DrawContext con)
        {
            if (brush != null)
                con.fillRectangle(brush, box);
            if (pen != null)
                con.drawRectangle(pen, box);
        }

        public override void update()
        {
        }


        public override DrawBox clone()
        {
            return new testBox(space, min, max, pen, brush);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "Word:");
            sb.AppendLine(tabs + "\tbox: " + box);
            sb.AppendLine(tabs + "\tspace: " + space);
            sb.AppendLine(tabs + "\tmin: " + min);
            sb.AppendLine(tabs + "\tmax: " + max);
            if (pen == null)
                sb.AppendLine(tabs + "\tpen: null");
            else
                sb.AppendLine(tabs + "\tpen: " + pen);
            if (brush == null)
                sb.AppendLine(tabs + "\tbrush: null");
            else
                sb.AppendLine(tabs + "\tbrush: " + brush);
            sb.AppendLine(tabs);
        }

        public override void setup(RectangleF box)
        {
            this.box = new RectangleF(box.X, box.Y, box.Width, space / box.Width);

        }
    }

}
