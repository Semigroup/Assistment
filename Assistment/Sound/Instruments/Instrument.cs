using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Sound.Notes;

namespace Assistment.Sound.Instruments
{
    public abstract class Instrument
    {
        /// <summary>
        /// Maps time values to amplitude valus in [-1, 1]. 
        /// </summary>
        /// <param name="t">The time >= 0 in seconds.</param>
        /// <returns></returns>
        public delegate float Wave(double Time);
        /// <summary>
        /// How many seconds does one bar last? It's measured in seconds.
        /// <para>Default value is one quarter of a second.</para>
        /// </summary>
        public double BarLength = 0.25f;
        /// <summary>
        /// What is the maximum Volume value? The volume of notes is divided by this value.
        /// <para>Default value is 1000.</para>
        /// </summary>
        public int MaxVolume = 1000;
        /// <summary>
        /// How loud is this instrument?
        /// <para>Default value is 1f.</para>
        /// </summary>
        public double MyVolume = 1f;
        /// <summary>
        /// What is the frequence of the nullniveau for all notes?
        /// <para>Default value is 400 Hz.</para>
        /// </summary>
        public double Root = 400;
        /// <summary>
        /// How many semitunes forms one octave?
        /// <para>Equivalent ist the equation: semitune^OctavLength = 2</para>
        /// <para>Default value is 12.</para>
        /// </summary>
        public int OctavLength = 12;
        /// <summary>
        /// The TailLength is a time interval which will be added to the end of each note. It's measured in seconds.
        /// <para>Default value is zero.</para>
        /// </summary>
        public double TailLength = 0;

        public abstract Wave IntepretNote(double Duration, double Frequency, int Channel);

        private void Write16(WaveWriter Writer, Composition Composition)
        {
            int duration = Composition.Duration;
            double secPerBar = Writer.sampleRate * BarLength;
            int samples = ceil(secPerBar * duration + TailLength);
            int channels = Writer.channels;
            short[,] data = new short[samples, channels];

            for (int t = 0; t < duration; t++)
                foreach (Note note in Composition.Step(t))
                    for (int channel = 0; channel < channels; channel++)
                    {
                        int offset = floor(t * secPerBar);
                        Wave wave = IntepretNote(note.Duration, note.Height, channel);

                    }
        }

        public double getFrequency(int height)
        {
            return Root * Math.Pow(2, height / OctavLength);
        }

        private static int ceil(double x)
        {
            return (int)Math.Ceiling(x);
        }
        private static int floor(double x)
        {
            return (int)Math.Floor(x);
        }

        public void Play(WaveWriter Writer, Composition Composition)
        {
            switch (Writer.bitsProSample)
            {
                case 16:
                    break;
                case 32:
                    break;
                default:
                    break;
            }
        }
    }
}
