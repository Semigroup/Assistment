using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Drawing.Geometries
{
    public static class Fragments
    {
        private static Random d = new Random();

        public enum Style
        {
            Gerade,
            Spitz,
            Wellen,
            Kreise,
            Sagezahn,
            _1_0_,
            _1_2_1_0_,
            Konig,
            Chaos,
            Kreuz,
            Triskelen,
            Pik,
            Blitz,
            ReissZahn,
            _1_Chaos_,
            Halbmond,
            _0_Rund_,
            _0_Dreieck_,
            Pentagon,
        }
        /// <summary>
        /// Ein Fragment ist ein Weg von (0,0) nach (width, 0)
        ///<para>mit einem Style, das bis zu (..., height) bzw. (..., - height) erreicht</para>
        ///<para>passt also alles in eine box (0, -height) - (width,  -height) - (width, height) - (0, height)</para>
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static OrientierbarerWeg GetFragment(Style style, float width, float height)
        {
            OrientierbarerWeg o1, o2, o3, o4, o5;
            switch (style)
            {
                case Style.Gerade:
                    o1 = new OrientierbarerWeg(new PointF(0, 0), new PointF(width, 0));
                    o1.Invertier();
                    return o1;

                case Style.Spitz:
                    o1 = new OrientierbarerWeg(width / 2, t => t * t * height, t => t * 2 * height);
                    o2 = new OrientierbarerWeg(width / 2, t => (t - 1) * (t - 1) * height,
                        t => 2 * ( t - 1) * height) + new PointF(width / 2, 0);
                    o1.Invertier();
                    o2.Invertier();
                    return o1 * o2;

                case Style.Wellen:
                    o1 = new OrientierbarerWeg(width,
                        t => (float)(height * Math.Sin(t * 2 * Math.PI)),
                        t => (float)(height * 2 * Math.PI * Math.Cos(t * 2 * Math.PI)));
                    o1.Invertier();
                    return o1;

                case Style.Kreise:
                    o1 = OrientierbarerWeg.EllipsenStuck(width / 4, height, Math.PI, 0) + new PointF(width / 4, 0);
                    o2 = OrientierbarerWeg.EllipsenStuck(width / 4, height, Math.PI, 2 * Math.PI) + new PointF(3 * width / 4, 0);
                    o2.Invertier();
                    return o1 * o2;

                case Style.Sagezahn:
                    o1 = OrientierbarerWeg.HartPolygon(new PointF(0, 0), new PointF(width, height), new PointF(width, 0));
                    o1.Invertier();
                    return o1;

                case Style._1_0_:
                    o1= OrientierbarerWeg.HartPolygon(
                        new PointF(0, 0),
                        new PointF(0, height),
                        new PointF(width / 2, height),
                        new PointF(width / 2, 0),
                        new PointF(width, 0));
                    o1.Invertier();
                    return o1;

                case Style._1_2_1_0_:
                    o2 = OrientierbarerWeg.HartPolygon(
                        new PointF(0, 0),
                        new PointF(0, height / 2),
                        new PointF(width / 4, height / 2),
                        new PointF(width / 4, height),
                        new PointF(width / 2, height),
                        new PointF(width / 2, height / 2),
                        new PointF(width * 3 / 4, height / 2),
                        new PointF(width * 3 / 4, 0),
                        new PointF(width, 0));
                    o2.Invertier();
                    return o2;

                case Style.Konig:
                    o1 = new OrientierbarerWeg(width / 4, t => t * t * height, t => t * 2 * height);
                    o2 = OrientierbarerWeg.HartPolygon(
                        new PointF(width / 4, height),
                        new PointF(width / 4, 0),
                        new PointF(width / 2, 0),
                        new PointF(width / 2, height));
                    o3 = new OrientierbarerWeg(width / 4, t => (t - 1) * (t - 1) * height,
                        t => 2 * (t - 1) * height) + new PointF(width / 2, 0);
                    o4 = OrientierbarerWeg.HartPolygon(
                      new PointF(3 * width / 4, 0),
                      new PointF(width, 0));
                    o1.Invertier();
                    o2.Invertier();
                    o3.Invertier();
                    o4.Invertier();
                    return o1 * o2 * o3 * o4;

                case Style.Chaos:
                    int numberOfChaosPoints = 20;
                    PointF[] points = new PointF[numberOfChaosPoints + 2];
                    for (int i = 1; i < numberOfChaosPoints + 1; i++)
                        points[i] = new PointF(i * width / (numberOfChaosPoints + 1), height * d.NextFloat());
                    points[0] = new PointF(0, 0);
                    points[numberOfChaosPoints + 1] = new PointF(width, 0);
                    o1 = OrientierbarerWeg.HartPolygon(points);
                    o1.Invertier();
                    return o1;

                case Style.Kreuz:
                    o1 = OrientierbarerWeg.HartPolygon(
                        new PointF(0, 0),
                        new PointF(width * 3 / 8, 0),
                        new PointF(width * 3 / 8, height / 3),
                        new PointF(width / 8, height / 3),
                        new PointF(width / 8, height * 2 / 3),
                        new PointF(width * 3 / 8, height * 2 / 3),
                        new PointF(width * 3 / 8, height),
                        new PointF(width * 5 / 8, height),
                        new PointF(width * 5 / 8, height * 2 / 3),
                        new PointF(width * 7 / 8, height * 2 / 3),
                        new PointF(width * 7 / 8, height / 3),
                        new PointF(width * 5 / 8, height / 3),
                        new PointF(width * 5 / 8, 0),
                        new PointF(width, 0)
                        );
                    o1.Invertier();
                    return o1;

                case Style.Triskelen:
                    float hw = Math.Min(height, width);
                    o1 = (OrientierbarerWeg.Triskele(hw / 4, 1, hw / 6).Trim(0.02f, 1) ^ Math.PI) + new PointF(width / 2, height / 2);
                    PointF T1 = o1.Weg(0);
                    PointF T2 = o1.Weg(1);
                    o2 = OrientierbarerWeg.HartPolygon(new PointF(), new PointF(T1.X, 0), T1);
                    o3 = OrientierbarerWeg.HartPolygon(T2, new PointF(T2.X, 0), new PointF(width, 0));
                    o1 = o2 * o1 * o3;
                    o1.Invertier();
                    return o1;

                case Style.Pik:
                    height = Math.Min(width, height);
                    o1 = OrientierbarerWeg.Pike(height).Trim(0.05f, 0.95f)+ new PointF((width - height)/2, 0);
                    o2 = OrientierbarerWeg.HartPolygon(new PointF(), o1.Weg(0));
                    o3 = OrientierbarerWeg.HartPolygon(o1.Weg(1), new PointF(width, 0));
                    o2.Invertier();
                    o3.Invertier();
                    return o2 *o1*o3;

                case Style.Blitz:
                    height = Math.Min(width, height);
                    float ratio = 1;
                    float a = height / (3 + ratio);
                    float b = ratio * a;
                    o1 = OrientierbarerWeg.HartPolygon(new PointF(),
                        new PointF(a, 0),
                        new PointF(0, a),
                        new PointF(a, 2 * a),
                        new PointF(0, 3 * a),
                        new PointF(b, 3 * a + b),
                        new PointF(2 * b + a, 2 * a),
                        new PointF(2 * b, a),
                        new PointF(2 * b + a, 0),
                        new PointF(2 * (a+b), 0));
                    o1 = o1 + new PointF((width -2*a -2* b) / 2, 0);
                    if (width > height)
                    o1 = new OrientierbarerWeg(new PointF(), new PointF(o1.Weg(0).X, 0))
                        * o1
                        * new OrientierbarerWeg(new PointF(o1.Weg(1).X, 0), new PointF(width, 0));
                    o1.Invertier();
                    return o1;

                case Style.ReissZahn:
                    height = Math.Min(height, width);
                    float r = (height * height + width * width) / (2 * height);
                    float alpha = (float)(Math.Asin(width / r) / (Math.PI * 2));
                    o1 = OrientierbarerWeg.Kreisbogen(r, 0.25 + alpha, 0.26) + new PointF(width,  height- r);
                    o2 = OrientierbarerWeg.Kreisbogen(height, 0.26, 0.5) + new PointF(width, 0);
                    o2.Invertier();
                    o3 = new OrientierbarerWeg(new PointF(width - height, 0), new PointF(width, 0));
                    o3.Invertier();
                    return o1 * o2 * o3;

                case Style._1_Chaos_:
                    o1 = OrientierbarerWeg.HartPolygon(
                        new PointF(0, 0),
                        new PointF(0, height),
                        new PointF(width / 2, height),
                        new PointF(width / 2, 0));
                    int nOCP = 20;
                    PointF[] pts = new PointF[nOCP + 2];
                    for (int i = 1; i < nOCP + 1; i++)
                        pts[i] = new PointF((width/2) + (i * width )/ (2 * nOCP + 2), height*d.NextFloat());
                    pts[0] = new PointF(width / 2, 0);
                    pts[nOCP + 1] = new PointF(width, 0);
                    o2 = OrientierbarerWeg.HartPolygon(pts);
                    o1.Invertier();
                    o2.Invertier();
                    return o1 * o2;

                case Style.Halbmond:
                    float midWidth = width * 0.5f;
                    width = width * 0.8f;
                    o1 = OrientierbarerWeg.EllipsenStuck(width / 2, height / 2, Math.PI / 2, 5 * Math.PI / 2) + new PointF(midWidth, height /2);
                    o2 = OrientierbarerWeg.EllipsenStuck(width * 2 / 5, height * 2 / 5, Math.PI / 2, 5 * Math.PI / 2) + new PointF(midWidth, height * 3 / 5);
                    width = width * 1.25f;
                    o2.Invertier();
                    PointF A = o1.Weg(0.45f);
                    PointF B = o1.Weg(0.55f);
                    return OrientierbarerWeg.HartPolygon(new PointF(), new PointF(A.X, 0), A)
                        * o1.Trim(0.45f, 0.1f)
                        | o2.Trim(0.1f, 0.9f)
                        | o1.Trim(0.9f, 0.55f)
                        * OrientierbarerWeg.HartPolygon(B, new PointF(B.X, 0), new PointF(width, 0));

                case Style._0_Rund_:
                    o1 = new OrientierbarerWeg(new PointF(), new PointF(width / 2, 0));
                    o1.Invertier();
                    o2 = OrientierbarerWeg.EllipsenStuck(width / 4, height, Math.PI, 0) + new PointF(width * 3 / 4,0);
                    return o1 * o2;

                case Style._0_Dreieck_:
                    o1 = OrientierbarerWeg.HartPolygon(
                        new PointF(),
                        new PointF(width / 2, 0),
                        new PointF(width * 3 / 4, height),
                        new PointF(width, 0));
                    o1.Invertier();
                    return o1;

                case Style.Pentagon:
                    double sd = height / (Math.Sin(Math.PI / 5) + Math.Sin(Math.PI * 2 / 5));
                    sd = Math.Min(sd, width / 2);
                    double h1 = sd * Math.Sin(Math.PI / 5);
                    double h2 = sd * Math.Sin(Math.PI * 2 / 5);
                    double b1 = sd * Math.Sin(Math.PI / 10);
                    PointF P1 = new PointF((float)((width - sd) / 2), 0);
                    PointF P2 = new PointF(P1.X - (float)b1, (float)h2);
                    PointF P3 = new PointF(width / 2, height);
                    PointF P4 = new PointF(width - P2.X, P2.Y);
                    PointF P5 = new PointF(width - P1.X, P1.Y);
                    o1= OrientierbarerWeg.HartPolygon(
                        new PointF(),
                        P1, P2, P3, P4, P5,
                        new PointF(width, 0));
                    o1.Invertier();
                    return o1;

                default:
                    throw new NotImplementedException();
            }
        }

        public static bool IsRandom(Style style)
            => (style == Style.Chaos) || (style == Style._1_Chaos_); 
    }
}
