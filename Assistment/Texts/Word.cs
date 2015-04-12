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

        public Word(string text, Brush brush, xFont font, byte style, Pen pen)
        {
            this.text = text;
            this.brush = brush;
            this.font = font;
            this.style = style;
            this.pen = pen;
            this.box = new RectangleF(0, 0, font.xMass(text), font.yMass(text));
        }

        public override float getSpace()
        {
            return this.box.Width * this.box.Height;
        }
        public override float getMin()
        {
            return this.box.Width;
        }
        public override float getMax()
        {
            return this.box.Width;
        }

        public override void setup(RectangleF box)
        {
            this.box.Location = box.Location;
        }
        public override void draw(DrawContext con)
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
            con.drawString(text, f, brush, box.Location, box.Height);

            if ((style & FONTSTYLE_UNDERLINED) != 0)
                con.drawLine(pen, box.Left, box.Bottom, box.Right, box.Bottom);
            if ((style & FONTSTYLE_CROSSEDOUT) != 0)
                con.drawLine(pen, box.Left, box.Top + box.Height / 2, box.Right, box.Top + box.Height / 2);
            if ((style & FONTSTYLE_OVERLINED) != 0)
                con.drawLine(pen, box.Left, box.Top, box.Right, box.Top);
            if ((style & FONTSTYLE_RIGHTLINED) != 0)
                con.drawLine(pen, box.Right, box.Top, box.Right, box.Bottom);
            if ((style & FONTSTYLE_LEFTLINED) != 0)
                con.drawLine(pen, box.Left, box.Top, box.Left, box.Bottom);
        }
        public override void update()
        {
        }
        public override DrawBox clone()
        {
            return new Word(text, brush, font, style, pen);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "Word:");
            sb.AppendLine(tabs + "\tbox: " + box);
            sb.AppendLine(tabs + "\ttext: \"" + text + "\"");
            sb.AppendLine(tabs + "\tbrush: " + brush);
            sb.AppendLine(tabs + "\tfont: " + font);
            sb.AppendLine(tabs + "\tstyle: " + style);
            sb.AppendLine(tabs + "\tpen: " + pen);
            sb.AppendLine(tabs + ".");
        }
    }
    public class Whitespace : DrawBox
    {
        public Whitespace(float width, float height, bool endsLine)
        {
            this.box = new RectangleF(0, 0, width, height);
            this.endsLine = endsLine;
        }

        public override float getSpace()
        {
            return this.box.Width * this.box.Height;
        }
        public override float getMin()
        {
            return this.box.Width;
        }
        public override float getMax()
        {
            return this.box.Width;
        }

        public override void setup(RectangleF box)
        {
            //this.box.Location = con.box.Location;
        }
        public override void draw(DrawContext con)
        {
        }
        public override void update()
        {
        }
        public override DrawBox clone()
        {
            return new Whitespace(box.Width, box.Height, endsLine);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "Whitespace:");
            sb.AppendLine(tabs + "\tbox: " + box);
            sb.AppendLine(tabs + ".");
        }
    }
    public class ImageBox : DrawBox
    {
        private Image img;

        public ImageBox(Image img)
        {
            this.img = img;
            this.box = new RectangleF(0, 0, img.Width, img.Height);
        }
        public ImageBox(float width, Image img)
        {
            this.box.Width = width;
            this.box.Height = width * img.Height / img.Width;
            this.img = img;
        }
        public ImageBox(float width, float height, Image img)
        {
            this.box.Width = width;
            this.box.Height = height;
            this.img = img;
        }
        public ImageBox(SizeF size, Image img)
        {
            this.box.Size = size;
            this.img = img;
        }

        public override float getSpace()
        {
            return box.Width * box.Height;
        }
        public override float getMin()
        {
            return box.Width;
        }
        public override float getMax()
        {
            return box.Width;
        }

        public override void update()
        {
        }
        public override void setup(RectangleF box)
        {
            this.box.Location = box.Location;
        }
        public override void draw(DrawContext con)
        {
            con.drawImage(img, box);
        }

        public override DrawBox clone()
        {
            return new ImageBox(box.Size, img);
        }
        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            sb.AppendLine(tabs + "ImageBox:");
            sb.AppendLine(tabs + "\tbox: " + box);
            sb.AppendLine(tabs + ".");
        }
    }
}
