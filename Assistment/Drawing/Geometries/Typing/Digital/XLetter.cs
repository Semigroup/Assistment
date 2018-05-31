﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistment.Drawing.Geometries.Typing.Digital
{
    public class XLetter : Letter
    {
        public XLetter()
        {
            Segments = new Segment[2];
            Segments[0] = new Segment(
                    0, 2,
                    1, 0);
            Segments[1] = new Segment(
                    1, 2,
                    0, 0);

            AssociatedCharacters = "X";
        }
    }
}
