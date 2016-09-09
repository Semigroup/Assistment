using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.form
{
    public interface IWertBox : IDisposable
    {
        void AddListener(EventHandler Handler);
        bool Valid();
        void AddInvalidListener(EventHandler Handler);
        void DDispose();
    }

    public interface IWertBox<T> : IWertBox
    {
        T GetValue();
        void SetValue(T Value);
    }
}
