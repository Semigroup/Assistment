using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Mathematik;

namespace Assistment.Drawing.Geometrie
{
    public class Gerade : Geometrie
    {
        public PointF Aufpunkt;
        public PointF Richtungsvektor;

        public Gerade(PointF Aufpunkt, PointF Richtungsvektor) : base()
        {
            this.Aufpunkt = Aufpunkt;
            this.Richtungsvektor = Richtungsvektor;
        }

        public Gerade(float x, float y, float dx, float dy)
        {
            this.Aufpunkt = new PointF(x, y);
            this.Richtungsvektor = new PointF(dx, dy);
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
            Aufpunkt = Aufpunkt.rot(RotatingAngle);
            Richtungsvektor = Richtungsvektor.rot(RotatingAngle);
            return this;
        }

        public override Geometrie MirroLocal(PointF MirroringAxis)
        {
            throw new NotImplementedException();
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
    }
}
