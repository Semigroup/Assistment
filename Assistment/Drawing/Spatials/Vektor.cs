using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Drawing.Spatials
{
    public struct Vektor
    {
        public float x;
        public float y;
        public float z;

        public PointF xy
        {
            get
            {
                return new PointF(x, y);
            }
        }
        public PointF xz
        {
            get
            {
                return new PointF(x, z);
            }
        }
        public PointF yz
        {
            get
            {
                return new PointF(y, z);
            }
        }

        public Vektor(float all)
        {
            this.x = all;
            this.y = all;
            this.z = all;
        }
        public Vektor(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vektor(Vektor Vektor)
        {
            this.x = Vektor.x;
            this.y = Vektor.y;
            this.z = Vektor.z;
        }

        public Vektor AddLocal(float x, float y, float z)
        {
            this.x += x;
            this.y += y;
            this.z += z;
            return this;
        }
        public Vektor AddLocal(Vektor Vektor)
        {
            this.x += Vektor.x;
            this.y += Vektor.y;
            this.z += Vektor.z;
            return this;
        }
        public Vektor SubLocal(float x, float y, float z)
        {
            this.x -= x;
            this.y -= y;
            this.z -= z;
            return this;
        }
        public Vektor SubLocal(Vektor Vektor)
        {
            this.x -= Vektor.x;
            this.y -= Vektor.y;
            this.z -= Vektor.z;
            return this;
        }
        public Vektor MulLocal(float x, float y, float z)
        {
            this.x *= x;
            this.y *= y;
            this.z *= z;
            return this;
        }
        public Vektor MulLocal(Vektor Vektor)
        {
            this.x *= Vektor.x;
            this.y *= Vektor.y;
            this.z *= Vektor.z;
            return this;
        }
        public Vektor DivLocal(float x, float y, float z)
        {
            this.x /= x;
            this.y /= y;
            this.z /= z;
            return this;
        }
        public Vektor DivLocal(Vektor Vektor)
        {
            this.x /= Vektor.x;
            this.y /= Vektor.y;
            this.z /= Vektor.z;
            return this;
        }

        public static Vektor operator +(Vektor Summand1, Vektor Summand2)
        {
            return new Vektor(Summand1).AddLocal(Summand2);
        }
        public static Vektor operator -(Vektor Minuend, Vektor Subtrahend)
        {
            return new Vektor(Minuend).SubLocal(Subtrahend);
        }
        public static Vektor operator *(Vektor Faktor1, Vektor Faktor2)
        {
            return new Vektor(Faktor1).MulLocal(Faktor2);
        }
        public static Vektor operator /(Vektor Dividend, Vektor Divisor)
        {
            return new Vektor(Dividend).DivLocal(Divisor);
        }
    }
}
