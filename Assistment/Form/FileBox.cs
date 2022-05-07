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
    public partial class FileBox : UserControl, IWertBox<string>
    {
        public event EventHandler FileChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public bool AllowMultiSelection { get; set; } = true;

        private bool valid;

        public string FilePath
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

        public FileBox()
        {
            InitializeComponent();

            stringBox1.TextChanged += FileChanged;
            stringBox1.AddInvalidListener(InvalidChange);
        }

        public string GetValue() => FilePath;

        public void SetValue(string Value)
        {
            FilePath = Value;
        }

        public void AddListener(EventHandler Handler)
        {
            FileChanged += Handler;
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
            this.openFileDialog1.Multiselect = AllowMultiSelection;
            var value = GetValue();
            if (File.Exists(value))
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(value);

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                FilePath = openFileDialog1.FileName;
        }

        private void stringBox1_TextChanged(object sender, EventArgs e)
        {
            valid = File.Exists(stringBox1.Text);
            stringBox1.ForeColor = valid ? Color.Black : Color.Red;
        }
    }
}
