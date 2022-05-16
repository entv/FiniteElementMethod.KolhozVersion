using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public interface IMatrixDecomposition
    {
        Vector ForwardStep(Vector vector);
        Vector ReverseStep(Vector vector);
    }
}
