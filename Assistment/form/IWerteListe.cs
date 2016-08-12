using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assistment.form
{
    public interface IWerteListe
    {
        void AddWerteBox<T>(IWertBox<T> WerteBox, string Name);
        T GetValue<T>(string Name);
        void SetValue<T>(string Name, T Value);
        void AddListener(EventHandler EventHandler);
    }

    public class WerteListe : ScrollBox, IWerteListe
    {
        private SortedDictionary<string, object> dictionary = new SortedDictionary<string, object>();
        public event EventHandler UserValueChanged = delegate { };
        private ControlList List;

        public WerteListe()
            : base(CreateList())
        {
            List = base.Control as ControlList;
        }
        private static ControlList CreateList()
        {
            return new ControlList();
        }

        public void AddWerteBox<T>(IWertBox<T> WerteBox, string Name)
        {
            dictionary.Add(Name, WerteBox);
            WerteBox.AddListener((sender, e) => OnUserValueChanged());
            List.Add(WerteBox as Control);
        }
        public T GetValue<T>(string Name)
        {
            object ob;
            if (!dictionary.TryGetValue(Name, out ob))
                throw new NotImplementedException();

            IWertBox<T> wb = ob as IWertBox<T>;
            return wb.GetValue();
        }
        public void SetValue<T>(string Name, T Value)
        {
            object ob;
            if (!dictionary.TryGetValue(Name, out ob))
                throw new NotImplementedException();

            IWertBox<T> wb = ob as IWertBox<T>;
            wb.SetValue(Value);
        }
        public void Setup()
        {
            List.Setup();
            this.OnSizeChanged(EventArgs.Empty);
        }
        public void OnUserValueChanged()
        {
            UserValueChanged(this, EventArgs.Empty);
        }
        public void AddListener(EventHandler EventHandler)
        {
            UserValueChanged += EventHandler;
        }
    }

    public static class WerteListeErweiterer
    {
        public static void AddWertePaar<T>(this IWerteListe WerteListe, IWertBox<T> WerteBox, T value, string Name)
        {
            WerteBox.SetValue(value);
            WerteListe.AddWerteBox(new WertPaar<T>(Name, WerteBox), Name);
        }
        public static void AddIntBox(this IWerteListe WerteListe, int value, string Name)
        {
            AddWertePaar(WerteListe, new IntBox(), value, Name);
        }
        public static void AddBoolBox(this IWerteListe WerteListe, bool value, string Name)
        {
            BoolBox bb = new BoolBox(Name);
            bb.SetValue(value);
            WerteListe.AddWerteBox(bb, Name);
        }
        public static void AddFloatBox(this IWerteListe WerteListe, float value, string Name)
        {
            AddWertePaar(WerteListe, new FloatBox(), value, Name);
        }
        public static void AddColorBox(this IWerteListe WerteListe, Color value, string Name)
        {
            AddWertePaar(WerteListe, new ColorBox(), value, Name);
        }
        public static void AddEnumBox(this IWerteListe WerteListe, object value, string Name)
        {
            AddWertePaar(WerteListe, new EnumBox(), value, Name);
        }
        public static void AddPenBox(this IWerteListe WerteListe, Pen value, string Name)
        {
            AddWertePaar(WerteListe, new PenBox(), value, Name);
        }
        public static void AddPointBox(this IWerteListe WerteListe, Point value, string Name)
        {
            AddWertePaar(WerteListe, new PointBox(), value, Name);
        }
        public static void AddPointFBox(this IWerteListe WerteListe, PointF value, string Name)
        {
            AddWertePaar(WerteListe, new PointFBox(), value, Name);
        }
        public static void AddSizeBox(this IWerteListe WerteListe, Size value, string Name)
        {
            AddWertePaar(WerteListe, new PointBox(), value, Name);
        }
        public static void AddSizeFBox(this IWerteListe WerteListe, SizeF value, string Name)
        {
            AddWertePaar(WerteListe, new PointFBox(), value, Name);
        }
        public static void AddImageBox(this IWerteListe WerteListe, string value, string Name)
        {
            AddWertePaar(WerteListe, new ImageSelectBox(), value, Name);
        }
        public static void AddFontBox(this IWerteListe WerteListe, Font value, string Name)
        {
            AddWertePaar(WerteListe, new FontBox(), value, Name);
        }
    }
}
