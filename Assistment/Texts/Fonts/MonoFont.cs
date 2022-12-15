using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assistment.Texts.Fonts
{
    /// <summary>
    /// Simple, Abstract Font that assigns to each character the same height and width.
    /// Does not provide Fonts!
    /// </summary>
    public class MonoFont : xFont
    {
        public float SymbolWidth;
        public float SymbolHeight;

        public MonoFont(float SymbolWidth, float SymbolHeight)
        {
            this.SymbolWidth = SymbolWidth;
            this.SymbolHeight = SymbolHeight;
        }


        public float GetCharToleranz()
            => 0;

        public Font GetFont()
        {
            throw new NotSupportedException();
        }
        public Font GetFont(float scale)
        {
            throw new NotSupportedException();
        }
        public Font GetFontBold()
        {
            throw new NotSupportedException();
        }
        public Font GetFontBold(float scale)
        {
            throw new NotSupportedException();
        }
        public Font GetFontBoldAndItalic()
        {
            throw new NotSupportedException();
        }
        public Font GetFontBoldAndItalic(float scale)
        {
            throw new NotSupportedException();
        }
        public Font GetFontItalic()
        {
            throw new NotSupportedException();
        }
        public Font GetFontItalic(float scale)
        {
            throw new NotSupportedException();
        }

        public xFont GetFontOfSize(float size)
            => new MonoFont(SymbolWidth * size, SymbolHeight * size);

        public float GetGeviertgrose()
            => SymbolWidth;

        public float GetWhitespace()
            => SymbolWidth;

        public float GetZeilenabstand()
            => SymbolHeight;

        public SizeF Size(char c)
            => new SizeF(SymbolWidth, SymbolHeight);

        public SizeF Size(string s)
            => new SizeF(SymbolWidth * s.Length, SymbolHeight);

        public float XMass(char c)
            => SymbolWidth;

        public float XMass(string s)
            => SymbolWidth * s.Length;

        public float YMass(char c)
            => SymbolHeight;

        public float YMass(string s)
            => SymbolHeight;
    }
}
