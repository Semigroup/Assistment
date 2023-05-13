using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Assistment.Drawing.Geometries.Extensions;
using Assistment.Drawing;
using Assistment.Mathematik;

namespace Assistment.Drawing.Geometries
{
    public class Gerade : Geometrie, IComparable<Gerade>
    {
        public PointF Aufpunkt;
        public PointF Richtungsvektor;

        public Gerade(PointF Aufpunkt, PointF Richtungsvektor)
            : base()
        {
            this.Aufpunkt = Aufpunkt;
            this.Richtungsvektor = Richtungsvektor;
        }

        public Gerade(float x, float y, float dx, float dy)
        {
            this.Aufpunkt = new PointF(x, y);
            this.Richtungsvektor = new PointF(dx, dy);
        }
        /// <summary>
        /// gibt Aufpunkt + t * Richtungsvektor zurück
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public PointF Stelle(float t)
        {
            return Aufpunkt.saxpy(t, Richtungsvektor);
        }

        public override IEnumerable<float> Cut(PointF Aufpunkt, PointF Richtungsvektor)
        {
            //(A,B) = this, (C,D) = params
            //A + B * t = C + D * s
            //A - C = (D|B) * (s, -t)
            //(s, -t) = (D|B)^-1 (A-C)

            Matrix2 m = new Matrix2(Richtungsvektor, this.Richtungsvektor);
            if (m.Determinante().isZero())
                return new float[0];

            PointF st = this.Aufpunkt.sub(Aufpunkt) / m;
            return new float[] { st.X };
        }

        public override IEnumerable<PointF> Samples(int Number)
        {
            PointF[] S = new PointF[Number];
            for (int i = 0; i < Number; i++)
                S[i] = Aufpunkt.add(Richtungsvektor.mul(i));
            return S;
        }

        public override Geometrie Clone()
        {
            return new Gerade(Aufpunkt, Richtungsvektor);
        }

        public override Geometrie ScaleLocal(PointF ScalingFactor)
        {
            Aufpunkt = Aufpunkt.mul(ScalingFactor);
            Richtungsvektor = Richtungsvektor.mul(ScalingFactor);
            return this;
        }

        public override Geometrie TranslateLocal(PointF TranslatingVector)
        {
            Aufpunkt = Aufpunkt.add(TranslatingVector);
            return this;
        }

        public override Geometrie RotateLocal(double RotatingAngle)
        {
            throw new NotImplementedException();
            ///Fehlerhaft implementiert
            Aufpunkt = Aufpunkt.rot(RotatingAngle);
            Richtungsvektor = Richtungsvektor.rot(RotatingAngle);
            return this;
        }

        public bool Parallel(Gerade Gerade)
        {
            return Gerade.Richtungsvektor.Parallel(Richtungsvektor);
        }
        /// <summary>
        /// true iff Punkt liegt auf der Gerade
        /// </summary>
        /// <param name="Punkt"></param>
        /// <returns></returns>
        public bool Hat(PointF Punkt)
        {
            return Aufpunkt.sub(Punkt).Parallel(Richtungsvektor);
        }

        public override IEnumerable<Gerade> Tangents(PointF Aufpunkt)
        {
            if (Hat(Aufpunkt))
                return new Gerade[] { (Gerade)Clone() };
            else
                return new Gerade[0];
        }

        public override Geometrie MirrorLocal(PointF Aufpunkt, PointF RichtungsVektor)
        {
            Richtungsvektor = Richtungsvektor.normalize();
            this.Aufpunkt = Aufpunkt.add(Richtungsvektor.mul(Richtungsvektor.SKP(this.Aufpunkt.sub(Aufpunkt))));
            this.Richtungsvektor = Richtungsvektor.mul(Richtungsvektor.SKP(this.Richtungsvektor));
            return this;
        }

        public int CompareTo(Gerade other)
        {
            double d = this.Richtungsvektor.atan() - other.Richtungsvektor.atan();
            if (d > 0)
                return 1;
            else if (d < 0)
                return -1;
            else
                return 0;
        }
        public override string ToString()
        {
            return Aufpunkt + " + t * " + Richtungsvektor;
        }

        public override PointF Lot(PointF Punkt)
        {
            float t = Richtungsvektor.SKP(Punkt.sub(Aufpunkt)) / Richtungsvektor.normSquared();
            return Stelle(t);
        }
    }
}
