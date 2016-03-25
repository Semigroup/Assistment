using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace Assistment.PDF
{
    public static class PDFHelper
    {
        /// <summary>
        /// kein .pdf an input oder output anhängen, macht er automatisch
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        /// <param name="seiten"></param>
        public static void CopyPages(string output, string input, params int[] seiten)
        {
            using (var ms = new MemoryStream())
            {
                var document = new Document();
                PdfCopy writer = new PdfCopy(document, ms);
                document.Open();
                PdfReader reader = new PdfReader(input + ".pdf");
                foreach (int seite in seiten)
                    writer.AddPage(writer.GetImportedPage(reader, seite));
                document.Close();
                File.WriteAllBytes(output + ".pdf", ms.ToArray());
            }
        }
        /// <summary>
        /// kein .pdf an output anhängen, macht er automatisch
        /// </summary>
        /// <param name="output"></param>
        /// <param name="directory"></param>
        public static void Concat(string output, string directory)
        {
            Concat(output, Directory.GetFiles(directory, "*.pdf"));
        }
        /// <summary>
        /// kein .pdf an output anhängen, macht er automatisch
        /// </summary>
        /// <param name="output"></param>
        /// <param name="pdfFiles"></param>
        public static void Concat(string output, params string[] pdfFiles)
        {
            using (var ms = new MemoryStream())
            {
                var document = new Document();
                PdfCopy writer = new PdfCopy(document, ms);
                document.Open();
                foreach (var item in pdfFiles)
                {
                    PdfReader reader = new PdfReader(item);
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                        writer.AddPage(writer.GetImportedPage(reader, i));
                    writer.FreeReader(reader);
                }
                document.Close();
                File.WriteAllBytes(output + ".pdf", ms.ToArray());
            }
        }
        /// <summary>
        /// Halbiert alle Seiten von input und schmeißt sie in output zusammen (in der Theorie...)
        /// <para>kein .pdf an input oder output anhängen, macht er automatiscj</para>
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        public static void HalfSize(string output, string input)
        {
            using (var ms = new MemoryStream())
            {
                Document document = new Document();
                PdfCopy writer = new PdfCopy(document, ms);
                document.Open();
                PdfReader reader = new PdfReader(input + ".pdf");

                bool up = true;
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    Rectangle r = reader.GetPageSize(i);
                    float w = (float)Math.Sqrt(0.5f);
                    float h = up ? 1 : 0.5f;
                    up = !up;
                    iTextSharp.awt.geom.AffineTransform aff =
                        iTextSharp.awt.geom.AffineTransform.GetTranslateInstance(0, 0);
                    aff.Translate(0, r.Height * h);
                    aff.Rotate(-Math.PI / 2, 0, 0);
                    aff.Scale(r.Width / r.Height, r.Height / r.Width / 2);
                    writer.DirectContent.AddTemplate(writer.GetImportedPage(reader, i), aff);
                    if (up)
                        document.NewPage();
                }
                document.Close();
                File.WriteAllBytes(output + ".pdf", ms.ToArray());
            }
        }
        /// <summary>
        /// Vergrößert alle A4 Seiten auf A3 Seiten
        /// <para>kein .pdf an input oder output anhängen, macht er automatisch</para>
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        public static void A4ToA3(string output, string input)
        {
            using (var ms = new MemoryStream())
            {
                PdfReader reader = new PdfReader(input + ".pdf");
                Document document = new Document(PageSize.A3);
                PdfCopy writer = new PdfCopy(document, ms);
                document.Open();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    Rectangle r = reader.GetPageSize(i);
                    double w = Math.Sqrt(2);
                    iTextSharp.awt.geom.AffineTransform aff = iTextSharp.awt.geom.AffineTransform.GetScaleInstance(w, w);
                    writer.DirectContent.AddTemplate(writer.GetImportedPage(reader, i), aff);
                    document.NewPage();
                }

                document.Close();
                File.WriteAllBytes(output + ".pdf", ms.ToArray());
            }
        }
    }
}
