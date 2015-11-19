using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Assistment.Mathematik;

namespace Assistment.Sound.Fourier
{
    public class SoundFilter
    {
        public delegate void Filter(float[] Realteil, float[] Imaginarteil);

        public WaveReader WaveReader { get; private set; }
        public WaveWriter WaveWriter { get; private set; }

        public int Length { get; private set; }

        public SoundFilter(string PathOfWave)
        {
            WaveReader = new WaveReader(PathOfWave);
            Length = WaveReader.NumberOfSamples;
        }

        private void ApplyFilter16(Filter filter, int Package, int PackageSize, int Channel)
        {
            int offset = Package * PackageSize;
            PackageSize = Math.Min(PackageSize, Length - offset);
            DFFT dfft = new DFFT(WaveReader.data16, Channel, offset, PackageSize);
            dfft.Execute();
            filter(dfft.Realteil, dfft.Imaginarteil);
            dfft.Invert();
            for (int i = 0; i < PackageSize; i++)
                WaveReader.data16[offset + i, Channel] = (short)dfft.Realteil[i];
        }

        public void ApplyFilter16(Filter filter)
        {
            int PackageSize = FastMath.NextPower((int)WaveReader.sampleRate, 2);
            int N = FastMath.Ceil(WaveReader.data16.GetLength(0) * 1.0 / PackageSize);

            for (int i = 0; i < N; i++)
                for (int c = 0; c < WaveReader.channels; c++)
                    ApplyFilter16(filter, i, PackageSize, c);
        }

        public void Save(string newPath)
        {
            WaveWriter ww = new WaveWriter(newPath, WaveReader);
            ww.WriteData(WaveReader);
            ww.Close();
        }
    }
}
