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

        public override int Count => Eintrage;

        public override bool IsReadOnly => false;

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

        public override void Add(DrawBox word)
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

        public override void Insert(int index, DrawBox word)
        {
            throw new NotImplementedException();
        }

        public override void Remove(int index)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(DrawBox word)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<DrawBox> GetEnumerator()
        {
            return Tabulars.GetEnumerator();
        }

        public override float Space => Tabulars.Count * PageSize.Width * PageSize.Height;

        public override float Min => PageSize.Width;

        public override float Max => PageSize.Width;

        public override void Update()
        {
            foreach (var item in Tabulars)
                item.Update();
        }

        public override void Setup(RectangleF box)
        {
            int n = (box.Top / PageSize.Height).Ceil();
            FirstBreak = n > 0;
            this.Box.Y = PageSize.Height * n;
            this.Box.X = 0;
            this.Box.Width = PageSize.Width;
            RectangleF r = new RectangleF(this.Box.Location, PageSize);
            foreach (var item in Tabulars)
            {
                item.Setup(r);
                this.Box.Height += PageSize.Height;
                r.Y += PageSize.Height;
            }
        }

        public override void Draw(DrawContext con)
        {
            if (FirstBreak)
                con.NewPage();
            foreach (var item in Tabulars)
            {
                item.Draw(con);
                con.NewPage();
            }
        }

        public override DrawBox Clone()
        {
            throw new NotImplementedException();
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(DrawBox item)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(DrawBox[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
    }
}
