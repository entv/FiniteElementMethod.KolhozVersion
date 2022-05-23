using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes.Formatted;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public class SparseFormattedMatrix : FormattedMatrix, IDecomposableFormattedMatrix, ITransposableFormattedMatrix
    {
        private IList<double> _diagonalElements;
        private IList<double> _upperElements;
        private IList<double> _lowerElements;
        private IList<int> _columns;
        private IList<int> _offsets;

        public SparseFormattedMatrix(IList<double> diagonalElements, IList<double> upperElements, IList<double> lowerElements, IList<int> columns, IList<int> offsets)
        {
            _diagonalElements = diagonalElements;
            _upperElements = upperElements;
            _lowerElements = lowerElements;
            _columns = columns;
            _offsets = offsets;
        }

        public IMatrixDecomposition IncompleteDecomposition(IDecompositionMethod decompositionMethod)
        {
            return decompositionMethod.IncompleteDecomposition(
                _diagonalElements,
                _upperElements,
                _lowerElements,
                _columns,
                _offsets
                );
        }

        public override Vector MultiplyByVector(Vector vector)
        {
            var result = new Vector(vector.Size);

            for (int i = 0; i < vector.Size; i++)
            {
                result[i] += _diagonalElements[i] * vector[i];

                for (int k = _offsets[i]; k < _offsets[i + 1]; k++)
                {
                    result[i] += _lowerElements[k] * vector[_columns[k]];
                    result[_columns[k]] += _upperElements[k] * vector[i];
                }
            }

            return result;
        }

        public FormattedMatrix TransposedMatrix()
        {
            return new SparseFormattedMatrix(
                    _diagonalElements,
                    _lowerElements,
                    _upperElements,
                    _columns,
                    _offsets
                );
        }

        protected override SparseFormattedMatrix FirstBoundary(IEnumerable<int> rows)
        {
            var diagonalElements = new double[_diagonalElements.Count];
            var upperElements = new double[_upperElements.Count];
            var lowerElements = new double[_lowerElements.Count];

            _diagonalElements.CopyTo(diagonalElements, 0);
            _upperElements.CopyTo(upperElements, 0);
            _lowerElements.CopyTo(lowerElements, 0);

            foreach (var row in rows)
            {
                diagonalElements[row] = 1.0;

                for (var i = _offsets[row]; i < _offsets[row + 1]; i++)
                {
                    lowerElements[i] = 0.0;
                }

                for (var i = 0; i < _diagonalElements.Count; i++)
                {
                    var countOfElementsInPreviousColumns = _offsets[i];
                    var countOfElementsInCurrentColumn = _offsets[i + 1] - _offsets[i];
                    if (
                        _columns.Skip(countOfElementsInPreviousColumns)
                                .Take(countOfElementsInCurrentColumn)
                                .Contains(row)
                       )
                    {
                        var indexOfFirstElementInColumn = _offsets[i];
                        var numberOfDesiredElement = countOfElementsInCurrentColumn - _columns.Skip(countOfElementsInPreviousColumns)
                                                                                              .Take(countOfElementsInCurrentColumn)
                                                                                              .SkipWhile(x => x != row)
                                                                                              .Count();
                        upperElements[indexOfFirstElementInColumn + numberOfDesiredElement] = 0.0;
                    }
                }
            }

            return new SparseFormattedMatrix(
                    diagonalElements, 
                    upperElements, 
                    lowerElements, 
                    _columns, 
                    _offsets
                );
        }

        protected override FormattedMatrix MultiplyBy(double coefficient)
        {
            var diagonalElements = new double[_diagonalElements.Count];
            var upperElements = new double[_upperElements.Count];
            var lowerElements = new double[_lowerElements.Count];

            for (int i = 0; i < _diagonalElements.Count; i++)
            {
                diagonalElements[i] = _diagonalElements[i] * coefficient;
            }

            for (int i = 0; i < _upperElements.Count; i++)
            {
                upperElements[i] = _upperElements[i] * coefficient;
            }

            for (int i = 0; i < _lowerElements.Count; i++)
            {
                lowerElements[i] = _lowerElements[i] * coefficient;
            }

            return new SparseFormattedMatrix(diagonalElements, upperElements, lowerElements, _columns, _offsets);
        }

        protected override FormattedMatrix SumWith(FormattedMatrix other)
        {
            try
            {
                var ohterMatrixWithThisType = (SparseFormattedMatrix)other;

                var diagonalElements = new double[_diagonalElements.Count];
                var upperElements = new double[_upperElements.Count];
                var lowerElements = new double[_lowerElements.Count];

                for (int i = 0; i < _diagonalElements.Count; i++)
                {
                    diagonalElements[i] = _diagonalElements[i] + ohterMatrixWithThisType._diagonalElements[i];
                }

                for (int i = 0; i < _upperElements.Count; i++)
                {
                    upperElements[i] = _upperElements[i] + ohterMatrixWithThisType._upperElements[i];
                }

                for (int i = 0; i < _lowerElements.Count; i++)
                {
                    lowerElements[i] = _lowerElements[i] + ohterMatrixWithThisType._lowerElements[i];
                }

                return new SparseFormattedMatrix(diagonalElements, upperElements, lowerElements, _columns, _offsets);
            }
            catch (Exception ex)
            {
                //todo portrait exception, type missmatch exception
                throw new Exception();
            }
        }

        /*public override void FirstCondition(int numberOfLine) 
        {
           
        }*/
    }
}
