using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class HLetter : Letter
    {
        public HLetter()
        {
            Segments = new Segment[3];
            Segments[0] = new Segment(
                    0, 0,
                    0, 2);
            Segments[1] = new Segment(
                    0, 1,
                    1, 1);
            Segments[2] = new Segment(
                    1, 0,
                    1, 2);

            AssociatedCharacters = "H";
        }
    }
}
