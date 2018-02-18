using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Assistment.Extensions;

namespace Assistment.Texts
{
    public interface xFont
    {
        Font GetFont();
        Font GetFontBold();
        Font GetFontItalic();
        Font GetFontBoldAndItalic();
        Font GetFont(float scale);
        Font GetFontBold(float scale);
        Font GetFontItalic(float scale);
        Font GetFontBoldAndItalic(float scale);
        float XMass(char c);
        float XMass(string s);
        float YMass(char c);
        float YMass(string s);
        SizeF Size(char c);
        SizeF Size(string s);
        float GetZeilenabstand();
        float GetCharToleranz();
        float GetWhitespace();
        float GetGeviertgrose();
        xFont GetFontOfSize(float size);
    }
}
