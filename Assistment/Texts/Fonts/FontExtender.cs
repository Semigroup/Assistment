using System.Drawing;

namespace Assistment.Texts
{
    public static class FontExtender
    {
        public static FontGraphicsMeasurer GetMeasurer(this Font Font)
        {
            return new FontGraphicsMeasurer(Font);
        }

        public static float XMass(this IFontMeasurer fontMeasurer, string s)
            => fontMeasurer.XMass(s, (byte)0);
        public static float XMass(this IFontMeasurer fontMeasurer, char c)
          => fontMeasurer.XMass(c, (byte)0);
        public static float YMass(this IFontMeasurer fontMeasurer, string s)
        => fontMeasurer.YMass(s, (byte)0);
        public static float YMass(this IFontMeasurer fontMeasurer, char c)
            => fontMeasurer.YMass(c, (byte)0);

        public static SizeF Size(this IFontMeasurer fontMeasurer, char c)
           => fontMeasurer.Size(c, (byte)0);
        public static SizeF Size(this IFontMeasurer fontMeasurer, string s)
      => fontMeasurer.Size(s, (byte)0);
    }
}
