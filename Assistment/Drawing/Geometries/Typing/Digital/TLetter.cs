using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class TLetter : Letter
    {
        public TLetter()
        {
            Segments = new Segment[2];
            Segments[0] = new Segment(
                    0, 2,
                    1, 2);
            Segments[1] = new Segment(
                    0.5f, 2,
                    0.5f, 0);

            AssociatedCharacters = "T";
        }
    }
}
