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
    public partial class EnumBox : UserControl, IWertBox<object>
    {
        private Type enumType;
        public Type EnumType
        {
            get { return enumType; }
            set
            {
                if (this.enumType != value)
                {
                    this.enumType = value;
                    this.comboBox1.Items.Clear();
                    foreach (var item in Enum.GetNames(enumType))
                        this.comboBox1.Items.Add(item);
                }
            }
        }
        private object userValue;
        public object UserValue
        {
            get
            {
                return userValue;
            }
            set
            {
                if (value != null)
                {
                    this.EnumType = value.GetType();
                    this.comboBox1.Text = value.ToString();
                }
                else
                {
                    EnumType = null;
                    comboBox1.Text = "";
                }
            }
        }

        private bool valid = true;

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
                valid = true;
                UserValueChanged(sender, e);
            }
            catch (Exception)
            {
                comboBox1.ForeColor = Color.Red;
                valid = false;
                InvalidChange(sender, e);
            }
        }

        public object GetValue()
        {
            return UserValue;
        }
        public void SetValue(object Value)
        {
            this.UserValue = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            UserValueChanged += Handler;
        }


        public bool Valid()
        {
            return valid;
        }

        public void AddInvalidListener(EventHandler Handler)
        {
            InvalidChange += Handler;
        }
        public void DDispose()
        {
            this.Dispose();
        }
    }
}
