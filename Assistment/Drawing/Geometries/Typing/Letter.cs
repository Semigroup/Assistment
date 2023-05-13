using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Assistment.Drawing.Geometries.Extensions;
using Assistment.Extensions;

namespace Assistment.Drawing.Geometries.Typing
{
    public abstract class Letter
    {
        public class Segment
        {
            public Segment(params float[] coordinates)
            {
                Points = new PointF[coordinates.Length / 2];
                for (int i = 0; i < coordinates.Length; i += 2)
                {
                    Points[i / 2].X = coordinates[i];
                    Points[i / 2].Y = coordinates[i + 1];
                }
            }

            /// <summary>
            /// 0 - 1 Mittelhöhe
            /// 1 - 2 Oberhöhe
            /// -1 - 0 Unterhöhe
            /// 0 - 1 Volle Breite
            /// </summary>
            public PointF[] Points { get; set; }

            public PointF[] Transform(LetterBox letterBox)
            {
                //high = 0
                float middle = letterBox.UpperHeight;
                float ground = letterBox.MiddleHeight + middle;

                PointF[] transformed = new PointF[Points.Length];
                for (int i = 0; i < Points.Length; i++)
                {
                    transformed[i].X = letterBox.Width * Points[i].X;
                    float t = Points[i].Y;
                    if (t > 1)
                        transformed[i].Y = (2 - t) * letterBox.UpperHeight;
                    else if (t > 0)
                        transformed[i].Y = middle + (1 - t) * letterBox.MiddleHeight;
                    else
                        transformed[i].Y = ground - t * letterBox.LowerHeight;
                    transformed[i] = transformed[i].add(letterBox.Offset);
                }
                return transformed;
            }
        }

        public string AssociatedCharacters { get; set; }
        public Segment[] Segments { get; set; }

        /// <summary>
        /// S(t) = r * (cos(t), sin(t))
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="radius"></param>
        /// <param name="midPoint"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        public bool GetMiddlePointOfTangentCircleAtBeta(PointF a, PointF b, PointF c, float radius,
            out PointF midPoint, out double alpha, out double beta, out double delta,
            float minPart = 0, float maxPart = 0.5f)
        {
            midPoint = new PointF();
            alpha = 0;
            beta = 0;
            delta = 0;

            PointF u = a.sub(b);
            PointF v = c.sub(b);
            PointF orthoU = u.linksOrtho();
            PointF orthoV = v.linksOrtho();

            float utv = orthoU.SKP(v);
            float vtu = orthoV.SKP(u);

            if (Math.Abs(utv) < float.Epsilon)
                return false;

            orthoU = orthoU.mul(utv);
            orthoV = orthoV.mul(vtu);
            orthoU = orthoU.normalize();
            orthoV = orthoV.normalize();

            PointF offA = b.saxpy(radius, orthoU);
            PointF offB = b.saxpy(radius, orthoV);

            PointF offDiff = offA.sub(offB);
            float det = u.SKP(v.linksOrtho());
            if (Math.Abs(det) < float.Epsilon)
                return false;

            float s = u.SKP(offDiff.linksOrtho()) / det;
            float t = v.SKP(offDiff.linksOrtho()) / det;

            if (s > maxPart || s < minPart || t > maxPart || t < minPart)
                return false;

            PointF a1 = b.saxpy(s, v);
            midPoint = a1.saxpy(radius, orthoV);

            midPoint = offB.saxpy(s, v);

            alpha = Math.PI - orthoU.atan();
            beta = Math.PI - orthoV.atan();
            delta = beta - alpha;
            while (delta > Math.PI)
                delta -= 2 * Math.PI;
            while (delta < -Math.PI)
                delta += 2 * Math.PI;

            return true;
        }
    }
}
