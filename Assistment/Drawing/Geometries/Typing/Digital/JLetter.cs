﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class JLetter : Letter
    {
        public JLetter()
        {
            Segments = new Segment[1];
            Segments[0] = new Segment(
                    0, 1,
                    0, 0,
                    1, 0,
                    1, 2);

            AssociatedCharacters = "J";
        }
    }
}
