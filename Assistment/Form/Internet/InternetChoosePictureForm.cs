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
        public class DialogResult
        {
            public bool Success { get; set; }
            public Result Result { get; set; }
        }

        /// <summary>
        /// Pixel pro Millimeter
        /// </summary>
        public float PPM { get; set; }
        private bool Searching;
        private CustomsearchService Search;
        private CseResource.ListRequest List;
        public Button MoreResults { get; set; }
        public DialogResult Dialog { get; set; }

        public InternetChoosePictureForm()
        {
            InitializeComponent();
            Search = ServiceProvider.GetSearchService();
            PPM = 600 / 25.4f;

            MoreResults = new Button()
            {
                Text = "Mehr Suchergebnisse anzeigen!",
                AutoSize = true
            };
            MoreResults.Click += MoreResults_Click;
            this.Dialog = new DialogResult();
        }

        private void MoreResults_Click(object sender, EventArgs e)
        {
            AppendResults();
        }
        private void SuchButton_Click(object sender, EventArgs e)
        {
            if (SuchTextBox.Text.Length == 0)
                return;
            List = Search.SearchImages(SuchTextBox.Text);

            AppendResults();
        }
        private async void AppendResults()
        {
            if (Searching)
                return;
            Searching = true;
            Search Results = await List.ExecuteAsync();
            if (List.Start == 1)
                scrollList1.ControlList.Clear();
            else
                scrollList1.ControlList.Remove(MoreResults);
            if (Results.Items == null)
                scrollList1.AddControl(new Label
                {
                    Text = "Keine Suchergebnisse gefunden :(",
                    AutoSize = true
                });
            else
            {
                foreach (Result Result in Results.Items)
                {
                    InternetImageResult pb = new InternetImageResult();
                    pb.SetUp(Result, this);
                    scrollList1.AddControl(pb);
                }
                scrollList1.AddControl(MoreResults);
            }
            scrollList1.SetUp();
            List.Start += 10;
            Searching = false;
        }
        private void InternetChoosePictureForm_SizeChanged(object sender, EventArgs e)
        {
            scrollList1.Height = this.ClientSize.Height - scrollList1.Location.Y;
        }
        private void SuchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                SuchButton_Click(sender, e);
            }
        }
        public void RemoveResult(InternetImageResult Result)
        {
            scrollList1.ControlList.Remove(Result);
            scrollList1.SetUp();
        }
        public void SaveResult(Result Result)
        {
            Dialog.Result = Result;
            Dialog.Success = true;
            this.Close();
        }
    }
}
