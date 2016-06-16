using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Drawing.LinearAlgebra
{
    public static class RectangleFExtensions
    {
        public static RectangleF Scale(this RectangleF Rectangle, float Scaling)
        {
            return new RectangleF(Rectangle.X * Scaling, Rectangle.Y * Scaling, Rectangle.Width * Scaling, Rectangle.Height * Scaling);
        }
    }
}
