using System;
using System.Drawing;

namespace Assistment.Extensions
{
    public class ColorF : ICloneable
    {
        /// <summary>
        /// ARGB in [0,1]^4
        /// </summary>
        public float[] Values { get; set; }

        public ColorF()
        {
            this.Values = new float[4];
        }
        public ColorF(Color Color)
            : this()
        {
            this.Values[0] = Color.A / 255f;
            this.Values[1] = Color.R / 255f;
            this.Values[2] = Color.G / 255f;
            this.Values[3] = Color.B / 255f;
        }
        public ColorF(byte[] data, int offset, bool reverse)
            : this()
        {
            if (reverse)
                for (int i = 0; i < 4; i++)
                    Values[i] = data[offset + 3 - i] / 255f;
            else
                for (int i = 0; i < 4; i++)
                    Values[i] = data[offset + i] / 255f;
        }
        public ColorF(params float[] Values)
            : this()
        {
            for (int i = 0; i < 4; i++)
                this.Values[i] = Values[i];
        }

        public float this[int index]
        {
            get { return Values[index]; }
            set { Values[index] = value; }
        }
        public Color ToColor()
        {
            return Color.FromArgb(
                Math.Max(0, Math.Min(255, (int)Math.Round(Values[0] * 255))),
                Math.Max(0, Math.Min(255, (int)Math.Round(Values[1] * 255))),
                Math.Max(0, Math.Min(255, (int)Math.Round(Values[2] * 255))),
                Math.Max(0, Math.Min(255, (int)Math.Round(Values[3] * 255))));

        }
        public object Clone()
        {
            return new ColorF(Values);
        }

        public static ColorF[] GramSchmidt(params ColorF[] Colors)
        {
            ColorF[] Output = new ColorF[Colors.Length];
            for (int i = 0; i < Colors.Length; i++)
            {
                Output[i] = Colors[i];
                for (int j = 0; j < i; j++)
                    Output[i] -= (Output[j] | Colors[i]) * Output[j];
                Output[i] = Output[i].Normalize();
            }
            return Output;
        }

        public ColorF Normalize()
        {
            float n = !this;
            if (n > 0.001f)
                return this / !this;
            else
                return new ColorF();
        }

        public static float operator |(ColorF A, ColorF B)
        {
            float skp = 0;
            for (int i = 0; i < 4; i++)
                skp += A[i] * B[i];
            return skp;
        }
        public static float operator ~(ColorF A)
        {
            return (A | A);
        }
        public static float operator !(ColorF A)
        {
            return (float)Math.Sqrt(~A);
        }

        public static ColorF operator *(float A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A * B[i]);
            return C;
        }
        public static ColorF operator *(ColorF B, float A)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A * B[i]);
            return C;
        }
        public static ColorF operator /(ColorF B, float A)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => B[i] / A);
            return C;
        }
        public static ColorF operator *(ColorF A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A[i] * B[i]);
            return C;
        }
        public static ColorF operator /(ColorF A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A[i] / B[i]);
            return C;
        }
        public static ColorF operator +(ColorF A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A[i] + B[i]);
            return C;
        }
        public static ColorF operator -(ColorF A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A[i] - B[i]);
            return C;
        }

        public static implicit operator SolidBrush(ColorF d)
        {
            return new SolidBrush(d.ToColor());
        }
        public static implicit operator Color(ColorF d)
        {
            return d.ToColor();
        }
        public static implicit operator ColorF(Color d)
        {
            return new ColorF(d);
        }

        public override string ToString()
        {
            string s = "(";
            for (int i = 0; i < 4; i++)
            {
                s += Values[i];
                if (i < 3)
                    s += ", ";
            }
            return s + ")";
        }

    }
}
