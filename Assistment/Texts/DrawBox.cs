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

        public RectangleF Box;
        public bool EndsLine { get; set; }

        public float Top { get { return Box.Top; } set { Move(0, value - Box.Top); } }
        public float Bottom { get { return Box.Bottom; } set { Move(0, value - Box.Bottom); } }
        public float Left { get { return Box.Left; } set { Move(value - Box.Left, 0); } }
        public float Right { get { return Box.Right; } set { Move(value - Box.Right, 0); } }
        public PointF Location { get { return Box.Location; } set { Box.Location = value; } }
        public SizeF Size { get { return Box.Size; } set { Box.Size = value; } }

        /// <summary>
        /// gibt einen Wert zurück, der ungefähr breite*höhe entsprechen soll
        /// </summary>
        /// <returns></returns>
        public abstract float Space { get; }
        /// <summary>
        /// this method may only return zero iff space is zero, too
        /// </summary>
        /// <returns></returns>
        public abstract float Min { get; }
        /// <summary>
        /// this method may only return zero iff space is zero, too
        /// </summary>
        /// <returns></returns>
        public abstract float Max { get; }

        /// <summary>
        /// updates the values of min, max and space
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// computes the box of this and the boxes of any subdrawitems
        /// </summary>
        /// <param name="box"></param>
        /// <param name="isFirstInLine">Is this the first object of the current line? If so, Whitespaces will reduce their width to zero.</param>
        public virtual void Setup(RectangleF box, bool isFirstInLine)
            => Setup(box);
        /// <summary>
        /// computes the box of this and the boxes of any subdrawitems
        /// </summary>
        /// <param name="box"></param>
        public abstract void Setup(RectangleF box);
        public void Setup(PointF Location, float Width)
            => Setup(new RectangleF(Location, new SizeF(Width, float.MaxValue)));
        public void Setup(PointF Location)
            => Setup(new RectangleF(Location, new SizeF(this.Min, float.MaxValue)));
        public void Setup(SizeF Size)
            => Setup(new RectangleF(Box.Location, Size));
        public void Setup(float Width)
            => Setup(Box.Location, Width);
        /// <summary>
        /// draws the components of this item
        /// <para>use this after setup()</para>
        /// </summary>
        /// <param name="con"></param>
        public abstract void Draw(DrawContext con);
        /// <summary>
        /// gibt möglichst tiefe Kopien zurück
        /// </summary>
        /// <returns></returns>
        public abstract DrawBox Clone();
        public virtual void InStringBuilder(StringBuilder sb, string tabs)
        {
            throw new NotImplementedException();
        }
        public virtual void Move(PointF ToMove)
        {
            this.Box = new RectangleF(Box.Location.add(ToMove), Box.Size);
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

        public virtual bool Check(PointF punkt)
        {
            return Box.Contains(punkt);
        }
        public bool Check(float x, float y)
        {
            return Check(new PointF(x, y));
        }

        public void CreateImage(string name, float Scaling, Color BackColor)
        {
            CreateImage(name, Min, float.MaxValue, Scaling, BackColor);
        }
        public void CreateImage(string name)
        {
            CreateImage(name, Min, float.MaxValue, 1, Color.White);
        }
        public void CreateImage(string name, float width, float height)
        {
            CreateImage(name, width, height, 1, Color.White);
        }
        public void CreateImage(string name, float width, float height, float Scaling, Color BackColor)
        {
            this.Setup(new RectangleF(0, 0, width, 0));
            Size s = Box.Size.mul(Scaling).ToSize();
            using (Bitmap b = new Bitmap(s.Width, s.Height))
            {
                using (Graphics g = b.GetHighGraphics())
                {
                    g.ScaleTransform(Scaling, Scaling);
                    g.Clear(BackColor);
                    using (DrawContextGraphics dcg = new DrawContextGraphics(g, BackColor.ToBrush(), height))
                        this.Draw(dcg);
                }
                b.Save(name + ".png");
            }
        }

        public void CreateDinA3PDF(string name)
        {
            this.CreatePDF(name, 1400, float.MaxValue, iTextSharp.text.PageSize.A3);
        }
        public void CreateDinA5PDF(string name)
        {
            this.CreatePDF(name, 700, float.MaxValue, iTextSharp.text.PageSize.A5);
        }
        public void CreateDinA4PDFLandscape(string name)
        {
            this.CreatePDF(name, iTextSharp.text.PageSize.A4_LANDSCAPE);
        }
        public void CreatePDF(string name, iTextSharp.text.Rectangle PageSize)
        {
            CreatePDF(name, PageSize.Width / DrawContextDocument.factor, float.MaxValue, PageSize);
        }
        public void CreatePDF(string name, Color backColor)
        {
            iTextSharp.text.Rectangle PageSize = iTextSharp.text.PageSize.A4;
            CreatePDF(name, PageSize.Width / DrawContextDocument.factor, float.MaxValue, PageSize, backColor);
        }
        public void CreatePDF(string name, float width, float height, iTextSharp.text.Rectangle PageSize)
        {
            CreatePDF(name, width, height, PageSize, Color.White);
        }
        public void CreatePDF(string name, float width, float height, iTextSharp.text.Rectangle PageSize, Color backColor)
        {
            this.Update();
            this.Setup(new RectangleF(0, 0, width, 0));

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
                    this.Draw(dcd);
            }
        }
        public void CreatePDF(string name)
        {
            this.CreatePDF(name, 1000, float.MaxValue);
        }
        public void CreatePDF(string name, float width, float height)
        {
            this.CreatePDF(name, width, height, iTextSharp.text.PageSize.A4);
        }
        public void CreateLog(string name, float width)
        {
            this.Update();
            this.Setup(new RectangleF(0, 0, width, 0));

            StringBuilder sb = new StringBuilder();
            this.InStringBuilder(sb, "");
            StreamWriter f = File.CreateText(Directory.GetCurrentDirectory() + @"\" + name + ".txt");
            f.Write(sb.ToString());
            f.Close();
            f.Dispose();
        }

        /// <summary>
        /// Falls diese DrawBox ein Wort ist, übernimmt es diese Daten.
        /// Ist es ein Container, dann reicht er diese Daten rekursiv weiter.
        /// Werte, die null sind, werden nicht erzwungen.
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="font"></param>
        /// <param name="style"></param>
        /// <param name="pen"></param>
        public virtual void ForceWordStyle(Brush brush = null, xFont font = null, byte? style = null, Pen pen = null)
        {
        }

        public static CString operator *(CString box1, DrawBox box2)
        {
            CString t = box1.Clone() as CString;
            t.Add(box2);
            return t;
        }
        public static CString operator *(DrawBox box1, DrawBox box2)
        {
            CString t = new CString();
            t.Add(box1);
            t.AddAbsatz();
            t.Add(box2);
            return t;
        }
        public static CString operator +(DrawBox box1, DrawBox box2)
        {
            CString t = new CString();
            t.Add(box1);
            t.Add(box2);
            return t;
        }
        public static DrawContainer operator +(DrawContainer box1, DrawBox box2)
        {
            DrawContainer t = (DrawContainer)box1.Clone();
            t.Add(box2);
            return t;
        }
        public static implicit operator DrawBox(string text)
        {
            return (Text)text;
        }
    }
}
