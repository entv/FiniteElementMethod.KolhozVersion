using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.Approximation
{
    public interface ITimeApproximation
    {
        IEnumerable<Vector> Solution(IFormattedMatrixFactory<FormattedMatrix> factory, ISystemOfEquationSolutionMethod<FormattedMatrix> systemOfEquation);
    }
}
