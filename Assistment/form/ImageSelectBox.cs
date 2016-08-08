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
        private Graphics g;

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
                ImageChanged(this, new EventArgs());
                return;
            }
            try
            {
                Image img = Image.FromFile(path);
                this.path = path;
                g.DrawImage(img, new Rectangle(new Point(), label1.Size));
                this.button2.Enabled = true;
                label1.Refresh();
                ImageChanged(this, new EventArgs());
            }
            catch (Exception)
            {
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
        }
        public EventHandler GetUserValueChangedEvent()
        {
            return ImageChanged;
        }
    }
}
