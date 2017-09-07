using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;

namespace Assistment.Drawing.Algorithms
{
    public abstract class ImageAlgorithmBase : ImageAlgorithm
    {
        public abstract void Execute(Bitmap inputImage, Graphics g);
        public Bitmap Execute(Bitmap inputImage, Size sizeOfNewImage)
        {
            Bitmap b = new Bitmap(sizeOfNewImage.Width, sizeOfNewImage.Height);
            using (Graphics g = b.GetHighGraphics())
            {
                g.ScaleTransform(sizeOfNewImage.Width, sizeOfNewImage.Height);
                Execute(inputImage, g);
            }
            return b;
        }
        public Bitmap Execute(Bitmap inputImage, float width, float height)
        {
           return Execute(inputImage, new SizeF(width, height).Max(1, 1).ToPointF().Ceil().ToSize().ToSize());
        }
    }
}
