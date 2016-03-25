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
        public Logger Logger = new Logger("StartLogger", Logger.Mode.None);


        public ControlForm()
        {
            this.Size = new System.Drawing.Size(1200, 900);
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

        public void ChangeLogger(Logger.Mode Mode)
        {
            Logger = new Logger(Control.Name == null ? "null" : Control.Name, Mode);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public override void Refresh()
        {
            Control.Refresh();
            base.Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            if (Control != null)
                Control.Size = ClientSize;
            base.OnResize(e);
        }

        public static void Show(Control Control)
        {
            ControlForm cf = new ControlForm();
            cf.SetControl(Control);
            cf.ShowDialog();
        }
    }
}
