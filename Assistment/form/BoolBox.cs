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
    public class BoolBox : CheckBox, IWertBox<bool>
    {
        public BoolBox(string Name)
        {
            this.Text = Name;
            this.AutoSize = true;
        }
        public bool GetValue()
        {
            return Checked;
        }

        public void SetValue(bool Value)
        {
            this.Checked = Value;
        }

        public void AddListener(EventHandler Handler)
        {
            this.CheckedChanged += Handler;
        }
        public bool Valid()
        {
            return true;
        }

        public void AddInvalidListener(EventHandler Handler)
        {
        }
    }
}
