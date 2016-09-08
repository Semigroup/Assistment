using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Assistment.form
{
    public partial class ImageSelectBox : UserControl, IWertBox<string>
    {
        private string path;
        public string ImagePath
        {
            get
            {
                return path;
            }
            set
            {
                SetPath(value);
            }
        }
        public event EventHandler ImageChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };
        private Graphics g;
        private bool valid = true;

        private bool showImage = true;
        public bool ShowImage
        {
            get { return showImage; }
            set
            {
                showImage = value;
                label1.Visible = value;
                int y = value ? 147 : 0;
                button1.Location = new Point(button1.Left, y);
                button2.Location = new Point(button2.Left, y);
                this.Height = button1.Bottom;
            }
        }

        public ImageSelectBox()
        {
            InitializeComponent();
            label1.Image = new Bitmap(label1.Width, label1.Height);
            g = Graphics.FromImage(label1.Image);
            SetPath(null);
        }
        private void SetPath(string path)
        {
            if (path == null || path.Length == 0)
            {
                this.path = null;
                this.button2.Enabled = false;
                g.Clear(Color.Gray);
                label1.Refresh();
                this.valid = true;
                ImageChanged(this, new EventArgs());
                return;
            }
            try
            {
                using (Image Image = Image.FromFile(path))
                {
                    this.path = path;
                    g.Clear(Color.Gray);
                    g.DrawImage(Image, new Rectangle(new Point(), label1.Size));
                }
                this.button2.Enabled = true;
                label1.Refresh();
                ImageChanged(this, new EventArgs());
                valid = true;
            }
            catch (Exception)
            {
                valid = false;
                InvalidChange(this, EventArgs.Empty);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult d = openFileDialog1.ShowDialog();
            if (d == DialogResult.OK)
                SetPath(openFileDialog1.FileName);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SetPath(null);
        }
        public string GetValue()
        {
            return ImagePath;
        }
        public void SetValue(string Value)
        {
            ImagePath = Value;
            openFileDialog1.FileName = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            ImageChanged += Handler;
        }
        public bool Valid()
        {
            return valid;
        }
        public void AddInvalidListener(EventHandler Handler)
        {
            InvalidChange += Handler;
        }
        public void Dispose()
        {
            if (g != null)
                g.Dispose();
        }
    }
}
