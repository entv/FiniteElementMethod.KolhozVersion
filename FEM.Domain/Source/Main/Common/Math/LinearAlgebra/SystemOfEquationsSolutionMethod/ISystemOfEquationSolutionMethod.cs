using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod
{
    public interface ISystemOfEquationSolutionMethod<TFormattedMatrix>
    {
        Vector SolutionOfSystemWith(TFormattedMatrix matrix, Vector vector);
    }
}
