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
    public partial class ColorBox : UserControl,IWertBox<Color>
    {
        public event EventHandler ColorChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        private bool working = false;

        public Color Color
        {
            get
            {
                return Color.FromArgb(AlphaBox.UserValue, RedBox.UserValue, GreenBox.UserValue, BlueBox.UserValue);
            }
            set
            {
                working = true;
                this.AlphaBox.UserValue = value.A;
                this.RedBox.UserValue = value.R;
                this.GreenBox.UserValue = value.G;
                this.BlueBox.UserValue = value.B;
                working = false;
            }
        }

        public ColorBox()
        {
            InitializeComponent();

            this.AlphaBox.UserValueChanged += ColorBox_ColorChanged;
            this.RedBox.UserValueChanged += ColorBox_ColorChanged;
            this.GreenBox.UserValueChanged += ColorBox_ColorChanged;
            this.BlueBox.UserValueChanged += ColorBox_ColorChanged;

            AlphaBox.AddInvalidListener(InvalidChange);
            RedBox.AddInvalidListener(InvalidChange);
            GreenBox.AddInvalidListener(InvalidChange);
            BlueBox.AddInvalidListener(InvalidChange);
        }

        void ColorBox_ColorChanged(object sender, EventArgs e)
        {
            if (working)
                return;
            ColorChanged(sender, e);
        }

        public Color GetValue()
        {
            return Color;
        }
        public void SetValue(Color Value)
        {
            this.Color = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            ColorChanged += Handler;
        }
        public bool Valid()
        {
            return AlphaBox.Valid() && RedBox.Valid() && GreenBox.Valid() && BlueBox.Valid();
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
