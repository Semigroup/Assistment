using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace Assistment.form
{
    public delegate void WertEventHandler(object sender, WertEventArgs e);

    public interface IWerteListe : IWertBox
    {
        void AddWerteBox<T>(IWertBox<T> WerteBox, string Name);
        T GetValue<T>(string Name);
        void SetValue<T>(string Name, T Value);
        event WertEventHandler WertChanged;
    }
}
