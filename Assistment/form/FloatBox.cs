using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assistment.form
{
    public partial class FloatBox : UserControl
    {
        private float userValue;
        public float UserValue
        {
            get { return userValue; }
            set
            {
                this.textBox1.Text = value.ToString();
            }
        }

        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public FloatBox()
        {
            InitializeComponent();
            this.textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out userValue))
            {
                this.textBox1.ForeColor = Color.Black;
                UserValueChanged(sender, e);
            }
            else
            {
                this.textBox1.ForeColor = Color.Red;
                InvalidChange(sender, e);
            }
        }
    }
}
