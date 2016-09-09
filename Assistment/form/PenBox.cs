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
    public partial class PenBox : UserControl, IWertBox<Pen>
    {
        public event EventHandler PenChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };
        private Pen pen;
        public Pen Pen
        {
            get
            {
                return pen;
            }
            set
            {
                this.colorBox1.Color = value.Color;
                this.floatBox1.UserValue = value.Width;
            }
        }

        public PenBox()
        {
            InitializeComponent();
            pen = new Pen(Color.Black, 1);
            Pen = pen;

            this.colorBox1.ColorChanged += PenBox_PenChanged;
            this.floatBox1.UserValueChanged += PenBox_PenChanged;

            this.colorBox1.AddInvalidListener(InvalidChange);
            this.floatBox1.AddInvalidListener(InvalidChange);
        }

        private void PenBox_PenChanged(object sender, EventArgs e)
        {
            this.pen.Color = this.colorBox1.Color;
            this.pen.Width = this.floatBox1.UserValue;

            PenChanged(sender, e);
        }

        public Pen GetValue()
        {
            return Pen;
        }
        public void SetValue(Pen Value)
        {
            this.Pen = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            PenChanged += Handler;
        }
        public bool Valid()
        {
            return colorBox1.Valid() && floatBox1.Valid();
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
