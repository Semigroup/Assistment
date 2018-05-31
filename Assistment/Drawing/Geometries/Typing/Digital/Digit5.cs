using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class Digit5 : Letter
    {
        public Digit5()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    1, 2,
                    0, 2,
                    0, 1,
                    1, 1,
                    0, 0);

            AssociatedCharacters = "5";
        }
    }
}
