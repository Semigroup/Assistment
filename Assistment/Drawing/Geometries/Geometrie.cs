﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Assistment.Drawing.LinearAlgebra;

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
    public static class GeometrieErweiterer
    {
        public static float Distanz(this Geometrie Geometrie, PointF P)
        {
            return Geometrie.Lot(P).dist(P);
        }

        public static bool HasCutMatching(this Geometrie Geometrie, Gerade Gerade, Predicate<float> match)
        {
            IEnumerable<float> ts = Geometrie.Cut(Gerade);
            foreach (var item in ts)
                if (match(item))
                    return true;
            return false;
        }
        public static bool HasNotNegativeCut(this Geometrie Geometrie, Gerade Gerade)
        {
            IEnumerable<float> ts = Geometrie.Cut(Gerade);
            foreach (var item in ts)
                if (item >= 0)
                    return true;
            return false;
        }

        public static PointF NotNegativeCut(this Geometrie Geometrie, Gerade Gerade)
        {
            List<float> ts = new List<float>();
            foreach (var item in Geometrie.Cut(Gerade))
                if (item > 0)
                    ts.Add(item);
            if (ts.Count == 0)
                return new PointF();
            else
                return Gerade.Stelle(ts.Min());
        }

        public static void DrawGeometry(this Graphics g, Pen Pen, Geometrie Geometrie, int Samples)
        {
            g.DrawPolygon(Pen, Geometrie.Samples(Samples).ToArray());
        }
        public static void FillGeometry(this Graphics g, Brush Brush, Geometrie Geometrie, int Samples, FillMode FillMode)
        {
            g.FillPolygon(Brush, Geometrie.Samples(Samples).ToArray(), FillMode);
        }
        public static void FillDrawGeometry(this Graphics g, Brush Brush, Pen Pen, Geometrie Geometrie, int Samples, FillMode FillMode)
        {
            PointF[] Array = Geometrie.Samples(Samples).ToArray();
            g.FillPolygon(Brush, Array, FillMode);
            g.DrawPolygon(Pen, Array);
        }

        /// <summary>
        /// Bestimmt ob die Geometrie den Point enthält
        /// <para>indem die Anzahl der Schnitte mit einer Random Gerade (Point + t * (1, 0)) geprüft</para>
        /// <para>geht von der Annahme aus, dass es sich um eine offene Umgebung handelt</para>
        /// </summary>
        /// <param name="Geometrie"></param>
        /// <param name="Point"></param>
        /// <returns></returns>
        public static bool ContainsPoint(this Geometrie Geometrie, PointF Point)
        {
            Gerade g = new Gerade(Point, new PointF(1, 0));
            bool drin = false;
            foreach (float cut in Geometrie.Cut(g))
                if (cut < 0)
                    drin = !drin;
            return drin;
        }
    }
}
