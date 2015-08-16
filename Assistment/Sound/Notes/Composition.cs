using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Extensions;

namespace Assistment.Sound.Notes
{
    public abstract class Composition
    {
        /// <summary>
        /// Measured in bars.
        /// <para>Default value is 1.</para>
        /// </summary>
        public abstract int Duration { get; set; }

        /// <summary>
        /// Set of Notes which starts at Time.
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public abstract IEnumerable<Note> Step(int Time);

        /// <summary>
        /// Multiplies the volume of the Composition with the given VolumeFactor.
        /// </summary>
        /// <param name="Composition"></param>
        /// <param name="Volume"></param>
        /// <returns></returns>
        public static MultVolume operator *(float VolumeFactor, Composition Composition)
        {
            return new MultVolume(Composition, VolumeFactor);
        }
        /// <summary>
        /// Raises the height of the Composition by the given number of Semitunes.
        /// </summary>
        /// <param name="Composition"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static AddHeight operator +(Composition Composition, int Height)
        {
            return new AddHeight(Composition, Height);
        }
        /// <summary>
        /// Creates the union of two Compositions.
        /// </summary>
        /// <param name="Summand1"></param>
        /// <param name="Summand2"></param>
        /// <returns></returns>
        public static Sum operator +(Composition Summand1, Composition Summand2)
        {
            return new Sum(Summand1, Summand2);
        }
        /// <summary>
        /// Concatenate two Compositions.
        /// </summary>
        /// <param name="Part1"></param>
        /// <param name="Part2"></param>
        /// <returns></returns>
        public static Product operator *(Composition Part1, Composition Part2)
        {
            return new Product(Part1, Part2);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Duration: " + Duration);
            for (int i = 0; i <= Duration; i++)
            {
                string s = "";
                IEnumerable<Note> en = Step(i);
                foreach (var item in en)
                    s += item + "; ";
                if (s.Length > 0)
                {
                    sb.Append(i + ": ");
                    sb.AppendLine(s);             
                }
            }
            return sb.ToString();
        }
        public override bool Equals(object obj)
        {
            bool equals = false;
            if (obj is Composition)
            {
                Composition o = (Composition)obj;
                if (Duration == o.Duration)
                {
                    equals = true;
                    for (int i = 0; i < Duration + 1; i++)
                        if (!Step(i).Equals<Note>(o.Step(i)))
                        {
                            equals = false;
                            break;
                        }
                }
            }
            return equals;
        }
        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < Duration+1; i++)
                hash = i * Step(i).Sum(n => n.GetHashCode());
            return hash;
        }
    }
}
