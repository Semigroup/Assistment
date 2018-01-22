using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Texts;
using System.Windows.Forms;
using System.Drawing;

namespace Assistment.Forms
{
    public class FormContext : DrawContext
    {
        public const int DIFF_WIDTH = 18;
        public const int DIFF_HEIGHT = 47;

        public int X
        {
            get
            {
                return args.X;
            }
        }
        public int Y
        {
            get
            {
                return args.Y;
            }
        }
        public Point position
        {
            get
            {
                return args.Location;
            }
        }
        public MouseButtons button
        {
            get
            {
                return args.Button;
            }
        }

        public RectangleF box { get; private set; }
        public Form form { get; private set; }
        private FormWindowState oldWindowState = FormWindowState.Normal;
        private Graphics g;
        public long time { get; private set; }
        public MouseEventArgs args { get; private set; }
        private List<FormBox> activeBoxes = new List<FormBox>();

        private bool setupAll, drawAll = false;
        //private List<FormBox> setupThese = new List<FormBox>();
        private List<FormBox> drawThese = new List<FormBox>();

        public FormBox mainForm { get; private set; }

        public FormContext()
        {
            this.time = 0;
            this.backcolor = Brushes.White;
            this.makeForm();
        }
        private void makeForm()
        {
            this.form = new DoubleBufferedForm();
            this.form.MouseDown += new MouseEventHandler(mouseDown);
            this.form.MouseMove += new MouseEventHandler(mouseMove);
            this.form.MouseUp += new MouseEventHandler(mouseUp);
            this.form.ResizeEnd += new EventHandler(resizeEnd);
            this.form.Resize += new EventHandler(resize);
            this.form.BackgroundImageLayout = ImageLayout.None;

            this.form.Width = 1000;
            this.form.Height = 600;
        }

        public void setMain(FormBox mainForm)
        {
            this.mainForm = mainForm;
            mainForm.setContext(this);
        }
        public void setup()
        {
            this.form.BackgroundImage = new Bitmap(form.Width - DIFF_WIDTH, form.Height - DIFF_HEIGHT);
            this.g = Graphics.FromImage(this.form.BackgroundImage);
            box = new RectangleF(0, 0, this.form.BackgroundImage.Width, this.form.BackgroundImage.Height);
            this.mainForm.Setup(box);
        }
        public void draw()
        {
            this.g.FillRectangle(backcolor, box);
            mainForm.draw();
            form.Refresh();
        }
        public void open()
        {
            mainForm.Setup(box);
            draw();
            form.Show();
        }
        public void openDialog()
        {
            mainForm.Setup(box);
            draw();
            form.ShowDialog();
        }

        //public void SetupMe(FormBox obj)
        //{
        //    if (!setupThese.Contains(obj))
        //        this.setupThese.Add(obj);
        //}
        public void SetupAll()
        {
            this.setupAll = true;
        }
        public void DrawMe(FormBox obj)
        {
            if (!drawThese.Contains(obj))
                this.drawThese.Add(obj);
        }
        public void DrawAll()
        {
            this.drawAll = true;
        }

        public void mouseDown(object sender, MouseEventArgs args)
        {
            this.args = args;
            List<FormBox> boxes = new List<FormBox>(activeBoxes);
            mainForm.click();
            foreach (FormBox item in boxes)
                item.click();
            repair();
        }
        public void mouseUp(object sender, MouseEventArgs args)
        {
            this.args = args;
            List<FormBox> boxes = new List<FormBox>(activeBoxes);
            mainForm.release();
            foreach (FormBox item in boxes)
                item.release();
            repair();
        }
        public void mouseMove(object sender, MouseEventArgs args)
        {
            this.args = args;
            if (args.Button != MouseButtons.None)
            {
                List<FormBox> boxes = new List<FormBox>(activeBoxes);
                mainForm.move();
                foreach (FormBox item in boxes)
                    item.move();
                repair();
            }
        }
        public void resizeEnd(object sender, EventArgs args)
        {
            if (mainForm != null)
            {
                setup();
                draw();
            }

        }
        public void resize(object sender, EventArgs args)
        {
            if (this.form.WindowState == FormWindowState.Maximized || this.oldWindowState == FormWindowState.Maximized)
                resizeEnd(sender, args);
            this.oldWindowState = this.form.WindowState;
        }

        public void repair()
        {
            if (setupAll)
            {
                setupAll = false;
                mainForm.Setup(box);
                drawAll = true;
            }
            if (drawAll)
            {
                draw();
                drawAll = false;
            }
            else foreach (var item in drawThese)
                    item.drawAgain();
            drawThese.Clear();
            form.Refresh();
        }

        public void activate(FormBox box)
        {
            if ((box != mainForm) && !activeBoxes.Contains(box))
                activeBoxes.Add(box);
        }
        public void deactivate(FormBox box)
        {
            activeBoxes.Remove(box);
        }
        public bool isActive(FormBox box)
        {
            return activeBoxes.Contains(box) || (box == mainForm);
        }

        public override void drawRectangle(Pen pen, float x, float y, float width, float height)
        {
            g.DrawRectangle(pen, x, y, width, height);
        }
        public override void fillRectangle(Brush brush, float x, float y, float width, float height)
        {
            g.FillRectangle(brush, x, y, width, height);
        }
        public override void drawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            g.DrawLine(pen, x1, y1, x2, y2);
        }
        public override void drawString(string text, Font font, Brush brush, float x, float y, float height)
        {
            g.DrawString(text, font, brush, x, y);
        }
        public override void drawImage(Image img, float x, float y)
        {
            g.DrawImage(img, x, y);
        }
        public override void drawImage(Image img, float x, float y, float width, float height)
        {
            g.DrawImage(img, x, y, width, height);
        }
        public override void drawClippedImage(Image img, float x, float y, RectangleF source)
        {
            g.DrawImage(img, x, y, source, GraphicsUnit.Pixel);
        }
        public override void drawClippedImage(Image img, RectangleF destination, RectangleF source)
        {
            g.DrawImage(img, destination, source, GraphicsUnit.Pixel);
        }
        public override void drawEllipse(Pen pen, float x, float y, float width, float height)
        {
            g.DrawEllipse(pen, x, y, width, height);
        }
        public override void fillEllipse(Brush brush, float x, float y, float width, float height)
        {
            g.FillEllipse(brush, x, y, width, height);
        }
        public override void drawPolygon(Pen pen, PointF[] polygon)
        {
            g.DrawPolygon(pen, polygon);
        }
        public void clear(RectangleF box)
        {
            g.FillRectangle(backcolor, box);
        }

        public override void Dispose()
        {
            g.Dispose();
            form.Close();
            form.Dispose();
        }

        public override void newPage()
        {
            throw new NotImplementedException();
        }

        public override void fillPolygon(Brush Brush, PointF[] polygon)
        {
            throw new NotImplementedException();
        }
    }
}
