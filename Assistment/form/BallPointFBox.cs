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
        private PointF KorrekturWert = new PointF(0, 0);// new PointF(50, 50); //wegen 4k HD scaling notwendig?
        private PointF MousePoint;
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

        public void SetImage(string Pfad)
        {
            if (Pfad == null || Pfad.Length == 0)
                Image = null;
            else
                Image = Image.FromFile(Pfad);
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
            if (e.Button == MouseButtons.Left)
            {
                Working = true;
                pointFBox1.UserPoint = ((PointF)e.Location).sub(KorrekturWert).sub(offPoint).div(imgSize);
                this.MousePoint = e.Location;
                Working = false;
                OnUserValueChanged(this, e);
            }
        }
        public void DrawLabel(object sender, EventArgs e)
        {
            float radius = 10;

            g.Clear(Color.White);
            if (image != null)
                g.DrawImage(image, new RectangleF(offPoint, imgSize.ToSize()));
            g.FillEllipse(BallColor,
                new RectangleF(
                MousePoint
                .sub(radius, radius),
                new SizeF(2, 2).mul(radius)));
            label1.Refresh();
        }

        protected void OnUserValueChanged(object sender, EventArgs e)
        {
            if (Working)
                return;
            this.MousePoint = offPoint.add(pointFBox1.UserPoint.mul(imgSize));
            UserValueChanged(sender, e);
        }
        protected void OnInvalidChanged(object sender, EventArgs e)
        {
            InvalidChange(sender, e);
        }

        public void DDispose()
        {
            Dispose();
            if (g != null)
                g.Dispose();
            if (image != null)
                image.Dispose();
        }
    }
}
