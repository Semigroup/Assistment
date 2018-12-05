using System;
using System.Drawing;
using System.Diagnostics;

namespace Assistment.Drawing.Huge
{
    public static class SlaveErweiterer
    {
        public static void Draw(this Slave Slave, Size Grose, float Uberhangsfaktor, Size Teile, string name)
        {
            Size Uberhang = new Size((int)Math.Ceiling(Grose.Width * Uberhangsfaktor / Teile.Width), (int)Math.Ceiling(Grose.Height * Uberhangsfaktor / Teile.Height));
            Draw(Slave, Grose, Uberhang, Teile, name);
        }
        public static void Draw(this Slave Slave, Size Grose, Size Uberhang, Size Teile, string name)
        {
            string digits = "D" + (int)Math.Ceiling(Math.Log10(Math.Max(Teile.Width, Teile.Height)));
            Rectangle Bereich = new Rectangle();
            for (int y = 1; y <= Teile.Height; y++)
            {
                Bereich.Y = Bereich.Bottom;
                Bereich.Height = (y * Grose.Height) / Teile.Height - Bereich.Y;
                for (int x = 1; x <= Teile.Width; x++)
                {
                    Bereich.X = Bereich.Right;
                    Bereich.Width = (x * Grose.Width) / Teile.Width - Bereich.X;
                    Slave.Draw(Bereich, Uberhang).Save(name + ", Teil (" + x.ToString(digits) + ", " + y.ToString(digits) + ").png");
                }
                Bereich.X = 0;
                Bereich.Width = 0;
            }
        }

        public static void DrawProcesses(this Slave Slave, string processName, string[] args, Size Grose, float Uberhangsfaktor, Size Teile, string name)
        {
            Size Uberhang = new Size((int)Math.Ceiling(Grose.Width * Uberhangsfaktor / Teile.Width), (int)Math.Ceiling(Grose.Height * Uberhangsfaktor / Teile.Height));
            DrawProcesses(Slave, processName, args, Grose, Uberhang, Teile, name);
        }
        public static void DrawProcesses(this Slave Slave, string processName, string[] args, Size Grose, Size Uberhang, Size Teile, string name)
        {
            string digits = "D" + (int)Math.Ceiling(Math.Log10(Math.Max(Teile.Width, Teile.Height)));
            Rectangle Bereich = new Rectangle();
            if (args.Length == 0)
            {
                for (int y = 1; y <= Teile.Height; y++)
                {
                    Bereich.Y = Bereich.Bottom;
                    Bereich.Height = (y * Grose.Height) / Teile.Height - Bereich.Y;
                    for (int x = 1; x <= Teile.Width; x++)
                    {
                        Bereich.X = Bereich.Right;
                        Bereich.Width = (x * Grose.Width) / Teile.Width - Bereich.X;
                        Process.Start(processName,
                              "\"" + Bereich.X + "\" "
                            + "\"" + Bereich.Y + "\" "
                            + "\"" + Bereich.Width + "\" "
                            + "\"" + Bereich.Height + "\" "
                            + "\"" + x + "\" "
                            + "\"" + y + "\" ");
                    }
                    Bereich.X = 0;
                    Bereich.Width = 0;
                }
            }
            else
            {
                Bereich = new Rectangle(int.Parse(args[0]), int.Parse(args[1]), int.Parse(args[2]), int.Parse(args[3]));
                int x = int.Parse(args[4]);
                int y = int.Parse(args[5]);
                Slave.Draw(Bereich, Uberhang).Save(name + ", Teil (" + x.ToString(digits) + ", " + y.ToString(digits) + ").png");
            }
        }
    }
}
