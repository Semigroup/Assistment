using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Sound.Fourier
{
    public class DFFT
    {
        public float[] Realteil;
        public float[] Imaginarteil;
        public float[] ZwischenspeicherRealteil;
        public float[] ZwischenspeicherImaginarteil;
        public int Length;

        public DFFT(float[] Realteil, float[] Imaginarteil)
        {
            Length = 1 << ((int)Math.Ceiling(Math.Log(Math.Max(Realteil.Length, Imaginarteil.Length), 2)));

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

        public void Execute()
        {
            IterationStep(0, 1);
        }

        private void IterationStep(int offset, int increment)
        {
            if (increment == Length) return;

            IterationStep(2 * offset, 2 * increment);
            IterationStep(2 * offset + 1, 2 * increment);

            for (int i = offset; i < Length / 2; i += increment)
            {
                //float Sinus = Math.Sin(2 * Math.PI * 
            }
        }
    }
}
