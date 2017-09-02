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

using Assistment.Drawing.LinearAlgebra;


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
            SizeF resultSize = new SizeF(Result.Image.Width.Value / Form.PPM, Result.Image.Height.Value / Form.PPM);
            this.label2.Text = resultSize.Width.ToString("F1") + " mm x " + resultSize.Height.ToString("F1") + " mm";
            this.label3.Text = Result.Title;
            float quality = Math.Min(resultSize.div(Form.DesiredSize).Min(),1);
            this.BackColor = Color.Red.tween(Color.Green, quality);
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
            Form.ShowResult(Result);
        }
    }
}
