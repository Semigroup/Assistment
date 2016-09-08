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
    public class ButtonReihe : UserControl
    {
        public event EventHandler ButtonClick = delegate { };
        public string Message { get; private set; }
        protected Button[] buttons;
        private ToolTip ToolTip = new ToolTip();

        public ButtonReihe(bool horizontal, params string[] Buttons)
        {
            Message = "";

            Point p = new Point();
            Size s = new Size();
            buttons = new Button[Buttons.Length];
            int i = 0;
            foreach (var item in Buttons)
            {
                Button b = new Button();
                this.Controls.Add(b);
                b.Text = item;
                b.Click += OnButtonClicked;
                b.Location = p;
                b.AutoSize = true;
                if (!horizontal)
                {
                    p.Y = b.Bottom + 0;
                    s.Width = Math.Max(s.Width, b.Width);
                    s.Height = b.Bottom;
                }
                else
                {
                    p.X = b.Right;
                    s.Height = Math.Max(s.Height, b.Height);
                    s.Width = b.Right;
                }
                buttons[i++] = b;
            }
            this.Size = s;
        }
        protected virtual void OnButtonClicked(object sender, EventArgs e)
        {
            Message = (sender as Button).Text;
            ButtonClick(this, e);
        }
        public void Enable(bool Enabled, params string[] Buttons)
        {
            foreach (var item in buttons)
                if (Buttons.Contains(item.Text))
                    item.Enabled = Enabled;
        }
        public void SetToolTip(params string[] ToolTips)
        {
            for (int i = 0; i < buttons.Length; i++)
                ToolTip.SetToolTip(buttons[i], ToolTips[i]);
        }
    }
}
