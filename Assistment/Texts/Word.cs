using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assistment.Texts
{
    public class Word : DrawBox
    {
        public const byte FONTSTYLE_BOLD = 1;
        public const byte FONTSTYLE_ITALIC = 2;
        public const byte FONTSTYLE_UNDERLINED = 4;
        public const byte FONTSTYLE_CROSSEDOUT = 8;
        public const byte FONTSTYLE_OVERLINED = 16;
        public const byte FONTSTYLE_RIGHTLINED = 32;
        public const byte FONTSTYLE_LEFTLINED = 64;
        public const byte FONTSTYLE_FRAMED = FONTSTYLE_LEFTLINED | FONTSTYLE_RIGHTLINED | FONTSTYLE_OVERLINED | FONTSTYLE_UNDERLINED;

        private string text;
        private Brush brush;
        /// <summary>
        /// von höherwertig (rechts) nach niederwertig (links)
        /// <para>[6:linksgestrichen][5:rechtsgestrichen][4:(overline)quer][3:durchgestrichen][2:unterstrichen][1:kursiv][0:fett]</para>
        /// </summary>
        private byte style;
        private xFont font;
        private Pen pen;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="brush"></param>
        /// <param name="font"></param>
        /// <param name="style">von höherwertig (rechts) nach niederwertig (links)
        /// <para>[6:linksgestrichen][5:rechtsgestrichen][4:(overline)quer][3:durchgestrichen][2:unterstrichen][1:kursiv][0:fett]</para></param>
        /// <param name="pen"></param>
        public Word(string text, Brush brush, xFont font, byte style, Pen pen)
        {
            this.text = text;
            this.brush = brush;
            this.font = font;
            this.style = style;
            this.pen = pen;
            this.Box = new RectangleF(0, 0, font.xMass(text), font.yMass(text));
        }

        public override float Space => this.Box.Width * this.Box.Height;
        public override float Min => this.Box.Width;
        public override float Max => this.Box.Width;

        public override void Setup(RectangleF box)
        {
            this.Box.Location = box.Location;
        }
        public override void Draw(DrawContext con)
        {
            Font f;
            if ((style & FONTSTYLE_BOLD) == 0)
                if ((style & FONTSTYLE_ITALIC) == 0)
                    f = font.getFont();
                else
                    f = font.getFontItalic();
            else
                if ((style & FONTSTYLE_ITALIC) == 0)
                    f = font.getFontBold();
                else
                    f = font.getFontBoldAndItalic();
            con.drawString(text, f, brush, Box.Location, Box.Height);

            if ((style & FONTSTYLE_UNDERLINED) != 0)
                con.drawLine(pen, Box.Left, Box.Bottom, Box.Right, Box.Bottom);
            if ((style & FONTSTYLE_CROSSEDOUT) != 0)
                con.drawLine(pen, Box.Left, Box.Top + Box.Height / 2, Box.Right, Box.Top + Box.Height / 2);
            if ((style & FONTSTYLE_OVERLINED) != 0)
                con.drawLine(pen, Box.Left, Box.Top, Box.Right, Box.Top);
            if ((style & FONTSTYLE_RIGHTLINED) != 0)
                con.drawLine(pen, Box.Right, Box.Top, Box.Right, Box.Bottom);
            if ((style & FONTSTYLE_LEFTLINED) != 0)
                con.drawLine(pen, Box.Left, Box.Top, Box.Left, Box.Bottom);
        }
        public override void Update()
        {
        }
        public override DrawBox Clone()
        {
            return new Word(text, brush, font, style, pen);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "Word:");
            sb.AppendLine(tabs + "\tbox: " + Box);
            sb.AppendLine(tabs + "\ttext: \"" + text + "\"");
            sb.AppendLine(tabs + "\tbrush: " + brush);
            sb.AppendLine(tabs + "\tfont: " + font);
            sb.AppendLine(tabs + "\tstyle: " + style);
            sb.AppendLine(tabs + "\tpen: " + pen);
            sb.AppendLine(tabs + ".");
        }

        public override string ToString()
        {
            return text;
        }
    }
}
