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
        protected UInt32 GroseMinusAcht
        {
            get
            {
                return dataLength + 36;
            }
        }
        protected static readonly char[] riffType = new char[] { 'W', 'A', 'V', 'E' };
        #endregion
        #region FORMAT
        protected readonly char[] formatSignatur = new char[] { 'f', 'm', 't', ' ' };
        protected const UInt32 fmtLength = 16;
        protected UInt16 fmtTag = 0x0001;
        public UInt16 channels;
        /// <summary>
        /// Frames pro Sekunde
        /// </summary>
        public UInt32 sampleRate;
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
        public UInt16 bitsProSample { get; private set; }
        #endregion
        #region DATA
        protected readonly char[] dataSignatur = new char[] { 'd', 'a', 't', 'a' };
        protected UInt32 dataLength
        {
            get
            {
                return (uint)(simpleDataLength * channels * bytesProSampel);
            }
        }
        #endregion
        private int simpleDataLength = 0;
        public int bytesProSampel
        {
            get
            {
                return (bitsProSample + 7) / 8;
            }
        }

        public WaveWriter(string pfad, UInt16 channels, UInt32 sampleRate, UInt16 bitsProSample)
            : base(File.Create(pfad))
        {
            if ((bitsProSample != 16) && (bitsProSample != 32))
                throw new NotImplementedException(bitsProSample + " Bits pro Sample werden von WaveWriter nicht unterstützt!");

            this.channels = channels;
            this.sampleRate = sampleRate;
            this.bitsProSample = bitsProSample;
            this.blockAlign = (UInt16)(channels * bytesProSampel);
            this.bytesProSec = sampleRate * blockAlign;

            WritePreamble();
        }
        public WaveWriter(string pfad, WaveReader wr)
            : this(pfad, wr.channels, wr.sampleRate, wr.bitsProSample)
        {

        }

        private void WritePreamble()
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
        /// data = UInt16[samples][channels]
        /// <para>nur benutzen, falls bitsProSample == 16</para>
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(short[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < channels; j++)
                    Write(data[i, j]);
            simpleDataLength += data.GetLength(0);
        }
        /// <summary>
        /// data = UInt32[samples][channels]
        /// <para>nur benutzen, falls bitsProSample == 16</para>
        /// </summary>
        /// <param name="data"></param>
        public void WriteData(int[,] data)
        {
            for (int i = 0; i < data.Length; i++)
                for (int j = 0; j < channels; j++)
                    Write(data[i, j]);
            simpleDataLength += data.GetLength(0);
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

        public override void Close()
        {
            this.Flush();
            this.Seek(4, SeekOrigin.Begin);
            this.Write(GroseMinusAcht);

            this.Flush();
            this.Seek(40, SeekOrigin.Begin);
            this.Write(dataLength);

            base.Close();
        }
    }
}
