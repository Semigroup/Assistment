using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Sound.Notes
{
    public class Note : Composition
    {
        /// <summary>
        /// The height of this note is the number of semitones this note is above or below an origin tone.
        /// <para>Default value is 0.</para>
        /// </summary>
        public int Height = 0;
        /// <summary>
        /// Will be multiplied with a ground volume.
        /// <para>Default value is 100.</para>
        /// </summary>
        public int Volume = 100;

        private int duration = 1;

        public override int Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }

        public Note(int Duration, int Height, int Volume)
        {
            this.Height = Height;
            this.Volume = Volume;
            this.duration = Duration;
        }
        public Note()
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is Note)
            {
                Note n = (Note)obj;
                return n.Duration == this.Duration
                    && n.Height == this.Height
                    && n.Volume == this.Volume;
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            return Volume
                + (Height << 10)
                + (Duration << 20);
        }
        public override string ToString()
        {
            return "(T = " + duration + ", H = " + Height + ", V = " + Volume + ")";
        }
        public override IEnumerable<Note> Step(int Time)
        {
            List<Note> list = new List<Note>();
            if (Time == 0)
                list.Add(this);
            return list;
        }
    }
}
