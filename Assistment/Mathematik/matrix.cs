using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Mathematik
{
    public abstract class matrix
    {
        public int Rows
        {
            get
            {
                return GetRows();
            }
        }
        public int Columns
        {
            get
            {
                return GetColumns();
            }
        }

        public float this[int Row, int Column]
        {
            get { return GetValue(Row, Column); }
            set { SetValue(Row, Column, value); }
        }
        public float this[int Row]
        {
            get { return GetValue(Row, 0); }
            set { SetValue(Row, 0, value); }
        }
        

        public abstract float GetValue(int Row, int Column);
        public abstract void SetValue(int Row, int Column, float Value);

        public abstract int GetRows();
        public abstract int GetColumns();

        /// <summary>
        /// return this * B
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public virtual matrix Mult(matrix B)
        {
            float[,] M = new float[this.Rows, B.Columns];
            for (int i = 0; i < this.Rows; i++)
                for (int j = 0; j < B.Columns; j++)
                    for (int k = 0; k < this.Columns; k++)
                        M[i, j] += this[i, k] * B[k, j];
            return M;
        }

        public virtual float[,] GetValues()
        {
            float[,] arr = new float[Rows, Columns];
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    arr[i, j] = this[i, j];
            return arr;
        }

        public static implicit operator float[,](matrix Matrix)
        {
            return Matrix.GetValues();
        }
        public static implicit operator matrix(float[,] Werte)
        {
            return new RawMatrix(Werte);
        }
        public static implicit operator matrix(float[] Werte)
        {
            return new RawVector(Werte);
        }

        public static matrix operator *(matrix A, matrix B)
        {
            return A.Mult(B);
        }
    }
    /// <summary>
    /// Ein Vektor in Spaltenform: v[i,0] = i-ter Wert
    /// </summary>
    public class RawVector : matrix
    {
        public float[] Werte;

        public RawVector(float[] Werte)
        {
            this.Werte = Werte;
        }

        public override float GetValue(int Row, int Column)
        {
            return Werte[Row];
        }

        public override void SetValue(int Row, int Column, float Value)
        {
            Werte[Row] = Value;
        }

        public override int GetRows()
        {
            return Werte.Length;
        }

        public override int GetColumns()
        {
            return 1;
        }
    }


    public class RawMatrix : matrix
    {
        public float[,] Werte;

        public RawMatrix(float[,] Werte)
        {
            this.Werte = Werte;
        }

        public override float GetValue(int Row, int Column)
        {
            return Werte[Row, Column];
        }
        public override void SetValue(int Row, int Column, float Value)
        {
            Werte[Row, Column] = Value;
        }

        public override int GetRows()
        {
            return Werte.GetLength(0);
        }
        public override int GetColumns()
        {
            return Werte.GetLength(1);
        }
    }
}
