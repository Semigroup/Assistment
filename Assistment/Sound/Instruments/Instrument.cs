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
        public delegate float Wave(float Time);
        /// <summary>
        /// How many seconds does one bar last? It's measured in seconds.
        /// <para>Default value is one quarter of a second.</para>
        /// </summary>
        public float BarLength = 0.25f;
        /// <summary>
        /// What is the maximum Volume value? The volume of notes is divided by this value.
        /// <para>Default value is 1000.</para>
        /// </summary>
        public int MaxVolume = 1000;
        /// <summary>
        /// How loud is this instrument?
        /// <para>Default value is 1f.</para>
        /// </summary>
        public float MyVolume = 1f;
        /// <summary>
        /// What is the frequence of the nullniveau for all notes?
        /// <para>Default value is 400 Hz.</para>
        /// </summary>
        public float Root = 400f;
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
        public float TailLength = 0;

        public abstract Wave IntepretNote(float Duration, float Frequency, int Channel);

        private void Write16(WaveWriter Writer, Composition Composition)
        {
            int samples = (int)Math.Ceiling(Writer.sampleRate * Composition.Duration * BarLength + TailLength);
            int channels = Writer.channels;
            short[,] data = new short[samples, channels];

            for (int t = 0; t < Composition.Duration; t++)
            {
                foreach (Note note in Composition.Step(t))
                {
                    
                }
            }
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
