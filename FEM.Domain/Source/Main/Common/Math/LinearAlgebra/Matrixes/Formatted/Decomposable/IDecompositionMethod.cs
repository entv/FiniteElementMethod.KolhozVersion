

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public interface IDecompositionMethod
    {
        IMatrixDecomposition IncompleteDecomposition(
            IList<double> diagonalElements,
            IList<double> upperElements,
            IList<double> lowerElements,
            IList<int> columns,
            IList<int> offsets
            );
    }
}
