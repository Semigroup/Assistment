using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class BLetter : Letter
    {
        public BLetter()
        {
            Segments = new Segment[2];
            Segments[0] = new Segment(
                    0, 0,
                    0, 2,
                    0.8f, 2,
                    0.8f, 1,
                    1, 1,
                    1, 0,
                    0, 0);
            Segments[1] = new Segment(0, 1,
                0.8f, 1);

            AssociatedCharacters = "B";
        }
    }
}
