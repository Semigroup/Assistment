using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment;
using Assistment.Texts;
using System.Drawing;
using iTextSharp.text;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Mathematik;

namespace Assistment.Texts.Paper
{
    public class CardSheet : DrawContainer
    {
        public static readonly iTextSharp.text.Rectangle PageSizeA4 = iTextSharp.text.PageSize.A4;

        public List<Tabular> Tabulars { get; private set; }
        private Tabular Current;

        public int Spalten { get; private set; }
        public int Zeilen { get; private set; }

        public int Eintrage { get; private set; }

        public SizeF KartenSize { get; private set; }
        public SizeF Zwischenplatz { get; private set; }
        public SizeF PageSize { get; private set; }

        private bool FirstBreak = false;

        public CardSheet(int Spalten, int Zeilen, SizeF KartenSize) 
            : this(Spalten, Zeilen, KartenSize, PageSizeA4)
        {

        }

        public CardSheet(int Spalten, int Zeilen, SizeF KartenSize, iTextSharp.text.Rectangle PageSize)
        {
            this.PageSize = new SizeF(PageSize.Width, PageSize.Height).mul(1 / DrawContextDocument.factor);
            this.Spalten = Spalten;
            this.Zeilen = Zeilen;
            this.KartenSize = KartenSize;
            this.Zwischenplatz = this.PageSize.sub(KartenSize.mul(Spalten, Zeilen));
            this.Zwischenplatz = Zwischenplatz.mul(1f / (Spalten + 1), 1f / (Zeilen + 1));

            Tabulars = new List<Tabular>();
            Eintrage = 0;
            Current = null;
        }

        public override void add(DrawBox word)
        {
            int n = Zeilen * Spalten;
            int m = Eintrage % n;
            if (m == 0)
            {
                Current = new Tabular(2 * Spalten + 1);
                Current.addRow(2 * Zeilen + 1);
                for (int i = 0; i < Current.Rows; i += 2)
                    for (int j = 0; j < Current.Columns; j += 2)
                        Current[i, j] = new Whitespace(Zwischenplatz.Width, Zwischenplatz.Height, false);
                Tabulars.Add(Current);
            }
            int s = m % Spalten;
            int z = (m - s) / Spalten;
            Current[1 + 2 * z, 1 + 2 * s] = new FixedBox(KartenSize, word);

            Eintrage++;
        }

        public override void insert(int index, DrawBox word)
        {
            throw new NotImplementedException();
        }

        public override void remove(int index)
        {
            throw new NotImplementedException();
        }

        public override bool remove(DrawBox word)
        {
            throw new NotImplementedException();
        }

        public override void clear()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return Tabulars.GetEnumerator();
        }

        public override float getSpace()
        {
            return Tabulars.Count * PageSize.Width * PageSize.Height;
        }

        public override float getMin()
        {
            return PageSize.Width;
        }

        public override float getMax()
        {
            return PageSize.Width;
        }

        public override void update()
        {
            foreach (var item in Tabulars)
                item.update();
        }

        public override void setup(RectangleF box)
        {
            int n = (box.Top / PageSize.Height).Ceil();
            FirstBreak = n > 0;
            this.box.Y = PageSize.Height * n;
            this.box.X = 0;
            this.box.Width = PageSize.Width;
            RectangleF r = new RectangleF(this.box.Location, PageSize);
            foreach (var item in Tabulars)
            {
                item.setup(r);
                this.box.Height += PageSize.Height;
                r.Y += PageSize.Height;
            }
        }

        public override void draw(DrawContext con)
        {
            if (FirstBreak)
                con.newPage();
            foreach (var item in Tabulars)
            {
                item.draw(con);
                con.newPage();
            }
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            throw new NotImplementedException();
        }
    }
}
