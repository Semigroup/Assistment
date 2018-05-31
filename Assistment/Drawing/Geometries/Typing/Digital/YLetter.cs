using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class YLetter : Letter
    {
        public YLetter()
        {
            Segments = new Segment[2];
            Segments[0] = new Segment(
                    0, 2,
                    0.5f, 1,
                    1, 2);
            Segments[1] = new Segment(
                    0.5f, 1,
                    0.5f, 0);

            AssociatedCharacters = "Y";
        }
    }
}
