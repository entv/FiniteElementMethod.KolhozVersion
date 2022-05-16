using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public class IncompleteLuDecomposition : IMatrixDecomposition
    {
        private IList<double> _diagonalElements;
        private IList<double> _upperElements;
        private IList<double> _lowerElements;
        private IList<int> _columns;
        private IList<int> _offsets;

        public IncompleteLuDecomposition(IList<double> diagonalElements, IList<double> upperElements, IList<double> lowerElements, IList<int> columns, IList<int> offsets)
        {
            _diagonalElements = diagonalElements;
            _upperElements = upperElements;
            _lowerElements = lowerElements;
            _columns = columns;
            _offsets = offsets;
        }

        public Vector ForwardStep(Vector vector)
        {
            var result = new Vector(vector.Size);

            for (int i = 0; i < vector.Size; i++)
            {
                double sum = 0.0;

                for (int j = _offsets[i]; j < _offsets[i + 1]; j++)
                {
                    sum += _lowerElements[j] * vector[_columns[j]];
                }

                result[i] = (vector[i] - sum) / _diagonalElements[i];
            }

            return result;
        }

        public Vector ReverseStep(Vector vector)
        {
            var result = new Vector(vector.Size);
            for (int i = 0; i < vector.Size; i++)
            {
                result[i] = vector[i];
            }

            for (int i = vector.Size - 1; i >= 0; i--)
            {
                for (int j = _offsets[i]; j < _offsets[i + 1]; j++)
                {
                    result[_columns[j]] -= _upperElements[j] * result[i];
                }
            }

            return result;
        }
    }
}
