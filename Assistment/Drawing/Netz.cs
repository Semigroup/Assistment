using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Drawing.Style;
using System.Drawing;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Geometries;
using Assistment.Mathematik;

namespace Assistment.Drawing
{
    public class Netz : IEnumerable<Polygon>
    {
        private class Horizontale
        {
            public PointF[] Punkte;
            public Netz Netz;

            public Horizontale(Netz Netz, int BoxNumber)
            {
                this.Netz = Netz;
                Punkte = new PointF[Netz.Samples.X];
                float hohe = BoxNumber * 1f / Netz.Schema.Boxes.Y;
                Punkte.CountMap(i => new PointF(i / (Netz.Samples.X - 1f), hohe));
            }

            public void BoundEnds()
            {
                this.Punkte[0].X = 0;
                this.Punkte[this.Punkte.Length - 1].X = 1;
            }
            public void BoundTop()
            {
                for (int i = 0; i < Punkte.Length; i++)
                    Punkte[i].Y = 0;
            }
            public void BoundBottom()
            {
                for (int i = 0; i < Punkte.Length; i++)
                    Punkte[i].Y = 1;
            }

            public IEnumerable<PointF> GetThumb(int x)
            {
                float left = Netz.Offset.X + x * Netz.Thumb.X;
                left /= Netz.Boxes.X;
                float right = left + Netz.Thumb.X;
                right /= Netz.Boxes.X;

                left = Math.Max(left, 0);
                left *= Netz.Schema.Samples.X - 1;

                right = Math.Min(right, 1);
                right *= Netz.Schema.Samples.X - 1;

                return Punkte.FromTo((int)left, (int)right);
            }
            public IEnumerable<PointF> GetReversedThumb(int x)
            {
                float left = Netz.Offset.X + x * Netz.Thumb.X;
                left /= Netz.Boxes.X;
                float right = left + Netz.Thumb.X;
                right /= Netz.Boxes.X;

                left = Math.Max(left, 0);
                left *= Netz.Schema.Samples.X - 1;

                right = Math.Min(right, 1);
                right *= Netz.Schema.Samples.X - 1;

                return Punkte.FromTo((int)right, (int)left);
            }

            public void Map(FlachenFunktion<PointF> f)
            {
                Punkte.SelfMap(p => f(p.X, p.Y));
            }
        }
        private class Vertikale
        {
            public PointF[] Punkte;
            public Netz Netz;

            public Vertikale(Netz Netz, int BoxNumber)
            {
                this.Netz = Netz;
                Punkte = new PointF[Netz.Samples.Y];
                float breite = BoxNumber * 1f / Netz.Schema.Boxes.X;
                Punkte.CountMap(i => new PointF(breite, i / (Netz.Samples.Y - 1f)));
            }
            public void BoundEnds()
            {
                this.Punkte[0].Y = 0;
                this.Punkte[this.Punkte.Length - 1].Y = 1;
            }
            public void BoundLeft()
            {
                for (int i = 0; i < Punkte.Length; i++)
                    Punkte[i].X = 0;
            }
            public void BoundRight()
            {
                for (int i = 0; i < Punkte.Length; i++)
                    Punkte[i].X = 1;
            }

            public IEnumerable<PointF> GetThumb(int y)
            {
                float top = Netz.Offset.Y + y * Netz.Thumb.Y;
                top /= Netz.Boxes.Y;
                float bottom = top + Netz.Thumb.Y;
                bottom /= Netz.Boxes.Y;

                top = Math.Max(top, 0);
                top *= Netz.Schema.Samples.Y - 1;

                bottom = Math.Min(bottom, 1);
                bottom *= Netz.Schema.Samples.Y - 1;

                return Punkte.FromTo((int)top, (int)bottom);
            }
            public IEnumerable<PointF> GetReversedThumb(int y)
            {
                float top = Netz.Offset.Y + y * Netz.Thumb.Y;
                top /= Netz.Boxes.Y;
                float bottom = top + Netz.Thumb.Y;
                bottom /= Netz.Boxes.Y;

                top = Math.Max(top, 0);
                top *= Netz.Schema.Samples.Y - 1;

                bottom = Math.Min(bottom, 1);
                bottom *= Netz.Schema.Samples.Y - 1;

                return Punkte.FromTo((int)bottom, (int)top);
            }

            public void Map(FlachenFunktion<PointF> f)
            {
                Punkte.SelfMap(p => f(p.X, p.Y));
            }
        }

        FlachenSchema Schema;
        Point Offset;
        Point AntiOffset;
        public Point Thumb
        {
            get
            {
                return Schema.Thumb;
            }
        }
        public Point Boxes
        {
            get
            {
                return Schema.Boxes;
            }
        }
        public Point Samples
        {
            get
            {
                return Schema.Samples;
            }
        }
        public Point Thumbs;

        Horizontale[] Horizontalen;
        Vertikale[] Vertikalen;

        public Netz(FlachenSchema Schema, Point Offset)
        {
            this.Schema = Schema;
            this.AdjustOffset(Offset);

            int i = 0;
            Horizontalen = new Horizontale[Thumbs.Y + 1];
            for (int y = this.Offset.Y; y <= AntiOffset.Y; y += Thumb.Y)
                Horizontalen[i++] = new Horizontale(this, y);

            i = 0;
            Vertikalen = new Vertikale[Thumbs.X + 1];
            for (int x = this.Offset.X; x <= AntiOffset.X; x += Thumb.X)
                Vertikalen[i++] = new Vertikale(this, x);
        }

        public void Bound()
        {
            foreach (var item in Horizontalen)
                item.BoundEnds();
            foreach (var item in Vertikalen)
                item.BoundEnds();

            Horizontalen.First().BoundTop();
            Horizontalen.Last().BoundBottom();
            Vertikalen.First().BoundLeft();
            Vertikalen.Last().BoundRight();
        }

        public void Map(FlachenFunktion<PointF> f)
        {
            foreach (var item in Horizontalen)
                item.Map(f);
            foreach (var item in Vertikalen)
                item.Map(f);
        }

        private void AdjustOffset(Point Offset)
        {
            this.Offset = Offset;
            while (this.Offset.X > 0)
                this.Offset.X -= this.Schema.Thumb.X;
            while (this.Offset.X <= -this.Schema.Thumb.X)
                this.Offset.X += this.Schema.Thumb.X;
            while (this.Offset.Y > 0)
                this.Offset.Y -= this.Schema.Thumb.Y;
            while (this.Offset.Y <= -this.Schema.Thumb.Y)
                this.Offset.Y += this.Schema.Thumb.Y;

            this.AntiOffset = this.Offset;
            this.Thumbs = (Boxes.sub(this.AntiOffset)).divCeil(Thumb);
            this.AntiOffset = this.AntiOffset.add(
                Thumb.mul(Thumbs));
        }

        public Polygon GetThumb(Point whichThumb)
        {
            Point p = Offset.add(Thumb.mul(whichThumb));
            int x = whichThumb.X;
            int y = whichThumb.Y;
            IEnumerable<PointF> res = Horizontalen[y].GetThumb(x);
            res = res.Concat(Vertikalen[x + 1].GetThumb(y));
            res = res.Concat(Horizontalen[y + 1].GetReversedThumb(x));
            res = res.Concat(Vertikalen[x].GetReversedThumb(y));
            return new Polygon(res);
        }

        public void Paint(Graphics g)
        {
            foreach (var item in Thumbs.Enumerate())
            {
                Polygon toDraw = GetThumb(item);
                PointF z = Offset.add(Thumb.mul(item));
                z = z.saxpy(0.5f, Thumb);
                z = z.div(Boxes);
                z = z.sat();

                if (Schema.Pinsel != null)
                    g.FillPolygon(Schema.Pinsel(z.X, z.Y), toDraw);
            }
        }

        public IEnumerator<Polygon> GetEnumerator()
        {
            return Thumbs.Enumerate().Map(p => GetThumb(p)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
