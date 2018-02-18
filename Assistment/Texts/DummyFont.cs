using System.Drawing;

namespace Assistment.Texts
{
    public class DummyFont : xFont
    {
        public static readonly Font STANDARD_FONT = new Font("Calibri", 11);
        public static readonly xFont DUMMY_FONT = new DummyFont();


        public Font GetFont()
        {
            return STANDARD_FONT;
        }
        public Font GetFontBold()
        {
            return STANDARD_FONT;
        }
        public Font GetFontItalic()
        {
            return STANDARD_FONT;
        }
        public Font GetFontBoldAndItalic()
        {
            return STANDARD_FONT;
        }
        public Font GetFont(float scale)
        {
            return STANDARD_FONT;
        }
        public Font GetFontBold(float scale)
        {
            return STANDARD_FONT;
        }
        public Font GetFontItalic(float scale)
        {
            return STANDARD_FONT;
        }
        public Font GetFontBoldAndItalic(float scale)
        {
            return STANDARD_FONT;
        }
        public float XMass(char c)
        {
            return 0;
        }

        public float XMass(string s)
        {
            return 0;
        }

        public float YMass(char c)
        {
            return 0;
        }

        public float YMass(string s)
        {
            return 0;
        }

        public SizeF Size(char c)
        {
            return SizeF.Empty;
        }

        public SizeF Size(string s)
        {
            return SizeF.Empty;
        }

        public float GetZeilenabstand()
        {
            return 0;
        }

        public float GetCharToleranz()
        {
            return 0;
        }

        public float GetWhitespace()
        {
            return 0;
        }
        public float GetGeviertgrose()
        {
            return 0;
        }

        public xFont GetFontOfSize(float size)
        {
            return new DummyFont();
        }
    }
}
