using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.Geometries.Extensions;

namespace Assistment.Drawing.Geometries
{
    public class PointFComparer : IComparer<PointF>
    {
        public int Compare(PointF x, PointF y)
        {
            double d = x.atan() - y.atan();
            if (d > 0)
                return 1;
            else if (d < 0)
                return -1;
            else
                return 0;
        }
    }
}
