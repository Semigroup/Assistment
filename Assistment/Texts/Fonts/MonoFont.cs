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
    public class MonoFont : IFontMeasurer
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

        public IFontMeasurer GetFontOfSize(float size)
            => new MonoFont(SymbolWidth * size, SymbolHeight * size);

        public float GetGeviertgrose()
            => SymbolWidth;

        public float GetWhitespace()
            => SymbolWidth;

        public float GetZeilenabstand()
            => SymbolHeight;

        public SizeF Size(char c, byte style)
            => new SizeF(SymbolWidth, SymbolHeight);

        public SizeF Size(string s, byte style)
            => new SizeF(SymbolWidth * s.Length, SymbolHeight);

        public float XMass(char c, byte style)
            => SymbolWidth;

        public float XMass(string s, byte style)
            => SymbolWidth * s.Length;

        public float YMass(char c, byte style)
            => SymbolHeight;

        public float YMass(string s, byte style)
            => SymbolHeight;

        public float XMass(char c, FontStyle style)
            => XMass(c, (byte)style);
        public float XMass(string s, FontStyle style)
            => XMass(s, (byte)style);
        public float YMass(char c, FontStyle style)
            => YMass(c, (byte)style);
        public float YMass(string s, FontStyle style)
            => YMass(s, (byte)style);
        public SizeF Size(char c, FontStyle style)
            => Size(c, (byte)style);
        public SizeF Size(string s, FontStyle style)
            => Size(s, (byte)style);

        public Font GetFont(FontStyle style)
            => GetFont((byte)style);
        public Font GetFont(byte style)
        {
            switch (style & 3)
            {
                case 0:
                    return GetFont();
                case 1:
                    return GetFontBold();
                case 2:
                    return GetFontItalic();
                case 3:
                    return GetFontBoldAndItalic();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
