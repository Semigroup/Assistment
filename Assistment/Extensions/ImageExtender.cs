using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Texts;

namespace Assistment.Extensions
{
    public static class ImageExtender
    {
        public static void saveDinA3Pdf(this Image Image, string name)
        {
            ImageBox ib = new ImageBox(1403, Image);
            ib.createDinA3PDF(name);
        }
        public static void savePdf(this Image Image, string name)
        {
            ImageBox ib = new ImageBox(701, Image);
            ib.createPDF(name);
        }
    }
}
