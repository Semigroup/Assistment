using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class WLetter : Letter
    {
        public WLetter()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    0, 2,
                    0.25f, 0,
                    0.5f, 1,
                    0.75f, 0,
                    1, 2);

            AssociatedCharacters = "W";
        }
    }
}
