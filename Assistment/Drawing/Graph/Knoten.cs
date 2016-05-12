using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Assistment.Drawing.Graph
{
    public struct Kante
    {
        /// <summary>
        /// endknoten
        /// </summary>
        public Knoten ziel;
        public Pen stift;

        public Kante(Knoten ziel, Pen stift)
        {
            this.ziel = ziel;
            this.stift = stift;
        }
    }

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

    public class Knoten : IEnumerable<Knoten>, IEnumerable<Kante>, ICollection<Kante>
    {
        public int token;
        /// <summary>
        /// von diesem Knoten weiterführende Kanten
        /// </summary>
        protected List<Kante> kanten;
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
            : this(new Punkt(ort, rand))
        {
        }
        public Knoten(ZeichenObjekt objekt)
        {
            this.kanten = new List<Kante>();
            this.token = 0;
            this.objekt = objekt;
        }

        public void drawGraph(Graphics g)
        {
            foreach (Knoten knoten in this)
            {
                knoten.drawKanten(g);
                knoten.draw(g);
            }
        }
        public void drawGraph(Bild b)
        {
            foreach (Knoten knoten in this)
            {
                knoten.drawKanten(b);
                knoten.draw(b);
            }
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
        /// <summary>
        /// zeichnet diesen Knoten
        /// </summary>
        /// <param name="g"></param>
        public void draw(Graphics g)
        {
            objekt.draw(g);
        }
        /// <summary>
        /// zeichnet diesen Knoten
        /// </summary>
        /// <param name="b"></param>
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

        public void Add(Kante item)
        {
            kanten.Add(item);
        }
        public void Add(Pen stift, Knoten ziel)
        {
            Add(new Kante(ziel, stift));
        }
        public void Clear()
        {
            kanten.Clear();
        }
        public bool Contains(Kante item)
        {
            return kanten.Contains(item);
        }
        public void CopyTo(Kante[] array, int arrayIndex)
        {
            kanten.CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get { return kanten.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public bool Remove(Kante item)
        {
            return kanten.Remove(item);
        }

        public IEnumerator<Knoten> GetEnumerator()
        {
            return new Traversor(this, true);
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        IEnumerator<Kante> IEnumerable<Kante>.GetEnumerator()
        {
            return new Traversor(this, false);
        }

        /// <summary>
        /// traversiert den Graphen via Breitensuche
        /// <para>Auf einem Graphen dürfen keine zwei Operationen gleichzeitig traversieren</para>
        /// <para>(führt auf Grund der Token zu Endlosschleifen)</para>
        /// </summary>
        protected class Traversor : IEnumerator<Knoten>, IEnumerator<Kante>
        {
            private static int TOKENS = 0;
            private Queue<Knoten> knotenListe = new Queue<Knoten>();
            private IEnumerator<Kante> kanten;
            private int token;
            private Knoten root;
            private bool inklusiveRoot;

            /// <summary>
            /// falls root mit aufgezählt werden soll, muss inklusiveRoot geflaggt sein
            /// </summary>
            /// <param name="root"></param>
            /// <param name="inklusiveRoot"></param>
            public Traversor(Knoten root, bool inklusiveRoot)
            {
                this.root = root;
                this.inklusiveRoot = inklusiveRoot;
                Reset();
            }

            private void setToken()
            {
                token = ++TOKENS;
            }
            public Knoten Current
            {
                get { return kanten.Current.ziel; }
            }
            public void Dispose()
            {
                kanten.Dispose();
            }
            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }
            public bool MoveNext()
            {
                if (kanten.MoveNext())
                {
                    if (kanten.Current.ziel.token == token)
                        return MoveNext();
                    else
                    {
                        kanten.Current.ziel.token = token;
                        knotenListe.Enqueue(kanten.Current.ziel);
                        return true;
                    }
                }
                else if (knotenListe.Count > 0)
                {
                    kanten = knotenListe.Dequeue().kanten.GetEnumerator();
                    return MoveNext();
                }
                else
                    return false;
            }
            public void Reset()
            {
                setToken();
                knotenListe.Clear();
                List<Kante> kantenListe;
                if (inklusiveRoot)
                {
                    kantenListe = new List<Kante>();
                    kantenListe.Add(new Kante(root, null));
                }
                else
                    kantenListe = root.kanten;
                kanten = kantenListe.GetEnumerator();
            }
            Kante IEnumerator<Kante>.Current
            {
                get { return kanten.Current; }
            }
        }
    }
}
