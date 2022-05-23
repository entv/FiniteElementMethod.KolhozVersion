

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public interface IDecomposableFormattedMatrix
    {
        public IMatrixDecomposition IncompleteDecomposition(IDecompositionMethod decompositionMethod);
    }
}
