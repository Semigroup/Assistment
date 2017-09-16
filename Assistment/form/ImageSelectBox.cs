using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.IO;
using System.Net;

using Assistment.Extensions;

using Assistment.form.Internet;

using Assistment.Drawing.LinearAlgebra;

namespace Assistment.form
{
    public partial class ImageSelectBox : UserControl, IWertBox<string>
    {
        /// <summary>
        /// Maximum für Image.Width * Image.Height
        /// </summary>
        public int MaximumImageSize { get; set; }
        private string internetResultsDirectory;
        /// <summary>
        /// Bilder aus dem Internet werden hier abgespeichert.
        /// <para>Ist dieser Wert null (Defaultwert), so wird der Internet-Button ausgegraut.</para>
        /// </summary>
        public string InternetResultsDirectory
        {
            get { return internetResultsDirectory; }
            set
            {
                internetResultsDirectory = value;
                InternetButton.Enabled = value != null;
            }
        }

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
        /// <summary>
        /// in mm
        /// </summary>
        public SizeF DesiredInternetSize { get; set; } = new SizeF(10, 10);

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
                InternetButton.Location = new Point(InternetButton.Left, y);
                this.Height = button1.Bottom;
            }
        }

        public ImageSelectBox()
        {
            InitializeComponent();
            label1.Image = new Bitmap(label1.Width, label1.Height);
            g = Graphics.FromImage(label1.Image);
            SetPath(null);
            internetResultsDirectory = Path.GetTempPath();
            this.InternetButton.Enabled = true;
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
                if (MaximumImageSize > 0)
                    using (var imageStream = File.OpenRead(path))
                    {
                        BitmapDecoder decoder = BitmapDecoder.Create(imageStream,
                            BitmapCreateOptions.IgnoreColorProfile,
                            BitmapCacheOption.None);
                        Size s = new Size(decoder.Frames[0].PixelWidth, decoder.Frames[0].PixelHeight);
                        if (s.Width * s.Height > MaximumImageSize)
                        {
                            MessageBox.Show("Das Bild " + path + " wurde nicht geladen, da seine Größe von "
                                + (s.Width * s.Height) + " = " + s.Width + " x " + s.Height + " Pixeln, die maximal erlaubte Größe von "
                                + MaximumImageSize + " Pixeln überschreitet.", "Memory-Überschreitung!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new OutOfMemoryException();
                        }
                    }
                using (Image Image = Image.FromFile(path))
                {
                    this.path = path;
                    g.Clear(Color.Gray);
                    SizeF Size = Image.Size;
                    Size = Size.Constraint(label1.Size);
                    PointF p = Size.sub(label1.Size).div(-2).ToPointF();
                    g.DrawImage(Image, new RectangleF(p, Size));
                }
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(path);
                openFileDialog1.FileName = Path.GetFileName(path);
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
        public void DDispose()
        {
            this.Dispose();
            if (g != null)
                g.Dispose();
        }
        private void InternetButton_Click(object sender, EventArgs e)
        {
            using (InternetChoosePictureForm Form = new InternetChoosePictureForm())
            {
                Form.SetDesiredSize(DesiredInternetSize);
                Form.ShowDialog();
                if (Form.Dialog.Success)
                    using (WebClient myClient = new WebClient())
                    {
                        Google.Apis.Customsearch.v1.Data.Result Result = Form.Dialog.Result;
                        if (!Directory.Exists(internetResultsDirectory))
                            Directory.CreateDirectory(internetResultsDirectory);
                        string path = Path.Combine(internetResultsDirectory,
                            Result.Title.ToFileName() + Path.GetExtension(Result.Link));
                        path = path.DecollideFilename();
                        try
                        {
                            myClient.DownloadFile(Result.Link, path);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Das Bild bei der Adresse\r\n"
                                + Result.Link
                                + "\r\nkonnte nicht am Ort\r\n"
                                + path
                                + "\r\ngespeichert werden.");
                            return;
                        }
                        this.ImagePath = path;
                    }
            }
        }
    }
}
