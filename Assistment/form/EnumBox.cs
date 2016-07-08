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
    public partial class EnumBox : UserControl
    {
        public Type EnumType { get; private set; }
        private object userValue;
        public object UserValue
        {
            get
            {
                return userValue;
            }
            set
            {
                this.comboBox1.Text = value.ToString();
            }
        }

        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public EnumBox()
        {
            InitializeComponent();
            this.comboBox1.TextChanged += new EventHandler(comboBox1_TextChanged);
        }

        void comboBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                userValue = Enum.Parse(EnumType, comboBox1.Text);
                comboBox1.ForeColor = Color.Black;
                UserValueChanged(sender, e);
            }
            catch (Exception)
            {
                comboBox1.ForeColor = Color.Red;
                InvalidChange(sender, e);
            }
        }

        public void SetType(Type value)
        {
            this.EnumType = value;
            this.comboBox1.Items.Clear();
            foreach (var item in Enum.GetNames(EnumType))
                this.comboBox1.Items.Add(item);
        }
    }
}
