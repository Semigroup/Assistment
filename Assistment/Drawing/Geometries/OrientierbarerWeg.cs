using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;

namespace Assistment.Drawing.Geometries
{
    public delegate PointF Weg(float t);
    public delegate float Hohe(float t);
    public delegate PointF Punkt(float x, float y);
    /// <summary>
    /// bildet I^2 auf R^2 ab
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public delegate T FlachenFunktion<T>(float u, float v);
    public delegate T RandFunktion<T>(float t);

    public unsafe delegate void AddierPolygone(PointF* p, PointF* q);
    public unsafe delegate void ModPolygon(PointF* p);
    public delegate void Mach();

    public class OrientierbarerWeg
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
        /// tangente = normale.y , -normale.x
        /// </summary>
        public Weg tangente { get { return t => normale(t).linksOrtho().mul(-1); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weg">sollte konstante ableitung besitzen
        /// <para>(damit alle Wegstücke gleich groß sind)</para></param>
        /// <param name="normale">normalenvektor muss auf wegpunkt drauf addiert werden
        /// <para>die normale muss normiert sein!</para></param>
        /// <param name="c">konstanter Wert der Norm der Ableitung</param>
        public OrientierbarerWeg(Weg weg, Weg normale, float L)
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
        public OrientierbarerWeg(PointF A, PointF B)
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
        public OrientierbarerWeg(PointF A, PointF dA, PointF B, PointF dB)
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
        public OrientierbarerWeg(Gerade A, Gerade B)
            : this(A.Aufpunkt, A.Richtungsvektor, B.Aufpunkt, B.Richtungsvektor)
        {

        }
        public OrientierbarerWeg(OrientierbarerWeg OrientierbarerWeg)
            : this(OrientierbarerWeg.weg, OrientierbarerWeg.normale, OrientierbarerWeg.L)
        {
        }

        /// <summary>
        /// f(u,v) = y(u) + n(u) * h(v)
        /// </summary>
        /// <param name="hohe"></param>
        /// <returns></returns>
        public FlachenFunktion<PointF> getFlache(Hohe hohe)
        {
            return (u, v) => weg(u).add(normale(u).mul(hohe(v)));
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
        /// <summary>
        /// result[i] = weg((t1 - t0)*i/(samples-1) + t0)
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="t0"></param>
        /// <param name="t1"></param>
        /// <returns></returns>
        public PointF[] getPolygon(int samples, float t0, float t1)
        {
            PointF[] P = new PointF[samples];
            float d = (t1 - t0) / (samples - 1);
            for (int i = 0; i < samples; i++)
                P[i] = weg(i * d + t0);
            return P;
        }
        /// <summary>
        /// result[i] = normale(t) * hohe(t)
        /// <para>t = t0 + i*(t1-t0)/(samples-1)</para>
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="t0"></param>
        /// <param name="t1"></param>
        /// <param name="hohe"></param>
        /// <returns></returns>
        public PointF[] getNormalenPolygon(int samples, float t0, float t1, Hohe hohe)
        {
            PointF[] P = new PointF[samples];
            float d = (t1 - t0) / (samples - 1);
            for (int i = 0; i < samples; i++)
            {
                float t = d * i + t0;
                P[i] = normale(t).mul(hohe(t));
            }
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
        /// <summary>
        /// gibt y|[a,b] = y(a + (b-a) * t) zurück
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public OrientierbarerWeg Trim(float a, float b)
        {
            float d = b - a;
            return new OrientierbarerWeg(t => weg(a + d * t),
                t => normale(a + d * t),
                L * Math.Abs(d));
        }
        /// <summary>
        /// return t => this(t => (t+a)%1)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public OrientierbarerWeg Shift(float a)
        {
            return new OrientierbarerWeg(t => weg((t + a) % 1), t => normale((t + a) % 1), L);
        }

        private static float normSqared(PointF p)
        {
            return p.X * p.X + p.Y * p.Y;
        }
        private static PointF sub(PointF p, PointF q)
        {
            return new PointF(p.X - q.X, p.Y - q.Y);
        }
        private static PointF add(PointF p, PointF q)
        {
            return new PointF(p.X + q.X, p.Y + q.Y);
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

        public static OrientierbarerWeg operator *(OrientierbarerWeg gamma1, OrientierbarerWeg gamma2)
        {
            float L = gamma1.L + gamma2.L;
            float m = gamma1.L / (gamma1.L + gamma2.L);
            float m2 = gamma2.L / (gamma1.L + gamma2.L);
            return new OrientierbarerWeg(t =>
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
        public static OrientierbarerWeg operator *(OrientierbarerWeg Gamma, float Scaling)
        {
            return new OrientierbarerWeg(t => Gamma.weg(t).mul(Scaling), Gamma.normale, Gamma.L * Scaling);
        }
        public static OrientierbarerWeg operator ^(OrientierbarerWeg gamma, int n)
        {
            if (n == 0)
                return new OrientierbarerWeg(t => gamma.weg(0), t => gamma.normale(0), 0);
            else if (n > 0)
            {
                OrientierbarerWeg y = new OrientierbarerWeg(gamma);
                for (int i = 1; i < n; i++)
                    y *= (gamma + y.weg(1));
                //Hier könnte man einen intelligenteren Multiplikationsalgorithmus mit zweier Potenzen benutzen...
                return y;
            }
            else
            {
                OrientierbarerWeg y = new OrientierbarerWeg(t => gamma.weg(1 - t), t => gamma.normale(1 - t), gamma.L);
                return y ^ (-n);
            }
        }
        public static OrientierbarerWeg operator +(OrientierbarerWeg gamma, PointF point)
        {
            return new OrientierbarerWeg(add(gamma.weg, point), gamma.normale, gamma.L);
        }
        public static OrientierbarerWeg operator -(OrientierbarerWeg gamma, PointF point)
        {
            return gamma + point.mul(-1);
        }
        /// <summary>
        /// Dreht den Weg gegen den Uhrzeigersinn, Winkel in Bogenmaß
        /// </summary>
        /// <param name="gamma"></param>
        /// <param name="Winkel"></param>
        /// <returns></returns>
        public static OrientierbarerWeg operator ^(OrientierbarerWeg gamma, double Winkel)
        {
            return new OrientierbarerWeg(gamma.weg.rot(Winkel), gamma.normale.rot(Winkel), gamma.L);
        }

        /// <summary>
        /// 0,5 mal Pi
        /// </summary>
        protected const float Rho = (float)(0.5 * Math.PI);
        /// <summary>
        /// 1 mal Pi
        /// </summary>
        protected const float Pi = (float)Math.PI;
        /// <summary>
        /// 1,5 mal Pi
        /// </summary>
        protected const float Phi = (float)(3 * Math.PI / 2);
        /// <summary>
        /// 2 mal Pi
        /// </summary>
        protected const float Tau = (float)(2 * Math.PI);

        /// <summary>
        /// von 0 bis 1 (nicht 0 bis 2 Pi)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static float cos(double t)
        {
            return (float)Math.Cos(t * Tau);
        }
        /// <summary>
        /// von 0 bis 1 (nicht 0 bis 2 Pi)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static float sin(double t)
        {
            return (float)Math.Sin(t * Tau);
        }

        /// <summary>
        /// gibt an, ob das Polygon geschlossen ist
        /// </summary>
        /// <param name="punkte"></param>
        /// <returns></returns>
        private static bool closed(PointF[] polygon)
        {
            PointF d = sub(polygon.Last(), polygon.First());
            return normSqared(d) < 1;
        }

        /// <summary>
        /// gibt A * Brücke * B zurück
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public OrientierbarerWeg ConcatGlatt(OrientierbarerWeg B)
        {
            PointF a = this.weg(1);
            PointF b = B.weg(0);
            float d = a.dist(b);
            OrientierbarerWeg ow = new OrientierbarerWeg(a, this.tangente(1).mul(d), b, B.tangente(0).mul(d));
            return this * ow * B;
        }
        /// <summary>
        /// gibt A * Brücke * B zurück
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public OrientierbarerWeg ConcatLinear(OrientierbarerWeg B)
        {
            PointF a = this.weg(1);
            PointF b = B.weg(0);
            OrientierbarerWeg ow = new OrientierbarerWeg(a, b);
            return this * ow * B;
        }
        /// <summary>
        /// Macht praktisch ConcatGlatt(this)
        /// </summary>
        /// <returns></returns>
        public OrientierbarerWeg Abschluss()
        {
            PointF a = this.weg(1);
            PointF b = this.weg(0);
            float d = a.dist(b);
            OrientierbarerWeg ow = new OrientierbarerWeg(a, this.tangente(1).mul(d), b, this.tangente(0).mul(d));
            return this * ow;
        }
        /// <summary>
        /// Macht praktisch ConcatLinear(this)
        /// </summary>
        /// <returns></returns>
        public OrientierbarerWeg AbschlussLinear()
        {
            PointF a = this.weg(1);
            PointF b = this.weg(0);
            OrientierbarerWeg ow = new OrientierbarerWeg(a, b);
            return this * ow;
        }

        public OrientierbarerWeg Spiegel(Gerade Gerade)
        {
            PointF V = Gerade.Richtungsvektor.normalize();

            return new OrientierbarerWeg(t =>
            {
                PointF X = weg(t);
                PointF Lot = Gerade.Lot(X);
                return Lot.mul(2).sub(X);
            }, t =>
            {
                PointF N = normale(t);
                float a = N.SKP(V);
                return N.add(V.mul(-2));
            }, L);
        }

        /// <summary>
        /// Gibt im Wesentlichen new OW(punkte[0], punkte[1]) * new OW(punkte[1], punkte[2]) * ... zurück 
        /// </summary>
        /// <param name="punkte">mindestens zwei Punkte!</param>
        /// <returns></returns>
        public static OrientierbarerWeg HartPolygon(params PointF[] punkte)
        {
            OrientierbarerWeg ow = new OrientierbarerWeg(punkte[0], punkte[1]);
            for (int i = 2; i < punkte.Length; i++)
                ow *= new OrientierbarerWeg(punkte[i - 1], punkte[i]);
            return ow;
        }
        /// <summary>
        /// approximiert polynomiell ein Polygon
        /// <para>normale zeigt links der ersten Kante</para>
        /// </summary>
        /// <param name="punkte"></param>
        /// <returns></returns>
        public static OrientierbarerWeg ApproxPolygon(params PointF[] punkte)
        {
            return ApproxPolygon(punkte, 0.5f);
        }
        /// <summary>
        /// approximiert polynomiell ein Polygon
        /// <para>normale zeigt links der ersten Kante</para>
        /// </summary>
        /// <param name="punkte"></param>
        /// <returns></returns>
        public static OrientierbarerWeg ApproxPolygon(PointF[] punkte, float gewicht)
        {
            int n = punkte.Length;
            bool geschlossen = closed(punkte);
            if (n < 2)
                throw new NotSupportedException();

            PointF[] tangenten = new PointF[n];
            if (geschlossen)
                tangenten[n - 1] = tangenten[0] = punkte[1].sub(punkte[n - 2]).mul(gewicht);
            else
            {
                tangenten[0] = punkte[1].sub(punkte[0]).mul(gewicht * 2);
                tangenten[n - 1] = punkte[n - 1].sub(punkte[n - 2]).mul(gewicht * 2);
            }
            for (int i = 1; i < n - 1; i++)
                tangenten[i] = punkte[i + 1].sub(punkte[i - 1]).mul(gewicht);

            OrientierbarerWeg y = new OrientierbarerWeg(punkte[0], tangenten[0], punkte[1], tangenten[1]);

            for (int i = 1; i < n - 1; i++)
                y *= new OrientierbarerWeg(punkte[i], tangenten[i], punkte[i + 1], tangenten[i + 1]);

            return y;
        }
        /// <summary>
        /// approximiert passbar ein Polygon
        /// <para>normale zeigt links der ersten Kante</para>
        /// </summary>
        /// <param name="punkte"></param>
        /// <returns></returns>
        public static OrientierbarerWeg PassPolygon(params PointF[] punkte)
        {
            int n = punkte.Length;
            bool geschlossen = closed(punkte);
            if (n < 2)
                throw new NotSupportedException();

            PointF[] tangenten = new PointF[n];
            for (int i = 0; i < n - 1; i++)
                tangenten[i] = punkte[i + 1].sub(punkte[i]);
            if (geschlossen)
                tangenten[n - 1] = punkte[1].sub(punkte[n - 1]);
            else
                tangenten[n - 1] = tangenten[n - 2];

            OrientierbarerWeg y;
            if (geschlossen)
                y = PassKante(tangenten[n - 2], punkte.First(), punkte[1], tangenten[1]);
            else
                y = PassKante(tangenten.First(), punkte.First(), punkte[1], tangenten[1]);

            for (int i = 1; i < n - 1; i++)
                y *= PassKante(tangenten[i - 1], punkte[i], punkte[i + 1], tangenten[i + 1]);

            return y;
        }
        /// <summary>
        /// normale zeigt links von A -> B
        /// </summary>
        /// <param name="eingehendeTangente"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="ausgehendeTangente"></param>
        /// <returns></returns>
        public static OrientierbarerWeg PassKante(PointF eingehendeTangente, PointF A, PointF B, PointF ausgehendeTangente)
        {
            PointF n = B.sub(A).linksOrtho();

            PointF nA = n.add(eingehendeTangente.linksOrtho());
            PointF nB = n.add(ausgehendeTangente.linksOrtho());

            OrientierbarerWeg y = new OrientierbarerWeg(A, B);
            y.normale = t => nA.tween(nB, t).normalize();
            return y;
        }

        /// <summary>
        /// Normale zeigt nach Außen
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static OrientierbarerWeg Rechteck(float x, float y, float width, float height)
        {
            return Rechteck(new RectangleF(x, y, width, height));
        }
        /// <summary>
        /// Normale zeigt nach Außen
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static OrientierbarerWeg Rechteck(RectangleF r)
        {
            PointF OL = r.Location;
            PointF OR = new PointF(r.Right, r.Top);
            PointF UL = new PointF(r.Left, r.Bottom);
            PointF UR = new PointF(r.Right, r.Bottom);
            return new OrientierbarerWeg(OL, OR) * new OrientierbarerWeg(OR, UR) * new OrientierbarerWeg(UR, UL) * new OrientierbarerWeg(UL, OL);
        }
        public static OrientierbarerWeg RundesRechteck(RectangleF r, float radius)
        {
            OrientierbarerWeg ecke1, ecke2, ecke3, ecke4;
            OrientierbarerWeg rand1, rand2, rand3, rand4;

            float bm = Rho * radius;
            ecke1 = new OrientierbarerWeg(t => new PointF(r.X + radius + radius * cos(t / 4 + 0.25f), r.Y + radius - radius * sin(t / 4 + 0.25f)),
                t => new PointF(cos(t / 4 + 0.25f), -sin(t / 4 + 0.25f)),
             bm);
            ecke2 = new OrientierbarerWeg(t => new PointF(r.X + radius + radius * cos(t / 4 + 0.5f), r.Bottom - radius - radius * sin(t / 4 + 0.5f)),
                t => new PointF(cos(t / 4 + 0.5f), -sin(t / 4 + 0.5f)),
             bm);
            ecke3 = new OrientierbarerWeg(t => new PointF(r.Right - radius + radius * cos(t / 4 + 0.75f), r.Bottom - radius - radius * sin(t / 4 + 0.75f)),
                t => new PointF(cos(t / 4 + 0.75f), -sin(t / 4 + 0.75f)),
             bm);
            ecke4 = new OrientierbarerWeg(t => new PointF(r.Right - radius + radius * cos(t / 4), r.Y + radius - radius * sin(t / 4)),
                t => new PointF(cos(t / 4), -sin(t / 4)),
             bm);

            float Lv = r.Height - 2 * radius;
            float Lh = r.Width - 2 * radius;
            rand1 = new OrientierbarerWeg(t => new PointF(r.X, Lv * t + radius + r.Y),
                t => new PointF(-1, 0),
                Lv);
            rand2 = new OrientierbarerWeg(t => new PointF(Lh * t + radius + r.X, r.Bottom),
                t => new PointF(0, 1),
                Lh);
            rand3 = new OrientierbarerWeg(t => new PointF(r.Right, r.Bottom - radius - t * Lv),
                t => new PointF(1, 0),
                Lv);
            rand4 = new OrientierbarerWeg(t => new PointF(r.Right - radius - t * Lh, r.Y),
                t => new PointF(0, -1),
                Lh);

            return ((ecke1 * rand1) * (ecke2 * rand2)) * ((ecke3 * rand3) * (ecke4 * rand4));
        }
        //public static OrientierbarerWeg InnenRundesRechteck(RectangleF r, float radius)
        //{
        //    r.X += radius;
        //    r.Y += radius;
        //    float ab = 2 * radius;
        //    r.Width -= ab;
        //    r.Height -= ab;
        //    return RundesRechteck(r, radius);
        ////}
        public static OrientierbarerWeg RundesRechteck(RectangleF r)
        {
            return RundesRechteck(r, Math.Min(r.Width, r.Height) / 2);
        }
        public static OrientierbarerWeg Ellipse(RectangleF r)
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
            return new OrientierbarerWeg(y, n, Rho * (r.Width + r.Height));
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
        public static OrientierbarerWeg kreisbogen(float radius, double alpha, double beta)
        {
            double delta = beta - alpha;
            Weg y = t =>
            {
                double omega = alpha + t * delta;
                return new PointF(radius * cos(omega), radius * sin(omega));
            };
            Weg n = t =>
            {
                double omega = alpha + t * delta;
                return new PointF(cos(omega), sin(omega));
            };
            return new OrientierbarerWeg(y, n, (float)Math.Abs(delta * Tau * radius));
        }

        /// <summary>
        /// Nicht nach Bogenlänge parametrisiert. Normale und Länge werden billig approximiert.
        /// </summary>
        /// <param name="Radius"></param>
        /// <param name="Windungen"></param>
        /// <returns></returns>
        public static OrientierbarerWeg Spirale(float Radius, int Windungen)
        {
            Weg y = t => new PointF(sin(Windungen * t), cos(Windungen * t)).mul(Radius * (1 - t));
            Weg n = t => new PointF(sin(Windungen * t), cos(Windungen * t));//Ganz billige Approximation
            double w = Windungen * 2 * Math.PI;
            double d = Math.Sqrt(w * w + 1);
            double L = (d + Math.Log(d + w) / w); //exakte Berechnung
            L *= Radius;

            return new OrientierbarerWeg(y, n, (float)L);
        }

        public static OrientierbarerWeg Triskele(float Radius, int Windungen, float Dicke)
        {
            OrientierbarerWeg Spirale = OrientierbarerWeg.Spirale(Radius, Windungen);
            OrientierbarerWeg KleineSpirale = OrientierbarerWeg.Spirale(Radius - Dicke, Windungen).Trim(1, 0);
            Spirale.invertier();

            float a = Radius * 2;
            PointF Unten = new PointF(0, (float)(a / Math.Sqrt(3)));
            PointF Links = new PointF(-a / 2, -(float)(a / Math.Sqrt(12)));
            PointF Rechts = new PointF(a / 2, -(float)(a / Math.Sqrt(12)));

            OrientierbarerWeg Spirale1 = (Spirale ^ (Math.PI * 7 / 6)) + Unten;
            OrientierbarerWeg KleineSpirale1 = (KleineSpirale ^ (Math.PI * 7 / 6)) + Unten;
            OrientierbarerWeg Spirale2 = (Spirale ^ (Math.PI * 1 / 2)) + Links;
            OrientierbarerWeg KleineSpirale2 = (KleineSpirale ^ (Math.PI * 1 / 2)) + Links;
            OrientierbarerWeg Spirale3 = (Spirale ^ (Math.PI * 11 / 6)) + Rechts;
            OrientierbarerWeg KleineSpirale3 = (KleineSpirale ^ (Math.PI * 11 / 6)) + Rechts;

            PointF Normale1 = new PointF(0, -Radius);
            PointF Normale2 = Normale1.rot(2 * Math.PI * 2 / 3);
            PointF Normale3 = Normale2.rot(2 * Math.PI * 2 / 3);


            //OrientierbarerWeg Brucke12 = new OrientierbarerWeg(Unten.add(Links).mul(0.5f), Normale2, Rechts.add(Links).mul(0.5f).add(-Dicke, 0), Normale1.mul(-1));
            //PointF rechteKante = Rechts.sub(Unten).normalize();
            //OrientierbarerWeg Brucke23 = new OrientierbarerWeg(Rechts.add(Links).mul(0.5f), Normale1, Rechts.add(Unten).mul(0.5f).saxpy(Dicke, rechteKante), Normale3.mul(-1));
            //PointF linkeKante = Links.sub(Unten).normalize();
            //OrientierbarerWeg Brucke31 = new OrientierbarerWeg(Rechts.add(Unten).mul(0.5f), Normale3, Links.add(Unten).mul(0.5f).saxpy(-Dicke, linkeKante), Normale2.mul(-1));

            //OrientierbarerWeg end = Spirale1 * KleineSpirale1 * Brucke12 * Spirale2 * KleineSpirale2 * Brucke23 * Spirale3 * KleineSpirale3 * Brucke31;

            OrientierbarerWeg end = (Spirale1 * KleineSpirale1).ConcatGlatt(Spirale3 * KleineSpirale3).ConcatGlatt(Spirale2 * KleineSpirale2);
            end = end.Abschluss().Shift(1f / (Windungen * 15));

            return end;
        }
        public static OrientierbarerWeg Pike(float Hohe)
        {
            //Hohe = 1000;
            float a = Hohe / 3;
            float d = a * 2;
            OrientierbarerWeg ow = OrientierbarerWeg.HartPolygon(new PointF(Hohe / 2, 0), new PointF(a, 0), new PointF(Hohe / 2, a));
            ow *= new OrientierbarerWeg(new PointF(Hohe / 2, a), new PointF(-d, -d), new PointF(0, Hohe / 2), new PointF(0, d));
            ow *= new OrientierbarerWeg(new PointF(0, Hohe / 2), new PointF(0, d), new PointF(Hohe / 2, Hohe), new PointF(0, d));

            ow *= new OrientierbarerWeg(new PointF(Hohe / 2, Hohe), new PointF(0, -d), new PointF(Hohe, Hohe / 2), new PointF(0, -d));
            ow *= new OrientierbarerWeg(new PointF(Hohe, Hohe / 2), new PointF(0, -d), new PointF(Hohe / 2, a), new PointF(-d, d));
            ow *= OrientierbarerWeg.HartPolygon(new PointF(Hohe / 2, a), new PointF(2 * a, 0), new PointF(Hohe / 2, 0));


            //ow *= new OrientierbarerWeg(new PointF(Hohe / 2, a), new PointF(-d, -d), new PointF(Hohe / 2, Hohe), new PointF(0, d));
            //ow *= new OrientierbarerWeg(new PointF(Hohe / 2, Hohe), new PointF(0, -Hohe), new PointF(Hohe / 2, a), new PointF(a / 2, a));
            ow.invertier();
            //ow.print(1000, 1000, 100);
            //System.Windows.Forms.MessageBox.Show("Test");

            return ow;
        }

        public static FlachenFunktion<PointF> Fortsetzen(OrientierbarerWeg Basis, OrientierbarerWeg Fortsetzung)
        {
            PointF d = Fortsetzung.normale(0);
            double Alpha = d.atan();

            return (u, v) =>
                {
                    PointF normB = Basis.normale(u);
                    return Basis.weg(u).add(Fortsetzung.weg(v).rot(normB.atan() - Alpha + Math.PI / 2));
                };
        }
    }
}
