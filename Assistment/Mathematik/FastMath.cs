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

        public static float Saturate(this float f)
        {
            if (f > 1)
                return 1;
            else if (f < 0)
                return 0;
            else
                return f;
        }

        public static int Ceil(this float d)
        {
            return (int)Math.Ceiling(d);
        }
        public static int Floor(this float d)
        {
            return (int)Math.Floor(d);
        }
        public static int Ceil(this double d)
        {
            return (int)Math.Ceiling(d);
        }
        public static int Floor(this double d)
        {
            return (int)Math.Floor(d);
        }

        /// <summary>
        /// gibt die nächstgrößte zahl m wieder, die eine potenz der gegebenen basis ist; d.h.
        /// <para> return basis ^ Ceil(log_basis(n)) </para>
        /// </summary>
        /// <param name="n"></param>
        /// <param name="basis"></param>
        /// <returns></returns>
        public static int NextPower(int n, int basis)
        {
            return (int)Math.Pow(basis, Math.Ceiling(Math.Log(n, basis)));
        }

        public static float Max(this float a, float b)
        {
            return Math.Max(a, b);
        }
        public static float Min(this float a, float b)
        {
            return Math.Min(a, b);
        }
        /// <summary>
        /// Nicht optimal implementiert
        /// </summary>
        /// <param name="As"></param>
        /// <returns></returns>
        public static int Min(params int[] As)
        {
            int a = int.MaxValue;
            foreach (var item in As)
                a = Math.Min(a, item);
            return a;
        }
        /// <summary>
        /// Nicht optimal implementiert
        /// </summary>
        /// <param name="As"></param>
        /// <returns></returns>
        public static int Max(params int[] As)
        {
            int a = int.MinValue;
            foreach (var item in As)
                a = Math.Max(a, item);
            return a;
        }
    }
}
