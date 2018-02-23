using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assistment.form
{
    public partial class RectangleFBox : UserControl, IWertBox<RectangleF>
    {
        public PointFBox LocationBox { get { return locationBox; } }
        public PointFBox SizeBox { get { return sizeBox; } }

        public event EventHandler RectangleFChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public RectangleF UserRectangleF
        {
            get { return new RectangleF(locationBox.UserPoint, sizeBox.UserSize); }
            set {
                locationBox.UserPoint = value.Location;
                sizeBox.UserSize = value.Size;
            }
        }
        public PointF UserLocation
        {
            get{ return locationBox.UserPoint;}
            set { locationBox.UserPoint = value; }
        }
        public SizeF UserSize
        {
            get { return sizeBox.UserSize; }
            set{ sizeBox.UserSize = value; }
        }

        public RectangleFBox()
        {
            InitializeComponent();

            this.locationBox.PointChanged += RectangleFBox_RectangleFBoxChanged;
            this.sizeBox.PointChanged += RectangleFBox_RectangleFBoxChanged;

            this.locationBox.AddInvalidListener(InvalidChange);
            this.sizeBox.AddInvalidListener(InvalidChange);
        }

        void RectangleFBox_RectangleFBoxChanged(object sender, EventArgs e)
        {
            RectangleFChanged(sender, e);
        }
        public void AddInvalidListener(EventHandler Handler)
        {
            InvalidChange += Handler;
        }
        public void AddListener(EventHandler Handler)
        {
            RectangleFChanged += Handler;
        }
        public void DDispose()
        {
            this.Dispose();
        }
        public RectangleF GetValue()
        {
            return this.UserRectangleF;
        }
        public void SetValue(RectangleF Value)
        {
            this.UserRectangleF = Value;
        }
        public bool Valid()
        {
            return locationBox.Valid() && sizeBox.Valid();
        }
    }
}
