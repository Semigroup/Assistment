using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class CLetter : Letter
    {
        public CLetter()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    1, 0.2f,
                    1, 0,
                    0, 0,
                    0, 2,
                    1, 2,
                    1, 1.8f);

            AssociatedCharacters = "C";
        }
    }
}
