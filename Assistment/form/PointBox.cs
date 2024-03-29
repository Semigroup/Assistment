﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistment.Mathematik;

namespace Assistment.form
{
    public partial class PointBox : UserControl, IWertBox<Size>, IWertBox<Point>
    {
        public event EventHandler PointChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public int UserX
        {
            get { return intBox1.UserValue; }
            set { intBox1.UserValue = value; }
        }
        public int UserY
        {
            get { return intBox2.UserValue; }
            set { intBox2.UserValue = value; }
        }

        public Point UserPoint
        {
            get
            {
                return new Point(UserX, UserY);
            }
            set
            {
                UserX = value.X;
                UserY = value.Y;
            }
        }
        public Size UserSize
        {
            get
            {
                return new Size(UserX, UserY);
            }
            set
            {
                UserX = value.Width;
                UserY = value.Height;
            }
        }

        public PointBox()
        {
            InitializeComponent();

            this.intBox1.UserValueChanged += PointBox_PointChanged;
            this.intBox2.UserValueChanged += PointBox_PointChanged;

            intBox1.AddInvalidListener(InvalidChange);
            intBox2.AddInvalidListener(InvalidChange);
        }

        void PointBox_PointChanged(object sender, EventArgs e)
        {
            PointChanged(sender, e);
        }

        public Size GetValue()
        {
            return UserSize;
        }
        public void SetValue(Size Value)
        {
            UserSize = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            PointChanged += Handler;
        }
        Point IWertBox<Point>.GetValue()
        {
            return UserPoint;
        }
        public void SetValue(Point Value)
        {
            UserPoint = Value;
        }
        public bool Valid()
        {
            return intBox1.Valid() && intBox2.Valid();
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
