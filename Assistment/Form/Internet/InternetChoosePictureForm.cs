using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;

using Google.Apis;
using Google.Apis.Customsearch;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1;
using Google;
using Assistment.Google;

namespace Assistment.form.Internet
{
    public partial class InternetChoosePictureForm : Form
    {
        private CustomsearchService Search;

        public InternetChoosePictureForm()
        {
            InitializeComponent();
            Search = ServiceProvider.GetSearchService();
        }

        private void SuchButton_Click(object sender, EventArgs e)
        {
            CseResource.ListRequest List = Search.SearchImages(SuchTextBox.Text);
                foreach (Result Result in List.Execute().Items)
                {
                    PictureBox pb = new PictureBox();
                    pb.ImageLocation = Result.Image.ThumbnailLink;
                    pb.Height = Result.Image.ThumbnailHeight.Value;
                    pb.Width = Result.Image.ThumbnailWidth.Value;
                    scrollList1.AddControl(pb);
                }
            scrollList1.SetUp();
        }
    }
}
