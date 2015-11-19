using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Extensions;
using Assistment.Mathematik;

namespace Assistment.Sound.Fourier
{
    public class DFFT
    {
        public float[] Realteil { get; set; }
        public float[] Imaginarteil { get; set; }
        private float[] ZwischenspeicherRealteil;
        private float[] ZwischenspeicherImaginarteil;
        public int Length { get; private set; }

        public DFFT(float[] Realteil, float[] Imaginarteil)
        {
            Length = FastMath.NextPower(Math.Max(Realteil.Length, Imaginarteil.Length), 2);

            this.Realteil = new float[Length];
            this.Imaginarteil = new float[Length];
            this.ZwischenspeicherRealteil = new float[Length];
            this.ZwischenspeicherImaginarteil = new float[Length];

            Realteil.CopyTo(this.Realteil, 0);
            Imaginarteil.CopyTo(this.Imaginarteil, 0);
        }
        public DFFT(float[] Realteil)
            : this(Realteil, new float[1])
        {
        }
        public DFFT(int[,] Realteil, int channel, int offset, int size)
        {
            Length = FastMath.NextPower(size, 2);

            this.Realteil = new float[Length];
            this.Imaginarteil = new float[Length];
            this.ZwischenspeicherRealteil = new float[Length];
            this.ZwischenspeicherImaginarteil = new float[Length];

            for (int i = 0; i < size; i++)
                this.Realteil[i] = Realteil[i + offset, channel];
        }
        public DFFT(short[,] Realteil, int channel, int offset, int size)
        {
            Length = FastMath.NextPower(size, 2);

            this.Realteil = new float[Length];
            this.Imaginarteil = new float[Length];
            this.ZwischenspeicherRealteil = new float[Length];
            this.ZwischenspeicherImaginarteil = new float[Length];

            for (int i = 0; i < size; i++)
                this.Realteil[i] = Realteil[i + offset, channel];
        }

        public void Execute()
        {
            IterationStep(0, 1, 1);
        }
        public void Invert()
        {
            IterationStep(0, 1, -1);
            Realteil.CountMap(i => Realteil[i] * Length);
            Imaginarteil.CountMap(i => Imaginarteil[i] * Length);
        }

        private void IterationStep(int offset, int increment, float sign)
        {
            int n = Length / increment;
            if (n == 1) return;
            int newIncrement = 2 * increment;

            IterationStep(offset, newIncrement, sign);
            IterationStep(offset + increment, newIncrement, sign);

            for (int k = 0; k < n / 2; k++)
            {
                double t = -k * 2 * Math.PI / n * sign;
                float Sinus = (float)Math.Sin(t);
                float Kosinus = (float)Math.Cos(t);

                int index = offset + increment * k;
                int indexH1 = offset + k * newIncrement;
                int indexH2 = indexH1 + increment;

                ZwischenspeicherRealteil[index] = Realteil[indexH1] + Kosinus * Realteil[indexH2] - Sinus * Imaginarteil[indexH2];
                ZwischenspeicherImaginarteil[index] = Imaginarteil[indexH1] + Kosinus * Imaginarteil[indexH2] + Sinus * Realteil[indexH2];

                index += Length / 2;

                ZwischenspeicherRealteil[index] = Realteil[indexH1] - Kosinus * Realteil[indexH2] + Sinus * Imaginarteil[indexH2];
                ZwischenspeicherImaginarteil[index] = Imaginarteil[indexH1] - Kosinus * Imaginarteil[indexH2] - Sinus * Realteil[indexH2];
            }
            for (int k = 0; k < n / 2; k++)
            {
                int index = increment * k + offset;

                Realteil[index] = 0.5f * ZwischenspeicherRealteil[index];
                Imaginarteil[index] = 0.5f * ZwischenspeicherImaginarteil[index];

                index += Length / 2;

                Realteil[index] = 0.5f * ZwischenspeicherRealteil[index];
                Imaginarteil[index] = 0.5f * ZwischenspeicherImaginarteil[index];
            }
        }
    }
}
