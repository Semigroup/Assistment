using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Windows.Forms;
using System.IO;

using Assistment.Drawing;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;

namespace Assistment.Texts
{
    public abstract class DrawBox
    {
        public static readonly xFont StandardFont = new FontGraphicsMeasurer("Calibri", 11);

        public RectangleF box;
        public bool endsLine;

        public float Top { get { return box.Top; } set { Move(0, value - box.Top); } }
        public float Bottom { get { return box.Bottom; } set { Move(0, value - box.Bottom); } }
        public float Left { get { return box.Left; } set { Move(value - box.Left, 0); } }
        public float Right { get { return box.Right; } set { Move(value - box.Right, 0); } }
        public PointF Location { get { return box.Location; } set { box.Location = value; } }
        public SizeF Size { get { return box.Size; } set { box.Size = value; } }

        /// <summary>
        /// gibt einen Wert zurück, der ungefähr breite*höhe entsprechen soll
        /// </summary>
        /// <returns></returns>
        public abstract float getSpace();
        /// <summary>
        /// this method may only return zero iff space is zero, too
        /// </summary>
        /// <returns></returns>
        public abstract float getMin();
        public abstract float getMax();

        /// <summary>
        /// updates the values of min, max and space
        /// </summary>
        public abstract void update();
        /// <summary>
        /// computes the box of this and the boxes of any subdrawitems
        /// </summary>
        /// <param name="box"></param>
        public abstract void setup(RectangleF box);
        public void setup(PointF Location, float Width)
        {
            this.setup(new RectangleF(Location, new SizeF(Width, float.MaxValue)));
        }
        public void setup(PointF Location)
        {
            this.setup(new RectangleF(Location, new SizeF(this.getMin(), float.MaxValue)));
        }
        public void setup(SizeF Size)
        {
            this.setup(new RectangleF(box.Location, Size));
        }
        public void setup(float Width)
        {
            this.setup(box.Location, Width);
        }
        /// <summary>
        /// draws the components of this item
        /// <para>use this after setup()</para>
        /// </summary>
        /// <param name="con"></param>
        public abstract void draw(DrawContext con);
        /// <summary>
        /// gibt möglichst tiefe Kopien zurück
        /// </summary>
        /// <returns></returns>
        public abstract DrawBox clone();
        public virtual void InStringBuilder(StringBuilder sb, string tabs)
        {
            throw new NotImplementedException();
        }
        public virtual void Move(PointF ToMove)
        {
            this.box = new RectangleF(box.Location.add(ToMove), box.Size);
        }
        public void Move(float x, float y)
        {
            this.Move(new PointF(x, y));
        }

        public GeometryBox Geometry(float Left, float Top, float Right, float Bottom)
        {
            return new GeometryBox(this, Top, Bottom, Right, Left);
        }
        public GeometryBox Geometry(SizeF Abstand)
        {
            return Geometry(Abstand.Width, Abstand.Height);
        }
        public GeometryBox Geometry(float XAbstand, float YAbstand)
        {
            return Geometry(XAbstand / 2, YAbstand / 2, XAbstand / 2, YAbstand / 2);
        }
        public GeometryBox Geometry(float Abstand)
        {
            return Geometry(Abstand, Abstand, Abstand, Abstand);
        }

        public BackColorBox Colorize(Pen RandFarbe, Brush BackColor)
        {
            return new BackColorBox(BackColor, RandFarbe, this);
        }
        public BackColorBox Colorize(Brush BackColor)
        {
            return this.Colorize(null, BackColor);
        }
        public BackColorBox Colorize(Color BackColor)
        {
            return this.Colorize(BackColor.ToBrush());
        }

        public virtual bool check(PointF punkt)
        {
            return box.Contains(punkt);
        }
        public bool check(float x, float y)
        {
            return check(new PointF(x, y));
        }

        public void createImage(string name, float Scaling, Color BackColor)
        {
            createImage(name, getMin(), float.MaxValue, Scaling, BackColor);
        }
        public void createImage(string name)
        {
            createImage(name, getMin(), float.MaxValue, 1, Color.White);
        }
        public void createImage(string name, float width, float height)
        {
            createImage(name, width, height, 1, Color.White);
        }
        public void createImage(string name, float width, float height, float Scaling, Color BackColor)
        {
            this.setup(new RectangleF(0, 0, width, 0));
            Size s = box.Size.mul(Scaling).ToSize();
            using (Bitmap b = new Bitmap(s.Width, s.Height))
            {
                using (Graphics g = b.GetHighGraphics())
                {
                    g.ScaleTransform(Scaling, Scaling);
                    g.Clear(BackColor);
                    using (DrawContextGraphics dcg = new DrawContextGraphics(g, BackColor.ToBrush(), height))
                        this.draw(dcg);
                }
                b.Save(name + ".png");
            }
        }

        public void createDinA3PDF(string name)
        {
            this.createPDF(name, 1400, float.MaxValue, iTextSharp.text.PageSize.A3);
        }
        public void createDinA5PDF(string name)
        {
            this.createPDF(name, 700, float.MaxValue, iTextSharp.text.PageSize.A5);
        }
        public void createDinA4PDFLandscape(string name)
        {
            this.createPDF(name, iTextSharp.text.PageSize.A4_LANDSCAPE);
        }
        public void createPDF(string name, iTextSharp.text.Rectangle PageSize)
        {
            createPDF(name, PageSize.Width / DrawContextDocument.factor, float.MaxValue, PageSize);
        }
        public void createPDF(string name, Color backColor)
        {
            iTextSharp.text.Rectangle PageSize = iTextSharp.text.PageSize.A4;
            createPDF(name, PageSize.Width / DrawContextDocument.factor, float.MaxValue, PageSize, backColor);
        }
        public void createPDF(string name, float width, float height, iTextSharp.text.Rectangle PageSize)
        {
            createPDF(name, width, height, PageSize, Color.White);
        }
        public void createPDF(string name, float width, float height, iTextSharp.text.Rectangle PageSize, Color backColor)
        {
            this.update();
            this.setup(new RectangleF(0, 0, width, 0));

            iTextSharp.text.Rectangle pageSize = new iTextSharp.text.Rectangle(PageSize);
            pageSize.BackgroundColor = new iTextSharp.text.BaseColor(backColor);

            using (iTextSharp.text.Document doc = new iTextSharp.text.Document())
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, System.IO.File.Create(name + ".pdf"));
                doc.SetPageSize(pageSize);
                doc.NewPage();
                doc.Open();
                PdfContentByte pCon = writer.DirectContent;
                pCon.SetLineWidth(0.3f);
                using (DrawContextDocument dcd = new DrawContextDocument(pCon, height))
                    this.draw(dcd);
            }
        }
        public void createPDF(string name)
        {
            this.createPDF(name, 1000, float.MaxValue);
        }
        public void createPDF(string name, float width, float height)
        {
            this.createPDF(name, width, height, iTextSharp.text.PageSize.A4);
        }
        public void createLog(string name, float width)
        {
            this.update();
            this.setup(new RectangleF(0, 0, width, 0));

            StringBuilder sb = new StringBuilder();
            this.InStringBuilder(sb, "");
            StreamWriter f = File.CreateText(Directory.GetCurrentDirectory() + @"\" + name + ".txt");
            f.Write(sb.ToString());
            f.Close();
            f.Dispose();
        }

        public static CString operator *(CString box1, DrawBox box2)
        {
            CString t = box1.clone() as CString;
            t.add(box2);
            return t;
        }
        public static CString operator *(DrawBox box1, DrawBox box2)
        {
            CString t = new CString();
            t.add(box1);
            t.addAbsatz();
            t.add(box2);
            return t;
        }
        public static CString operator +(DrawBox box1, DrawBox box2)
        {
            CString t = new CString();
            t.add(box1);
            t.add(box2);
            return t;
        }
        public static DrawContainer operator +(DrawContainer box1, DrawBox box2)
        {
            DrawContainer t = (DrawContainer)box1.clone();
            t.add(box2);
            return t;
        }
        public static implicit operator DrawBox(string text)
        {
            return (Text)text;
        }
    }
}
