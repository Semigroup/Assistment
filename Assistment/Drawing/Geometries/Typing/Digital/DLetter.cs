using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class DLetter : Letter
    {
        public DLetter()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    0, 0,
                    0, 2,
                    0.75f, 2,
                    1, 1.75f,
                    1, 0.25f,
                    0.75f, 0,
                    0, 0);

            AssociatedCharacters = "D";
        }
    }
}
