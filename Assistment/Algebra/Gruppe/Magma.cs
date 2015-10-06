using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Algebra.Gruppe
{
    public interface Magma
    {
        /// <summary>
        /// Berechnet this * m;
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        Magma mul(Magma m);
        /// <summary>
        /// Werden die Multiplikationen sofort ausgerechnet oder werden sie erst bei Bedarf verwertet?
        /// </summary>
        /// <returns></returns>
        bool isLazy();
    }

    public static class MagmaErweiterer
    {
    }
}
