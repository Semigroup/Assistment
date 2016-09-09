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
    public partial class ChainedSizeFBox : UserControl, IWertBox<SizeF>
    {
        public bool Chained
        {
            get { return checkBox1.Checked; }
            set { checkBox1.Checked = value; }
        }

        private bool working = false;
        private SizeF oldValue = new SizeF(1, 1);

        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };


        public ChainedSizeFBox()
        {
            InitializeComponent();

            XBox.UserValueChanged += new EventHandler(XBox_UserValueChanged);
            YBox.UserValueChanged += new EventHandler(YBox_UserValueChanged);
            XBox.InvalidChange += OnInvalidChange;
            YBox.InvalidChange += OnInvalidChange;
        }

        void YBox_UserValueChanged(object sender, EventArgs e)
        {
            if (working)
                return;
            if (Chained)
            {
                working = true;
                if (Math.Abs(oldValue.Height) > 0.00001f)
                    XBox.UserValue = oldValue.Width / oldValue.Height * YBox.UserValue;
                oldValue = GetValue();
                working = false;
            }
            OnUserValueChanged(sender, e);
        }
        void XBox_UserValueChanged(object sender, EventArgs e)
        {
            if (working)
                return;
            if (Chained)
            {
                working = true;
                if (Math.Abs(oldValue.Width) > 0.00001f)
                    YBox.UserValue = oldValue.Height / oldValue.Width * XBox.UserValue;
                oldValue = GetValue();
                working = false;
            }
            OnUserValueChanged(sender, e);
        }

        protected virtual void OnUserValueChanged(object sender, EventArgs e)
        {
            UserValueChanged(this, e);
        }
        protected virtual void OnInvalidChange(object sender, EventArgs e)
        {
            if (working)
                return;
            InvalidChange(this, e);
        }

        public SizeF GetValue()
        {
            return new SizeF(XBox.UserValue, YBox.UserValue);
        }
        public void SetValue(SizeF Value)
        {
            working = true;
            XBox.UserValue = Value.Width;
            YBox.UserValue = Value.Height;
            oldValue = Value;
            working = false;
        }
        public void AddListener(EventHandler Handler)
        {
            UserValueChanged += Handler;
        }
        public bool Valid()
        {
            return XBox.Valid() && YBox.Valid();
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
