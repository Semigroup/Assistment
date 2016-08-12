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
    public class WertPaar<T> : UserControl, IWertBox<T>
    {
        public string Key { get; private set; }
        public IWertBox<T> WertBox { get; private set; }

        public WertPaar(string Key, IWertBox<T> WertBox)
        {
            this.Key = Name;
            this.WertBox = WertBox;

            Label label = new Label();
            label.Text = Name;
            label.AutoSize = true;
            Controls.Add(label);
            Control Wert = WertBox as Control;
            Wert.Location = new Point(label.Width, 0);
            Controls.Add(Wert);

            this.Size = new Size(label.Width + Wert.Width, Math.Max(label.Height, Wert.Height));
        }

        public T GetValue()
        {
            return WertBox.GetValue();
        }
        public void SetValue(T Value)
        {
            WertBox.SetValue(Value);
        }

        public void AddListener(EventHandler Handler)
        {
            WertBox.AddListener(Handler);
        }
    }
}
