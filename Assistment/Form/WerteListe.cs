using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Assistment.form
{
    public class WerteListe : ScrollBox, IWerteListe
    {
        private SortedDictionary<string, IWertBox> dictionary = new SortedDictionary<string, IWertBox>();
        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };
        public event WertEventHandler WertChanged = delegate { };
        public ControlList List;

        public WerteListe()
            : base(new ControlList())
        {
            List = base.Control as ControlList;
        }

        public IWertBox<T> GetWerteBox<T>(string Name)
        {
            return dictionary[Name] as IWertBox<T>;
        }
        public void AddWerteBox<T>(IWertBox<T> WerteBox, string Name)
        {
            dictionary.Add(Name, WerteBox);
            WerteBox.AddListener(OnUserValueChanged);
            WerteBox.AddListener((sender, e) => WertChanged(sender, new WertEventArgs(Name, WerteBox.GetValue())));
            WerteBox.AddInvalidListener(OnInvalidChange);
            List.Add(WerteBox as Control);
        }
        public T GetValue<T>(string Name)
        {
            IWertBox ob;
            if (!dictionary.TryGetValue(Name, out ob))
                throw new NotImplementedException();

            IWertBox<T> wb = ob as IWertBox<T>;
            if (wb == null)
                throw new ArgumentException();
            return wb.GetValue();
        }
        /// <summary>
        /// Gibt nicht null, sondern leerer String bei null aus
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string GetString(string Name)
        {
            string s = GetValue<string>(Name);
            if (s == null)
                return "";
            else
                return s;
        }
        public void ShowBox(string Name, bool Visible)
        {
            IWertBox ob;
            if (!dictionary.TryGetValue(Name, out ob))
                throw new NotImplementedException();

            Control wb = ob as Control;
            wb.Visible = Visible;
        }
        public void SetValue<T>(string Name, T Value)
        {
            IWertBox ob;
            if (!dictionary.TryGetValue(Name, out ob))
                throw new NotImplementedException();
            IWertBox<T> wb = ob as IWertBox<T>;
            if (wb == null)
                throw new ArgumentException();
            wb.SetValue(Value);
        }
        public void Setup()
        {
            List.Setup();
        }
        public void OnUserValueChanged(object sender, EventArgs e)
        {
            UserValueChanged(sender, e);
        }
        public void OnInvalidChange(object sender, EventArgs e)
        {
            InvalidChange(sender, e);
        }
        public void AddListener(EventHandler EventHandler)
        {
            UserValueChanged += EventHandler;
        }
        public void AddInvalidListener(EventHandler EventHandler)
        {
            InvalidChange += EventHandler;
        }
        public bool Valid()
        {
            foreach (IWertBox item in List)
                if (!item.Valid())
                    return false;
            return true;
        }
        public void DDispose()
        {
            List.Dispose();
            foreach (IWertBox item in dictionary.Values)
                item.DDispose();
            this.Dispose();
        }
    }
}
