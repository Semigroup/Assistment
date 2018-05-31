using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class ELetter : Letter
    {
        public ELetter()
        {
            Segments = new Segment[2];
            Segments[0] = new Segment(
                    1, 0,
                    0, 0,
                    0, 2,
                    1, 2);
            Segments[1] = new Segment(
                0, 1,
                0.8f, 1);

            AssociatedCharacters = "E";
        }
    }
}
