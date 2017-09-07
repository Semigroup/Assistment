using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assistment.Drawing.Algorithms
{
    /// <summary>
    /// Creates a new Image based on the given one
    /// </summary>
   public interface ImageAlgorithm
    {
        Bitmap Execute(Bitmap inputImage, Size sizeOfNewImage);
    }
}
