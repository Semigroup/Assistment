using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Assistment.form
{
    public partial class Logger : Form
    {
        [DllImport("kernel32.dll")]
        private static extern Boolean AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern Boolean FreeConsole();

        public enum Mode
        {
            Konsole,
            TextBox,
            None
        }

        private ScrollBox ScrollBox;
        private TextBox TextBox;
        public Mode Modus { get; private set; }

        public Logger(string Name, Mode Modus)
        {
            this.Name = Name;
            this.Modus = Modus;

            Init();
        }

        private void Init()
        {
            switch (Modus)
            {
                case Mode.Konsole:
                    AllocConsole();
                    break;
                case Mode.TextBox:
                    InitializeComponent();
                    TextBox = new TextBox();
                    this.TextBox.Multiline = true;
                    this.TextBox.Height = 50;
                    this.TextBox.TextChanged += new EventHandler(TextBox_TextChanged);
                    this.ScrollBox = new ScrollBox(this.TextBox);
                    this.Controls.Add(ScrollBox);

                    this.OnSizeChanged(new EventArgs());
                    this.Show();
                    break;
                case Mode.None:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void Clean()
        {
        }

        void TextBox_TextChanged(object sender, EventArgs e)
        {
            this.TextBox.Height = this.TextBox.Text.Count(x => x == '\n') * 16 + 50;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.ScrollBox != null)
            {
                this.ScrollBox.Size = this.ClientSize;
                TextBox.Width = this.ClientSize.Width;
            }
        }

        public void Log()
        {
            Log("");
        }
        public void Log(object Object)
        {
            switch (Modus)
            {
                case Mode.Konsole:
                    Console.WriteLine(Object + "");
                    break;
                case Mode.TextBox:
                    this.TextBox.Text += Object + "\r\n";
                    break;
                case Mode.None:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
