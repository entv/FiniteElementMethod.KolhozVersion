

using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes.Formatted;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public class SparseMatrixFactory : IFormattedMatrixFactory<SparseFormattedMatrix>
    {
        public SparseFormattedMatrix CreateFormattedMatrix(IList<double> diagonalElements, IList<double> upperElements, IList<double> lowerElements, IList<int> columns, IList<int> offsets)
        {
            return new SparseFormattedMatrix(
                diagonalElements,
                upperElements,
                lowerElements,
                columns,
                offsets
                );
        }
    }
}
