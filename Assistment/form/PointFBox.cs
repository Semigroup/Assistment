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
    public partial class PointFBox : UserControl, IWertBox<SizeF>, IWertBox<PointF>
    {
        public FloatBox FloatXBox { get { return floatBox1; } }
        public FloatBox FloatYBox { get { return floatBox2; } }

        public event EventHandler PointChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public float UserX
        {
            get { return floatBox1.UserValue; }
            set { floatBox1.UserValue = value; }
        }
        public float UserY
        {
            get { return floatBox2.UserValue; }
            set { floatBox2.UserValue = value; }
        }

        public PointF UserPoint
        {
            get
            {
                return new PointF(UserX, UserY);
            }
            set
            {
                UserX = value.X;
                UserY = value.Y;
            }
        }
        public SizeF UserSize
        {
            get
            {
                return new SizeF(UserX, UserY);
            }
            set
            {
                UserX = value.Width;
                UserY = value.Height;
            }
        }

        public PointFBox()
        {
            InitializeComponent();

            this.floatBox1.UserValueChanged += PointBox_PointChanged;
            this.floatBox2.UserValueChanged += PointBox_PointChanged;

            this.floatBox1.AddInvalidListener(InvalidChange);
            this.floatBox2.AddInvalidListener(InvalidChange);
        }

        void PointBox_PointChanged(object sender, EventArgs e)
        {
            PointChanged(sender, e);
        }

        public SizeF GetValue()
        {
            return UserSize;
        }
        public void SetValue(SizeF Value)
        {
            UserSize = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            PointChanged += Handler;
        }
        PointF IWertBox<PointF>.GetValue()
        {
            return UserPoint;
        }
        public void SetValue(PointF Value)
        {
            UserPoint = Value;
        }
        public bool Valid()
        {
            return floatBox1.Valid() && floatBox2.Valid();
        }
        public void AddInvalidListener(EventHandler Handler)
        {
            InvalidChange += Handler;
        }
    }
}
