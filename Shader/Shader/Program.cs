using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Shader
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(string[] args)
        {
            Bitmap b = new Bitmap("Test.png");
            BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, b.PixelFormat);
            int bufferSize = data.Height * data.Stride;
            byte[] bytes = new byte[bufferSize];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            for (int i = 0; i < bufferSize; i += 4)
            {
                if (bytes[i + 3] > 0)
                {
                    bytes[i + 1] = bytes[i + 2] = 0;
                    bytes[i] = bytes[i + 3] = 255;
                }
                else
                {
                    bytes[i] = 0; //B
                    bytes[i + 1] = 0; // G
                    bytes[i + 2] = 0;
                    bytes[i + 3] = 0;
                }
            }
            Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
            b.UnlockBits(data);
            b.Save("Test2.png");
        }

        static void Play()
        {
            using (Game1 game = new Game1())
                game.Run();
        }
    }
#endif
}

