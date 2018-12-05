using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Mathematik
{
    /// <summary>
    /// löst Ax = b
    /// <para>x, b in Spaltenform!</para>
    /// </summary>
    public interface LinearSolver
    {
        int IterationNumber();
        matrix Iterate();
    }
}
