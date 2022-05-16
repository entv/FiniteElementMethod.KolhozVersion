using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod
{
    public interface ISystemOfEquationSolutionMethod<TFormattedMatrix> where TFormattedMatrix : FormattedMatrix
    {
        Vector SolutionOfSystemWith(TFormattedMatrix matrix, Vector vector);
    }
}
