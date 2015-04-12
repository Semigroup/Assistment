using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Assistment.Sound
{
    public class WaveReader : BinaryReader
    {
        public string pfad { get; private set; }

        #region RIFF-HEADER
        public char[] chunkID = new char[4];
        public UInt32 GroseMinusAcht;
        public char[] riffType = new char[4];
        #endregion
        #region FORMAT
        public char[] formatSignatur = new char[4];
        public UInt32 fmtLength;
        public UInt16 fmtTag;
        public UInt16 channels;
        public UInt32 sampleRate;
        /// <summary>
        /// Abtastrate * Framgröße
        /// </summary>
        public UInt32 bytesProSec;
        /// <summary>
        /// Frame-Größe = Anzahl der Kanäle · ((Bits/Sample (eines Kanals) + 7) / 8)   (Division ohne Rest)
        /// </summary>
        public UInt16 blockAlign;
        /// <summary>
        /// Anzahl der Datenbits pro Samplewert je Kanal
        /// </summary>
        public UInt16 bitsProSample;
        #endregion
        #region DATA
        public char[] dataSignatur = new char[4];
        public UInt32 dataLength;
        /// <summary>
        /// erster Index bestimmt das Sampel, der zweite bestimmt den Kanal
        /// <para>dieser Wert ist null, falls bitsProSample != 16</para>
        /// </summary>
        public UInt16[][] data16;
        /// <summary>
        /// erster Index bestimmt das Sampel, der zweite bestimmt den Kanal
        /// <para>dieser Wert ist null, falls bitsProSample != 32</para>
        /// </summary>
        public UInt32[][] data32;
        #endregion

        public WaveReader(string pfad)
            : base(File.OpenRead(pfad))
        {
            this.pfad = pfad;
            this.setRiff();
            this.setFormat();
            this.setData();
        }

        private void setRiff()
        {
            this.chunkID = ReadChars(4);
            this.GroseMinusAcht = ReadUInt32();
            this.riffType = ReadChars(4);
        }
        private void setFormat()
        {
            this.formatSignatur = ReadChars(4);
            this.fmtLength = ReadUInt32();
            this.fmtTag = ReadUInt16();
            this.channels = ReadUInt16();
            this.sampleRate = ReadUInt32();
            this.bytesProSec = ReadUInt32();
            this.blockAlign = ReadUInt16();
            this.bitsProSample = ReadUInt16();
        }
        private void setData()
        {
            this.dataSignatur = ReadChars(4);
            this.dataLength = ReadUInt32();

            if (bitsProSample == 16)
            {
                UInt32 sampleLength = (UInt32)(dataLength / (2 * channels));
                data16 = new UInt16[sampleLength][];

                for (int i = 0; i < sampleLength; i++)
                {
                    data16[i] = new UInt16[channels];
                    for (int j = 0; j < channels; j++)
                        data16[i][j] = ReadUInt16();
                }
            }
            else if (bitsProSample == 32)
            {
                UInt32 sampleLength = (UInt32)(dataLength / (4 * channels));
                data32 = new UInt32[sampleLength][];
                for (int i = 0; i < sampleLength; i++)
                {
                    data32[i] = new UInt32[channels];
                    for (int j = 0; j < channels; j++)
                        data32[i][j] = ReadUInt32();
                }
            }
            else
                throw new InvalidDataException("Bits Pro Sample: " + bitsProSample + " bei " + pfad + " kann nicht gewertet werden");
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            #region RIFF
            s.Append("Chunk ID: ");
            s.Append(chunkID);
            s.AppendLine();
            s.Append("Größe - 8: ");
            s.Append(GroseMinusAcht);
            s.AppendLine();
            s.Append("Riff Typ: ");
            s.Append(riffType);
            s.AppendLine();
            #endregion
            s.AppendLine();
            #region FORMAT
            s.Append("Format-Signatur: ");
            s.Append(formatSignatur);
            s.AppendLine();
            s.Append("Format Länge - 8: ");
            s.Append(fmtLength);
            s.AppendLine();
            s.Append("Format Tag: ");
            s.Append(fmtTag);
            s.AppendLine();
            s.Append("Kanäle: ");
            s.Append(channels);
            s.AppendLine();
            s.Append("Sample Rate: ");
            s.Append(sampleRate);
            s.AppendLine();
            s.Append("Bytes / Sec: ");
            s.Append(bytesProSec);
            s.AppendLine();
            s.Append("Block Größe: ");
            s.Append(blockAlign);
            s.AppendLine();
            s.Append("Bits pro Sample: ");
            s.Append(bitsProSample);
            s.AppendLine();
            #endregion
            s.AppendLine();
            #region DATA
            s.Append("Daten Signatur: ");
            s.Append(dataSignatur);
            s.AppendLine();
            s.Append("Datengröße: ");
            s.Append(dataLength);
            s.AppendLine();
            #endregion
            return s.ToString();
        }
    }
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
