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
    public class LabelBox : Label, IWertBox<string>
    {
        public LabelBox()
        {
            this.AutoSize = true;
            this.Text = "";
            this.Font = new Font("Arial", 12);
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            Padding p = new Padding();
            p.All = 10;
            this.Margin = p;
        }

        public string GetValue()
        {
            return this.Text;
        }

        public void SetValue(string Value)
        {
            this.Text = Value;
        }

        public void AddListener(EventHandler Handler)
        {
            
        }

        public bool Valid()
        {
            return true;
        }

        public void AddInvalidListener(EventHandler Handler)
        {
        }

        public void DDispose()
        {
            this.Dispose();
        }
    }
}
