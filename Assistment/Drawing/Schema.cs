using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Drawing.LinearAlgebra;
using System.Drawing;

namespace Assistment.Drawing
{
    public struct Schema
    {
        public Brush[] farben;
        public float burst;
        public int strings;
        public Hohe hohe;
        /// <summary>
        /// anzahlSamples/wegLange
        /// </summary>
        public float sampleRate;


        public void setFarbmuster(int anzahlPerioden, int schrittGrose, params Color[] farbe)
        {
            int n = farbe.Length - 1;
            farben = new Brush[anzahlPerioden * schrittGrose * n];
            for (int i = 0; i < n; i++)
            {
                int off = i * schrittGrose;
                Color basis = farbe[i];
                Color diff = farbe[i + 1].sub(basis);

                for (int j = 0; j < schrittGrose; j++)
                    farben[j + off] = new SolidBrush(basis.saxpy(j * 1f / schrittGrose, diff));
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
    }
}
