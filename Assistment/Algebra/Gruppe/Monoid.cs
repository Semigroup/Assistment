using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Algebra.Gruppe
{
    public interface Monoid : Magma
    {
        /// <summary>
        /// Neutralelement der Multiplikation.
        /// </summary>
        /// <returns></returns>
        Monoid eins();
    }
}
