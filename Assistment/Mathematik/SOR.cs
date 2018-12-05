namespace Assistment.Mathematik
{
    public class SOR : LinearSolver
    {
        public float w = 1;
        public matrix A;
        public matrix b;
        public matrix x;

        public int n = 0;

        public SOR(matrix A, matrix b, matrix x, float w)
        {
            this.A = A;
            this.b = b;
            this.x = x;
            this.w = w;
        }

        public int IterationNumber()
        {
            return n;
        }

        public matrix Iterate()
        {
            matrix Ax = A * x;

            n++;

            float[] xn = new float[A.Rows];
            for (int i = 0; i < A.Rows; i++)
            {
                float subsum = 0;
                for (int j = 0; j < i; j++)
                    subsum += A[i, j] * xn[j];

                xn[i] = x[i] + w / A[i, i] * (b[i] - subsum - Ax[i]);
            }

            x = xn;
            return x;
        }
    }
}
