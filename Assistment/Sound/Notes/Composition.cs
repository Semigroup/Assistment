using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Sound.Notes
{
    public abstract class Composition
    {
        /// <summary>
        /// Measured in bars.
        /// <para>Default value is 1.</para>
        /// </summary>
        public int Duration { get; private set; }

        public Composition(int Duration)
        {
            this.Duration = Duration;
        }

        /// <summary>
        /// Set of Notes which starts at Time.
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public abstract IEnumerable<Note> Step(int Time);

        public static AddHeight operator +(Composition Composition, int Height)
        {
            return new AddHeight(Composition, Height);
        }

        public static Sum operator +(Composition Summand1, Composition Summand2)
        {
            return new Sum(Summand1, Summand2);
        }

        public static Product operator *(Composition Part1, Composition Part2)
        {
            return new Product(Part1, Part2);
        }
    }
}
