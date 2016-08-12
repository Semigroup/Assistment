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
    public partial class FontBox : UserControl, IWertBox<Font>
    {
        private Font userValue;
        public Font UserValue
        {
            get { return userValue; }
            set
            {
                if (this.userValue != value)
                {
                    this.userValue = value;
                    this.fontDialog1.Font = value;
                    UserValueChanged(this, EventArgs.Empty);
                    if (value != null)
                        this.Schriftart.Text = value.Name + ", " + value.Size + ", " + value.Style;
                    else
                        this.Schriftart.Text = "Schriftart Auswählen";
                }
            }
        }

        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public FontBox()
        {
            InitializeComponent();
        }

        private void Schriftart_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
                UserValue = fontDialog1.Font;
        }

        public Font GetValue()
        {
            return UserValue;
        }

        public void SetValue(Font Value)
        {
            UserValue = Value;
        }

        public void AddListener(EventHandler Handler)
        {
            UserValueChanged += Handler;
        }
    }
}
