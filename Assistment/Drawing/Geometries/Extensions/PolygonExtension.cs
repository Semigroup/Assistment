using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Drawing.Geometries
{
    public static class PolygonExtension
    {
        public static void DrawPolygon(this Graphics Graphics, Pen Pen, Polygon Polygon)
        {
            Graphics.DrawPolygon(Pen, Polygon.punkte);
        }
        public static void FillPolygon(this Graphics Graphics, Brush Brush, Polygon Polygon)
        {
            Graphics.FillPolygon(Brush, Polygon.punkte);
        }
        public static void DrawLines(this Graphics Graphics, Pen Pen, IEnumerable<PointF> Points)
        {
            IEnumerator<PointF> en = Points.GetEnumerator();
            en.MoveNext();
            PointF old = en.Current;
            while (en.MoveNext())
            {
                Graphics.DrawLine(Pen, old, en.Current);
                old = en.Current;
            }
        }

        public static void DrawRectangle(this Graphics g, Pen Pen, RectangleF RectangleF)
        {
            g.DrawRectangle(Pen, RectangleF.X, RectangleF.Y, RectangleF.Width, RectangleF.Height);
        }

        public static void FillDrawWegAufOrientierbarerWeg(this Graphics g, Brush Brush, Pen Pen, Weg y, OrientierbarerWeg oy, int samples)
        {
            PointF[] Samples = new PointF[samples];
            float L = y(1).X - y(0).X;

            if (Math.Abs(L) <= float.Epsilon)
                throw new NotImplementedException();

            for (int i = 0; i < samples - 1; i++)
            {
                float t = i / (samples - 1f);
                PointF v = y(t);
                float x = v.X / L;
                Samples[i] = oy.Weg(x).saxpy(v.Y, oy.Normale(x));
                //Samples[i] = oy.weg(t).add(-v.X * n.Y + v.Y * n.X, v.X * n.X + v.Y * n.Y); n = oy.normale(t)
            }
            Samples[samples - 1] = Samples[0];
            if (Brush != null)
                g.FillPolygon(Brush, Samples);
            if (Pen != null)
                g.DrawPolygon(Pen, Samples);
        }
    }
}
