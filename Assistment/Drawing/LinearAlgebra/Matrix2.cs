using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Drawing.LinearAlgebra
{
    public class Matrix2
    {
        public float m11, m12, m21, m22;

        public PointF Spalte1
        {
            get
            {
                return new PointF(m11, m21);
            }
            set
            {
                m11 = value.X;
                m21 = value.Y;
            }
        }
        public PointF Spalte2
        {
            get
            {
                return new PointF(m12, m22);
            }
            set
            {
                m12 = value.X;
                m22 = value.Y;
            }
        }

        /// <summary>
        /// m11, m12, m21, m22
        /// </summary>
        /// <param name="ms"></param>
        public Matrix2(params float[] ms)
        {
            m11 = ms[0];
            m12 = ms[1];
            m21 = ms[2];
            m22 = ms[3];
        }

        public Matrix2(PointF Spalte1, PointF Spalte2)
        {
            this.Spalte1 = Spalte1;
            this.Spalte2 = Spalte2;
        }

        public float Determinante()
        {
            return m11 * m22 - m12 * m21;
        }

        public Matrix2 Invert()
        {
            return new Matrix2(m22, -m12, -m21, m11).divLocal(Determinante());
        }
        public Matrix2 divLocal(float r)
        {
            return mulLocal(1 / r);
        }
        public Matrix2 mulLocal(float r)
        {
            this.m11 *= r;
            this.m12 *= r;
            this.m21 *= r;
            this.m22 *= r;
            return this;
        }
        /// <summary>
        /// Berechnet x / A = A^-1 * x = b
        /// </summary>
        /// <param name="X"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static PointF operator /(PointF x, Matrix2 A)
        {
            return new PointF(A.m22 * x.X - A.m12 * x.Y, A.m11 * x.Y - A.m21 * x.X).div(A.Determinante());
        }
    }
}
