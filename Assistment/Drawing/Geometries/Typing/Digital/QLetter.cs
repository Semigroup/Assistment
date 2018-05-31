using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class QLetter : Letter
    {
        public QLetter()
        {
            Segments = new Segment[2];
            Segments[0] = new Segment(
                    0, 0,
                    0, 2,
                    1, 2,
                    1, 0,
                    0, 0);
            Segments[1] = new Segment(0.8f, 0.2f, 1, -0.5f);

            AssociatedCharacters = "P";
        }
    }
}
