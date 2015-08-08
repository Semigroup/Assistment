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
}
