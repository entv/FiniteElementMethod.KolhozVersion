

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public abstract class DecomposableFormattedMatrix : FormattedMatrix
    {
        public abstract IMatrixDecomposition IncompleteDecomposition(IDecompositionMethod decompositionMethod);
    }
}
