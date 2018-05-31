using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class MLetter : Letter
    {
        public MLetter()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    0, 0,
                    0, 2,
                    0.5f, 1,
                    1, 2,
                    1, 0);

            AssociatedCharacters = "M";
        }
    }
}
