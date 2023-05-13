using System;
using System.Drawing;

namespace Assistment.Drawing.Geometries.Extensions
{
    public static class RectangleFExtensions
    {
        public static RectangleF Scale(this RectangleF Rectangle, float Scaling)
        {
            return new RectangleF(Rectangle.X * Scaling, Rectangle.Y * Scaling, Rectangle.Width * Scaling, Rectangle.Height * Scaling);
        }
        /// <summary>
        /// Kreiert das kleinste Rechteck, das 1 und 2 umfasst
        /// </summary>
        /// <param name="Rectangle1"></param>
        /// <param name="Rectangle2"></param>
        /// <returns></returns>
        public static RectangleF Extend(this RectangleF Rectangle1, RectangleF Rectangle2)
        {
            PointF loc = new PointF(Math.Min(Rectangle1.Left, Rectangle2.Left), Math.Min(Rectangle1.Top, Rectangle2.Top));
            PointF antiLoc = new PointF(Math.Max(Rectangle1.Right, Rectangle2.Right), Math.Max(Rectangle1.Bottom, Rectangle2.Bottom));
            return new RectangleF(loc, antiLoc.sub(loc).ToSize());
        }
    }
}
