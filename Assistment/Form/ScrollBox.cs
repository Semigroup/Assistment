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

        public ScrollBox(Control Control)
        {
            this.ControlChangesSize = new EventHandler((o, e) => OnSizeChanged(e));

            vScrollBar = new VScrollBar();
            vScrollBar.ValueChanged += new EventHandler(AdjustControlLocation);
            vActive = false;

            hScrollBar = new HScrollBar();
            hScrollBar.ValueChanged += new EventHandler(AdjustControlLocation);
            hActive = false;

            this.Control = Control;
            this.Control.SizeChanged += ControlChangesSize;
            this.Controls.Add(Control);
        }

        public void SetControl(Control Control)
        {
            this.Control.SizeChanged -= ControlChangesSize;
            this.Controls.Remove(Control);

            this.Control = Control;
            Control.SizeChanged += ControlChangesSize;
            this.Controls.Add(Control);
        }

        public void AdjustControlLocation(object sender, EventArgs e)
        {
            Control.Location = new Point(-hScrollBar.Value, -vScrollBar.Value);
            Control.Refresh();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            vScrollBar.Location = new Point(Width - barSize.Width, 0);
            vScrollBar.Size = new Size(barSize.Width, Height - barSize.Height);
            vScrollBar.LargeChange = Height - barSize.Height;
            vScrollBar.BringToFront();

            hScrollBar.Location = new Point(0, Height - barSize.Height);
            hScrollBar.Size = new Size(Width - barSize.Width, barSize.Height);
            hScrollBar.LargeChange = Width - barSize.Width;
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
