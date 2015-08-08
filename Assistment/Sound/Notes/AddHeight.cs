using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Extensions;

namespace Assistment.Sound.Notes
{
    public class AddHeight : Composition
    {
        /// <summary>
        /// To the heights of the notes of the composition will this value be added.
        /// </summary>
        public int AddedHeight;
        /// <summary>
        /// To the heights of the notes of this composition will AddedHeight be added.
        /// </summary>
        public Composition Composition;

        public AddHeight(Composition Composition, int AddedHeight)
            : base(Composition.Duration)
        {

        }
        public override IEnumerable<Note> Step(int Time)
        {
            return Composition.Step(Time)
                .Map(n => new Note(n.Duration, n.Height + AddedHeight, n.Volume));
        }
    }
}
