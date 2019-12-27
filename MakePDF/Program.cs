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
using Assistment.Drawing;
using Assistment.Drawing.LinearAlgebra;
using iTextSharp.text.pdf;
using Assistment.PDF;

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

        /// <summary>
        /// imageFile, targetDinA, hoch?, widthInMM, heightInMM, horizontalAlignment, verticalAlignment
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                float pdfCoordinateByMM = DinAs[4].Width / (DrawContextDocument.factor * 210);
                Console.WriteLine("pdfCoordinateByMM: " + pdfCoordinateByMM);
                Console.WriteLine("DrawContextDocument.factor: " + DrawContextDocument.factor);

                string imageFile = args[0];
                string Speicherort = imageFile.Verzeichnis() + imageFile.FileName() + ".pdf";
                int targetDinA = int.Parse(args[1]);
                bool hoch = bool.Parse(args[2]);
                float widthInMM = float.Parse(args[3]);
                float heightInMM = float.Parse(args[4]);
                SizeF totalSizeMM = new SizeF(widthInMM, heightInMM);
                SizeF alignment = new SizeF(float.Parse(args[5]), float.Parse(args[6]));

                Console.WriteLine(imageFile);
                Console.WriteLine(Speicherort);
                Console.WriteLine(widthInMM + " mm x " + heightInMM + " mm -> din A" + targetDinA + " (hoch: " + hoch + ")");
                Console.WriteLine("alignment: " + alignment);

                iTextSharp.text.Rectangle targetRect = DinAs[targetDinA];
                if (!hoch)
                    targetRect = targetRect.Rotate();
                Size pageSizeMM = targetDinA.DinA(hoch);

                Console.WriteLine("targetRect: " + targetRect);
                Console.WriteLine("pageSizeMM: " + pageSizeMM);

                SizeF fCount = totalSizeMM.div(pageSizeMM);
                Size count = ImageSplitter.Round(fCount);
                Console.WriteLine("fCount: " + fCount);
                Console.WriteLine("count: " + count);

                SizeF totalPixelSize;
                Bitmap[,] parts;
                using (Image image = Image.FromFile(imageFile))
                {
                    totalPixelSize = image.Size;
                    parts = ImageSplitter.Split(image, alignment, totalPixelSize, count.Height, count.Width);
                }
                SizeF pageSizePixel = totalPixelSize.div(count);
                Console.WriteLine("pageSizePixel: " + pageSizePixel);
                Console.WriteLine("totalPixelSize: " + totalPixelSize);

                List<string> pages = new List<string>();

                for (int i = 0; i < count.Height; i++)
                    for (int j = 0; j < count.Width; j++)
                    {
                        Console.WriteLine("Writing Page " + (1 + i * count.Width + j) + " of " + (count.Width * count.Height));
                        pages.Add(WritePage(Speicherort, parts[i, j], i, j, targetRect));
                    }
                Console.WriteLine("Concatting...");
                PDFHelper.Concat(Speicherort, pages);
                Console.WriteLine("Cleaning Up...");
                foreach (var item in pages)
                    File.Delete(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine();
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

        static string WritePage(string speicherort, Bitmap part, int i, int j, iTextSharp.text.Rectangle targetRect)
        {
            string partFile = speicherort + "." + i + "." + j + ".pdf";
            RectangleF currentPagePixel = new RectangleF(0, 0, targetRect.Width, targetRect.Height).div(DrawContextDocument.factor);
            using (iTextSharp.text.Document doc = new iTextSharp.text.Document())
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, File.Create(partFile));
                doc.SetPageSize(targetRect);
                doc.NewPage();
                doc.Open();
                PdfContentByte pCon = writer.DirectContent;
                using (DrawContextDocument dcd = new DrawContextDocument(pCon))
                    dcd.DrawImage(part, currentPagePixel);
            }
            //part.Save(speicherort + "." + i + "." + j + ".png");

            return partFile;
        }
    }
}
