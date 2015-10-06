using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Drawing.LinearAlgebra;
using System.Drawing;
using Assistment.Drawing.Geometrie;

namespace Assistment.Drawing
{
    public class Schema
    {
        /// <summary>
        /// Schwarz - Dunkelgrau - Dunkelblau - Schwarz
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Schema WithTeeth(float scale)
        {
            Schema schema = new Schema("WithTeeth");
            schema.setFarbmuster(1, (int)(10 * scale), Color.Black, Color.DarkGray, Color.DarkBlue, Color.Black);
            schema.stift = new Pen(Color.White, scale / 5);
            schema.scale = scale;

            schema.burst = 20*scale;
            schema.strings = 100;
            schema.sampleRate = 10 * schema.strings;
            schema.background = Color.White;

            return schema;
        }
        /// <summary>
        /// Hellgrau - Weiß - Grau
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Schema Clinic(float scale)
        {
            Schema schema = new Schema("Clinic");
            schema.setFarbmuster(1, (int)(10 * scale), Color.LightGray, Color.White, Color.Gray);
            schema.stift = new Pen(Color.Black, scale / 5);
            schema.scale = scale;

            schema.burst = 20 * scale;
            schema.strings = 100;
            schema.sampleRate = 10 * schema.strings;
            schema.background = Color.Black;

            return schema;
        }
        /// <summary>
        /// Schwarz - Dunkelgrau - Grau - Schwarz
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Schema Edgy(float scale)
        {
            Schema schema = new Schema("Edgy");
            schema.setFarbmuster(1, (int)(10 * scale), Color.Black, Color.DarkGray, Color.Gray, Color.Black);
            schema.stift = new Pen(Color.White, scale / 5);
            schema.scale = scale;

            schema.burst = 20 * scale;
            schema.strings = 100;
            schema.sampleRate = 10 * schema.strings;
            schema.background = Color.White;

            return schema;
        }
        /// <summary>
        /// Schwarz - Rot - Schwarz - Dunkelrot
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Schema Vampire(float scale)
        {
            Schema schema = new Schema("Vampire");
            schema.setFarbmuster(1, (int)(10 * scale), Color.Black, Color.Red, Color.Black, Color.DarkRed);
            schema.stift = new Pen(Color.White, scale / 5);
            schema.scale = scale;

            schema.burst = 20 * scale;
            schema.strings = 100;
            schema.sampleRate = 10 * schema.strings;
            schema.background = Color.White;

            return schema;
        }
        /// <summary>
        /// Grau - Schwarz
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Schema Stahl(float scale)
        {
            Schema schema = new Schema("Stahl");
            schema.setFarbmuster(1, (int)(30 * scale), 120, Color.Gray, Color.Black);
            schema.stift = new Pen(Color.White, scale / 5);
            schema.scale = scale;

            schema.burst = scale * 30;
            schema.hohe = t => schema.burst / 8;
            schema.strings = 30;
            schema.sampleRate = 10 * schema.strings;
            schema.background = Color.White;

            return schema;
        }
        /// <summary>
        /// Schwarz - Sandbraun
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Schema Dreck(float scale)
        {
            Schema schema = new Schema("Dreck");
            schema.setFarbmuster(1, (int)(30 * scale), 200, Color.Black, Color.SandyBrown);
            schema.stift = new Pen(Color.White, scale / 5);
            schema.scale = scale;

            schema.burst = scale * 20;
            schema.hohe = t => schema.burst / 8;
            schema.strings = 100;
            schema.sampleRate = 10 * schema.strings;
            schema.background = Color.White;

            return schema;
        }
        public Schema(string name)
        {
            this.name = name;
        }

        public string name;

        public float scale = 1;
        public Brush[] farben;
        public float burst;
        public int strings;
        public Hohe hohe;
        /// <summary>
        /// anzahlSamples/wegLange
        /// </summary>
        public float sampleRate;

        public Pen stift = Pens.Black;
        public Color background;

        public Random random = new Random();

        /// <summary>
        /// Gleichverteilt aus [-1,1]
        /// </summary>
        /// <returns></returns>
        public float NextCentered()
        {
            return (float)(random.NextDouble() * 2 - 1);
        }

        public void setFarbmuster(int anzahlPerioden, int schrittGrose, params Color[] farbe)
        {
            int n = farbe.Length - 1;
            farben = new Brush[anzahlPerioden * schrittGrose * n];
            for (int i = 0; i < n; i++)
            {
                int off = i * schrittGrose;
                for (int j = 0; j < schrittGrose; j++)
                    farben[j + off] = new SolidBrush(farbe[i].tween(farbe[i + 1], j * 1f / schrittGrose));
            }
            wiederholeFarben(n * schrittGrose);
        }
        public void setFarbmuster(int anzahlPerioden, int schrittGrose, int alpha, params Color[] farbe)
        {
            Color[] farbeA = new Color[farbe.Length];
            for (int i = 0; i < farbe.Length; i++)
                farbeA[i] = Color.FromArgb(alpha, farbe[i]);

            setFarbmuster(anzahlPerioden, schrittGrose, farbeA);
        }
        /// <summary>
        /// wiederholt die ersten n Farbwerte für den Rest des farben-Arrays
        /// </summary>
        /// <param name="n"></param>
        private void wiederholeFarben(int n)
        {
            for (int i = 0; i < n; i++)
                for (int j = i + n; j < farben.Length; j += n)
                    farben[j] = farben[i];
        }
        /// <summary>
        /// setzt als farben einen linearen Farbübergang
        /// </summary>
        /// <param name="startfarbe"></param>
        /// <param name="endfarbe"></param>
        /// <param name="anzahlFarben"></param>
        public void setFarbubergang(Color startfarbe, Color endfarbe, int anzahlFarben)
        {
            Color differenz = Color.FromArgb(endfarbe.A - startfarbe.A,
                                            endfarbe.R - startfarbe.R,
                                            endfarbe.G - startfarbe.G,
                                            endfarbe.B - startfarbe.B);
            farben = new Brush[anzahlFarben];
            for (int i = 0; i < anzahlFarben; i++)
            {
                float t = i / (anzahlFarben - 1f);
                farben[i] = new SolidBrush(Color.FromArgb((int)(startfarbe.A + t * differenz.A),
                                            (int)(startfarbe.R + t * differenz.R),
                                            (int)(startfarbe.G + t * differenz.G),
                                            (int)(startfarbe.B + t * differenz.B)));
            }
        }
        /// <summary>
        /// setzt als farben einen linearen Farbübergang
        /// <para>behält einen konstanten AlphaWert bei</para>
        /// </summary>
        /// <param name="startfarbe"></param>
        /// <param name="endfarbe"></param>
        /// <param name="anzahlFarben"></param>
        public void setFarbubergang(Color startfarbe, Color endfarbe, int anzahlFarben, int alphaWert)
        {
            Color a = Color.FromArgb(alphaWert, startfarbe);
            Color b = Color.FromArgb(alphaWert, endfarbe);
            setFarbubergang(a, b, anzahlFarben);
        }

        public void Scale(float scale)
        {
            this.scale *= scale;
            burst *= scale;
            if (hohe != null)
            {
                Hohe alt = (Hohe)hohe.Clone();
                hohe = t => scale * alt(t);
            }
            if (stift != null)
            {
                stift = (Pen)stift.Clone();
                stift.Width *= scale;
            }
        }
    }
}
