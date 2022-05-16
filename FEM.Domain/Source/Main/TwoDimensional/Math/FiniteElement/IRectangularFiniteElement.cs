using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.TwoDimensional.Physic.Material;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement
{
    public interface IRectangularFiniteElement
    {
        int CountOfNodes();
        Matrix MassMatrixOnElement(double width, double height, IMaterial material);
        Matrix StiffnessMatrixOnElement(double width, double height, IMaterial material);
        Matrix MassMatrixWithoutMaterial(double width, double height);
    }
}
