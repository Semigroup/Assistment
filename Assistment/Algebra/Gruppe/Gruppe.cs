using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Algebra.Gruppe
{
    public interface Gruppe
    {
        /// <summary>
        /// Erzeugt die Inverse des Elements.
        /// <para>Es gilt: this.invert().mul(this) = this.eins()</para>
        /// </summary>
        /// <returns></returns>
        Gruppe invert();
    }
}
