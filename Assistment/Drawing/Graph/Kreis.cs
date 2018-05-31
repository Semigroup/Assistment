using System;
using System.Drawing;

namespace Assistment.Drawing.Graph
{
    public class Kreis : ZeichenObjekt
    {
        public float radius;

        public Kreis(float X, float Y, float radius, Brush back, Pen rand)
            : this(new PointF(X, Y), radius, back, rand)
        {

        }
        public Kreis(PointF mittelPunkt, float radius, Brush back, Pen rand)
            : base(mittelPunkt, back, rand)
        {
            this.radius = radius;
        }

        public override void draw(Graphics g)
        {
            RectangleF r = new RectangleF(ort.X - radius, ort.Y - radius, 2 * radius, 2 * radius);
            if (back != null)
                g.FillEllipse(back, r);
            if (rand != null)
                g.DrawEllipse(rand, r);
        }
        public override PointF getPoint(PointF start)
        {
            float X = start.X - ort.X;
            float Y = start.Y - ort.Y;
            float norm = (float)Math.Sqrt(X * X + Y * Y);
            if (norm >= radius)
            {
                norm = radius / norm;
                return new PointF(ort.X + X * norm, ort.Y + Y * norm);
            }
            else
                return ort;
        }
        public override void draw(Bild b)
        {
            RectangleF r = b.align(new RectangleF(ort.X - radius, ort.Y - radius, 2 * radius, 2 * radius));
            if (back != null)
                b.g.FillEllipse(back, r);
            if (rand != null)
                b.g.DrawEllipse(rand, r);
        }
    }
}
