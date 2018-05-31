﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class Digit2 : Letter
    {
        public Digit2()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    0, 2,
                    1, 2,
                    1, 1,
                    0, 1,
                    0, 0,
                    1, 0);

            AssociatedCharacters = "2";
        }
    }
}
