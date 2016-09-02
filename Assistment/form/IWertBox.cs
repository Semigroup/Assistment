using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.form
{
    public interface IWertBox<T>
    {
        T GetValue();
        void SetValue(T Value);
        void AddListener(EventHandler Handler);
        bool Valid();
        void AddInvalidListener(EventHandler Handler);
    }
}
