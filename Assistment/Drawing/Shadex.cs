using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Assistment.Drawing
{
    public delegate PointF Weg(float t);
    public delegate float Hohe(float t);
    public delegate PointF Punkt(float x, float y);
    public unsafe delegate void AddierPolygone(PointF* p, PointF* q);
    public unsafe delegate void ModPolygon(PointF* p);
    public delegate void Mach();

    public struct Gerade
    {
        public PointF Punkt;
        public PointF Vektor;

        public Gerade(PointF Punkt, PointF Vektor)
        {
            this.Punkt = Punkt;
            this.Vektor = Vektor;
        }

        public Gerade(float x, float y, float dx, float dy)
        {
            this.Punkt = new PointF(x, y);
            this.Vektor = new PointF(dx, dy);
        }
    }

    public class orientierbarerWeg
    {
        /// <summary>
        /// sollte konstante ableitung besitzen
        /// <para>(damit alle Wegstücke gleich groß sind)</para>
        /// </summary>
        public Weg weg;
        /// <summary>
        /// normalenvektor muss auf wegpunkt drauf addiert werden
        /// <para>die normale muss normiert sein!</para>
        /// </summary>
        public Weg normale;
        /// <summary>
        /// konstanter Wert der Norm der Ableitung
        /// <para>und Länge des Weges</para>
        /// </summary>
        public float L;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weg">sollte konstante ableitung besitzen
        /// <para>(damit alle Wegstücke gleich groß sind)</para></param>
        /// <param name="normale">normalenvektor muss auf wegpunkt drauf addiert werden
        /// <para>die normale muss normiert sein!</para></param>
        /// <param name="c">konstanter Wert der Norm der Ableitung</param>
        public orientierbarerWeg(Weg weg, Weg normale, float L)
        {
            this.weg = weg;
            this.normale = normale;
            this.L = L;
        }
        /// <summary>
        /// Zieht einen Weg von A nach B
        /// <para>Normale zeigt links von A->B</para>
        /// <para>(mathematisch rechts..., y-Achse wird gespiegelt!)</para>
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public orientierbarerWeg(PointF A, PointF B)
        {
            float dx = B.X - A.X;
            float dy = B.Y - A.Y;

            L = sqrt(dx * dx + dy * dy);
            weg = t => new PointF(A.X + dx * t, A.Y + dy * t);
            PointF n = new PointF(dy / L, -dx / L);
            normale = x => n;
        }
        /// <summary>
        /// <para>Länge wird ganz billig approximiert!</para>
        /// </summary>
        /// <param name="A"></param>
        /// <param name="dA"></param>
        /// <param name="B"></param>
        /// <param name="dB"></param>
        public orientierbarerWeg(PointF A, PointF dA, PointF B, PointF dB)
        {
            float ddx = dA.X + dB.X;
            float ppx = A.X - B.X;
            float ax = ddx + 2 * ppx;
            float bx = -3 * ppx - ddx - dA.X;
            float ddy = dA.Y + dB.Y;
            float ppy = A.Y - B.Y;
            float ay = ddy + 2 * ppy;
            float by = -3 * ppy - ddy - dA.Y;
            weg = t =>
            {
                float tt = t * t;
                PointF P = saxpy(A, t, dA);
                P.X += bx * tt;
                P.Y += by * tt;
                tt *= t;
                P.X += ax * tt;
                P.Y += ay * tt;
                return P;
            };
            normale = t =>
            {
                PointF P = new PointF(dA.Y, -dA.X);
                P.X += 2 * by * t;
                P.Y -= 2 * bx * t;
                t *= t;
                P.X += 3 * ay * t;
                P.Y -= 3 * ax * t;
                float norm = (float)Math.Sqrt(P.X * P.X + P.Y * P.Y);
                P.X /= norm;
                P.Y /= norm;
                return P;
            };
            #region Länge
            {
                double l = 0;
                double dx, dy;
                PointF[] punkte = getPolygon(10, 0, 1);
                for (int i = 0; i < punkte.Length - 1; i++)
                {
                    dx = punkte[i + 1].X - punkte[i].X;
                    dy = punkte[i + 1].Y - punkte[i].Y;
                    l += Math.Sqrt(dx * dx + dy * dy);
                }
                L = (float)l;
            }
            #endregion
        }
        /// <summary>
        /// <para>Länge wird ganz billig approximiert!</para>
        /// </summary>
        /// <param name="A"></param>
        /// <param name="dA"></param>
        /// <param name="B"></param>
        /// <param name="dB"></param>
        public orientierbarerWeg(Gerade A, Gerade B)
            : this(A.Punkt, A.Vektor, B.Punkt, B.Vektor)
        {

        }

        /// <summary>
        /// setzt P[offset],...P[offset + samples - 1] auf weg(t0), ..., weg(t1-d)
        /// </summary>
        /// <param name="P"></param>
        /// <param name="offset"></param>
        /// <param name="samples"></param>
        /// <param name="t0"></param>
        /// <param name="t1"></param>
        public void getPolygon(PointF[] P, int offset, int samples, float t0, float t1)
        {
            float d = (t1 - t0) / (samples);
            for (int i = 0; i < samples; i++)
                P[i + offset] = weg(i * d + t0);
        }
        public PointF[] getPolygon(int samples, float t0, float t1)
        {
            PointF[] P = new PointF[samples];
            float d = (t1 - t0) / (samples);
            for (int i = 0; i < samples; i++)
                P[i] = weg(i * d + t0);
            return P;
        }
        /// <summary>
        /// setzt P[offset],...P[offset + samples - 1] auf (weg+hohe*n)(t0), ..., (weg+hohe*n)(t1-d)
        /// </summary>
        /// <param name="P"></param>
        /// <param name="offset"></param>
        /// <param name="samples"></param>
        /// <param name="t0"></param>
        /// <param name="t1"></param>
        public void getPolygon(PointF[] P, int offset, int samples, float t0, float t1, Hohe hohe)
        {
            float d = (t1 - t0) / (samples);
            float f;
            for (int i = 0; i < samples; i++)
            {
                f = i * d + t0;
                P[i + offset] = saxpy(weg(f), hohe(f), normale(f));
            }
        }
        public PointF[] getPolygon(int samples, float t0, float t1, Hohe hohe)
        {
            PointF[] P = new PointF[samples];
            float d = (t1 - t0) / (samples);
            float f;
            for (int i = 0; i < samples; i++)
            {
                f = i * d + t0;
                P[i] = saxpy(weg(f), hohe(f), normale(f));
            }
            return P;
        }
        /// <summary>
        /// liefert ein Polygon mit 2*samples viele Punkte
        /// <para>die ersten samples Punkte sind der Weg von t0 bis t1</para>
        /// <para>die letzten samples Punkte sind der Weg + normale*hohe von t1 nach t0</para>
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="t0"></param>
        /// <param name="t1"></param>
        /// <param name="hohe"></param>
        /// <returns></returns>
        public PointF[] getCircularPolygon(int samples, float t0, float t1, Hohe hohe)
        {
            PointF[] P = new PointF[2 * samples];
            float d = (t1 - t0) / (samples - 1);
            float f;
            int sam2 = 2 * samples - 1;
            for (int i = 0; i < samples; i++)
            {
                f = i * d + t0;
                P[i] = weg(f);
                P[sam2 - i] = saxpy(P[i], hohe(f), normale(f));
            }
            return P;
        }

        /// <summary>
        /// dreht das Vorzeichen der Normalen um
        /// </summary>
        public void invertier()
        {
            Weg norm = this.normale;
            this.normale = t =>
            {
                PointF n = norm(t);
                return new PointF(-n.X, -n.Y);
            };
        }
        /// <summary>
        /// Kreiert ein Testbild namens test.png
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hohe"></param>
        public void print(int width, int height, float hohe)
        {
            Bitmap b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);
            PointF P;
            float f;
            float d = 3f / L;
            for (int i = 0; i < (int)L / 3; i++)
            {
                f = i * d;
                P = weg(f);
                g.DrawLine(Pens.Black, P, saxpy(P, hohe, normale(f)));
            }
            b.Save("test.png");
        }

        private static Weg add(Weg y, PointF p)
        {
            return t =>
            {
                PointF yt = y(t);
                yt.X += p.X;
                yt.Y += p.Y;
                return yt;
            };
        }
        private static Hohe add(Hohe h, Hohe g)
        {
            return t => h(t) + g(t);
        }
        private static PointF saxpy(PointF p, float a, PointF q)
        {
            return new PointF(p.X + a * q.X, p.Y + a * q.Y);
        }
        private static float sqrt(float t)
        {
            return (float)(Math.Sqrt(Math.Abs(t)));
        }

        public static orientierbarerWeg operator *(orientierbarerWeg gamma1, orientierbarerWeg gamma2)
        {
            float L = gamma1.L + gamma2.L;
            float m = gamma1.L / (gamma1.L + gamma2.L);
            float m2 = gamma2.L / (gamma1.L + gamma2.L);
            return new orientierbarerWeg(t =>
            {
                if (t <= m)
                    return gamma1.weg(t / m);
                else
                    return gamma2.weg((t - m) / m2);
            }, t =>
            {
                if (t <= m)
                    return gamma1.normale(t / m);
                else
                    return gamma2.normale((t - m) / m2);
            }, L);
        }
        public static orientierbarerWeg operator +(orientierbarerWeg gamma, PointF point)
        {
            return new orientierbarerWeg(add(gamma.weg, point), gamma.normale, gamma.L);
        }
    }

    public class Knoten
    {
        private const float TAU = Shadex.Tau;
        private static Random d = Shadex.dice;

        public struct Kante
        {
            /// <summary>
            /// endknoten
            /// </summary>
            public Knoten ziel;
            public Pen stift;
        }
        public abstract class ZeichenObjekt
        {
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
                float norm = (float)(radius / Math.Sqrt(X * X + Y * Y));
                return new PointF(ort.X + X * norm, ort.Y + Y * norm);
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

        public int token;
        /// <summary>
        /// von diesem Knoten weiterführende Kanten
        /// </summary>
        public List<Kante> kanten;
        public ZeichenObjekt objekt;

        public PointF ort
        {
            get
            {
                return objekt.ort;
            }
            set
            {
                objekt.ort = value;
            }
        }
        public float X
        {
            get
            {
                return objekt.ort.X;
            }
            set
            {
                objekt.ort.X = value;
            }
        }
        public float Y
        {
            get
            {
                return objekt.ort.Y;
            }
            set
            {
                objekt.ort.Y = value;
            }
        }

        public Knoten(float X, float Y)
            : this(new PointF(X, Y), null)
        {
        }
        public Knoten(PointF ort, Pen rand)
        {
            this.kanten = new List<Kante>();
            this.token = d.Next();
            this.objekt = new Punkt(ort, rand);
        }
        public Knoten(ZeichenObjekt objekt)
        {
            this.kanten = new List<Kante>();
            this.token = d.Next();
            this.objekt = objekt;
        }

        /// <summary>
        /// malt alle ausgehenden Kanten
        /// </summary>
        /// <param name="g"></param>
        public void drawKanten(Graphics g)
        {
            PointF ziel, start;
            foreach (Kante kante in kanten)
            {
                ziel = kante.ziel.getPoint(this.objekt.ort);
                start = getPoint(ziel);
                g.DrawLine(kante.stift, start, ziel);
            }
        }
        private void drawKaskade(Graphics g, int token)
        {
            if (token == this.token)
                return;
            this.token = token;
            drawKanten(g);
            draw(g);
            foreach (Kante kante in kanten)
                kante.ziel.drawKaskade(g, token);
        }
        public void drawKaskade(Graphics g)
        {
            this.drawKaskade(g, d.Next());
        }
        public void draw(Graphics g)
        {
            objekt.draw(g);
        }

        /// <summary>
        /// malt alle ausgehenden Kanten
        /// </summary>
        /// <param name="g"></param>
        public void drawKanten(Bild b)
        {
            PointF ziel, start;
            foreach (Kante kante in kanten)
            {
                ziel = b.align(kante.ziel.getPoint(this.objekt.ort));
                start = b.align(getPoint(ziel));
                b.g.DrawLine(kante.stift, start, ziel);
            }
        }
        private void drawKaskade(Bild b, int token)
        {
            if (token == this.token)
                return;
            this.token = token;
            drawKanten(b);
            draw(b);
            foreach (Kante kante in kanten)
                kante.ziel.drawKaskade(b, token);
        }
        public void drawKaskade(Bild b)
        {
            this.drawKaskade(b, d.Next());
        }
        public void draw(Bild b)
        {
            objekt.draw(b);
        }

        /// <summary>
        /// gibt das Ziel einer Kante an, die bei start beginnt und diesen Knoten erreichen soll
        /// </summary>
        /// <param name="start"></param>
        public PointF getPoint(PointF start)
        {
            return objekt.getPoint(start);
        }

        public void addKante(Pen stift, Knoten ziel)
        {
            Kante k;
            k.stift = stift;
            k.ziel = ziel;
            this.kanten.Add(k);
        }

        /// <summary>
        /// gibt eine Liste aller erreichbaren Knoten
        /// </summary>
        /// <returns></returns>
        public List<Knoten> getKnoten()
        {
            List<Knoten> lKnoten = new List<Knoten>();
            Queue<Knoten> queue = new Queue<Knoten>();
            queue.Enqueue(this);
            int tok = d.Next();
            while (queue.Count > 0)
            {
                Knoten knoten = queue.Dequeue();
                lKnoten.Add(knoten);
                knoten.token = tok;
                foreach (Kante kante in knoten.kanten)
                    if (kante.ziel.token != tok)
                        queue.Enqueue(kante.ziel);
            }
            return lKnoten;
        }
    }

    public static class Shadex
    {
        private class knotenHohenVergleicher : IComparer<Knoten>
        {
            public int Compare(Knoten x, Knoten y)
            {
                float f = x.ort.Y - y.ort.Y;
                if (f > 0)
                    return 1;
                else if (f < 0)
                    return -1;
                else
                    return 1;
            }
        }

        /// <summary>
        /// 0,5 mal Pi
        /// </summary>
        public const float Rho = (float)(0.5 * Math.PI);
        /// <summary>
        /// 1 mal Pi
        /// </summary>
        public const float Pi = (float)Math.PI;
        /// <summary>
        /// 1,5 mal Pi
        /// </summary>
        public const float Phi = (float)(3 * Math.PI / 2);
        /// <summary>
        /// 2 mal Pi
        /// </summary>
        public const float Tau = (float)(2 * Math.PI);

        public static Random dice = new Random();

        /// <summary>
        /// von 0 bis 1 (nicht 0 bis 2 Pi)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static float cos(float t)
        {
            return (float)Math.Cos(t * Tau);
        }
        /// <summary>
        /// von 0 bis 1 (nicht 0 bis 2 Pi)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static float sin(float t)
        {
            return (float)Math.Sin(t * Tau);
        }
        private static float sqrt(float t)
        {
            return (float)(Math.Sqrt(Math.Abs(t)));
        }
        /// <summary>
        /// p + a*q
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static PointF saxpy(PointF p, float a, PointF q)
        {
            return new PointF(p.X + a * q.X, p.Y + a * q.Y);
        }
        /// <summary>
        /// a * q
        /// </summary>
        /// <param name="a"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static PointF mul(float a, PointF q)
        {
            return new PointF(a * q.X, a * q.Y);
        }
        /// <summary>
        /// p + q
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        private static PointF add(PointF p, PointF q)
        {
            return new PointF(p.X + q.X, p.Y + q.Y);
        }
        private static PointF add(PointF p, float x, float y)
        {
            return new PointF(p.X + x, p.Y + y);
        }
        private static Weg add(Weg y, PointF p)
        {
            return t =>
            {
                PointF yt = y(t);
                yt.X += p.X;
                yt.Y += p.Y;
                return yt;
            };
        }
        private static Hohe add(Hohe h, Hohe g)
        {
            return t => h(t) + g(t);
        }
        private static Hohe add(Hohe h, float g)
        {
            return t => h(t) + g;
        }
        public static orientierbarerWeg RundesRechteck(RectangleF r, float radius)
        {
            orientierbarerWeg ecke1, ecke2, ecke3, ecke4;
            orientierbarerWeg rand1, rand2, rand3, rand4;

            float bm = Rho * radius;
            ecke1 = new orientierbarerWeg(t => new PointF(r.X + radius + radius * cos(t / 4 + 0.25f), r.Y + radius - radius * sin(t / 4 + 0.25f)),
                t => new PointF(cos(t / 4 + 0.25f), -sin(t / 4 + 0.25f)),
             bm);
            ecke2 = new orientierbarerWeg(t => new PointF(r.X + radius + radius * cos(t / 4 + 0.5f), r.Bottom - radius - radius * sin(t / 4 + 0.5f)),
                t => new PointF(cos(t / 4 + 0.5f), -sin(t / 4 + 0.5f)),
             bm);
            ecke3 = new orientierbarerWeg(t => new PointF(r.Right - radius + radius * cos(t / 4 + 0.75f), r.Bottom - radius - radius * sin(t / 4 + 0.75f)),
                t => new PointF(cos(t / 4 + 0.75f), -sin(t / 4 + 0.75f)),
             bm);
            ecke4 = new orientierbarerWeg(t => new PointF(r.Right - radius + radius * cos(t / 4), r.Y + radius - radius * sin(t / 4)),
                t => new PointF(cos(t / 4), -sin(t / 4)),
             bm);

            float Lv = r.Height - 2 * radius;
            float Lh = r.Width - 2 * radius;
            rand1 = new orientierbarerWeg(t => new PointF(r.X, Lv * t + radius + r.Y),
                t => new PointF(-1, 0),
                Lv);
            rand2 = new orientierbarerWeg(t => new PointF(Lh * t + radius + r.X, r.Bottom),
                t => new PointF(0, 1),
                Lh);
            rand3 = new orientierbarerWeg(t => new PointF(r.Right, r.Bottom - radius - t * Lv),
                t => new PointF(1, 0),
                Lv);
            rand4 = new orientierbarerWeg(t => new PointF(r.Right - radius - t * Lh, r.Y),
                t => new PointF(0, -1),
                Lh);

            return ((ecke1 * rand1) * (ecke2 * rand2)) * ((ecke3 * rand3) * (ecke4 * rand4));
        }
        public static orientierbarerWeg Ellipse(RectangleF r)
        {
            PointF M = new PointF((r.Right + r.Left) / 2, (r.Bottom + r.Top) / 2);
            PointF rad = new PointF((r.Right - r.Left) / 2, (r.Bottom - r.Top) / 2);
            Weg y = t => new PointF(M.X + rad.X * cos(t), M.Y + rad.Y * sin(t));
            Weg n = t =>
                {
                    PointF p = new PointF(rad.Y * cos(t), rad.X * sin(t));
                    float norm = sqrt(p.X * p.X + p.Y * p.Y);
                    return new PointF(p.X / norm, p.Y / norm);
                };
            //Länge nur eine Annäherung
            return new orientierbarerWeg(y, n, Rho * (r.Width + r.Height));
        }
        public static Hohe bezierStachelrand(Hohe h, int stachel)
        {
            float f = 1.0f / stachel;
            /// t[0] zw. 0 und f
            /// t[1] zw. f und 2f
            /// ...
            /// h = c*(x-t)^2
            float[] c = new float[stachel];
            float[] t = new float[stachel];
            float t0, t1;
            /// wurzel von h(t0) und h(t1) 
            float h0, h1;

            t1 = 0;
            h1 = sqrt(h(t1));
            for (int i = 0; i < stachel; i++)
            {
                t0 = t1;
                t1 += f;
                h0 = h1;
                h1 = sqrt(h(t1));
                t[i] = (h0 * t1 + h1 * t0) / (h1 + h0);
                if (h0 != 0)
                    c[i] = h0 / (t0 - t[i]);
                else
                    c[i] = h1 / (t1 - t[i]);
                c[i] *= c[i];
            }
            return T =>
                {
                    int i = (int)Math.Floor(T / f);
                    return c[i] * (t[i] - T) * (t[i] - T);
                };
        }
        public static Hohe negativerBezierStachelrand(Hohe h, int stachel)
        {
            float f = 1.0f / stachel;
            /// t[0] zw. 0 und f
            /// t[1] zw. f und 2f
            /// ...
            /// h = c*(x-t)^2
            float[] c = new float[stachel];
            float[] t = new float[stachel];
            float t0, t1;
            /// wurzel von h(t0) und h(t1) 
            float h0, h1;

            t1 = 0;
            h1 = sqrt(h(t1));
            for (int i = 0; i < stachel; i++)
            {
                t0 = t1;
                t1 += f;
                h0 = h1;
                h1 = sqrt(h(t1));
                t[i] = (h0 * t1 + h1 * t0) / (h1 + h0);
                if (h0 != 0)
                    c[i] = h0 / (t0 - t[i]);
                else
                    c[i] = h1 / (t1 - t[i]);
                c[i] *= c[i];
            }
            return T =>
            {
                int i = (int)Math.Floor(T / f);
                return -c[i] * (t[i] - T) * (t[i] - T);
            };
        }
        /// <summary>
        /// liefert eine Verklebung f von Polys 1. Grades
        /// <para>s.d. f(i/n) = werte[i]</para>
        /// </summary>
        /// <param name="werte"></param>
        /// <returns></returns>
        public static Hohe linearApprox(float[] werte)
        {
            float[] diff = new float[werte.Length];
            for (int i = 0; i < werte.Length - 1; i++)
                diff[i] = werte[i + 1] - werte[i];
            diff[werte.Length - 1] = werte[0] - werte[werte.Length - 1];

            return t =>
                {
                    float tw = t * werte.Length;
                    int n = Math.Min((int)(tw), werte.Length - 1);
                    return werte[n] + diff[n] * (tw - n);
                };
        }

        /// <summary>
        /// b[0] innen, b[layer-1] außen
        /// </summary>
        /// <param name="g"></param>
        /// <param name="brushes"></param>
        /// <param name="y"></param>
        /// <param name="basis"></param>
        /// <param name="samples"></param>
        /// <param name="stachel"></param>
        public static void malBezierrand(Graphics g, Brush[] brushes, orientierbarerWeg y, Hohe hohe, int samples, int stachel)
        {
            int layer = brushes.Length;
            Hohe h = bezierStachelrand(t => hohe(t) / layer, stachel);
            PointF[] P = new PointF[2 * samples];
            PointF[] N = new PointF[samples];
            #region Berechen P und N
            {
                float d = 1f / (samples - 1);
                float f;
                for (int i = 0; i < samples; i++)
                {
                    f = i * d;
                    N[i] = mul(h(f), y.normale(f));
                    P[samples + i] = y.weg(f);
                    P[i] = saxpy(P[samples + i], layer, N[i]);
                }
            }
            #endregion
            for (int i = 0; i < layer; i++)
            {
                g.FillPolygon(brushes[layer - 1 - i], P);
                if (i != layer - 1)
                    for (int j = 0; j < samples; j++)
                    {
                        P[j].X -= N[j].X;
                        P[j].Y -= N[j].Y;
                    }
            }
        }
        /// <summary>
        /// b[0] innen, b[layer-1] außen
        /// </summary>
        /// <param name="g"></param>
        /// <param name="brushes"></param>
        /// <param name="y"></param>
        /// <param name="basis"></param>
        /// <param name="samples"></param>
        /// <param name="stachel"></param>
        public static void malBezierhulle(Graphics g, Brush[] brushes, orientierbarerWeg y, Hohe hohe, int samples, int stachel)
        {
            int layer = brushes.Length;
            Hohe h = bezierStachelrand(t => hohe(t) / layer, stachel);
            PointF[] P = new PointF[samples];
            PointF[] N = new PointF[samples];
            #region Berechen P und N
            {
                float d = 1f / (samples - 1);
                float f;
                for (int i = 0; i < samples; i++)
                {
                    f = i * d;
                    N[i] = mul(h(f), y.normale(f));
                    P[i] = saxpy(y.weg(f), layer, N[i]);
                }
            }
            #endregion
            for (int i = 0; i < layer; i++)
            {
                g.FillPolygon(brushes[layer - 1 - i], P);
                if (i != layer - 1)
                    for (int j = 0; j < samples; j++)
                    {
                        P[j].X -= N[j].X;
                        P[j].Y -= N[j].Y;
                    }
            }
        }

        public static Brush[] brushTable(Color c, int layer)
        {
            Brush[] b = new Brush[layer];
            for (int i = 0; i < layer; i++)
                b[i] = new SolidBrush(Color.FromArgb((int)(255.0f / (layer - i)), c));
            return b;
        }
        /// <summary>
        /// gibt die Distanz von p und dem ersten Schnittpunkt von tan*x + p und r wieder (x >= 0)
        /// <para>setzt voraus, dass p IN r LIEGT !!!</para>
        /// <para>tan muss NORMIERT sein</para>
        /// </summary>
        /// <param name="r"></param>
        /// <param name="p"></param>
        /// <param name="tan">muss normiert sein</param>
        /// <returns></returns>
        public static float getDist(RectangleF r, PointF p, PointF tan)
        {
            float dx, dy;
            if (tan.X >= 0)
                dx = (r.Right - p.X) / tan.X;
            else
                dx = (r.Left - p.X) / tan.X;
            if (tan.Y >= 0)
                dy = (r.Bottom - p.Y) / tan.Y;
            else
                dy = (r.Top - p.Y) / tan.Y;
            return Math.Min(dx, dy);
        }
        /// <summary>
        /// eine kreislinie von winkel alpha*2*Pi bis beta*2*Pi
        /// <para>beachte: winkel 0 ist rechts und das ding dreht sich im uhrzeigersinn (pi/2 ist unten also (1,0))</para>
        /// <para>die normal schaut nach außen</para>
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        public static orientierbarerWeg kreisbogen(float radius, float alpha, float beta)
        {
            float delta = beta - alpha;
            Weg y = t =>
            {
                float omega = alpha + t * delta;
                return new PointF(radius * cos(omega), radius * sin(omega));
            };
            Weg n = t =>
            {
                float omega = alpha + t * delta;
                return new PointF(cos(omega), sin(omega));
            };
            return new orientierbarerWeg(y, n, Math.Abs(delta * Tau * radius));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="p">Mittelpunkt der Sphäre</param>
        /// <param name="radius"></param>
        /// <param name="burst"></param>
        /// <param name="strings">strings kleiner gleich 100; samples = 100</param>
        /// <param name="layers"></param>
        public static unsafe void chaosSphere(Graphics g, PointF p, float radius, float burst, int strings, Brush[] layers)
        {
            int samples = 100;
            int nLayers = layers.Length + 1;

            int ssamp = samples + 1;
            float fSamples = (float)samples;
            PointF[] vektoren = new PointF[samples];
            PointF[] punkte02 = new PointF[2 * ssamp];
            PointF[] punkte13 = new PointF[2 * ssamp];
            PointF[] ToDraw;
            float[] hohen = new float[strings];
            Hohe h;
            float hohe;

            for (int i = 0; i < samples; i++)
                vektoren[i] = new PointF((float)Math.Cos(i * 2 * Math.PI / samples), (float)Math.Sin(i * 2 * Math.PI / samples));

            #region Punkte 2 und 3
            for (int j = 0; j < samples; j++)
                punkte02[j] = p;
            punkte02[samples] = punkte02[0];
            for (int j = 0; j < strings; j++)
                hohen[j] = (float)(burst * (1 - 2 * dice.NextDouble())) + radius / nLayers;
            h = Shadex.linearApprox(hohen);
            for (int j = 0; j < samples; j++)
            {
                punkte13[j] = p;
                hohe = h(j / fSamples);
                punkte13[j].X += vektoren[j].X * hohe;
                punkte13[j].Y += vektoren[j].Y * hohe;
            }
            punkte13[samples] = punkte13[0];
            #endregion
            fixed (PointF* P02 = &punkte02[0], P13 = &punkte13[0])
            {
                for (int i = 2; i < nLayers; i++)
                {
                    for (int j = 0; j < strings; j++)
                        hohen[j] = (float)(burst * (1 - 2 * dice.NextDouble())) + radius * i / nLayers;
                    h = Shadex.linearApprox(hohen);
                    #region P zuweisen
                    PointF* P;
                    switch (i % 4)
                    {
                        case 0:
                            P = P02;
                            ToDraw = punkte02;
                            break;
                        case 1:
                            P = P13;
                            ToDraw = punkte13;
                            break;
                        case 2:
                            P = P02 + ssamp;
                            ToDraw = punkte02;
                            break;
                        case 3:
                            P = P13 + ssamp;
                            ToDraw = punkte13;
                            break;
                        default:
                            P = null;
                            ToDraw = null;
                            break;
                    }
                    #endregion
                    for (int j = 0; j < samples; j++)
                    {
                        P[j] = p;
                        hohe = h(j / fSamples);
                        P[j].X += vektoren[j].X * hohe;
                        P[j].Y += vektoren[j].Y * hohe;
                    }
                    P[samples] = P[0];
                    g.FillPolygon(layers[i - 2], ToDraw);
                }
                {
                    int i = nLayers;
                    #region P zuweisen
                    PointF* P;
                    switch (i % 4)
                    {
                        case 0:
                            P = P02;
                            ToDraw = punkte02;
                            break;
                        case 1:
                            P = P13;
                            ToDraw = punkte13;
                            break;
                        case 2:
                            P = P02 + ssamp;
                            ToDraw = punkte02;
                            break;
                        case 3:
                            P = P13 + ssamp;
                            ToDraw = punkte13;
                            break;
                        default:
                            P = null;
                            ToDraw = null;
                            break;
                    }
                    #endregion
                    for (int j = 0; j < samples; j++)
                    {
                        P[j] = p;
                        P[j].X += vektoren[j].X * radius;
                        P[j].Y += vektoren[j].Y * radius;
                    }
                    P[samples] = P[0];
                    g.FillPolygon(layers[i - 2], ToDraw);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="layers">layers.count = 2 + 4*n !</param>
        /// <param name="burst"></param>
        /// <param name="strings"></param>
        public static unsafe void knisterBack(Graphics g, RectangleF r, Brush[] layers, float burst, int strings)
        {
            float nenner = r.Width / (strings - 1);
            int sam2 = 2 * strings;
            PointF[] P02 = new PointF[sam2];
            PointF[] P13 = new PointF[sam2];
            for (int i = 0; i < strings; i++)
                P13[sam2 - 1 - i] = P02[sam2 - 1 - i] = P13[i] = P02[i] = new PointF(r.X + i * nenner, r.Y);
            float h;
            int n = (layers.Length - 2) / 4;
            float hNenner = r.Height / (layers.Length - 1);

            #region Induktionsanfang
            h = hNenner;
            for (int i = 0; i < strings; i++)
                P02[sam2 - 1 - i].Y = r.Y + h + (float)((1 - 2 * dice.NextDouble()) * burst);
            g.FillPolygon(layers[0], P02);
            #endregion
            #region Induktionsschritt (13), (20), (31), (02)
            fixed (PointF* p02 = P02, p13 = P13)
            {
                PointF* p20 = p02 + sam2 - 1, p31 = p13 + sam2 - 1;
                for (int N = 0; N < n; N++)
                {
                    h += hNenner;
                    for (int i = 0; i < strings; i++)
                    {
                        (p31 - i)->Y = r.Y + h + (float)((1 - 2 * dice.NextDouble()) * burst);
                        (p31 - i)->X = r.X + (float)(i + 1 - 2 * dice.NextDouble()) * nenner;
                    }
                    g.FillPolygon(layers[4 * N + 1], P13);

                    h += hNenner;
                    for (int i = 0; i < strings; i++)
                    {
                        (p02 + i)->Y = r.Y + h + (float)((1 - 2 * dice.NextDouble()) * burst);
                        (p02 + i)->X = r.X + (float)(i + 1 - 2 * dice.NextDouble()) * nenner;
                    }
                    g.FillPolygon(layers[4 * N + 2], P02);

                    h += hNenner;
                    for (int i = 0; i < strings; i++)
                    {
                        (p13 + i)->Y = r.Y + h + (float)((1 - 2 * dice.NextDouble()) * burst);
                        (p13 + i)->X = r.X + (float)(i + 1 - 2 * dice.NextDouble()) * nenner;
                    }
                    g.FillPolygon(layers[4 * N + 3], P13);

                    h += hNenner;
                    for (int i = 0; i < strings; i++)
                    {
                        (p20 - i)->Y = r.Y + h + (float)((1 - 2 * dice.NextDouble()) * burst);
                        (p20 - i)->X = r.X + (float)(i + 1 - 2 * dice.NextDouble()) * nenner;
                    }
                    g.FillPolygon(layers[4 * N + 4], P02);
                }
            }
            #endregion
            #region Induktionsschluss
            for (int i = 0; i < strings; i++)
                P13[sam2 - 1 - i].Y = r.Bottom;
            g.FillPolygon(layers[n * 4 + 1], P13);
            #endregion

        }
        /// <summary>
        /// layers.length = 2 mod 4
        /// </summary>
        /// <param name="g"></param>
        /// <param name="y"></param>
        /// <param name="layers"></param>
        /// <param name="burst"></param>
        /// <param name="strings"></param>
        /// <param name="hohe"></param>
        /// <param name="samples"></param>
        public static unsafe void chaosWeg(Graphics g, orientierbarerWeg y, Brush[] layers, float burst, int strings, Hohe hohe, int samples)
        {
            PointF[] p13 = new PointF[2 * samples];
            PointF[] p24 = new PointF[2 * samples];
            PointF[] n = new PointF[samples];
            PointF[] ladder = new PointF[samples];
            PointF[] norm = new PointF[samples];
            float[] werte = new float[strings];
            Hohe app;
            #region Vorrechnen
            float e, d = 1f / (samples - 1);
            for (int i = 0; i < samples; i++)
            {
                e = i * d;
                p13[i] = y.weg(e);
                norm[i] = y.normale(e);
                e = hohe(e);
                n[i].X = e * norm[i].X;
                n[i].Y = e * norm[i].Y;
                ladder[i].X = p13[i].X + 0.5f * n[i].X;
                ladder[i].Y = p13[i].Y + 0.5f * n[i].Y;
            }
            Mach ADD = () =>
            {
                for (int i = 0; i < samples; i++)
                {
                    ladder[i].X += n[i].X;
                    ladder[i].Y += n[i].Y;
                }
            };
            ModPolygon MOD = (p) =>
            {
                for (int i = 0; i < strings; i++)
                    werte[i] = (float)(burst * (1 - 2 * dice.NextDouble()));
                app = linearApprox(werte);
                for (int i = 0; i < samples; i++)
                {
                    e = i * d;
                    e = app(e);
                    p[i].X = norm[i].X * e + ladder[i].X;
                    p[i].Y = norm[i].Y * e + ladder[i].Y;
                }
            };
            ModPolygon MOD2 = (p) =>
            {
                for (int i = 0; i < strings; i++)
                    werte[i] = (float)(burst * (1 - 2 * dice.NextDouble()));
                app = linearApprox(werte);
                for (int i = 0; i < samples; i++)
                {
                    e = i * d;
                    e = app(e);
                    (p - i)->X = norm[i].X * e + ladder[i].X;
                    (p - i)->Y = norm[i].Y * e + ladder[i].Y;
                }
            };
            #endregion
            fixed (PointF* p1 = p13, p2 = p24)
            {
                PointF* p3 = p1 + 2 * samples - 1, p4 = p2 + 2 * samples - 1;

                #region Anfang
                MOD(p2);
                for (int i = 0; i < samples; i++)
                    *(p3 - i) = p2[i];
                g.FillPolygon(layers[0], p13);
                #endregion
                #region Schritt
                for (int i = 1; i < layers.Length - 1; )
                {

                    ADD();
                    MOD2(p3);
                    g.FillPolygon(layers[i++], p13);
                    ADD();
                    MOD2(p4);
                    g.FillPolygon(layers[i++], p24);
                    ADD();
                    MOD(p1);
                    g.FillPolygon(layers[i++], p13);
                    ADD();
                    MOD(p2);
                    g.FillPolygon(layers[i++], p24);
                }
                #endregion
                #region Ende
                for (int i = 0; i < samples; i++)
                    *(p3 - i) = ladder[i];
                g.FillPolygon(layers.Last(), p13);
                #endregion
            }
        }

        public static Rectangle spannAuf(Point A, Point B)
        {
            if (A.X > B.X)
            {
                if (A.Y > B.Y)
                    return new Rectangle(B.X, B.Y, A.X - B.X, A.Y - B.Y);
                else
                    return new Rectangle(B.X, A.Y, A.X - B.X, B.Y - A.Y);
            }
            else
            {
                if (A.Y > B.Y)
                    return new Rectangle(A.X, B.Y, B.X - A.X, A.Y - B.Y);
                else
                    return new Rectangle(A.X, A.Y, B.X - A.X, B.Y - A.Y);
            }
        }
        public static RectangleF scale(RectangleF A, float b)
        {
            return new RectangleF(A.X * b, A.Y * b, A.Width * b, A.Height * b);
        }

        public static Knoten getCyberPunkDraht(PointF[] Strecke, float burst, int linesLinks, int linesRechts, float breite, Pen stift)
        {
            return getCyberPunkDraht(Strecke, burst, linesLinks, linesRechts, breite, breite / (linesLinks + 1 + linesRechts) / 2, stift);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Strecke"></param>
        /// <param name="blockLange"></param>
        /// <param name="linesLinks">>= 0!</param>
        /// <param name="linesRechts">>= 0!</param>
        /// <param name="breite"></param>
        /// <param name="stift"></param>
        public static Knoten getCyberPunkDraht(PointF[] Strecke, float burst, int linesLinks, int linesRechts, float breite, float radius, Pen stift)
        {
            const float CHANCEA_KREIS = 0.10f;
            const float CHANCEB_LINKS = 0.20f + CHANCEA_KREIS;
            const float CHANCEC_RECHTS = 0.20f + CHANCEB_LINKS;
            //const float CHANCED_WEITER = 0.5f + CHANCEC_RECHTS;

            int n = linesLinks + 1 + linesRechts;
            breite /= n;
            bool[] offen = new bool[n];
            float[] streckenStucke = new float[Strecke.Length];
            SortedList<Knoten, int> sortedHohen = new SortedList<Knoten, int>(new knotenHohenVergleicher());
            Knoten wurzel = new Knoten(new PointF(0, 0), stift);
            offen[linesLinks] = true;
            sortedHohen.Add(wurzel, linesLinks);

            for (int i = 1; i < Strecke.Length; i++)
            {
                float X, Y;
                X = Strecke[i].X - Strecke[i - 1].X;
                Y = Strecke[i].Y - Strecke[i - 1].Y;
                streckenStucke[i] = (float)Math.Sqrt(X * X + Y * Y) + streckenStucke[i - 1];
            }
            for (int i = 1; i < Strecke.Length; i++)
            {
                while (true)
                {
                    KeyValuePair<Knoten, int> it = sortedHohen.First();
                    if (it.Key.Y >= streckenStucke[i])
                        break;
                    sortedHohen.RemoveAt(0);

                    float f = dice.NextFloat();

                    if ((f < CHANCEA_KREIS) && (it.Value != linesLinks))
                    {
                        it.Key.objekt = new Knoten.Kreis(it.Key.ort, radius, null, stift);
                        offen[it.Value] = false;
                    }
                    else if (f < CHANCEC_RECHTS)
                    {
                        float hohe = Math.Min(breite + it.Key.Y, streckenStucke[i]);
                        int branch = (f < CHANCEB_LINKS) ? it.Value - 1 : it.Value + 1;

                        if ((branch >= 0) && (branch <= linesLinks + linesRechts) && (!offen[branch]))
                        {
                            Knoten knotBranch = new Knoten(breite * (branch - linesLinks), hohe);
                            offen[branch] = true;
                            it.Key.addKante(stift, knotBranch);
                            knotBranch.addKante(stift, knotBranch);
                            Knoten knotMain = new Knoten(breite * (it.Value - linesLinks), hohe);
                            it.Key.addKante(stift, knotMain);
                            sortedHohen.Add(knotMain, it.Value);
                            sortedHohen.Add(knotBranch, branch);
                        }
                        else
                        {
                            Knoten knotMain = new Knoten(breite * (it.Value - linesLinks), hohe);
                            it.Key.addKante(stift, knotMain);
                            sortedHohen.Add(knotMain, it.Value);
                        }
                    }
                    else
                    {
                        float hohe = Math.Min(burst * dice.NextFloat() + it.Key.Y, streckenStucke[i]);
                        Knoten knotMain = new Knoten(breite * (it.Value - linesLinks), hohe);
                        it.Key.addKante(stift, knotMain);
                        sortedHohen.Add(knotMain, it.Value);
                    }
                }
            }
            foreach (var item in sortedHohen)
                if (item.Value != linesLinks)
                    item.Key.objekt = new Knoten.Kreis(item.Key.ort, radius, null, stift);

            Transformiere(Strecke, wurzel.getKnoten());

            return wurzel;
        }
        public static void Transformiere(PointF[] Punkte, List<Knoten> knoten)
        {
            int n = Punkte.Length - 1;
            float[] lange = new float[n];
            PointF[] weg = new PointF[n];
            PointF[] normale = new PointF[Punkte.Length];
            PointF W = new PointF();
            for (int i = 0; i < n; i++)
            {
                W.X = Punkte[i + 1].X - Punkte[i].X;
                W.Y = Punkte[i + 1].Y - Punkte[i].Y;
                lange[i] = (float)Math.Sqrt(W.X * W.X + W.Y * W.Y);
                weg[i] = W;
            }
            W.X = -weg[0].Y / lange[0];
            W.Y = weg[0].X / lange[0];
            normale[0] = W;
            for (int i = 1; i < n; i++)
            {
                W.X = -weg[i].Y / lange[i] - weg[i - 1].Y / lange[i - 1];
                W.Y = weg[i].X / lange[i] + weg[i - 1].X / lange[i - 1];
                float lambda = lange[i] * lange[i - 1] / (lange[i] * lange[i - 1] + (weg[i - 1].X * weg[i].X + weg[i - 1].Y * weg[i].Y));
                W.X *= lambda;
                W.Y *= lambda;
                normale[i] = W;
            }
            W.X = -weg[n - 1].Y / lange[n - 1];
            W.Y = weg[n - 1].X / lange[n - 1];
            normale[n] = W;

            foreach (Knoten knot in knoten)
            {
                int i;
                for (i = 0; i < n; i++)
                    if (knot.Y <= lange[i])
                        break;
                    else
                        knot.Y -= lange[i];
                i = Math.Min(i, n - 1);

                float f = knot.Y / lange[i];
                knot.ort = new PointF(Punkte[i].X + f * weg[i].X + knot.X * ((1 - f) * normale[i].X + f * normale[i + 1].X),
                                      Punkte[i].Y + f * weg[i].Y + knot.X * ((1 - f) * normale[i].Y + f * normale[i + 1].Y));
            }
        }

        public static void list(PointF[] poly)
        {
            StringBuilder sb = new StringBuilder(poly.Length * 10);
            foreach (PointF item in poly)
            {
                sb.AppendLine(item.X.ToString("F") + "; " + item.Y.ToString("F"));
            }
            System.IO.File.WriteAllText("list.txt", sb.ToString());
        }
        public static void show(params object[] objekte)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in objekte)
                sb.AppendLine(item.ToString());
            System.Windows.Forms.MessageBox.Show(sb.ToString());
        }

        /// <summary>
        /// x \in [0, 1]
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float NextFloat(this Random d)
        {
            return (float)d.NextDouble();
        }
        /// <summary>
        /// x \in [0, max]
        /// </summary>
        /// <param name="d"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float NextFloat(this Random d, float max)
        {
            return (float)d.NextDouble();
        }
    }
}
