using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace Assistment.Drawing
{
    public static class CyberShadex
    {
        private static Random d = new Random();
        public const float TAU = (float)(Math.PI * 2);

        public class Bild
        {
            public Graphics g;
            public Bitmap b;
            public float scale;
            public PointF offset;

            public Bild(float width, float height)
            {
                b = new Bitmap((int)width, (int)height);
                g = Graphics.FromImage(b);
                scale = 1;
                offset = new PointF();
            }
            public Bild(float xOffset, float yOffset, float width, float height, float scale)
            {
                b = new Bitmap((int)(scale * (2 * xOffset + width)), (int)(scale * (yOffset * 2 + height)));
                this.scale = scale;
                this.offset = new PointF(xOffset, yOffset);
                g = Graphics.FromImage(b);
            }

            public void clear()
            {
                clear(Color.White);
            }
            public void clear(Color color)
            {
                g.Clear(color);
            }
            public void raiseGraphics()
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            }
            public void lowerGraphics()
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            }
            /// <summary>
            /// hängt automatisch .png an
            /// </summary>
            /// <param name="fileName"></param>
            public void save(string fileName)
            {
                b.Save(fileName + ".png");
            }
            public void save()
            {
                save("test");
            }
            public void DrawString(string s, Font font, Brush brush, PointF P)
            {
                g.DrawString(s, font, brush, align(P));
            }
            public void DrawLine(Pen pen, PointF pt1, PointF pt2)
            {
                pen.Width *= scale;
                g.DrawLine(pen, align(pt1), align(pt2));
                pen.Width /= scale;
            }
            public void DrawLines(Pen pen, PointF[] points)
            {
                pen.Width *= scale;
                PointF[] p = new PointF[points.Length];
                for (int i = 0; i < p.Length; i++)
                    p[i] = align(points[i]);
                g.DrawLines(pen, p);
                pen.Width /= scale;
            }
            public void DrawPolygon(Pen pen, PointF[] points)
            {
                pen.Width *= scale;
                PointF[] p = new PointF[points.Length];
                for (int i = 0; i < p.Length; i++)
                    p[i] = align(points[i]);
                g.DrawPolygon(pen, p);
                pen.Width /= scale;
            }
            public void DrawRectangle(Pen pen, RectangleF re)
            {
                pen.Width *= scale;
                g.DrawRectangle(pen, scale * (re.X + offset.X), scale * (re.Y + offset.Y), scale * re.Width, scale * re.Height);
                pen.Width /= scale;
            }
            public void DrawEllipse(Pen pen, RectangleF re)
            {
                pen.Width *= scale;
                g.DrawEllipse(pen, scale * (re.X + offset.X), scale * (re.Y + offset.Y), scale * re.Width, scale * re.Height);
                pen.Width /= scale;
            }

            public void FillEllipse(Brush brush, RectangleF re)
            {
                g.FillEllipse(brush, scale * (re.X + offset.X), scale * (re.Y + offset.Y), scale * re.Width, scale * re.Height);
            }

            public RectangleF align(RectangleF R)
            {
                return new RectangleF(align(R.Location), new SizeF(R.Width * scale, R.Height * scale));
            }
            public PointF align(PointF P)
            {
                return new PointF(scale * (P.X + offset.X), scale * (P.Y + offset.Y));
            }
            public PointF[] align(PointF[] polygon)
            {
                PointF[] p = new PointF[polygon.Length];
                for (int i = 0; i < polygon.Length; i++)
                    p[i] = align(polygon[i]);
                return p;
            }

            public void drawGrid(int zeilen, int spalten, Pen stift)
            {
                float d = b.Height / zeilen;
                for (float a = d; a < b.Height; a += d)
                    g.DrawLine(stift, 0, a, b.Width, a);
                d = b.Width / zeilen;
                for (float a = d; a < b.Width; a += d)
                    g.DrawLine(stift, a, 0, a, b.Height);
            }
            public void drawGrid(Brush[,] feld)
            {
                int zeilen = feld.GetLength(0);
                int spalten = feld.GetLength(1);
                RectangleF r = new RectangleF(0, 0, b.Width / spalten, b.Height / zeilen);
                for (int i = 0; i < zeilen; i++)
                {
                    for (int j = 0; j < spalten; j++)
                    {
                        g.FillRectangle(feld[i, j], r);
                        r.X += r.Width;
                    }
                    r.X = 0;
                    r.Y += r.Height;
                }
            }
        }
        public struct Kante
        {
            /// <summary>
            /// endknoten
            /// </summary>
            public Knoten ziel;
            public Pen stift;
        }
        public class Knoten
        {
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
            const float CHANCED_WEITER = 0.5f + CHANCEC_RECHTS;

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

                    float f = d.NextFloat();

                    if ((f < CHANCEA_KREIS) && (it.Value != linesLinks))
                    {
                        it.Key.objekt = new Kreis(it.Key.ort, radius, null, stift);
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
                        float hohe = Math.Min(burst * d.NextFloat() + it.Key.Y, streckenStucke[i]);
                        Knoten knotMain = new Knoten(breite * (it.Value - linesLinks), hohe);
                        it.Key.addKante(stift, knotMain);
                        sortedHohen.Add(knotMain, it.Value);
                    }
                }
            }
            foreach (var item in sortedHohen)
                if (item.Value != linesLinks)
                    item.Key.objekt = new Kreis(item.Key.ort, radius, null, stift);

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

        ///// <summary>
        ///// x \in [0, 1]
        ///// </summary>
        ///// <param name="d"></param>
        ///// <returns></returns>
        //public static float NextFloat(this Random d)
        //{
        //    return (float)d.NextDouble();
        //}
        ///// <summary>
        ///// x \in [0, max]
        ///// </summary>
        ///// <param name="d"></param>
        ///// <param name="max"></param>
        ///// <returns></returns>
        //public static float NextFloat(this Random d, float max)
        //{
        //    return (float)d.NextDouble();
        //}
    }
}
