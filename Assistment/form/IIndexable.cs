using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.form
{
    public interface IIndexable
    {
        IDictionary<string, object> GetWerte();
        void SetWerte(IDictionary<string, object> Werte);
    }
}
