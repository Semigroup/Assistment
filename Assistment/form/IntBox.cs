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
    public partial class IntBox : UserControl, IWertBox<int>
    {
        private int userValue;
        public int UserValue
        {
            get { return userValue; }
            set {
                this.textBox1.Text = value.ToString();
            }
        }
        public int UserValueMaximum { get; set; }
        public int UserValueMinimum { get; set; }

        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public IntBox()
        {
            InitializeComponent();
            this.textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
            UserValueMaximum = int.MaxValue;
            UserValueMinimum = int.MinValue;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int loc;
            if (int.TryParse(textBox1.Text, out loc) && loc <= UserValueMaximum && loc >= UserValueMinimum)
            {
                this.userValue = loc;
                this.textBox1.ForeColor = Color.Black;
                UserValueChanged(sender, e);
            }
            else
            {
                this.textBox1.ForeColor = Color.Red;
                InvalidChange(sender, e);
            }
        }

        public int GetValue()
        {
            return UserValue;
        }
        public void SetValue(int Value)
        {
            UserValue = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            UserValueChanged += Handler;
        }
        public bool Valid()
        {
            int loc;
            return int.TryParse(textBox1.Text, out loc) && loc <= UserValueMaximum && loc >= UserValueMinimum;
        }
        public void AddInvalidListener(EventHandler Handler)
        {
            InvalidChange += Handler;
        }
    }
}
