using System.Drawing;

namespace Assistment.Drawing.Graph
{
    public abstract class ZeichenObjekt
    {
        protected const float TAU = Shadex.Tau;

        public Brush back;
        public Pen rand;
        public PointF ort;

        public ZeichenObjekt(PointF ort, Brush back, Pen rand)
        {
            this.ort = ort;
            this.back = back;
            this.rand = rand;
        }

        /// <summary>
        /// gibt das Ziel einer Kante an, die bei start beginnt und diesen Knoten erreichen soll
        /// </summary>
        /// <param name="start"></param>
        public abstract PointF getPoint(PointF start);
        public abstract void draw(Graphics g);
        public abstract void draw(Bild b);
    }
}
