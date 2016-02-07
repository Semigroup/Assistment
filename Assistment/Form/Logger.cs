using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assistment.form
{
    public partial class Logger : Form
    {
        private ScrollBox ScrollBox;
        private TextBox TextBox;

        public Logger(string Name)
        {
            this.Name = Name;

            InitializeComponent();
            TextBox = new TextBox();
            this.TextBox.Multiline = true;
            this.TextBox.Height = 50;
            this.TextBox.TextChanged += new EventHandler(TextBox_TextChanged);
            this.ScrollBox = new ScrollBox(this.TextBox);
            this.Controls.Add(ScrollBox);

            this.OnSizeChanged(new EventArgs());
        }

        void TextBox_TextChanged(object sender, EventArgs e)
        {
            this.TextBox.Height = this.TextBox.Text.Count(x => x == '\n') * 16 + 50;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.ScrollBox != null)
            {
                this.ScrollBox.Size = this.ClientSize;
                TextBox.Width = this.ClientSize.Width;
            }
        }

        public void Log()
        {
            Log("");
        }
        public void Log(object Object)
        {
            this.TextBox.Text += Object + "\r\n";
        }
    }
}
