using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Mathematik;
using System;

namespace Assistment.Drawing.Geometries
{
    public static class FlacheErweiterer
    {
        public static FlachenFunktion<PointF> mul(this FlachenFunktion<PointF> flache, float c)
        {
            return (u, v) => flache(u, v).mul(c);
        }

        public static RandFunktion<T> Rand<T>(this FlachenFunktion<T> flache, float u, float v)
        {
            float uv = (u + v);
            float uv2 = 2 * uv;
            float u2v = uv + u;

            float u1 = u / uv / 2;
            float v1 = v / uv / 2;
            float u2 = u1 + 0.5f;
            float v2 = v1 + 0.5f;

            return t =>
            {
                float f = uv2 * t;
                if (f < u)
                    return flache(f / u, 0);
                else if (f < uv)
                    return flache(1, (f - u) / v);
                else if (t < u2v)
                    return flache((u2v - f) / u, 1);
                else
                    return flache(0, (uv2 - f) / v);
            };
        }
        /// <summary>
        /// Erzeugt die Gerade: flache(cU * t / norm, cV * t / norm) wobei t von 0 nach 1 laufen soll
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flache"></param>
        /// <param name="cU"></param>
        /// <param name="cV"></param>
        /// <returns></returns>
        public static RandFunktion<T> Gerade<T>(this FlachenFunktion<T> flache, float cU, float cV)
        {
            float norm = FastMath.Sqrt(cU * cU + cV * cV);
            norm /= cU < cV ? cV : cU;
            cU /= norm;
            cV /= norm;

            return t => flache(cU * t, cV * t);
        }

        public static FlachenFunktion<PointF> Determinieren(this FlachenFunktion<PointF> flache, int USamples, int VSamples)
        {
            return ReSample(Samples(flache, USamples, VSamples));
        }
        /// <summary>
        /// Die FlächenFunktion ist auf den Rändern Null.
        /// </summary>
        /// <param name="flache"></param>
        /// <param name="USamples"></param>
        /// <param name="VSamples"></param>
        /// <returns></returns>
        public static FlachenFunktion<PointF> KompaktDeterminieren(this FlachenFunktion<PointF> flache, int USamples, int VSamples)
        {
            PointF[,] samples = Samples(flache, USamples, VSamples);

            for (int i = 0; i < USamples; i++)
                samples[i, VSamples - 1] = samples[i, 0] = new PointF();
            for (int i = 0; i < VSamples; i++)
                samples[USamples - 1, i] = samples[0, i] = new PointF();

            return ReSample(samples);
        }


        public static FlachenFunktion<PointF> ReSample(this PointF[,] samples)
        {
            int n = samples.GetLength(0) - 1;
            int m = samples.GetLength(1) - 1;
            return (u, v) =>
            {
                u = ((u % 1) + 1) % 1;
                v = ((v % 1) + 1) % 1;
                u *= n;
                v *= m;
                int x = (int)u;
                int y = (int)v;

                PointF bottomLeft = samples[x, y];
                PointF bottomRight = samples[Math.Min(x + 1, n), y];
                PointF topLeft = samples[x, Math.Min(y + 1, m)];
                PointF topRight = samples[Math.Min(x + 1, n), Math.Min(y + 1, m)];

                u -= x;
                v -= y;

                PointF bottom = bottomLeft.tween(bottomRight, u);
                PointF top = topLeft.tween(topRight, u);
                return bottom.tween(top, v);
            };
        }

        /// <summary>
        /// Vektorfeld : [0,1]^2 --> [0,1]^2
        /// </summary>
        /// <param name="Vektorfeld"></param>
        /// <param name="t"></param>
        /// <param name="schritte"></param>
        /// <returns></returns>
        public static FlachenFunktion<PointF> Potential(this FlachenFunktion<PointF> Vektorfeld, float t, int schritte)
        {
            float d = t / schritte;
            return (u, v) =>
            {
                PointF position = new PointF(u, v);
                for (int i = 0; i < schritte; i++)
                    position = position.saxpy(d, Vektorfeld(position.X, position.Y)).sat();
                return position;
            };
        }

        public static T[] Process<T>(this FlachenFunktion<T> flache, PointF[] punkte)
        {
            T[] ts = new T[punkte.Length];
            for (int i = 0; i < punkte.Length; i++)
                ts[i] = flache(punkte[i].X, punkte[i].Y);
            return ts;
        }

        public static T[,] Samples<T>(this FlachenFunktion<T> flache, int USamples, int VSamples)
        {
            T[,] testWerte = new T[USamples, VSamples];
            for (int u = 0; u < USamples; u++)
                for (int v = 0; v < VSamples; v++)
                    testWerte[u, v] = flache(u / (USamples - 1f), v / (VSamples - 1f));
            return testWerte;
        }
        public static T[] Samples<T>(this RandFunktion<T> rand, int samples)
        {
            T[] array = new T[samples];
            for (int i = 0; i < samples; i++)
                array[i] = rand(i / (samples - 1f));
            return array;
        }
    }
}
