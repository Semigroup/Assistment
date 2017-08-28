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

namespace Assistment.form.Internet
{
    public partial class InternetChoosePictureForm : Form
    {
        private Regex ImagePattern = new Regex(@"affe", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        public InternetChoosePictureForm()
        {
            InitializeComponent();
        }

        private void SuchButton_Click(object sender, EventArgs e)
        {
            string URL = "https://www.google.de/search?safe=off&biw=2500&bih=2000&tbm=isch&sa=1&q=" + HttpUtility.UrlEncode(SuchTextBox.Text);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string source = sr.ReadToEnd();
            sr.Close();
            response.Close();

            textBox1.Text = "Gesucht nach:\r\n" + URL + "\r\n\r\n";
            Match m = ImagePattern.Match(source);
            MatchCollection mc = ImagePattern.Matches(source);
            textBox1.Text += mc.Count + "\r\n\r\n";
            foreach (Match Match in mc)
                textBox1.Text += "\r\n"+Match.Index;// +"; " + Match.Groups[1].Value + "; " + Match.Index;

            textBox1.Text = source;
        }
    }
}
