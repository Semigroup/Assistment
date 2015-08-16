using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Sound.Notes
{
    public class Sum : Composition
    {
        public Composition Summand1 { get; private set; }
        public Composition Summand2 { get; private set; }

        public Sum(Composition Summand1, Composition Summand2)
        {
            this.Summand1 = Summand1;
            this.Summand2 = Summand2;
        }
        public override IEnumerable<Note> Step(int Time)
        {
            return Summand1.Step(Time).Concat(Summand2.Step(Time));
        }

        public override int Duration
        {
            get { return Math.Max(Summand1.Duration, Summand2.Duration); }
            set { throw new NotImplementedException(); }
        }
    }
}
