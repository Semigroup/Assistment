using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Drawing.Geometries;


namespace Assistment.Drawing.Spatials
{
    public abstract class Spatial
    {
        public abstract Geometrie Cut(Plane Plane); 
    }
}
