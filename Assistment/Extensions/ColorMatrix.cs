namespace Assistment.Extensions
{
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
}
