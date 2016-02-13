using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assistment.form
{
    public class ControlForm : Form
    {
        public Control Control { get; set; }
        public Logger Logger = new Logger("ControlForm");
        public bool logging;

        public ControlForm()
            : this(false)
        {

        }
        public ControlForm(bool logging)
        {
            this.Size = new System.Drawing.Size(800, 600);
            this.logging = logging;
        }

        public void SetControl(Control Control)
        {
            if (this.Control != null)
                this.Controls.Remove(this.Control);
            this.Control = Control;
            if (this.Control != null)
                this.Controls.Add(Control);
            this.Text = Control != null ? Control.Name : "Null";
            this.OnResize(new EventArgs());
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (logging)
                Logger.Show();
        }

        public override void Refresh()
        {
            Control.Refresh();
            base.Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            if (Control != null)
            {
                Control.Size = ClientSize;
            }
            base.OnResize(e);
        }

        public static void Show(Control Control)
        {
            ControlForm cf = new ControlForm(false);
            cf.SetControl(Control);
            cf.ShowDialog();
        }
    }
}
