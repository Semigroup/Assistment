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

        public PDFDialog(IDrawer Drawer, string Speicherort)
        {
            InitializeComponent();

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
            this.Drawer = Drawer;
            this.button1.Text = Speicherort + ".* erschaffen";
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
            int a = Drawer.GetDInA();
            using (Image img = Drawer.Draw(Hoch, ppm))
            {
                using (FileStream fs = new FileStream(Speicherort + "." + Format, FileMode.Create))
                {
                    img.Save(fs, Format);
                    fs.Close();
                    img.Dispose();
                }
            }
            GC.Collect();

            if (PDF)
            {
                iTextSharp.text.Rectangle r = DinAs[a];
                if (!Hoch)
                    r = r.Rotate();
                float width = r.Width / DrawContextDocument.factor;
                float height = r.Height / DrawContextDocument.factor;

                using (Image img2 = Image.FromFile(Speicherort + "." + Format))
                {
                    ImageBox ib = new ImageBox(width, height, img2);
                    ib.createPDF(Speicherort, r);
                    img2.Dispose();
                }
            }
        }
    }
}
