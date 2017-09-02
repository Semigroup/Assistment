﻿using System;
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
using Assistment.GoogleKeys;

namespace Assistment.form.Internet
{
    public partial class InternetChoosePictureForm : Form
    {
        public class DialogResult
        {
            public bool Success { get; set; }
            public Result Result { get; set; }
        }

        private bool Searching;
        private CustomsearchService Search;
        private CseResource.ListRequest List;
        public Button MoreResults { get; set; }
        public DialogResult Dialog { get; set; }
        private ScrollBox ScrollBox;
        private PictureBox PictureBox;

        public float PPM => ppmBox1.Ppm;

        public InternetChoosePictureForm()
        {
            InitializeComponent();
            Search = ServiceProvider.GetSearchService();

            MoreResults = new Button()
            {
                Text = "Mehr Suchergebnisse anzeigen!",
                AutoSize = true
            };
            MoreResults.Click += MoreResults_Click;
            this.Dialog = new DialogResult();

            ScrollBox = new ScrollBox();
            PictureBox = new PictureBox();
            ScrollBox.SetControl(PictureBox);
            ScrollBox.Top = scrollList1.Top;
            this.Controls.Add(ScrollBox);
            InternetChoosePictureForm_SizeChanged(this, new EventArgs());
        }

        public void SetDesiredSize(SizeF SizeInMM)
        {
            pointFBox1.UserSize = SizeInMM;
        }
        private void MoreResults_Click(object sender, EventArgs e)
        {
            AppendResults(10, 100);
        }
        private void SuchButton_Click(object sender, EventArgs e)
        {
            if (SuchTextBox.Text.Length == 0)
                return;
            List = Search.SearchImages(SuchTextBox.Text);

            MoreResults_Click(sender, e);
        }
        private async void AppendResults(int iteration, int requiredSolutions)
        {
            if (Searching || List.Start > 100)
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
                    requiredSolutions--;
                }
                if (List.Start < 100)
                    scrollList1.AddControl(MoreResults);
                else
                    scrollList1.AddControl(new Label
                    {
                        Text = "Sorry, es können nur 100 Objekte pro Suche durchsucht werden.\r\nFür mehr Suchergebnisse musst Du schon Google persönlich fragen :(",
                        AutoSize = true
                    });
            }
            scrollList1.SetUp();
            List.Start += 10;
            Searching = false;
            if (requiredSolutions > 0 && iteration > 0)
                AppendResults(--iteration, requiredSolutions);
        }
        private void InternetChoosePictureForm_SizeChanged(object sender, EventArgs e)
        {
            ScrollBox.Height = scrollList1.Height = this.ClientSize.Height - scrollList1.Location.Y;
            ScrollBox.Width = scrollList1.Left = this.ClientSize.Width - scrollList1.Width;
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
        public void ShowResult(Result Result)
        {
            PictureBox.ImageLocation = Result.Link;
            PictureBox.Height = Result.Image.Height.Value;
            PictureBox.Width = Result.Image.Width.Value;
            PictureBox.Refresh();
        }
    }
}
