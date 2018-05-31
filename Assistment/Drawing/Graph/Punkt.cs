using System.Drawing;

namespace Assistment.Drawing.Graph
{
    public class Punkt : ZeichenObjekt
    {
        public Punkt(float X, float Y, Pen rand)
            : this(new PointF(X, Y), rand)
        {
        }
        public Punkt(PointF ort, Pen rand)
            : base(ort, null, rand)
        {

        }

        public override void draw(Graphics g)
        {
            if (rand != null)
                g.DrawLine(rand, ort, ort);
        }
        public override PointF getPoint(PointF start)
        {
            return ort;
        }
        public override void draw(Bild b)
        {
            if (rand != null)
            {
                PointF P = b.align(ort);
                b.g.DrawLine(rand, P, P);
            }
        }
    }
}
