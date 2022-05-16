

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public interface IFormattedMatrixFactory<TFormattedMatrix> where TFormattedMatrix : FormattedMatrix
    {
        TFormattedMatrix CreateFormattedMatrix(
            IList<double> diagonalElements, 
            IList<double> upperElements,
            IList<double> lowerElements,
            IList<int> columns,
            IList<int> offsets
            );
    }
}
