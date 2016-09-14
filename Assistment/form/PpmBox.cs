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
    public partial class PpmBox : UserControl, IWertBox<float>
    {
        private bool working = false;

        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public float Ppm { get { return GetValue(); } set { SetValue(value); } }
        public float PpmMaximum
        {
            get { return floatBoxppm.UserValueMaximum; }
            set
            {
                floatBoxppm.UserValueMaximum = value;
                floatBoxDpI.UserValueMaximum = 25.4f * value;
            }
        }
        public float PpmMinimum
        {
            get { return floatBoxppm.UserValueMinimum; }
            set
            {
                floatBoxppm.UserValueMinimum = value;
                floatBoxDpI.UserValueMinimum = 25.4f * value;
            }
        }

        public PpmBox()
        {
            InitializeComponent();

            this.floatBoxDpI.UserValueChanged += new EventHandler(floatBoxDpI_UserValueChanged);
            this.floatBoxppm.UserValueChanged += new EventHandler(floatBoxDpI_UserValueChanged);

            this.floatBoxDpI.InvalidChange += OnInvalidChange;
            this.floatBoxppm.InvalidChange += OnInvalidChange;
        }

        void floatBoxDpI_UserValueChanged(object sender, EventArgs e)
        {
            if (working)
                return;
            working = true;
            if (sender == floatBoxDpI)
                floatBoxppm.UserValue = floatBoxDpI.UserValue / 25.4f;
            else
                floatBoxDpI.UserValue = floatBoxppm.UserValue * 25.4f;
            working = false;
            UserValueChanged(this, e);
        }

        protected void OnInvalidChange(object sender, EventArgs e)
        {
            InvalidChange(this, e);
        }

        public float GetValue()
        {
            return floatBoxppm.GetValue();
        }

        public void SetValue(float Value)
        {
            floatBoxppm.SetValue(Value);
            floatBoxDpI.UserValue = floatBoxppm.UserValue * 25.4f;
        }

        public void AddListener(EventHandler Handler)
        {
            UserValueChanged += Handler;
        }

        public bool Valid()
        {
            return floatBoxppm.Valid() && floatBoxDpI.Valid();
        }

        public void AddInvalidListener(EventHandler Handler)
        {
            InvalidChange += Handler;
        }

        public void DDispose()
        {
            floatBoxDpI.DDispose();
            floatBoxppm.DDispose();
            base.Dispose();
        }
    }
}
