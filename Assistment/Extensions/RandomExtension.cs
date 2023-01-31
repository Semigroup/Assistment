using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Drawing;

namespace Assistment.Extensions
{
    public static class RandomExtension
    {
        /// <summary>
        /// x \in [0, 1]
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float NextFloat(this Random d)
        {
            return (float)d.NextDouble();
        }
        /// <summary>
        /// x \in [0, max]
        /// </summary>
        /// <param name="d"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float NextFloat(this Random d, float max)
        {
            return (float)d.NextDouble();
        }
        /// <summary>
        /// x \in [-1, 1]
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float NextCenterd(this Random d)
        {
            return (float)(d.NextDouble() * 2 - 1);
        }
        /// <summary>
        /// gibt einen zufälligen 2-dim Vektor zurück, mit Koordinaten zwischen -1 und 1
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static PointF NextPoint(this Random d)
        {
            return new PointF(NextCenterd(d), NextCenterd(d));
        }

        /// <summary>
        /// gibt einen zufälligen 2-dim Vektor mit Norm = 1 zurück
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static PointF NextSpherical(this Random d)
        {
            return NextPoint(d).normalize();
        }
        /// <summary>
        /// gibt einen zufälligen 2-dim Vektor mit Norm = Radius zurück
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static PointF NextSpherical(this Random d, float Radius)
        {
            PointF p = NextPoint(d);
            return p.mul(Radius / p.norm());
        }

        public static SolidBrush NextBrush(this Random d, int Alpha)
        {
            return new SolidBrush(d.NextColor(Alpha));
        }
        public static Color NextColor(this Random d, int Alpha)
        {
            return Color.FromArgb(Alpha, d.Next(256), d.Next(256), d.Next(256));
        }
        public static Color NextColor(this Random d, int Alpha, int teile)
        {
            int n = 255 / teile;
            teile++;
            return Color.FromArgb(Alpha, d.Next(teile) * n, d.Next(teile) * n, d.Next(teile) * n);
        }
    }
}
