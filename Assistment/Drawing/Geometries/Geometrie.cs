﻿using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Assistment.Drawing.Geometries.Extensions;

namespace Assistment.Drawing.Geometries
{
    public abstract class Geometrie
    {
        /// <summary>
        /// gibt t_i zurück, sodass für alle t \in R gilt:
        /// <para>
        /// Aufpunkt + t * Richtungsvektor \in this
        /// </para>
        /// <para>iff</para>
        /// <para> t_i = t </para>
        /// </summary>
        /// <param name="Aufpunkt"></param>
        /// <param name="Richtungsvektor"></param>
        /// <returns></returns>
        public abstract IEnumerable<float> Cut(PointF Aufpunkt, PointF Richtungsvektor);
        /// <summary>
        /// gibt t_i zurück, sodass für alle t \in R gilt:
        /// <para>
        /// Aufpunkt + t * Richtungsvektor \in this
        /// </para>
        /// <para>iff</para>
        /// <para> t_i = t </para>
        /// </summary>
        /// <param name="Gerade"></param>
        /// <returns></returns>
        public IEnumerable<float> Cut(Gerade Gerade)
        {
            return Cut(Gerade.Aufpunkt, Gerade.Richtungsvektor);
        }
        /// <summary>
        /// gibt N Samples wieder
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public abstract IEnumerable<PointF> Samples(int Number);
        /// <summary>
        /// Gibt alle Geraden wieder, die von Aufpunkt ausgehend diese Geometrie tangieren.
        /// </summary>
        /// <param name="Aufpunkt"></param>
        /// <returns></returns>
        public abstract IEnumerable<Gerade> Tangents(PointF Aufpunkt);
        /// <summary>
        /// Gibt einen Punkt auf dem Rand dieser Geometrie zurück, die den Abstand zum gegebenem Punkt minimiert.
        /// </summary>
        /// <param name="Punkt"></param>
        /// <returns></returns>
        public abstract PointF Lot(PointF Punkt);

        /// <summary>
        /// Gibt den Durchschnitt der ersten beiden Punkte von Samples(2) zurück.
        /// </summary>
        /// <param name="Geometrie"></param>
        /// <returns></returns>
        public virtual PointF Zentrum()
        {
            IEnumerator<PointF> en = Samples(2).GetEnumerator();
            en.MoveNext();
            PointF A = en.Current;
            en.MoveNext();
            return A.add(en.Current).mul(0.5f);
        }
        public bool HasCut(Gerade Gerade)
        {
            return HasCut(Gerade.Aufpunkt, Gerade.Richtungsvektor);
        }
        public bool HasCut(PointF Aufpunkt, PointF Richtungsvektor)
        {
            return Cut(Aufpunkt, Richtungsvektor).GetEnumerator().MoveNext();
        }

        public abstract Geometrie Clone();

        public abstract Geometrie ScaleLocal(PointF ScalingFactor);
        public Geometrie ScaleLocal(float ScalingFactor)
        {
            return ScaleLocal(new PointF(ScalingFactor, ScalingFactor));
        }
        public abstract Geometrie TranslateLocal(PointF TranslatingVector);
        /// <summary>
        /// Gegen den Uhrzeigersinn im Bogenmaß
        /// </summary>
        /// <param name="RotatingAngle"></param>
        public abstract Geometrie RotateLocal(double RotatingAngle);
        /// <summary>
        /// Spiegelt an der gegebenen Achse
        /// </summary>
        /// <param name="MirroringAxis"></param>
        public abstract Geometrie MirrorLocal(PointF Aufpunkt, PointF RichtungsVektor);
        /// <summary>
        /// Spiegelt an der gegebenen Achse
        /// </summary>
        /// <param name="MirroringAxis"></param>
        public Geometrie MirrorLocal(Gerade MirroringAxis)
        {
            return this.MirrorLocal(MirroringAxis.Aufpunkt, MirroringAxis.Richtungsvektor);
        }

        public static Geometrie operator +(Geometrie Geometrie, PointF TranslatingVector)
        {
            return Geometrie.Clone().TranslateLocal(TranslatingVector);
        }
        public static Geometrie operator -(Geometrie Geometrie, PointF TranslatingVector)
        {
            return Geometrie.Clone().TranslateLocal(TranslatingVector.mul(-1));
        }
        public static Geometrie operator *(Geometrie Geometrie, PointF ScalingFactor)
        {
            return Geometrie.Clone().ScaleLocal(ScalingFactor);
        }
        public static Geometrie operator *(Geometrie Geometrie, float ScalingFactor)
        {
            return Geometrie.Clone().ScaleLocal(ScalingFactor);
        }
        public static Geometrie operator ^(Geometrie Geometrie, float RotatingAngle)
        {
            return Geometrie.Clone().RotateLocal(RotatingAngle);
        }

    }
}
