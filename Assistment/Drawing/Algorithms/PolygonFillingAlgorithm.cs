using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Drawing;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.LinearAlgebra;
using System.Drawing.Imaging;
using Assistment.Extensions;

namespace Assistment.Drawing.Algorithms
{
    public class PolygonFillingAlgorithm : ImageAlgorithmBase
    {
        /// <summary>
        /// Kachelung über [0,1]^2
        /// </summary>
        public IList<Polygon> Tesselation;
        /// <summary>
        /// Kachelung über [0,1]^2
        /// </summary>
        /// <param name="Tesselation"></param>
        public PolygonFillingAlgorithm(IList<Polygon> Tesselation)
        {
            this.Tesselation = Tesselation;
        }

        public override void Execute(Bitmap inputImage, Graphics g)
        {
            using (Bitmap b = rasterize(inputImage.Size))
            {
                int N = Tesselation.Count;
                //dreieck nummer: summe alpha, summe rot, summe grün, summe blau, anzahl der pixel
                int[,] table = new int[N, 5];
                for (int x = 0; x < b.Width; x++)
                    for (int y = 0; y < b.Height; y++)
                    {
                        int n = ind(b.GetPixel(x, y));
                        if (n >= N) continue;
                        Color c = inputImage.GetPixel(x, y);
                        table[n, 0] += c.A;
                        table[n, 1] += c.R;
                        table[n, 2] += c.G;
                        table[n, 3] += c.B;
                        table[n, 4]++;
                    }
                int i = 0;
                foreach (var item in Tesselation)
                {
                    int n = table[i, 4];
                    Color c = Color.FromArgb(
                       (byte)(table[i, 0] * 1f / n),
                       (byte)(table[i, 1] * 1f / n),
                       (byte)(table[i, 2] * 1f / n),
                       (byte)(table[i, 3] * 1f / n));
                    g.FillPolygon(c.ToBrush(), item);
                    i++;
                }
            }
        }

        public int ind(Color c)
        {
            return c.R + (c.G << 8) + (c.B << 16);
        }
        public Color col(int index)
        {
            return Color.FromArgb(255, index % 256, (index >> 8) % 256, (index >> 16) % 256);
        }

        private Bitmap rasterize(Size s)
        {
            Bitmap map = new Bitmap(s.Width, s.Height);
            using (Graphics g = map.GetLowGraphics())
            {
                g.ScaleTransform(s.Width, s.Height);
                g.Clear(Color.FromArgb(-1));
                int i = 0;
                foreach (var item in Tesselation)
                    g.FillPolygon(col(i++).ToBrush(), item);
            }
            return map;
        }

        public static PolygonFillingAlgorithm TriangleTesselation(int numberX, int numberY)
        {
            List<Polygon> tess = new List<Polygon>();
            float height = 1f / numberY;
            float width = 1f / numberX;
            Polygon up = new Polygon(
                0, 0,
                width, 0,
                width / 2, height,
                0, 0);
            Polygon down = new Polygon(
               -width / 2, height,
               0, 0,
               width / 2, height,
               -width / 2, height);

            PointF offset = new PointF();
            for (int y = 0; y < numberY; y++)
            {
                for (int x = 0; x < numberX+1; x++)
                {
                    tess.Add(up + offset);
                    tess.Add(down + offset);
                    offset.X += width;
                }
                offset.X = (y % 2 == 0) ? 0 : - width / 2;
                offset.Y += height;
            }
            return new PolygonFillingAlgorithm(tess);
        }
    }
}
