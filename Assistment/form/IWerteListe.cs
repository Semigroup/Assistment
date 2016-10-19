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
    public class WertEventArgs : EventArgs
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public WertEventArgs(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
    public delegate void WertEventHandler(object sender, WertEventArgs e);

    public interface IWerteListe : IWertBox
    {
        void AddWerteBox<T>(IWertBox<T> WerteBox, string Name);
        T GetValue<T>(string Name);
        void SetValue<T>(string Name, T Value);
        event WertEventHandler WertChanged;
    }

    public class WerteListe : ScrollBox, IWerteListe
    {
        private SortedDictionary<string, IWertBox> dictionary = new SortedDictionary<string, IWertBox>();
        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };
        public event WertEventHandler WertChanged = delegate { };
        private ControlList List;

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

    public static class WerteListeErweiterer
    {
        public static void ShowBox(this WerteListe WerteListe, bool Visible, params string[] Namen)
        {
            foreach (var item in Namen)
                WerteListe.ShowBox(item, Visible);
        }

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
        public static void AddStringBox(this IWerteListe WerteListe, string value, string Name)
        {
            AddWertePaar(WerteListe, new StringBox(), value, Name);
        }
        public static void AddChainedSizeFBox(this IWerteListe WerteListe, SizeF value, string Name)
        {
            AddChainedSizeFBox(WerteListe, value, Name, true);
        }
        public static void AddChainedSizeFBox(this IWerteListe WerteListe, SizeF value, string Name, bool Chained)
        {
            ChainedSizeFBox csb = new ChainedSizeFBox();
            csb.Chained = Chained;
            AddWertePaar(WerteListe, csb, value, Name);
        }
        public static void AddChainedSizeFBox(this IWerteListe WerteListe, SizeF value, string Name, bool Chained, SizeF UserSizeMaximum)
        {
            ChainedSizeFBox csb = new ChainedSizeFBox();
            csb.Chained = Chained;
            csb.UserSizeMaximum = UserSizeMaximum;
            AddWertePaar(WerteListe, csb, value, Name);
        }
        public static void AddBigStringBox(this IWerteListe WerteListe, string value, string Name)
        {
            StringBox sb = new StringBox();
            sb.Multiline = true;
            sb.Size = new Size(300, 200);
            sb.ScrollBars = ScrollBars.Vertical;
            AddWertePaar(WerteListe, sb, value, Name);
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
        public static void AddImageBox(this IWerteListe WerteListe, string value, string Name, bool ShowImage)
        {
            ImageSelectBox isb = new ImageSelectBox();
            isb.ShowImage = ShowImage;
            AddWertePaar(WerteListe, isb, value, Name);
        }
        public static void AddImageBox(this IWerteListe WerteListe, string value, string Name, bool ShowImage, EventHandler ImageChanged)
        {
            ImageSelectBox isb = new ImageSelectBox();
            isb.ShowImage = ShowImage;
            isb.AddListener(ImageChanged);
            AddWertePaar(WerteListe, isb, value, Name);
        }
        public static void AddFontBox(this IWerteListe WerteListe, Font value, string Name)
        {
            AddWertePaar(WerteListe, new FontBox(), value, Name);
        }
    }
}
