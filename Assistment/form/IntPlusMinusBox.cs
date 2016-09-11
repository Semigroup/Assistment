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
    public partial class IntPlusMinusBox : IntBox
    {
        public IntPlusMinusBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (sender == PlusButton)
            {
                if (this.UserValue + 1 <= this.UserValueMaximum)
                    this.UserValue++;
            }
            else
            {
                if (this.UserValue - 1 >= this.UserValueMinimum)
                    this.UserValue--;
            }
        }
    }
}
