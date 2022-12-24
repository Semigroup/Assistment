using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Assistment.Extensions;

namespace Assistment.Texts
{
    public interface IFontMeasurer
    {
        Font GetFont(FontStyle style);
        Font GetFont();
        Font GetFontBold();
        Font GetFontItalic();
        Font GetFontBoldAndItalic();
        Font GetFont(float scale);
        Font GetFontBold(float scale);
        Font GetFontItalic(float scale);
        Font GetFontBoldAndItalic(float scale);
        float XMass(char c, FontStyle style);
        float XMass(char c, byte style);
        float XMass(string s, FontStyle style);
        float XMass(string s, byte style);
        float YMass(char c, FontStyle style);
        float YMass(char c, byte style);
        float YMass(string s, FontStyle style);
        float YMass(string s, byte style);
        SizeF Size(char c, FontStyle style);
        SizeF Size(string s, FontStyle style);
        SizeF Size(char c, byte style);
        SizeF Size(string s, byte style);
        float GetZeilenabstand();
        float GetCharToleranz();
        float GetWhitespace();
        float GetGeviertgrose();
        IFontMeasurer GetFontOfSize(float size);
    }
}
