using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.Approximation
{
    public interface IApproximation
    {
        Vector Solution<TFormattedMatrix>(IFormattedMatrixFactory<TFormattedMatrix> factory, ISystemOfEquationSolutionMethod<TFormattedMatrix> systemOfEquation) where TFormattedMatrix : FormattedMatrix;
    }
}
