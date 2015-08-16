using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Extensions;

namespace Assistment.Sound.Notes
{
    public class MultVolume : Composition
    {
        public float VolumeFactor;
        public Composition Composition;

        public MultVolume(Composition Composition, float VolumeFactor)
        {
            this.Composition = Composition;
            this.VolumeFactor = VolumeFactor;
        }

        public override IEnumerable<Note> Step(int Time)
        {
            return Composition.Step(Time)
                .Map(n => new Note(n.Duration, n.Height, (int)(n.Volume * VolumeFactor)));
        }

        public override int Duration
        {
            get { return Composition.Duration; }
            set { Composition.Duration = value; }
        }
    }
}
