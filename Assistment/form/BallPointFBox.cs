using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;

namespace Assistment.form
{
    public partial class BallPointFBox : UserControl, IWertBox<PointF>
    {
        public Brush BallColor { get; set; }

        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        private Image image;
        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                float r = value != null ? ((SizeF)value.Size).ratio() : 1;
                imgSize = r > 1 ?
                    new PointF(200, 200 / r) : new PointF(r * 200, 200);
                DrawLabel(this, EventArgs.Empty);
            }
        }

        private PointF imgSize = new PointF(200, 200);
        private PointF offPoint { get { return new PointF(200, 200).sub(imgSize).mul(0.5f); } }
        private Graphics g;

        private bool Working = false;

        public BallPointFBox()
        {
            InitializeComponent();

            pointFBox1.AddListener(OnUserValueChanged);
            pointFBox1.AddInvalidListener(OnInvalidChanged);

            UserValueChanged += DrawLabel;

            label1.Image = new Bitmap(200, 200);
            g = label1.Image.GetHighGraphics();
            BallColor = Color.Red.flat(128).ToBrush();

            DrawLabel(this, EventArgs.Empty);
        }

        public PointF GetValue()
        {
            return pointFBox1.UserPoint;
        }
        public void SetValue(PointF Value)
        {
            pointFBox1.UserPoint = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            UserValueChanged += Handler;
        }
        public bool Valid()
        {
            return pointFBox1.Valid();
        }
        public void AddInvalidListener(EventHandler Handler)
        {
            InvalidChange += Handler;
        }

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Working = true;
                pointFBox1.UserPoint = ((PointF)e.Location).sub(offPoint).div(imgSize);
                Working = false;
                OnUserValueChanged(this, e);
            }
        }
        public void DrawLabel(object sender, EventArgs e)
        {
            float radius = 10;

            g.Clear(Color.Black);
            if (image != null)
                g.DrawImage(image, new RectangleF(offPoint, imgSize.ToSize()));
            PointF Target = offPoint.add(pointFBox1.UserPoint.mul(imgSize));
            g.FillEllipse(BallColor,
                new RectangleF(
                Target.sub(radius, radius), new SizeF(2, 2).mul(radius)));
            label1.Refresh();
        }

        protected void OnUserValueChanged(object sender, EventArgs e)
        {
            if (Working)
                return;
            UserValueChanged(sender, e);
        }
        protected void OnInvalidChanged(object sender, EventArgs e)
        {
            InvalidChange(sender, e);
        }
    }
}
