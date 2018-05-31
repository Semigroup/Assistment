using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class QuestionMark : Letter
    {
        public QuestionMark()
        {
            Segments = new Segment[2];
            Segments[0] = new Segment(
                    0, 2,
                    1, 2,
                    1, 1,
                    0, 1,
                    0, 0,
                    1, 0);
            Segments[1] = new Segment(
                0.25f, -0.25f,
                0.75f, -0.25f,
                0.75f, -0.75f,
                0.25f, -0.75f,
                0.25f, -0.25f);

            AssociatedCharacters = "?";
        }
    }
}
