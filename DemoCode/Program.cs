using System.Drawing;
using System.Drawing.Imaging;
using System;
using Assistment;
using Assistment.Drawing.Algorithms;
using Assistment.Extensions;

namespace DemoCode
{
    internal class Program
    {
        static string outputDir = "D:\\Github\\Assistment\\images\\";
        static string ressourcesDir = "D:\\Github\\Assistment\\ressources\\";

        static void Main(string[] args)
        {
            Example1();
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
    }
}