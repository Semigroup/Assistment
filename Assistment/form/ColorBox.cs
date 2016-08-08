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
        public Color Color
        {
            get
            {
                return Color.FromArgb(AlphaBox.UserValue, RedBox.UserValue, GreenBox.UserValue, BlueBox.UserValue);
            }
            set
            {
                this.AlphaBox.UserValue = value.A;
                this.RedBox.UserValue = value.R;
                this.GreenBox.UserValue = value.G;
                this.BlueBox.UserValue = value.B;
            }
        }

        public ColorBox()
        {
            InitializeComponent();

            this.AlphaBox.UserValueChanged += ColorBox_ColorChanged;
            this.RedBox.UserValueChanged += ColorBox_ColorChanged;
            this.GreenBox.UserValueChanged += ColorBox_ColorChanged;
            this.BlueBox.UserValueChanged += ColorBox_ColorChanged;
        }

        void ColorBox_ColorChanged(object sender, EventArgs e)
        {
            ColorChanged(sender, e);
        }

        public Color GetValue()
        {
            return Color;
        }

        public void SetValue(Color Value)
        {
            this.Color = Color;
        }


        public EventHandler GetUserValueChangedEvent()
        {
            return ColorChanged;
        }
    }
}
