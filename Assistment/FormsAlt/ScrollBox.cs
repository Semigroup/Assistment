using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Assistment.Texts;

namespace Assistment.Forms
{
    /// <summary>
    /// Scrollboxen erhalten eine relative Größe und bieten Vertical und Horizontal bars an, um ihren body zu verschieben.
    /// </summary>
    public class ScrollBox : FormBox
    {
        /// <summary>
        /// gibt an, ob die scroll-Balken rechts, links, oben oder unten sind
        /// </summary>
        public bool rechts, unten;
        public float barBreite;

        ///// <summary>
        ///// verhältnis zwischen breite und höhe
        ///// <para>höhe = relative * breite</para>
        ///// </summary>
        //public float relative { get; private set; }

        /// <summary>
        /// wie weit wurde in x Richtung gescrollt?
        /// </summary>
        public float xBar { get; private set; }
        /// <summary>
        /// wie weit wurde in y Richtung gescrollt?
        /// </summary>
        public float yBar { get; private set; }

        protected Image cache;
        protected RectangleF destination, source;
        protected RectangleF horizontalBar, verticalBar;

        protected bool horizontalActive, verticalActive;
        protected PointF pressedPoint;

        public FormBox body { get; private set; }

        public ScrollBox(FormBox body)
        {
            this.body = body;
            this.rechts = this.unten = true;
            this.barBreite = 10;
            this.xBar = this.yBar = 0;
            this.horizontalActive = this.verticalActive = false;
            this.pressedPoint = new PointF();
        }

        public override float Space => 4 * barBreite * barBreite;// *Math.Max(relative, 1);
        public override float Min => 2 * barBreite;
        public override float Max => Math.Max(body.Max, 2 * barBreite);

        public override void Update()
        {
            body.Update();
            xBar = Math.Min(cache.Width - destination.Width, Math.Max(0, xBar));
            yBar = Math.Min(cache.Height - destination.Height, Math.Max(0, yBar));
        }
        /// <summary>
        /// ruft drawChache auf
        /// </summary>
        /// <param name="box"></param>
        public override void Setup(RectangleF box)
        {
            this.Box = box;

            drawCache();
            setupSourceAndBars();
        }
        private void drawCache()
        {
            RectangleF r = new RectangleF(0, 0, Box.Width - barBreite, Box.Height - barBreite);
            body.Setup(r);
            if (cache != null)
                cache.Dispose();
            cache = new Bitmap((int)Math.Ceiling(body.Box.Width), (int)Math.Ceiling(body.Box.Height));
            Graphics g = Graphics.FromImage(cache);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            body.Draw(new DrawContextGraphics(g));
            destination = new RectangleF(Box.Location, new SizeF(Math.Min(cache.Width, Box.Width - barBreite), Math.Min(cache.Height, Box.Height - barBreite)));
            if (!rechts)
                destination.X += barBreite;
            if (!unten)
                destination.Y += barBreite;
            body.Setup(destination);

        }
        private void setupSourceAndBars()
        {
            xBar = Math.Min(cache.Width - destination.Width, Math.Max(0, xBar));
            yBar = Math.Min(cache.Height - destination.Height, Math.Max(0, yBar));

            source = new RectangleF(xBar, yBar, destination.Width, destination.Height);

            float f = destination.Width / cache.Width;
            horizontalBar = new RectangleF(destination.X + f * xBar, destination.Y - barBreite, destination.Width * f, barBreite);
            if (unten)
                horizontalBar.Y += destination.Height + barBreite;

            f = destination.Height / cache.Height;
            verticalBar = new RectangleF(destination.X - barBreite, destination.Y + f * yBar, barBreite, destination.Height * f);
            if (rechts)
                verticalBar.X += destination.Width + barBreite;
        }
        public override void Draw(DrawContext con)
        {
            con.drawClippedImage(cache, destination, source);
            drawBars(con);
        }
        /// <summary>
        /// override this for more Design options
        /// </summary>
        /// <param name="con"></param>
        protected virtual void drawBars(DrawContext con)
        {
            if (cache.Width > destination.Width)
            {
                if (horizontalActive)
                    con.fillRectangle(Brushes.Red, horizontalBar);
                else
                    con.fillRectangle(Brushes.Blue, horizontalBar);
            }
            if (cache.Height + barBreite > Box.Height)
            {
                if (verticalActive)
                    con.fillRectangle(Brushes.Red, verticalBar);
                else
                    con.fillRectangle(Brushes.Blue, verticalBar);
            }
        }

        public override void click(PointF point)
        {
            if (Box.Contains(point))
            {
                context.activate(this);
                if (destination.Contains(point))
                {
                    if (body.Check(point))
                        body.click(point);
                }
                else
                {
                    if (verticalBar.Contains(point))
                    {
                        pressedPoint.Y = point.Y - verticalBar.Y + Box.Y;
                        verticalActive = true;
                    }
                    if (horizontalBar.Contains(point))
                    {
                        pressedPoint.X = point.X - horizontalBar.X + Box.X;
                        horizontalActive = true;
                    }
                }
            }
            else
            {
                verticalActive = horizontalActive = false;
                context.deactivate(this);
            }
            context.DrawMe(this);
        }
        public override void move(PointF point)
        {
            if (horizontalActive)
            {
                xBar = (cache.Width / destination.Width) * (point.X - pressedPoint.X);
                xBar = Math.Min(cache.Width - destination.Width, Math.Max(0, xBar));
            }
            if (verticalActive)
            {
                yBar = (cache.Height / destination.Height) * (point.Y - pressedPoint.Y);
                yBar = Math.Min(cache.Height - destination.Height, Math.Max(0, yBar));
            }
            if (horizontalActive || verticalActive)
            {
                setupSourceAndBars();
                context.DrawMe(this);
            }
        }
        public override void release(PointF point)
        {
            verticalActive = horizontalActive = false;
            context.deactivate(this);
            context.DrawMe(this);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            string ttabs = "\t" + tabs;
            sb.AppendLine(tabs + "Scrollbox:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            sb.AppendLine(tabs + "\txBar: " + xBar);
            sb.AppendLine(tabs + "\tyBar: " + yBar);
            sb.AppendLine(tabs + "\tbarBreite: " + barBreite);
            sb.AppendLine(tabs + "\tBody: ");
            body.InStringBuilder(sb, ttabs);
            sb.AppendLine(tabs + ".");
        }
        public override FormBox flone()
        {
            ScrollBox sb = new ScrollBox(this.body.flone());
            sb.barBreite = this.barBreite;
            sb.xBar = this.xBar;
            sb.yBar = this.yBar;
            sb.unten = this.unten;
            sb.rechts = this.rechts;
            return sb;
        }
        public override void setContext(FormContext context)
        {
            base.setContext(context);
            this.body.setContext(context);
        }
    }
}
