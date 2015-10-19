using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Mathematik
{
    public static class FastMath
    {
        /// <summary>
        /// Macht Bogenmaß zu Grad.
        /// </summary>
        /// <param name="Bogenmass"></param>
        /// <returns></returns>
        public static float Grad(double Bogenmass)
        {
            return (float)(Bogenmass * 180 / Math.PI);
        }

        /// <summary>
        /// (Cos(t), Sin(t)) = e^it
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static PointF Sphere(double t)
        {
            return new PointF(Cos(t), Sin(t));
        }

        public static float Sin(double t)
        {
            return (float)Math.Sin(t);
        }
        public static float Cos(double t)
        {
            return (float)Math.Cos(t);
        }

        public static bool LiesIn(this float f, float min, float max)
        {
            return min <= f && f <= max;
        }

        public static bool isZero(this float f)
        {
            return Math.Abs(f) < float.Epsilon;
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt(f);
        }
    }
}
