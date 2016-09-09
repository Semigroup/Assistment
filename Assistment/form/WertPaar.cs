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
            this.Key = Key;
            this.WertBox = WertBox;

            Label label = new Label();
            label.AutoSize = true;
            label.Text = Key;
            Controls.Add(label);
            Control Wert = WertBox as Control;
            Controls.Add(Wert);
            Wert.Location = new Point(label.Width, 0);
            this.AutoSize = true;

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
        public bool Valid()
        {
            return WertBox.Valid();
        }
        public void AddInvalidListener(EventHandler Handler)
        {
            WertBox.AddInvalidListener(Handler);
        }
        public void DDispose()
        {
            WertBox.DDispose();
            this.Dispose();
        }
    }
}
