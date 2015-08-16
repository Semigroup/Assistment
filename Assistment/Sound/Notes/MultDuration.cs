using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Extensions;

namespace Assistment.Sound.Notes
{
    public class MultDuration : Composition
    {
        public float DurationFactor;
        public Composition Composition;

        public MultDuration(Composition Composition, float DurationFactor)
        {
            this.Composition = Composition;
            this.DurationFactor = DurationFactor;
        }

        public override IEnumerable<Note> Step(int Time)
        {
            return Composition.Step((int)(Time / DurationFactor))
                .Map(n => new Note((int)(n.Duration * DurationFactor), n.Height, n.Volume));
        }

        public override int Duration
        {
            get { return (int)(Composition.Duration * DurationFactor); }
            set { Composition.Duration = (int)(value / DurationFactor); }
        }
    }
}
