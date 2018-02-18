using System.Drawing;

namespace Assistment.Texts
{
    public static class FontErweiterer
    {
        public static FontGraphicsMeasurer GetMeasurer(this Font Font)
        {
            return new FontGraphicsMeasurer(Font);
        }
    }
}
