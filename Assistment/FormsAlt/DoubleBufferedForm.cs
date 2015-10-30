using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assistment.Forms
{
    public partial class DoubleBufferedForm : Form
    {
        public DoubleBufferedForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
    }
}
