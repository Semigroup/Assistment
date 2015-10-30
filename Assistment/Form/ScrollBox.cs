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

        private Control control;
        private bool vActive;
        private VScrollBar vScrollBar;
        private bool hActive;
        private HScrollBar hScrollBar;

        public ScrollBox(Control control)
        {
            this.control = control;
            //control.SizeChanged += new EventHandler((o, e) => OnSizeChanged(e));

            vScrollBar = new VScrollBar();
            vScrollBar.ValueChanged += new EventHandler(AdjustControlLocation);
            vActive = false;

            hScrollBar = new HScrollBar();
            hScrollBar.ValueChanged += new EventHandler(AdjustControlLocation);
            hActive = false;

            this.Controls.Add(control);
        }

        public void AdjustControlLocation(object sender, EventArgs e)
        {
            control.Location = new Point(-hScrollBar.Value, -vScrollBar.Value);
            control.Refresh();
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
            bool vNotwendig = control.Height > Height;
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
                vScrollBar.Maximum = control.Height;

            bool hNotwendig = control.Width > Width;
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
                hScrollBar.Maximum = control.Width;
            //MessageBox.Show(vNotwendig + " : " + vActive + "; " + hNotwendig + " : " + hActive + ": " + Size + "\r\n" + control.Size);
            base.Refresh();
        }
    }
}
