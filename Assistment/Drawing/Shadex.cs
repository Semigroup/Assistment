using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Assistment.Drawing.Graph;
using System.Drawing.Drawing2D;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.Style;
using Assistment.Extensions;

namespace Assistment.Drawing
{
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
        public static void malBezierrand(Graphics g, Brush[] brushes, OrientierbarerWeg y, Hohe hohe, int samples, int stachel)
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
        public static void malBezierhulle(Graphics g, Brush[] brushes, OrientierbarerWeg y, Hohe hohe, int samples, int stachel)
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
        public static unsafe void chaosRect(Graphics g, RectangleF r, Schema schema)
        {
            chaosRect(g, r, schema.farben, schema.burst, schema.strings);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="layers">layers.count = 2 + 4*n !</param>
        /// <param name="burst"></param>
        /// <param name="strings"></param>
        public static unsafe void chaosRect(Graphics g, RectangleF r, Brush[] layers, float burst, int strings)
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
        public static unsafe void chaosWeg(Graphics g, OrientierbarerWeg y, Brush[] layers, float burst, int strings, Hohe hohe, int samples)
        {
            PointF[] p13 = new PointF[2 * samples];
            PointF[] p24 = new PointF[2 * samples];
            PointF[] n = new PointF[samples];
            PointF[] ladder = new PointF[samples];
            PointF[] norm = new PointF[samples];
            float[] werte = new float[strings];
            Hohe app;
            int layersLength = layers.Length + ((2 - layers.Length) % 4);
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
                for (int i = 1; i < layersLength - 1; )
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
                g.FillPolygon(layers[layersLength - 1], p13);
                #endregion
            }
        }
        public static unsafe void chaosWeg(Graphics g, OrientierbarerWeg y, Schema schema)
        {
            chaosWeg(g, y, schema.farben, schema.burst, schema.strings, schema.hohe, (int)(schema.sampleRate * y.L));
        }

        public static void ChaosFlache(Graphics g, FlachenSchema schema)
        {
            Polygon samples = new Polygon(2 * schema.Samples.X + 1);
            int n = 2 * schema.Samples.X - 1;
            float hohe = 1 / (schema.Samples.Y - 1f);
            for (int i = 0; i < schema.Samples.X; i++)
            {
                float t = i / (schema.Samples.X - 1f);
                samples[i] = new PointF(t, 0);
                samples[n - i] = new PointF(t, hohe);
            }
            samples.Close();

            g.FillPolygon(schema.Pinsel(0.5f, 0), samples.Map(schema.Flache));
            hohe = 1 - hohe;
            Polygon neu = samples + new PointF(0, hohe);
            g.FillPolygon(schema.Pinsel(0.5f, 1), neu.Map(schema.Flache));

            hohe = -1 / (schema.Samples.Y - 1f);
            for (int i = 0; i < schema.Samples.X; i++)
                samples[i] = samples[i].add(0, hohe);
            samples.Close();

            for (int i = 1; i < schema.Samples.Y - 1; i++)
            {
                hohe = i / (schema.Samples.Y - 1f);
                neu = samples + new PointF(0, hohe);
                g.FillPolygon(schema.Pinsel(0.5f, hohe), neu.Map(schema.Flache));
            }
        }

        /// <summary>
        /// Überdeckt r gleichmäßig mit (x-1)*(y-1) Rechtecken, verzerrt deren Ecken anhand und füllt sie mit farben aus.
        /// </summary>
        /// <param name="G"></param>
        /// <param name="r"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="schema"></param>
        public static void zitterQuadrate(Graphics g, RectangleF r, int x, int y, Schema schema)
        {
            float dx = r.Width / (x - 1) / 2;
            float dy = r.Height / (x - 1) / 2;
            PointF[,] p = new PointF[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    p[i, j] = new PointF(i * r.Width / (x - 1) + dx * schema.NextCentered(),
                                         j * r.Height / (y - 1) + dy * schema.NextCentered());
            for (int i = 0; i < x - 1; i++)
                for (int j = 0; j < y - 1; j++)
                    g.FillPolygon(schema.farben[i + j], new PointF[] { p[i, j], p[i, j + 1], p[i + 1, j + 1], p[i + 1, j], p[i, j] });
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
                        it.Key.objekt = new Assistment.Drawing.Graph.Kreis(it.Key.ort, radius, null, stift);
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
                            it.Key.Add(stift, knotBranch);
                            knotBranch.Add(stift, knotBranch);
                            Knoten knotMain = new Knoten(breite * (it.Value - linesLinks), hohe);
                            it.Key.Add(stift, knotMain);
                            sortedHohen.Add(knotMain, it.Value);
                            sortedHohen.Add(knotBranch, branch);
                        }
                        else
                        {
                            Knoten knotMain = new Knoten(breite * (it.Value - linesLinks), hohe);
                            it.Key.Add(stift, knotMain);
                            sortedHohen.Add(knotMain, it.Value);
                        }
                    }
                    else
                    {
                        float hohe = Math.Min(burst * dice.NextFloat() + it.Key.Y, streckenStucke[i]);
                        Knoten knotMain = new Knoten(breite * (it.Value - linesLinks), hohe);
                        it.Key.Add(stift, knotMain);
                        sortedHohen.Add(knotMain, it.Value);
                    }
                }
            }
            foreach (var item in sortedHohen)
                if (item.Value != linesLinks)
                    item.Key.objekt = new Assistment.Drawing.Graph.Kreis(item.Key.ort, radius, null, stift);

            Transformiere(Strecke, wurzel);

            return wurzel;
        }
        public static void Transformiere(PointF[] Punkte, IEnumerable<Knoten> knoten)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="weg"></param>
        /// <param name="wegFarbe"></param>
        /// <param name="schattenFarbe"></param>
        /// <param name="lichtRichtung">muss normiert sein</param>
        /// <param name="samples"></param>
        /// <param name="hohe"></param>
        public static void malSchatten(Graphics g, OrientierbarerWeg weg, Color wegFarbe, Color schattenFarbe, PointF lichtRichtung, int samples, Hohe hohe)
        {
            PointF[] punkte = weg.getPolygon(samples, 0, 1);
            PointF[] normale = weg.getNormalenPolygon(samples, 0, 1, hohe);
            PointF[] punkteOff = new PointF[samples];

            for (int i = 0; i < samples; i++)
            {
                float skp = lichtRichtung.SKP(normale[i]);
                if (skp < 0)
                {
                    skp *= -1;
                    normale[i] = normale[i].mul(-1);
                }
                punkteOff[i] = punkte[i].saxpy(skp, lichtRichtung);
            }

            for (int i = 0; i < samples - 1; i++)
            {
                LinearGradientBrush brush = new LinearGradientBrush(punkte[i], punkte[i].add(normale[i]), wegFarbe, schattenFarbe);
                //brush.Transform = new Matrix();
                //brush.Transform.Shear(lichtRichtung.X * 100, lichtRichtung.Y * 100);
                PointF[] poly = { punkte[i], punkte[i + 1], punkteOff[i + 1], punkteOff[i], punkte[i] };
                g.FillPolygon(brush, poly);
            }
        }
        public static void malWeg(Graphics g, OrientierbarerWeg weg, Pen stift, Brush pinsel, int samples)
        {
            PointF[] poly = weg.getPolygon(samples, 0, 1);
            if (pinsel != null)
                g.FillPolygon(pinsel, poly);
            if (stift != null)
                g.DrawPolygon(stift, poly);
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
        /// <summary>
        /// x \in [-1, 1]
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static float NextCenterd(this Random d)
        {
            return (float)(d.NextDouble() * 2 - 1);
        }
    }
}
