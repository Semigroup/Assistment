using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Assistment.Sound
{
    public class WaveWriter : BinaryWriter
    {
        #region RIFF-HEADER
        protected static readonly char[] chunkID = new char[] { 'R', 'I', 'F', 'F' };
        protected UInt32 GroseMinusAcht;
        protected static readonly char[] riffType = new char[] { 'W', 'A', 'V', 'E' };
        #endregion
        #region FORMAT
        protected readonly char[] formatSignatur = new char[] { 'f', 'm', 't', ' ' };
        protected const UInt32 fmtLength = 16;
        protected UInt16 fmtTag = 0x0001;
        protected UInt16 channels;
        protected UInt32 sampleRate;
        /// <summary>
        /// Abtastrate * Framgröße
        /// </summary>
        protected UInt32 bytesProSec;
        /// <summary>
        /// Frame-Größe = Anzahl der Kanäle · ((Bits/Sample (eines Kanals) + 7) / 8)   (Division ohne Rest)
        /// </summary>
        protected UInt16 blockAlign;
        /// <summary>
        /// Anzahl der Datenbits pro Samplewert je Kanal
        /// </summary>
        protected UInt16 bitsProSample;
        #endregion
        #region DATA
        protected readonly char[] dataSignatur = new char[] { 'd', 'a', 't', 'a' };
        protected UInt32 dataLength;
        #endregion

        public WaveWriter(string pfad, UInt16 channels, UInt32 sampleRate, UInt16 bitsProSample, UInt32 dataLength)
            : base(File.Create(pfad))
        {
            if ((bitsProSample != 16) && (bitsProSample != 32))
                throw new NotImplementedException(bitsProSample + " Bits pro Sample werden von WaveWriter nicht unterstützt!");

            this.channels = channels;
            this.sampleRate = sampleRate;
            this.bitsProSample = bitsProSample;
            this.blockAlign = (UInt16)(channels * bitsProSample / 8);
            this.bytesProSec = sampleRate * blockAlign;
            this.dataLength = dataLength;
            this.GroseMinusAcht = dataLength + 36;

            WritePreamble();
        }
        public WaveWriter(string pfad, WaveReader wr)
            : this(pfad, wr.channels, wr.sampleRate, wr.bitsProSample, wr.dataLength)
        {

        }
        public WaveWriter(string pfad, IEnumerable<WaveReader> wr)
            : this(pfad, wr.First().channels, wr.First().sampleRate, wr.First().bitsProSample, sum(wr))
        {

        }
        private static UInt32 sum(IEnumerable<WaveReader> wr)
        {
            UInt32 u = 0;
            foreach (var item in wr)
                u += item.dataLength;
            return u;
        }

        public void WritePreamble()
        {
            #region RIFF
            Write(chunkID);
            Write(GroseMinusAcht);
            Write(riffType);
            #endregion
            #region FORMAT
            Write(formatSignatur);
            Write(fmtLength);
            Write(fmtTag);
            Write(channels);
            Write(sampleRate);
            Write(bytesProSec);
            Write(blockAlign);
            Write(bitsProSample);
            #endregion
            #region DATA
            Write(dataSignatur);
            Write(dataLength);
            #endregion
        }
        /// <summary>
        /// sample = UInt16[channekls]
        /// <para>nur benutzen, falls bitsProSample == 16</para>
        /// </summary>
        /// <param name="sample"></param>
        public void WriteSample(params UInt16[] sample)
        {
            for (int i = 0; i < sample.Length; i++)
                Write(sample[i]);
        }
        /// <summary>
        /// sample = UInt32[channekls]
        /// <para>nur benutzen, falls bitsProSample == 16</para>
        /// </summary>
        /// <param name="sample"></param>
        public void WriteSample(params UInt32[] sample)
        {
            for (int i = 0; i < sample.Length; i++)
                Write(sample[i]);
        }
        /// <summary>
        /// data = UInt16[samples][channels]
        /// <para>nur benutzen, falls bitsProSample == 16</para>
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(UInt16[][] data)
        {
            for (int i = 0; i < data.Length; i++)
                for (int j = 0; j < channels; j++)
                    Write(data[i][j]);
        }
        /// <summary>
        /// data = UInt32[samples][channels]
        /// <para>nur benutzen, falls bitsProSample == 16</para>
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(UInt32[][] data)
        {
            for (int i = 0; i < data.Length; i++)
                for (int j = 0; j < channels; j++)
                    Write(data[i][j]);
        }
        public void WriteData(WaveReader data)
        {
            if (bitsProSample == 16)
                WriteData(data.data16);
            if (bitsProSample == 32)
                WriteData(data.data32);
        }

        /// <summary>
        /// konkateniert die Daten der Wavereader und schreibt sie in die Datei
        /// <para>Diese Funktion impliziert, dass der Writer und alle Reader die selben Einstellungen haben</para>
        /// </summary>
        /// <param name="data"></param>
        public void concat(IEnumerable<WaveReader> data)
        {
            if (bitsProSample == 16)
                foreach (var item in data)
                    WriteData(item.data16);
            if (bitsProSample == 32)
                foreach (var item in data)
                    WriteData(item.data32);
        }
    }
}
