using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using Assistment.Drawing;
using Assistment.Texts;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.Geometries.Extensions;
using Assistment.Drawing.Style;
using Assistment.Extensions;
using Assistment.Mathematik;

namespace Assistment.Texts
{
    public abstract class Titel : DrawBox
    {
        public enum Art
        {
            Rund,
            Stachel,
            Zahn,
            Wellen,
            Sagezahn,
            VierStufen,
            Konig,
            Chaos,
            Kreuz,
            Triskelen,
            Pik,
            Blitz
        }

        public Brush HintergrundFarbe { get;  set; }
        public Pen RandFarbe { get;  set; }
        public float RandHohe { get;  set; }
        public float TextHorizontalMargin { get;  set; }
        public DrawBox Inhalt { get; set; }
        public float Scaling { get; set; }

        public Titel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : this(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, 1)
        {
        }
        public Titel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
        {
            this.Inhalt = Inhalt.Geometry(RandHohe + TextHorizontalMargin, 0, RandHohe, 0);
            this.RandHohe = RandHohe;
            this.RandFarbe = RandFarbe;
            this.TextHorizontalMargin= TextHorizontalMargin;
            this.HintergrundFarbe = HintergrundFarbe;
            this.Scaling = Scaling;
        }

        public override float Space
        {
            get
            {
                float ab = RandHohe * 2;
                float breite = (Inhalt.Max+ Inhalt.Min) / 2;
                float hohe = Inhalt.Space / breite;

                return (breite + ab) * (hohe + ab);
            }
        }

        public override float Min => Inhalt.Min + 2 * RandHohe;
        public override float Max => Inhalt.Max + 2 * RandHohe;

        public override void Update()
        {
            Inhalt.Update();
        }
        public override void Setup(RectangleF box)
        {
            box = box.Inner(RandHohe, RandHohe);
            Inhalt.Setup(box);
            this.Box = Inhalt.Box.Inner(-RandHohe, -RandHohe);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Inhalt.Move(ToMove);
        }

        public abstract Weg GetVerlauf(float units);
        public abstract Art GetArt();

        public override void Draw(DrawContext con)
        {
            Size s = Box.Size.mul(Scaling).Max(1, 1).ToSize();
            using (Image Image = new Bitmap(s.Width, s.Height + 1))
            using (Graphics g = Image.GetHighGraphics(Scaling))
            {
                RectangleF pseudoBox = new RectangleF(RandHohe, RandHohe, Inhalt.Box.Width, Inhalt.Box.Height);
                OrientierbarerWeg ow = OrientierbarerWeg.RundesRechteck(pseudoBox);
                int samples = 10000;
                Weg y = GetVerlauf(ow.L / RandHohe);
                Weg z = t => y(t);
                Pen RandFarbe = (Pen)this.RandFarbe.Clone();
                g.FillDrawWegAufOrientierbarerWeg(HintergrundFarbe, RandFarbe, z, ow, samples);
                con.DrawImage(Image, Box);
            }
            Inhalt.Draw(con);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            throw new NotImplementedException();
        }

        public static Titel GetTitel(Art Art, DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
        {
            switch (Art)
            {
                case Art.Rund:
                    return new RunderTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Stachel:
                    return new StachelTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Zahn:
                    return new ZahnTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Wellen:
                    return new WellenTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Sagezahn:
                    return new SagezahnTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.VierStufen:
                    return new VierStufenTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Konig:
                    return new KonigTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Chaos:
                    return new ChaosTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Kreuz:
                    return new KreuzTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Triskelen:
                    return new TriskelenTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Pik:
                    return new PikTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                case Art.Blitz:
                    return new BlitzTitel(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class RunderTitel : Titel
    {
        public RunderTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public RunderTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            return t => new PointF(t, 0);
        }

        public override DrawBox Clone()
        {
            return new RunderTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }

        public override Titel.Art GetArt()
        {
            return Art.Rund;
        }
    }
    public class StachelTitel : Titel
    {
        public StachelTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin,
            RandFarbe, HintergrundFarbe)
        {

        }
        public StachelTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int n = units.Ceil();
            return t =>
            {
                float t0 = t * n;
                int i = t0.Floor();
                float y = (1 + 2 * (i - t0));
                y *= y * RandHohe;
                return new PointF(t, y);
            };
        }

        public override DrawBox Clone()
        {
            return new StachelTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }

        public override Titel.Art GetArt()
        {
            return Art.Stachel;
        }
    }
    public class ZahnTitel : Titel
    {
        public ZahnTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public ZahnTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int stachel = (units / 2).Ceil();

            OrientierbarerWeg ow = OrientierbarerWeg.HartPolygon(new PointF(),
                new PointF(0, RandHohe),
                new PointF(RandHohe, RandHohe),
                new PointF(RandHohe, 0),
                new PointF(2 * RandHohe, 0));

            ow ^= stachel;

            return ow.Weg;
        }

        public override DrawBox Clone()
        {
            return new ZahnTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.Zahn;
        }
    }
    public class WellenTitel : Titel
    {
        public WellenTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public WellenTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int stachel = (units / 2).Ceil();
            return
                t =>
                {
                    double y = RandHohe * (1 + Math.Sin(stachel * t * Math.PI * 2)) / 2;
                    return new PointF(t, (float)y);
                };
        }

        public override DrawBox Clone()
        {
            return new WellenTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin,  RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.Wellen;
        }
    }
    public class SagezahnTitel : Titel
    {
        public SagezahnTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public SagezahnTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }
        public override Weg GetVerlauf(float units)
        {
            int stachel = units.Ceil();
            return
                t =>
                {
                    double y = RandHohe * ((stachel * (1 - t)) % 1);
                    return new PointF(t, (float)y);
                };
        }
        public override DrawBox Clone()
        {
            return new SagezahnTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.Sagezahn;
        }
    }
    public class VierStufenTitel : Titel
    {
        public VierStufenTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public VierStufenTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int stachel = (units / 4).Ceil();

            float lx = RandHohe;
            float ly = RandHohe / 2;
            OrientierbarerWeg oy = OrientierbarerWeg.HartPolygon(
                new PointF(),
                new PointF(0, ly),
                new PointF(lx, ly),
                new PointF(lx, 2 * ly),
                new PointF(2 * lx, 2 * ly),
                new PointF(2 * lx, ly),
                new PointF(3 * lx, ly),
                new PointF(3 * lx, 0),
                new PointF(4 * lx, 0));

            oy ^= stachel;

            return oy.Weg;
        }

        public override DrawBox Clone()
        {
            return new VierStufenTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.VierStufen;
        }
    }
    public class KonigTitel : Titel
    {
        public KonigTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public KonigTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int stachel = (units / 2).Ceil();
            return
                t =>
                {
                    float y;
                    float T = (stachel * t) % 4;

                    if ((int)T == 0) y = 0;
                    else if ((int)T == 1) y = RandHohe * (T - 1) * (T - 1);
                    else if ((int)T == 2) y = 0;
                    else y = RandHohe * (T - 4) * (T - 4);

                    return new PointF(t, y);
                };
        }

        public override DrawBox Clone()
        {
            return new KonigTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.Konig;
        }
    }
    public class ChaosTitel : Titel
    {
        public ChaosTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public ChaosTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            Random d = new Random();

            float[] werte = new float[(int)(units * 5)];
            for (int i = 0; i < werte.Length; i++)
                werte[i] = RandHohe * d.NextFloat();

            return
                t =>
                {
                    float T = t * werte.Length;
                    int lower = T.Floor() % werte.Length;
                    int higher = (lower + 1) % werte.Length;
                    float y;
                    if (higher > lower)
                        y = werte[lower] * (T - lower) + werte[higher] * (higher - T);
                    else
                        y = werte[lower] * (T - lower) + werte[higher] * (werte.Length - T);
                    return new PointF(t, y);
                };
        }

        public override DrawBox Clone()
        {
            return new ChaosTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.Chaos;
        }
    }
    public class KreuzTitel : Titel
    {
        public KreuzTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public KreuzTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            //Weglänge
            float L = units * RandHohe;
            int stachel = (units / 2).Ceil();
            //Kleines Wegstück
            float l = L / stachel;
            //Ganz kleines Wegstück
            float ll = RandHohe / 3;
            OrientierbarerWeg w = OrientierbarerWeg.HartPolygon(
                new PointF(),
                new PointF(ll, 0),
                new PointF(ll, ll),
                new PointF(0, ll),
                new PointF(0, 2 * ll),
                new PointF(ll, 2 * ll),
                new PointF(ll, 3 * ll),
                new PointF(2 * ll, 3 * ll),
                new PointF(2 * ll, 2 * ll),
                new PointF(3 * ll, 2 * ll),
                new PointF(3 * ll, ll),
                new PointF(2 * ll, ll),
                new PointF(2 * ll, 0),
                new PointF(6 * ll, 0));
            w = w ^ stachel;

            return w.Weg;
        }

        public override DrawBox Clone()
        {
            return new KreuzTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.Kreuz;
        }
    }
    public class TriskelenTitel : Titel
    {
        public TriskelenTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public TriskelenTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            //Weglänge
            float L = units * RandHohe;
            int stachel = (units / 2).Ceil();
            //Kleines Wegstück
            float l = L / stachel;
            //Ganz kleines Wegstück
            float ll = RandHohe / 2;

            OrientierbarerWeg T = (OrientierbarerWeg.Triskele(RandHohe / 4, 1, RandHohe / 6).Trim(0.01f, 0.99f) ^ Math.PI) + new PointF(ll, RandHohe - RandHohe / 4 * (float)(1 + 2 / Math.Sqrt(3)));

            OrientierbarerWeg w =
                OrientierbarerWeg.HartPolygon(new PointF(), T.Weg(0))
                * T
                * OrientierbarerWeg.HartPolygon(T.Weg(1), new PointF(ll * 4, 0));


            w = w ^ stachel;

            return w.Weg;
        }

        public override DrawBox Clone()
        {
            return new TriskelenTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.Kreuz;
        }
    }
    public class PikTitel : Titel
    {
        public PikTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public PikTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            //Weglänge
            float L = units * RandHohe;
            int stachel = (units / 1.5f).Ceil();
            //Kleines Wegstück
            float l = L / stachel;
            //Ganz kleines Wegstück
            float ll = RandHohe / 2;

            OrientierbarerWeg T = (OrientierbarerWeg.Pike(RandHohe).Trim(0.05f, 0.95f)) + new PointF(ll, 0);

            OrientierbarerWeg w =
                OrientierbarerWeg.HartPolygon(new PointF(), T.Weg(0))
                * T
                * OrientierbarerWeg.HartPolygon(T.Weg(1), new PointF(ll * 4, 0));


            w = w ^ stachel;

            return w.Weg;
        }

        public override DrawBox Clone()
        {
            return new PikTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.Pik;
        }
    }
    public class BlitzTitel : Titel
    {
        public BlitzTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {

        }
        public BlitzTitel(DrawBox Inhalt, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe, float Scaling)
            : base(Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            //Weglänge
            int stachel = (units / 1.2f).Ceil();

            float ratio = 1;
            float a = RandHohe / (3 + ratio);
            float b = ratio * a;

            OrientierbarerWeg w = OrientierbarerWeg.HartPolygon(new PointF(),
                new PointF(a, 0),
                new PointF(0, a),
                new PointF(a, 2 * a),
                new PointF(0, 3 * a),
                new PointF(b, 3 * a + b),
                new PointF(2 * b + a, 2 * a),
                new PointF(2 * b, a),
                new PointF(2 * b + a, 0),
                new PointF(2 * b + a + a, 0));

            w = w ^ stachel;

            return w.Weg;
        }

        public override DrawBox Clone()
        {
            return new BlitzTitel(Inhalt.Clone(), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe, Scaling);
        }
        public override Titel.Art GetArt()
        {
            return Art.Blitz;
        }
    }
}
