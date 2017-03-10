using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Assistment.Mathematik;

namespace Assistment.Extensions
{
    public class ColorF : ICloneable
    {
        /// <summary>
        /// ARGB in [0,1]^4
        /// </summary>
        public float[] Values { get; set; }

        public ColorF()
        {
            this.Values = new float[4];
        }
        public ColorF(Color Color)
            : this()
        {
            this.Values[0] = Color.A / 255f;
            this.Values[1] = Color.R / 255f;
            this.Values[2] = Color.G / 255f;
            this.Values[3] = Color.B / 255f;
        }
        public ColorF(byte[] data, int offset, bool reverse)
            : this()
        {
            if (reverse)
                for (int i = 0; i < 4; i++)
                    Values[i] = data[offset + 3 - i] / 255f;
            else
                for (int i = 0; i < 4; i++)
                    Values[i] = data[offset + i] / 255f;
        }
        public ColorF(float[] Values)
            : this()
        {
            for (int i = 0; i < 4; i++)
                this.Values[i] = Values[i];
        }

        public float this[int index]
        {
            get { return Values[index]; }
            set { Values[index] = value; }
        }
        public Color ToColor()
        {
            return Color.FromArgb(
                Math.Max(0, Math.Min(255, (int)Math.Round(Values[0] * 255))),
                Math.Max(0, Math.Min(255, (int)Math.Round(Values[1] * 255))),
                Math.Max(0, Math.Min(255, (int)Math.Round(Values[2] * 255))),
                Math.Max(0, Math.Min(255, (int)Math.Round(Values[3] * 255))));

        }
        public object Clone()
        {
            return new ColorF(Values);
        }

        public static ColorF[] GramSchmidt(params ColorF[] Colors)
        {
            ColorF[] Output = new ColorF[Colors.Length];
            for (int i = 0; i < Colors.Length; i++)
            {
                Output[i] = Colors[i];
                for (int j = 0; j < i; j++)
                    Output[i] -= (Output[j] | Colors[i]) * Output[j];
                Output[i] = Output[i].Normalize();
            }
            return Output;
        }

        public ColorF Normalize()
        {
            float n = !this;
            if (n > 0.001f)
                return this / !this;
            else
                return new ColorF();
        }

        public static float operator |(ColorF A, ColorF B)
        {
            float skp = 0;
            for (int i = 0; i < 4; i++)
                skp += A[i] * B[i];
            return skp;
        }
        public static float operator ~(ColorF A)
        {
            return (A | A);
        }
        public static float operator !(ColorF A)
        {
            return (float)Math.Sqrt(~A);
        }

        public static ColorF operator *(float A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A * B[i]);
            return C;
        }
        public static ColorF operator *(ColorF B, float A)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A * B[i]);
            return C;
        }
        public static ColorF operator /(ColorF B, float A)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => B[i] / A);
            return C;
        }
        public static ColorF operator *(ColorF A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A[i] * B[i]);
            return C;
        }
        public static ColorF operator /(ColorF A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A[i] / B[i]);
            return C;
        }
        public static ColorF operator +(ColorF A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A[i] + B[i]);
            return C;
        }
        public static ColorF operator -(ColorF A, ColorF B)
        {
            ColorF C = new ColorF();
            C.Values.CountMap(i => A[i] - B[i]);
            return C;
        }

        public static implicit operator Color(ColorF d)
        {
            return d.ToColor();
        }
        public static implicit operator ColorF(Color d)
        {
            return new ColorF(d);
        }

        public override string ToString()
        {
            string s = "(";
            for (int i = 0; i < 4; i++)
            {
                s += Values[i];
                if (i < 3)
                    s += ", ";
            }
            return s + ")";
        }

    }
    public class ColorMatrix
    {
        /// <summary>
        /// ARGB x 1ARGB
        /// </summary>
        public float[,] Values { get; set; }

        public ColorMatrix()
        {
            Values = new float[4, 5];
            for (int i = 0; i < 4; i++)
                Values[i, i] = 1;
        }

        public float this[int output, int input]
        {
            get { return Values[output, input]; }
            set { Values[output, input] = value; }
        }

        public ColorF Apply(ColorF Input)
        {
            ColorF Output = new ColorF();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    Output[i] += Values[i, j] * Input[j];
                Output[i] += Values[i, 4];
            }
            return Output;
        }

        public static ColorMatrix Projection(params ColorF[] Space)
        {
            Space = ColorF.GramSchmidt(Space);
            ColorMatrix cm = new ColorMatrix();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    cm[i, j] = 0;
                    for (int l = 0; l < Space.Length; l++)
                        cm[i, j] += Space[l][i] * Space[l][j];
                }

            return cm;
        }
        public static ColorMatrix Projection(float Alpha, params ColorF[] Space)
        {
            ColorF[] FlatSpace = Space.DeepClone();
            for (int i = 0; i < FlatSpace.Length; i++)
                FlatSpace[i][0] = 0;

            Space = ColorF.GramSchmidt(FlatSpace);
            ColorMatrix cm = new ColorMatrix();
            for (int i = 1; i < 4; i++)
                for (int j = 1; j < 4; j++)
                {
                    cm[i, j] = 0;
                    for (int l = 0; l < Space.Length; l++)
                        cm[i, j] += Space[l][i] * Space[l][j];
                }
            cm[0, 0] = Alpha;
            cm[0, 4] = 0;

            return cm;
        }
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < 4; i++)
            {
                s += "|";
                for (int j = 0; j < 5; j++)
                    s += " " + Values[i, j].ToString("F3");
                s += "|\r\n";
            }
            return s;
        }
    }

    public static class ImageExtender
    {
        public static void saveDinA3Pdf(this Image Image, string name)
        {
            ImageBox ib = new ImageBox(1403, Image);
            ib.createDinA3PDF(name);
        }
        public static void savePdf(this Image Image, string name)
        {
            ImageBox ib = new ImageBox(701, Image);
            ib.createPDF(name);
        }
        public static void Raise(this Graphics Graphics)
        {
            Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        }
        public static void Lower(this Graphics Graphics)
        {
            Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
        }

        public static Graphics GetLowGraphics(this Image Image)
        {
            Graphics g = Graphics.FromImage(Image);
            g.Lower();
            return g;
        }
        public static Graphics GetHighGraphics(this Image Image)
        {
            Graphics g = Graphics.FromImage(Image);
            g.Raise();
            return g;
        }
        public static Graphics GetHighGraphics(this Image Image, float Scaling)
        {
            Graphics g = Graphics.FromImage(Image);
            g.Raise();
            g.ScaleTransform(Scaling, Scaling);
            return g;
        }
        public static Graphics GetHighGraphics(this Image Image, Color BackgroundColor)
        {
            Graphics g = Image.GetHighGraphics();
            g.Clear(BackgroundColor);
            return g;
        }
        public static Graphics GetGraphics(this Image Image, float Scaling, Color? BackgroundColor, bool high)
        {
            Graphics g = high ? Image.GetHighGraphics() : Image.GetLowGraphics();
            if (BackgroundColor.HasValue)
                g.Clear(BackgroundColor.Value);
            g.ScaleTransform(Scaling, Scaling);
            return g;
        }

        public static Bitmap Beschrifte(this Image Image, RectangleF mass, PointF rate, string einheit, Font Font, Pen Pen, Brush Brush, bool unten, bool links)
        {
            PointF zusatz = new PointF(2, 1).mul(Font.Height);
            Bitmap b = new Bitmap((int)(zusatz.X + Image.Width), (int)(zusatz.Y + Image.Height));
            Graphics g = b.GetHighGraphics(Color.White);
            g.DrawImage(Image, links ? zusatz.X : 0, unten ? 0 : zusatz.Y);

            PointF p = new PointF(0, unten ? Image.Height : 0);
            for (int i = (int)Math.Ceiling(mass.Left / rate.X); i < (int)Math.Ceiling(mass.Right / rate.X); i++)
            {
                float t = i * rate.X;
                p.X = (t - mass.Left) / mass.Width * Image.Width;
                if (links)
                    p.X += zusatz.X;
                g.DrawLine(Pen, p, p.add(0, zusatz.Y));
                g.DrawString(t.ToString("F1") + " " + einheit, Font, Brush, p);
            }

            p = new PointF(links ? 0 : Image.Width, 0);
            for (int i = (int)Math.Ceiling(mass.Top / rate.Y); i < (int)Math.Ceiling(mass.Bottom / rate.Y); i++)
            {
                float t = -i * rate.X;
                p.Y = (t - mass.Top) / mass.Height * Image.Height;
                if (!unten)
                    p.Y += zusatz.Y;
                g.DrawLine(Pen, p, p.add(zusatz.X, 0));
                g.DrawString(t.ToString("F1") + " " + einheit, Font, Brush, p);
            }
            return b;
        }

        public static void ForcedSave(this Image Image, FileStream FileStream, ImageFormat ImageFormat)
        {
            using (Bitmap b = new Bitmap(Image))
            {
                b.MakeTransparent();
                b.Save(FileStream, ImageFormat.Jpeg);
            }
        }
        public static void ForcedSave(this Image Image, string File)
        {
            ForcedSave(Image, File, GetFormat(File));
        }
        public static void ForcedSave(this Image Image, string File, ImageFormat ImageFormat)
        {
            using (FileStream fs = new FileStream(File, FileMode.Create))
            {
                ForcedSave(Image, fs, ImageFormat);
                fs.Close();
                fs.Dispose();
            }
        }

        public static ImageFormat GetFormat(this string FileName)
        {
            ImageFormat[] formats = new ImageFormat[] {
             ImageFormat.Jpeg, ImageFormat.Png,ImageFormat.Gif, ImageFormat.Bmp,
            ImageFormat.Tiff, ImageFormat.Emf, ImageFormat.Icon,   ImageFormat.Wmf,
            ImageFormat.Exif,ImageFormat.MemoryBmp
            };

            string ext = Path.GetExtension(FileName).ToLower().Substring(1);
            if (ext == "jpg")
                return ImageFormat.Jpeg;

            foreach (var item in formats)
                if (item.ToString().ToLower().Equals(ext))
                    return item;
            throw new NotImplementedException();
        }

        public static void Filter(this Bitmap Bitmap, Color FilterFarbe, float Alpha)
        {
            Filter(Bitmap, FilterFarbe, Color.FromArgb(0), Alpha);
        }
        public static void Filter(this Bitmap Bitmap, Color FilterFarbe)
        {
            Filter(Bitmap, FilterFarbe, Color.FromArgb(0), 1);
        }
        public static void Filter(this Bitmap Bitmap, Color FilterFarbe1, Color FilterFarbe2, float Alpha)
        {
            ColorMatrix cm = ColorMatrix.Projection(Alpha, FilterFarbe1, FilterFarbe2);
            Filter(Bitmap, cm);
        }
        public static void Filter(this Bitmap Bitmap, ColorMatrix Matrix)
        {
            BitmapData Data = Bitmap.LockBits(
               new Rectangle(new Point(), Bitmap.Size),
               ImageLockMode.ReadWrite,
               Bitmap.PixelFormat);
            int bufferSize = Data.Height * Data.Stride;
            byte[] bytes = new byte[bufferSize]; //BGRA
            Marshal.Copy(Data.Scan0, bytes, 0, bufferSize);
            FilterBytes(bytes, Matrix, Bitmap.PixelFormat);
            Marshal.Copy(bytes, 0, Data.Scan0, bufferSize);
            Bitmap.UnlockBits(Data);
        }
        private static float skp(float[] a, float[] b)
        {
            return skp(a, b, 0);
        }
        private static float skp(float[] a, float[] b, int indexOfB)
        {
            float s = 0;
            for (int i = 0; i < a.Length; i++)
                s += a[i] * b[i + indexOfB];
            return s;
        }
        private static float skp(float[] a, byte[] b, int indexOfB)
        {
            float s = 0;
            for (int i = 0; i < a.Length; i++)
                s += a[i] * b[i + indexOfB];
            return s;
        }
        private static float[] vector(Color Color)
        {
            return new float[] { Color.B, Color.G, Color.R, Color.A };
        }
        private static void FilterBytesNRGB(byte[] Data, Color FilterFarbe1, Color FilterFarbe2, int N)
        {
            float[] a1 = vector(FilterFarbe1),
                a2 = vector(FilterFarbe2);

            float n1 = FastMath.Sqrt(skp(a1, a1));
            a1.SelfMap(f => f / n1);

            float a12 = skp(a1, a2);
            for (int i = 0; i < 3; i++)
                a2[i] -= a12 * a1[i];

            float n2 = FastMath.Sqrt(skp(a2, a2));
            a2.SelfMap(f => f / n2);

            int n = N * (Data.Length / N);


            if (skp(a1, a1) > 0 && skp(a2, a2) > 0)
                for (int i = 0; i < n; i += N)
                {
                    float s1 = skp(a1, Data, i);
                    float s2 = skp(a2, Data, i);
                    for (int j = 0; j < N; j++)
                        Data[i + j] = (byte)Math.Min(255, Math.Max(0, a1[j] * s1 + a2[j] * s2));
                }
            else if (skp(a1, a1) > 0)
                for (int i = 0; i < n; i += N)
                {
                    float s = skp(a1, Data, i);
                    for (int j = 0; j < N; j++)
                        Data[i + j] = (byte)Math.Min(255, Math.Max(0, a1[j] * s));
                }

        }
        private static void FilterBytes(byte[] Data, Color FilterFarbe1, Color FilterFarbe2, PixelFormat Format)
        {
            switch (Format)
            {
                case PixelFormat.Alpha:
                case PixelFormat.Canonical:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    FilterBytesNRGB(Data, FilterFarbe1, FilterFarbe2, 4);
                    break;
                case PixelFormat.Format24bppRgb:
                    FilterBytesNRGB(Data, FilterFarbe1, FilterFarbe2, 3);
                    break;
                case PixelFormat.Format8bppIndexed:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private static void FilterBytesNRGB(byte[] Data, ColorMatrix Matrix, int N)
        {
            int n = N * (Data.Length / N);

            for (int i = 0; i < n; i += N)
            {
                ColorF Input = new ColorF(Data, i, true);
                ColorF Value = Matrix.Apply(Input);
                for (int j = 0; j < N; j++)
                    Data[i + j] = (byte)Math.Min(255, Math.Max(0, (int)Math.Round(Value[N - 1 - j] * 255)));
            }
        }
        private static void FilterBytes(byte[] Data, ColorMatrix Matrix, PixelFormat Format)
        {
            switch (Format)
            {
                case PixelFormat.Alpha:
                case PixelFormat.Canonical:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    FilterBytesNRGB(Data, Matrix, 4);
                    break;
                case PixelFormat.Format24bppRgb:
                    FilterBytesNRGB(Data, Matrix, 3);
                    break;
                case PixelFormat.Format8bppIndexed:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
