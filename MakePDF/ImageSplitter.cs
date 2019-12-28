using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Assistment.Extensions;
using Assistment.Texts;
using System.IO;
using System.Diagnostics;
using Assistment.Drawing;
using Assistment.Drawing.LinearAlgebra;
using iTextSharp.text.pdf;

namespace MakePDF
{
    public static class ImageSplitter
    {
        public static string[,] Split(Image image, 
            SizeF alignment, SizeF bigPixelSize,
            int rows, int columns,
            string fileName, ImageFormat partsFormat, string partsEnding)
        {
            PointF offset = bigPixelSize.sub(image.Size).mul(alignment).ToPointF();
            string[,] parts = new string[rows, columns];
            Size partPixelSize = Round(bigPixelSize.div(columns, rows));
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    PointF displace = offset.sub(j * partPixelSize.Width, i * partPixelSize.Height);
                    string name = fileName + "." + i + "." + j + "." + partsEnding;

                    using (Bitmap bitmap = new Bitmap(partPixelSize.Width, partPixelSize.Height))
                    using (Graphics g = bitmap.GetHighGraphics())
                    {
                        g.DrawImage(image, displace);
                        bitmap.Save(name, partsFormat);
                    }
                    parts[i, j] = name;
                }
            return parts;
        }

        public static Size Round(SizeF fCount)
            => new Size((int)Math.Round(fCount.Width), (int)Math.Round(fCount.Height));
    }
}
