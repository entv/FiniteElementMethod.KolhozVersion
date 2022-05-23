using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.OneDimensional.Physic.Parameters;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Nodes;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement
{
    public interface IHarmonicalFiniteElement
    {
        int CountOfNodes();
        Matrix MatrixOnElement(double width, double height, IHarmonicalParameters harmonicParameters);
        Matrix BaseMassMatrixOnElement(double width, double height);
    }
}
