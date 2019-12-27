using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Assistment.Extensions;
using Assistment.Texts;
using System.IO;
using System.Diagnostics;

namespace Assistment.form
{
    public interface IDrawer
    {
        Image Draw(bool Hoch, float ppm);
        int GetDInA();
    }

    public partial class PDFDialog : Form
    {
        public iTextSharp.text.Rectangle[] DinAs = { iTextSharp.text.PageSize.A0,
                                                       iTextSharp.text.PageSize.A1,
                                                       iTextSharp.text.PageSize.A2,
                                                       iTextSharp.text.PageSize.A3,
                                                       iTextSharp.text.PageSize.A4,
                                                       iTextSharp.text.PageSize.A5,
                                                       iTextSharp.text.PageSize.A6,
                                                       iTextSharp.text.PageSize.A7,
                                                       iTextSharp.text.PageSize.A8,
                                                       iTextSharp.text.PageSize.A9,
                                                       iTextSharp.text.PageSize.A10};

        public ImageFormat[] Formats = {  ImageFormat.Png,ImageFormat.Jpeg,
                                           ImageFormat.Bmp, ImageFormat.Emf, ImageFormat.Exif, ImageFormat.Gif,
                                           ImageFormat.Icon,  ImageFormat.MemoryBmp,
                                       ImageFormat.Tiff, ImageFormat.Wmf};
        public RadioButton[] RadioButtons;
        public ImageFormat Format = ImageFormat.Png;
        public bool PDF = false;
        public bool Hoch = false;
        public float ppm = 10;

        public IDrawer Drawer;
        public string Speicherort;

        public static string Subprozess = "\\Subprozesse\\MakePDF.exe";
        public bool UseSubprozess = true;

        private bool changing = false;

        public PDFDialog(IDrawer Drawer)
            : this(Drawer, "*")
        {
        }
        public PDFDialog(IDrawer Drawer, string Speicherort)
        {
            InitializeComponent();

            if (Drawer != null)
                this.targetDinAFormatBox.UserValue = Drawer.GetDInA();

            RadioButtons = new RadioButton[Formats.Length];
            for (int i = 0; i < RadioButtons.Length; i++)
            {
                RadioButtons[i] = new RadioButton();
                RadioButtons[i].Location = new Point(10, 10 + 20 * i);
                RadioButtons[i].Text = Formats[i].ToString();
                RadioButtons[i].AutoSize = true;
                RadioButtons[i].CheckedChanged += Change;
                this.Controls.Add(RadioButtons[i]);
            }
            RadioButtons[0].Checked = true;

            this.Speicherort = Speicherort;
            this.saveFileDialog1.FileName = Speicherort + ".png";
            this.Drawer = Drawer;
        }

        /// <summary>
        /// ohne Punkt
        /// </summary>
        /// <returns></returns>
        public string GetExtension()
        {
            if (PDF)
                return "pdf";
            else
                return Format.ToString();
        }

        private void Change(object sender, EventArgs e)
        {
            for (int i = 0; i < RadioButtons.Length; i++)
                if (RadioButtons[i].Checked)
                {
                    Format = Formats[i];
                    break;
                }
            this.PDF = checkBox1.Checked;
            this.Hoch = checkBox2.Checked;
            ppm = floatBox1.UserValue;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = GetExtension();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.Speicherort = saveFileDialog1.FileName.Verzeichnis() + saveFileDialog1.FileName.FileName();
                Make();
            }
        }
        private void Make()
        {
            backgroundWorker1.RunWorkerAsync();

            int a = Drawer.GetDInA();
            int targetDinA = targetDinAFormatBox.UserValue;
            string imageFile = Speicherort + "." + Format;
            bool targetHoch = Hoch ^ (targetDinA - a % 2 == 1);

            SizeF GrosseInMM = new SizeF();
            SizeF alignment = new SizeF(0.5f, 0.5f);

            using (Image img = Drawer.Draw(Hoch, ppm))
            {
                GrosseInMM.Width = img.Width / ppm;
                GrosseInMM.Height = img.Height / ppm;
                using (FileStream fs = new FileStream(imageFile, FileMode.Create))
                {
                    img.Save(fs, Format);
                    fs.Close();
                    img.Dispose();
                }
            }
            GC.Collect();

            if (PDF && !UseSubprozess)
            {
                iTextSharp.text.Rectangle r = DinAs[a];
                if (!Hoch)
                    r = r.Rotate();
                float width = r.Width / DrawContextDocument.factor;
                float height = r.Height / DrawContextDocument.factor;

                using (Image img2 = Image.FromFile(imageFile))
                {
                    ImageBox ib = new ImageBox(width, height, img2);
                    ib.CreatePDF(Speicherort, r);
                    img2.Dispose();
                }
            }
            else if (PDF && UseSubprozess)
                Process.Start(Directory.GetCurrentDirectory() + Subprozess,
                    "\"" + imageFile + "\""
                    + " " + targetDinA
                    + " " + targetHoch
                    + " " + GrosseInMM.Width
                     + " " + GrosseInMM.Height
                     + " " + alignment.Width
                     + " " + alignment.Height);
        }
        /// <summary>
        /// PPI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void floatBox1_UserValueChanged(object sender, EventArgs e)
        {
            if (changing)
                return;

            changing = true;
            this.floatBox2.UserValue = 25.4f * floatBox1.UserValue;
            changing = false;
            Change(sender, e);
        }
        /// <summary>
        /// DPI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void floatBox2_UserValueChanged(object sender, EventArgs e)
        {
            if (changing)
                return;

            changing = true;
            this.floatBox1.UserValue = floatBox2.UserValue / 25.4f;
            changing = false;
            Change(sender, e);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Value = (progressBar1.Value + 73) % 100;
                    progressBar1.Refresh();
                });
            }
            else
            {
                progressBar1.Value = (progressBar1.Value + 73) % 100;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Value = 0;
                    progressBar1.Refresh();
                });
            }
            else
            {
                progressBar1.Value = 0;
                progressBar1.Refresh();
            }
        }
    }
}
