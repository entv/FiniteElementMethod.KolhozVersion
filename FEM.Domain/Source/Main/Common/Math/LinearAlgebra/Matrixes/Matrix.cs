using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public class Matrix
    {
        private double[,] _elements;

        public Matrix(int size)
        {
            _elements = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _elements[i, j] = 0.0;
                }
            }
        }

        public int Size => _elements.GetLength(0);

        public double this[int row, int column]
        {
            get
            {
                return _elements[row, column];
            }
            set
            {
                _elements[row, column] = value;
            }
        }

        public static Matrix operator *(double constant, Matrix matrix)
        {
            var result = new Matrix(matrix.Size);

            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = 0; j < matrix.Size; j++)
                {
                    result[i, j] = matrix[i, j] * constant;
                }
            }

            return result;
        }

        public static Matrix operator *(Matrix matrix, double constant)
        {
            var result = new Matrix(matrix.Size);

            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = 0; j < matrix.Size; j++)
                {
                    result[i, j] = matrix[i, j] * constant;
                }
            }

            return result;
        }

        public static Matrix operator +(Matrix first, Matrix second)
        {
            if (first.Size != second.Size)
            {
                //throw new ArgumentException();
            }

            var result = new Matrix(first.Size);

            for (int i = 0; i < first.Size; i++)
            {
                for (int j = 0; j < first.Size; j++)
                {
                    result[i, j] = first[i, j] + second[i, j];
                }
            }

            return result;
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            if (matrix.Size != vector.Size)
            {
                //throw new ArgumentException();
            }

            var result = new Vector(matrix.Size);

            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = 0; j < matrix.Size; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
            }

            return result;
        }
    }
}
