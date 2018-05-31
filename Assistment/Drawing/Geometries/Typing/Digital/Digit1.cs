using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class Digit1 : Letter
    {
        public Digit1()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    0.5f, 1.5f,
                    1, 2,
                    1, 0);

            AssociatedCharacters = "1";
        }
    }
}
