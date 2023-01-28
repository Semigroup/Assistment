using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assistment.Drawing.Geometries.Typing
{
    public class Alphabet
    {
        public Dictionary<char, Letter> Letters { get; set; } = new Dictionary<char, Letter>();

        public Letter ErrorLetter { get; set; }

        public void MakeDigital()
        {
            ErrorLetter = new Digital.QuestionMark();

            Letter[] letters = {
                new Digital.ALetter(),
                new Digital.BLetter(),
                new Digital.CLetter(),
                new Digital.DLetter(),
                new Digital.ELetter(),
                new Digital.FLetter(),
                new Digital.GLetter(),
                new Digital.HLetter(),
                new Digital.ILetter(),
                new Digital.JLetter(),
                new Digital.KLetter(),
                new Digital.LLetter(),
                new Digital.MLetter(),
                new Digital.NLetter(),
                new Digital.OLetter(),
                new Digital.PLetter(),
                new Digital.QLetter(),
                new Digital.RLetter(),
                new Digital.SLetter(),
                new Digital.TLetter(),
                new Digital.ULetter(),
                new Digital.VLetter(),
                new Digital.WLetter(),
                new Digital.XLetter(),
                new Digital.YLetter(),
                new Digital.ZLetter(),
            };
            for (int i = 0; i < 26; i++)
            {
                Letters.Add((char)('a' + i), letters[i]);
                Letters.Add((char)('A' + i), letters[i]);
            }
            Letter[] digits = {
                new Digital.Digit0(),
                new Digital.Digit1(),
                new Digital.Digit2(),
                new Digital.Digit3(),
                new Digital.Digit4(),
                new Digital.Digit5(),
                new Digital.Digit6(),
                new Digital.Digit7(),
                new Digital.Digit8(),
                new Digital.Digit9(),
            };
            for (int i = 0; i < 10; i++)
                Letters.Add((char)('0' + i), digits[i]);
        }

        public Letter this[char index]
        {
            get
            {
                if (Letters.TryGetValue(index, out Letter letter))
                    return letter;
                else
                    return ErrorLetter;
            }
            set
            {
                if (Letters.ContainsKey(index))
                    Letters[index] = value;
                else
                    Letters.Add(index, value);
            }
        }

        public IEnumerable<OrientierbarerWeg> Type(PointF location, string line, float fontSize, LetterBox.Style style = LetterBox.Style.Hard)
        {
            var box = LetterBox.GetGoldenCut(fontSize, style);
            foreach (char symbol in line)
            {
                if (symbol == ' ')
                    location.X += box.Width;
                else
                {
                    foreach (var curve in box.GetCurves(this[symbol]))
                        yield return curve + location;
                    location.X += box.Width + box.InterimWidth;
                }
            }
        }
    }
}
