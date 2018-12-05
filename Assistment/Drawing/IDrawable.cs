using System.Drawing;

namespace Assistment.Drawing
{
    public interface IDrawable
    {
        void draw(ContextAligner contextAligner);
        SizeF getRelativeSize();
        bool getYSpiegel();
        bool getXSpiegel();
    }
}
