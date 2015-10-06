using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Algebra.Gruppe
{
    public interface Homomorphismus<in X, out Y>
        where X : Magma
        where Y : Magma
    {
    }
}
