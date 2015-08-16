using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Sound.Notes
{
    public class Product : Composition
    {
        public Composition Part1 { get; private set; }
        public Composition Part2 { get; private set; }

        public Product(Composition Part1, Composition Part2)
        {
            this.Part1 = Part1;
            this.Part2 = Part2;
        }

        public override IEnumerable<Note> Step(int Time)
        {
            if (Time < Part1.Duration)
                return Part1.Step(Time);
            else
                return Part2.Step(Time - Part1.Duration);
        }

        public override int Duration
        {
            get { return Part1.Duration + Part2.Duration; }
            set { throw new NotImplementedException(); }
        }
    }
}
