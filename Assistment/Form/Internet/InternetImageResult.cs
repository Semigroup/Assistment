using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Google;
using Google.Apis;
using Google.Apis.Customsearch;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;


namespace Assistment.form.Internet
{
    public partial class InternetImageResult : UserControl
    {
        public Result Result { get; private set; }
        public InternetChoosePictureForm Form { get; private set; }

        public InternetImageResult()
        {
            InitializeComponent();
        }

        public void SetUp(Result Result, InternetChoosePictureForm Form)
        {
            this.Form = Form;
            this.Result = Result;
            pictureBox1.ImageLocation = Result.Image.ThumbnailLink;
            this.label2.Text =
                (Result.Image.Width.Value / Form.PPM).ToString("F1") + " mm x " + (Result.Image.Height.Value / Form.PPM).ToString("F1") + " mm";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Form.SaveResult(Result);
        }
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            Form.RemoveResult(this);
        }
        private void Ansehen_Click(object sender, EventArgs e)
        {

        }
    }
}
