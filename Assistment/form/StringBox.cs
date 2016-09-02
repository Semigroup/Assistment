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
    public class StringBox : TextBox, IWertBox<string>
    {
        public StringBox()
        {
            this.Size = new Size(200, this.Height);
        }

        public string GetValue()
        {
            return Text;
        }
        public void SetValue(string Value)
        {
            Text = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            this.TextChanged += Handler;
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
