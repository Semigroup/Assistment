using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Texts;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

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
                return Args.X;
            }
        }
        public int Y
        {
            get
            {
                return Args.Y;
            }
        }
        public Point position
        {
            get
            {
                return Args.Location;
            }
        }
        public MouseButtons button
        {
            get
            {
                return Args.Button;
            }
        }

        public RectangleF Box { get; private set; }
        public Form Form { get; private set; }
        private FormWindowState oldWindowState = FormWindowState.Normal;
        private Graphics g;
        public long Time { get; private set; }
        public MouseEventArgs Args { get; private set; }
        private List<FormBox> activeBoxes = new List<FormBox>();

        private bool setupAll, drawAll = false;
        //private List<FormBox> setupThese = new List<FormBox>();
        private List<FormBox> drawThese = new List<FormBox>();

        public FormBox mainForm { get; private set; }

        public FormContext()
        {
            this.Time = 0;
            this.Backcolor = Brushes.White;
            this.MakeForm();
        }
        private void MakeForm()
        {
            this.Form = new DoubleBufferedForm();
            this.Form.MouseDown += new MouseEventHandler(MouseDown);
            this.Form.MouseMove += new MouseEventHandler(MouseMove);
            this.Form.MouseUp += new MouseEventHandler(MouseUp);
            this.Form.ResizeEnd += new EventHandler(ResizeEnd);
            this.Form.Resize += new EventHandler(Resize);
            this.Form.BackgroundImageLayout = ImageLayout.None;

            this.Form.Width = 1000;
            this.Form.Height = 600;
        }

        public void SetMain(FormBox mainForm)
        {
            this.mainForm = mainForm;
            mainForm.setContext(this);
        }
        public void Setup()
        {
            this.Form.BackgroundImage = new Bitmap(Form.Width - DIFF_WIDTH, Form.Height - DIFF_HEIGHT);
            this.g = Graphics.FromImage(this.Form.BackgroundImage);
            Box = new RectangleF(0, 0, this.Form.BackgroundImage.Width, this.Form.BackgroundImage.Height);
            this.mainForm.Setup(Box);
        }
        public void Draw()
        {
            this.g.FillRectangle(Backcolor, Box);
            mainForm.draw();
            Form.Refresh();
        }
        public void Open()
        {
            mainForm.Setup(Box);
            Draw();
            Form.Show();
        }
        public void OpenDialog()
        {
            mainForm.Setup(Box);
            Draw();
            Form.ShowDialog();
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

        public void MouseDown(object sender, MouseEventArgs args)
        {
            this.Args = args;
            List<FormBox> boxes = new List<FormBox>(activeBoxes);
            mainForm.click();
            foreach (FormBox item in boxes)
                item.click();
            repair();
        }
        public void MouseUp(object sender, MouseEventArgs args)
        {
            this.Args = args;
            List<FormBox> boxes = new List<FormBox>(activeBoxes);
            mainForm.release();
            foreach (FormBox item in boxes)
                item.release();
            repair();
        }
        public void MouseMove(object sender, MouseEventArgs args)
        {
            this.Args = args;
            if (args.Button != MouseButtons.None)
            {
                List<FormBox> boxes = new List<FormBox>(activeBoxes);
                mainForm.move();
                foreach (FormBox item in boxes)
                    item.move();
                repair();
            }
        }
        public void ResizeEnd(object sender, EventArgs args)
        {
            if (mainForm != null)
            {
                Setup();
                Draw();
            }

        }
        public void Resize(object sender, EventArgs args)
        {
            if (this.Form.WindowState == FormWindowState.Maximized || this.oldWindowState == FormWindowState.Maximized)
                ResizeEnd(sender, args);
            this.oldWindowState = this.Form.WindowState;
        }

        public void repair()
        {
            if (setupAll)
            {
                setupAll = false;
                mainForm.Setup(Box);
                drawAll = true;
            }
            if (drawAll)
            {
                Draw();
                drawAll = false;
            }
            else foreach (var item in drawThese)
                    item.drawAgain();
            drawThese.Clear();
            Form.Refresh();
        }

        public void Activate(FormBox box)
        {
            if ((box != mainForm) && !activeBoxes.Contains(box))
                activeBoxes.Add(box);
        }
        public void Deactivate(FormBox box)
        {
            activeBoxes.Remove(box);
        }
        public bool IsActive(FormBox box)
        {
            return activeBoxes.Contains(box) || (box == mainForm);
        }

        public override void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            g.DrawRectangle(pen, x, y, width, height);
        }
        public override void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            g.FillRectangle(brush, x, y, width, height);
        }
        public override void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            g.DrawLine(pen, x1, y1, x2, y2);
        }
        public override void DrawString(string text, Font font, Brush brush, float x, float y, float height)
        {
            g.DrawString(text, font, brush, x, y);
        }
        public override void DrawImage(Image img, float x, float y)
        {
            g.DrawImage(img, x, y);
        }
        public override void DrawImage(Image img, float x, float y, float width, float height, ImageAttributes imageAttributes)
        {
            g.DrawImage(img,
              new PointF[] { new PointF(x, y), new PointF(x + width, y), new PointF(x, y + height) },
              new RectangleF(0, 0, img.Width, img.Height),
              GraphicsUnit.Pixel,
              imageAttributes);
        }
        public override void DrawClippedImage(Image img, float x, float y, RectangleF source)
        {
            g.DrawImage(img, x, y, source, GraphicsUnit.Pixel);
        }
        public override void DrawClippedImage(Image img, RectangleF destination, RectangleF source)
        {
            g.DrawImage(img, destination, source, GraphicsUnit.Pixel);
        }
        public override void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            g.DrawEllipse(pen, x, y, width, height);
        }
        public override void FillEllipse(Brush brush, float x, float y, float width, float height)
        {
            g.FillEllipse(brush, x, y, width, height);
        }
        public override void DrawPolygon(Pen pen, PointF[] polygon)
        {
            g.DrawPolygon(pen, polygon);
        }
        public void Clear(RectangleF box)
        {
            g.FillRectangle(Backcolor, box);
        }

        public override void Dispose()
        {
            g.Dispose();
            Form.Close();
            Form.Dispose();
        }

        public override void NewPage()
        {
            throw new NotImplementedException();
        }

        public override void FillPolygon(Brush Brush, PointF[] polygon)
        {
            throw new NotImplementedException();
        }
    }
}
