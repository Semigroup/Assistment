using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Assistment.form
{
    public partial class DirectoryBox : UserControl, IWertBox<string>
    {
        public event EventHandler DirectoryChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        private bool valid;

        public string Directory
        {
            get
            {
                return stringBox1.Text;
            }
            set
            {
                this.stringBox1.Text = value;
            }
        }

        public DirectoryBox()
        {
            InitializeComponent();

            stringBox1.TextChanged += DirectoryChanged;
            stringBox1.AddInvalidListener(InvalidChange);
        }

        public string GetValue() => Directory;

        public void SetValue(string Value)
        {
            Directory = Value;
        }

        public void AddListener(EventHandler Handler)
        {
            DirectoryChanged += Handler;
        }

        public bool Valid() => valid;

        public void AddInvalidListener(EventHandler Handler)
        {
            InvalidChange += Handler;
        }

        public void DDispose()
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var value = GetValue();
            if (System.IO.Directory.Exists(value))
                folderBrowserDialog.SelectedPath = value;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                Directory = folderBrowserDialog.SelectedPath;
        }

        private void stringBox1_TextChanged(object sender, EventArgs e)
        {
            valid = System.IO.Directory.Exists(stringBox1.Text);
            stringBox1.ForeColor = valid ? Color.Black : Color.Red;
        }
    }
}
