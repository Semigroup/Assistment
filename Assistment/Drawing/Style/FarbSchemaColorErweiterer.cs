using System.Drawing;

namespace Assistment.Drawing.Style
{
    public static class FarbSchemaColorErweiterer
    {
        public static Brush GetBrush(this FarbSchema<Color> Schema, FarbSchema<Color>.Typ Typ)
        {
            return new SolidBrush(Schema[Typ]);
        }
        public static Brush GetBrush(this FarbSchema<Color> Schema, int Alpha, FarbSchema<Color>.Typ Typ)
        {
            return new SolidBrush(Color.FromArgb(Alpha, Schema[Typ]));
        }
        public static Brush GetBrush(this FarbSchema<Color> Schema, float Alpha, FarbSchema<Color>.Typ Typ)
        {
            return new SolidBrush(Color.FromArgb((int)(255 * Alpha), Schema[Typ]));
        }
        public static Pen GetPen(this FarbSchema<Color> Schema, FarbSchema<Color>.Typ Typ)
        {
            return new Pen(Schema[Typ]);
        }
    }
}
