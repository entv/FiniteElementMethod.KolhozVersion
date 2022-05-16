using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElementMethod
{
    public interface IFiniteElementMethod
    {
        Vector Solution();
    }
}
