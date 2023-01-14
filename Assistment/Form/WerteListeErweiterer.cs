using System;
using System.Drawing;
using System.Windows.Forms;

namespace Assistment.form
{
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
        public static void AddLabelBox(this IWerteListe WerteListe, object value, string Name)
        {
            AddWertePaar(WerteListe, new LabelBox(), value.ToString(), Name);
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
        public static void AddRectangleFBox(this IWerteListe WerteListe, RectangleF value, string Name)
        {
            AddWertePaar(WerteListe, new RectangleFBox(), value, Name);
        }
        public static void AddFileBox(this IWerteListe WerteListe, string file, string Name)
        {
            AddWertePaar(WerteListe, new FileBox(), file, Name);
        }
        public static void AddDirectoryBox(this IWerteListe WerteListe, string directory, string Name)
        {
            AddWertePaar(WerteListe, new DirectoryBox(), directory, Name);
        }
    }
}
