using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class GLetter : Letter
    {
        public GLetter()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    0.8f, 1,
                    1, 1,
                    1, 0,
                    0, 0,
                    0, 2,
                    1, 2);

            AssociatedCharacters = "G";
        }
    }
}
