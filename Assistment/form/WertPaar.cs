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
    public class WertPaar<T> : UserControl
    {
        public string Name { get; private set; }
        public IWertBox<T> WertBox { get; private set; }

        public WertPaar(string Name, IWertBox<T> WertBox)
        {
            this.Name = Name;
            this.WertBox = WertBox;
        }
    }
}
