using System;
using System.Drawing;

namespace Assistment.Drawing.Graph
{
    public class RegelEck : ZeichenObjekt
    {
        public float radius;
        public float drehwinkel;
        public PointF[] ecken;

        public RegelEck(float X, float Y, float radius, int anzahlEcken, float drehwinkel, Brush back, Pen rand)
            : this(new PointF(X, Y), radius, anzahlEcken, drehwinkel, back, rand)
        {
        }

        public RegelEck(PointF mittelPunkt, float radius, int anzahlEcken, float drehwinkel, Brush back, Pen rand)
            : base(mittelPunkt, back, rand)
        {
            this.radius = radius;
            this.ecken = new PointF[anzahlEcken + 1];
            this.drehwinkel = (drehwinkel + TAU) % TAU;
            float winkel = drehwinkel;
            float dWinkel = TAU / anzahlEcken;
            for (int i = 0; i < anzahlEcken; i++)
            {
                ecken[i] = new PointF((float)(mittelPunkt.X + Math.Cos(winkel) * radius), (float)(mittelPunkt.Y + Math.Sin(winkel) * radius));
                winkel += dWinkel;
            }
            ecken[anzahlEcken] = ecken[0];
        }

        public override void draw(Graphics g)
        {
            if (back != null)
                g.FillPolygon(back, ecken);
            if (rand != null)
                g.DrawPolygon(rand, ecken);
        }
        public override PointF getPoint(PointF start)
        {
            float X = start.X - ort.X;
            float Y = start.Y - ort.Y;
            float winkel = (float)((Math.Atan2(Y, X) + 2 * TAU - drehwinkel) % TAU);
            float off = winkel * (ecken.Length - 1) / TAU;
            int n = (int)off;
            off -= n;
            return new PointF((1 - off) * ecken[n].X + off * ecken[n + 1].X, (1 - off) * ecken[n].Y + off * ecken[n + 1].Y);
        }
        public override void draw(Bild b)
        {
            PointF[] p = b.align(ecken);
            if (back != null)
                b.g.FillPolygon(back, p);
            if (rand != null)
                b.g.DrawPolygon(rand, p);
        }
    }
}
