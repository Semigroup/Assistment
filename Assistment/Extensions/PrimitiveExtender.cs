using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Extensions
{
    public static class PrimitiveExtender
    {
        public static bool Equal(this float a, float b)
        {
            return Math.Abs(a - b) < 0.00001f;
        }
    }
}
