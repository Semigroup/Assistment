using System.Drawing;
using System.Drawing.Imaging;
using System;
using Assistment;
using Assistment.Drawing.Algorithms;
using Assistment.Extensions;
using Assistment.Drawing.Geometries.Typing;
using Assistment.Drawing.Geometries;
using Assistment.Drawing;
using System.Security.Cryptography;

namespace DemoCode
{
    internal class Program
    {
        static string outputDir = "D:\\Github\\Assistment\\images\\";
        static string ressourcesDir = "D:\\Github\\Assistment\\ressources\\";

        static void Main(string[] args)
        {
            Example4();
        }

        static Image getDogImage()
        {
            return Image.FromFile(ressourcesDir + "Dog.jpg");
        }

        /// <summary>
        /// Tiling and Mosaic Effect
        /// </summary>
        static void Example1()
        {
            using (var original = getDogImage())
            using (var bitmap = new Bitmap(original))
            {
                var size = original.Size;
                float b = 3;

                var steps = (int)Math.Log(size.Width, b);

                using (var demo = new Bitmap(steps * size.Width, size.Height))
                using (var graphics = demo.GetHighGraphics())
                {
                    graphics.DrawImageUnscaled(original, 0, 0);

                    float delta = 1;

                    for (int i = 1; i < steps; i++)
                    {
                        delta = b * delta;
                        var tesselation = PolygonFillingAlgorithm.TriangleTesselation(
                            (int)(size.Width / delta), (int)(size.Height / delta));
                        tesselation = tesselation.Scale(size);
                        tesselation.AverageMosaic(bitmap, graphics, new PointF(i * size.Width, 0));
                    }
                    demo.Save(outputDir + "demo1.jpg", ImageFormat.Jpeg);
                }
            }
        }
        /// <summary>
        /// Typing with Drawing.Geometries.Typing.Digital
        /// <para></para>
        /// Differences in Letterbox Styles: LetterBox.Style.Hard, LetterBox.Style.Fitting, LetterBox.Style.Approx 
        /// </summary>
        static void Example2()
        {
            var abc = new Alphabet();
            abc.MakeDigital();

            string line = "Typing with Curves";
            int fontSize = 100;

            int imageWidth = (int)(1.2f * fontSize * (line.Length + 1));
            int imageHeight = 9 * fontSize;

            var styles = new LetterBox.Style[] { LetterBox.Style.Hard, LetterBox.Style.Fitting, LetterBox.Style.Approx };

            Hohe heightFunction = t => (float)(1 + Math.Sin(Math.Tau * t));

            using (var demo = new Bitmap(imageWidth, imageHeight))
            using (var graphics = demo.GetHighGraphics())
            {
                graphics.Clear(Color.White);

                var offset = new PointF(fontSize / 2, fontSize / 2);

                foreach (var style in styles)
                {
                    foreach (var curve in abc.Type(offset, line, fontSize, style))
                    {
                        Hohe hScaled = t => 10 * heightFunction((curve.L * t / 30) % 1);
                        var redSinus = curve.GetPolygon(10000, 0, 1, hScaled);
                        graphics.DrawCurve(new Pen(Color.Red, 2), redSinus);

                        var blackBase = curve.GetPolygon(1000, 0, 1);
                        graphics.DrawCurve(new Pen(Color.Black, 3), blackBase);
                    }
                    offset.Y += 3 * fontSize;
                }

                demo.Save(outputDir + "demo2.jpg", ImageFormat.Jpeg);
            }
        }
        /// <summary>
        /// Typing with Shadow
        /// </summary>
        static void Example3()
        {
            var abc = new Alphabet();
            abc.MakeDigital();

            string line = "Typing with Shadow";
            int fontSize = 100;

            int imageWidth = (int)(1.2f * fontSize * (line.Length + 1));
            int imageHeight = 12 * fontSize;

            var heightFunctions = new Hohe[]{
                t => 3,
                t => (float)(3 * Math.Sin(Math.Tau * t)),
                t => (float)(2 + Math.Sin(Math.Tau * t)),
                t => t < 0.5 ? (t < 0.25 ? 1 : 0) : (t< 0.75 ? 1 : 2)
            };
            var shadow = new PointF(1f / (float)Math.Sqrt(2), 1f / (float)Math.Sqrt(2));

            using (var demo = new Bitmap(imageWidth, imageHeight))
            using (var graphics = demo.GetHighGraphics())
            {
                graphics.Clear(Color.White);

                var offset = new PointF(fontSize / 2, fontSize / 2);

                foreach (var h in heightFunctions)
                {
                    foreach (var curve in abc.Type(offset, line, fontSize, LetterBox.Style.Fitting))
                    {
                        Hohe hScaled = t => 10 * h((curve.L * t / 50) % 1);
                        Shadex.malSchatten(graphics, curve, Color.Red, Color.Black, shadow, 10000, hScaled);
                    }
                    offset.Y += 3 * fontSize;
                }

                demo.Save(outputDir + "demo3.jpg", ImageFormat.Jpeg);
            }
        }
        /// <summary>
        /// Typing on Shapes
        /// </summary>
        static void Example4()
        {
            var abc = new Alphabet();
            abc.MakeDigital();

            string line = "Typing on Shapes  ";
            int fontSize = 30;

            int imageWidth = 1000;
            int imageHeight = 1000;

            RectangleF topBox = new RectangleF(0, 0, fontSize * line.Length * 1.2f, fontSize * 2);

            var spiral = OrientierbarerWeg.Spirale(300, 8);
            spiral.Invertier();
            spiral = spiral.Spiegel(new Gerade(0, 0, 0, 1));

            OrientierbarerWeg[] baseCurves = {
                OrientierbarerWeg.Kreisbogen(400, 0, 1) + new PointF(500, 500),
                spiral + new PointF(500, 500)
            };


            using (var demo = new Bitmap(imageWidth, imageHeight))
            using (var graphics = demo.GetHighGraphics())
            {
                graphics.Clear(Color.White);
                foreach (var baseCurve in baseCurves)
                {
                    var blackBase = baseCurve.GetPolygon(1000, 0, 1);
                    graphics.DrawCurve(new Pen(Color.Black, 3), blackBase);

                    int iterations = (int)(baseCurve.L / 300);

                    foreach (var curve in abc.Type(new PointF(), line, fontSize))
                        for (int i = 0; i < iterations; i++)
                        {
                            float t0 = i * 1f / iterations;
                            float t1 = (i + 1f) / iterations;
                            var result = OrientierbarerWeg.TransformAlong(baseCurve, curve, topBox, t0, t1, fontSize);
                            var redTop = result.GetPolygon(10000, 0, 1);
                            graphics.DrawCurve(new Pen(Color.Red, 2), redTop);
                        }
                }

                demo.Save(outputDir + "demo4.jpg", ImageFormat.Jpeg);
            }
        }
    }
}