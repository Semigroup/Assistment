using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class VLetter : Letter
    {
        public VLetter()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    0, 2,
                    0.5f, 0,
                    1, 2);

            AssociatedCharacters = "V";
        }
    }
}
