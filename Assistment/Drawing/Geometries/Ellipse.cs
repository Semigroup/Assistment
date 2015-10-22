using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Mathematik;

namespace Assistment.Drawing.Geometries
{
    /// <summary>
    /// Rand einer Ellipse
    /// </summary>
    public class Ellipse : Geometrie
    {
        public new PointF Zentrum;
        public PointF Radius;

        public Ellipse(PointF Zentrum, float Radius)
        {
            this.Zentrum = Zentrum;
            this.Radius = new PointF(Radius, Radius);
        }
        public Ellipse(PointF Zentrum, PointF Radius)
        {
            this.Zentrum = Zentrum;
            this.Radius = Radius;
        }

        public override IEnumerable<float> Cut(PointF Aufpunkt, PointF Richtungsvektor)
        {
            PointF v = Richtungsvektor.div(Radius);
            PointF B = Aufpunkt.sub(Zentrum).div(Radius);

            float bv = B.SKP(v);
            float vv = v.normSquared();
            float disk = bv * bv - (B.normSquared() - 1) * vv;
            if (disk < 0)
                return new float[0];
            float root = (float)Math.Sqrt(disk);
            return new float[] { (root - bv) / vv, (-root - bv) / vv };
        }

        public override IEnumerable<PointF> Samples(int Number)
        {
            PointF[] s = new PointF[Number];
            for (int i = 0; i < Number; i++)
            {
                double t = i * Math.PI * 2 / Number;
                s[i] = FastMath.Sphere(t).mul(Radius).add(Zentrum);
            }
            return s;
        }
        /// <summary>
        /// Kreiert einen neuen Kreis, der dem alten entspricht um Translation verschoben.
        /// </summary>
        /// <param name="Kreis"></param>
        /// <param name="Translation"></param>
        /// <returns></returns>
        public static Ellipse operator +(Ellipse Kreis, PointF Translation)
        {
            return new Ellipse(Kreis.Zentrum.add(Translation), Kreis.Radius);
        }

        public override Geometrie Clone()
        {
            return new Ellipse(Zentrum, Radius);
        }

        public override Geometrie ScaleLocal(PointF ScalingFactor)
        {
            Zentrum = Zentrum.mul(ScalingFactor);
            Radius = Radius.mul(ScalingFactor);
            return this;
        }

        public override Geometrie TranslateLocal(PointF TranslatingVector)
        {
            this.Zentrum = this.Zentrum.add(TranslatingVector);
            return this;
        }
        /// <summary>
        /// Muss für Radius.X != Radius.Y richtig implementiert werden.
        /// </summary>
        /// <param name="RotatingAngle"></param>
        /// <returns></returns>
        public override Geometrie RotateLocal(double RotatingAngle)
        {
            Zentrum.rot(RotatingAngle);
            return this;
        }

        /// <summary>
        /// Muss für Radius.X != Radius.Y richtig implementiert werden.
        /// </summary>
        /// <param name="Aufpunkt"></param>
        /// <returns></returns>
        public override IEnumerable<Gerade> Tangents(PointF Aufpunkt)
        {
            PointF C = Zentrum.sub(Aufpunkt).div(Radius);
            PointF[] Ss = C.findLeftRight(-1);

            Gerade[] tangs = new Gerade[Ss.Length];
            for (int i = 0; i < Ss.Length; i++)
			{
                PointF K = Zentrum.add(Ss[i].mul(Radius));
                tangs[i] = new Gerade(K, K.sub(Aufpunkt));
			}
            return tangs;
        }

        public override string ToString()
        {
            return Zentrum + " + e^it * " + Radius;
        }

        public override Geometrie MirroLocal(PointF Aufpunkt, PointF RichtungsVektor)
        {
            throw new NotImplementedException();
        }
    }
}
