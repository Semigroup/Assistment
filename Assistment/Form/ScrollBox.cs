//#define CONTROLDEBUG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistment.Extensions;

namespace Assistment.form
{
    public class ScrollBox : UserControl
    {
        public static readonly Size barSize = new Size(20, 20);

        public Control Control { get; private set; }
        private bool vActive;
        private VScrollBar vScrollBar;
        private bool hActive;
        private HScrollBar hScrollBar;
        private EventHandler ControlChangesSize;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
#if CONTROLDEBUG
            e.Graphics.FillRectangle(Brushes.Red, 0, 0, this.Width, this.Height);
#endif
        }

        public ScrollBox()
        {
            this.ControlChangesSize = new EventHandler((o, e) => OnSizeChanged(e));

            vScrollBar = new VScrollBar();
            vScrollBar.ValueChanged += new EventHandler(AdjustControlLocation);
            vActive = false;

            hScrollBar = new HScrollBar();
            hScrollBar.ValueChanged += new EventHandler(AdjustControlLocation);
            hActive = false;
        }

        public ScrollBox(Control Control) : this()
        {
            SetControl(Control);
        }

        public void SetControl(Control Control)
        {
            if (this.Control != null)
            {
                this.Control.SizeChanged -= ControlChangesSize;
                this.Controls.Remove(Control);
            }
            this.Control = Control;
            if (this.Control != null)
            {
                Control.SizeChanged += ControlChangesSize;
                this.Controls.Add(Control);
            }
        }

        public void AdjustControlLocation(object sender, EventArgs e)
        {
            Control.Location = new Point(-hScrollBar.Value, -vScrollBar.Value);
            Control.Refresh();
            OnScroll(new ScrollEventArgs(ScrollEventType.First, 0));//Sinnvolle Werte hier...
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            int x = Math.Max(Width - barSize.Width, 0);
            int y = Math.Max(Height - barSize.Height, 0);

            vScrollBar.Location = new Point(x, 0);
            vScrollBar.Size = new Size(barSize.Width, y);
            vScrollBar.LargeChange = y;
            vScrollBar.BringToFront();

            hScrollBar.Location = new Point(0, y);
            hScrollBar.Size = new Size(x, barSize.Height);
            hScrollBar.LargeChange = x;
            hScrollBar.BringToFront();

            base.OnSizeChanged(e);
            Refresh();
        }

        public override void Refresh()
        {
            bool vNotwendig = Control.Height > Height;
            if (vNotwendig && !vActive)
            {
                vScrollBar.Value = 0;
                Controls.Add(vScrollBar);
                vActive = true;
            }
            else if (!vNotwendig && vActive)
            {
                vScrollBar.Value = 0;
                Controls.Remove(vScrollBar);
                vActive = false;
            }
            if (vActive)
                vScrollBar.Maximum = Control.Height;

            bool hNotwendig = Control.Width > Width;
            if (hNotwendig && !hActive)
            {
                hScrollBar.Value = 0;
                Controls.Add(hScrollBar);
                hActive = true;
            }
            else if (!hNotwendig && hActive)
            {
                hScrollBar.Value = 0;
                Controls.Remove(hScrollBar);
                hActive = false;
            }
            if (hActive)
                hScrollBar.Maximum = Control.Width;
            base.Refresh();
        }
    }
}
