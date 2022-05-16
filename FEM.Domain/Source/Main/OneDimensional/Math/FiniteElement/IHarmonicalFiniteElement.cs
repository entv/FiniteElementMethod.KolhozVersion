using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.OneDimensional.Physic.Parameters;

namespace FEM.Domain.Source.Main.OneDimensional.Math.FiniteElement
{
    public interface IHarmonicalFiniteElement
    {
        int CountOfNodes();
        Matrix MatrixOnElement(double length, IHarmonicalParameters harmonicParameters);
        Matrix BaseMassMatrixOnElement(double length);
    }
}
