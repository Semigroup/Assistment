using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Assistment.Extensions;
using Assistment.Texts;
using System.IO;
using System.Diagnostics;

namespace MakePDF
{
    class Program
    {
       static iTextSharp.text.Rectangle[] DinAs = { iTextSharp.text.PageSize.A0,
                                                       iTextSharp.text.PageSize.A1,
                                                       iTextSharp.text.PageSize.A2,
                                                       iTextSharp.text.PageSize.A3,
                                                       iTextSharp.text.PageSize.A4,
                                                       iTextSharp.text.PageSize.A5,
                                                       iTextSharp.text.PageSize.A6,
                                                       iTextSharp.text.PageSize.A7,
                                                       iTextSharp.text.PageSize.A8,
                                                       iTextSharp.text.PageSize.A9,
                                                       iTextSharp.text.PageSize.A10};

        static void Main(string[] args)
        {
            string imageFile = args[0];
            string Speicherort = imageFile.Verzeichnis() + imageFile.FileName() + ".pdf";
            int a = int.Parse(args[1]);

            iTextSharp.text.Rectangle r = DinAs[a];
          
            using (Image img2 = Image.FromFile(imageFile))
            {
                bool Hoch = img2.Width < img2.Height;

                if (!Hoch)
                    r = r.Rotate();
                float width = r.Width / DrawContextDocument.factor;
                float height = r.Height / DrawContextDocument.factor;

                ImageBox ib = new ImageBox(width, height, img2);
                ib.createPDF(Speicherort, r);
                img2.Dispose();
            }
        }
    }
}
