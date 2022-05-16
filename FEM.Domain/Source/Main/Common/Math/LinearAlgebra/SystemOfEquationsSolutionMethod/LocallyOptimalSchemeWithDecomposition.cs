using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod
{
    public class LocallyOptimalSchemeWithDecomposition : ISystemOfEquationSolutionMethod<DecomposableFormattedMatrix>
    {
        private readonly IDecompositionMethod _decompositionMethod;
        private const int _maxIterations = 10000;
        private const double _epsilon = 1e-12;

        public LocallyOptimalSchemeWithDecomposition(IDecompositionMethod decompositionMethod)
        {
            _decompositionMethod = decompositionMethod;
        }

        public Vector SolutionOfSystemWith(DecomposableFormattedMatrix matrix, Vector vector)
        {
            var result = new Vector(vector.Size);

            var r = new Vector(vector.Size);
            var z = new Vector(vector.Size);
            var p = new Vector(vector.Size);
            var tmp = new Vector(vector.Size);

            var matrixDecomposition = matrix.IncompleteDecomposition(_decompositionMethod);

            for (int i = 0; i < vector.Size; i++)
            {
                r[i] = vector[i];
            }
            r = matrixDecomposition.ForwardStep(r);
            z = matrixDecomposition.ReverseStep(r);
            p = matrix.MultiplyByVector(z);
            p = matrixDecomposition.ForwardStep(p);

            

            for (int iteration = 0; iteration < _maxIterations && (r.EuqlideanNorm / vector.EuqlideanNorm) > _epsilon; iteration++)
            {
                double alpha = (p * r) / (p * p);
                r -= alpha * p;
                result += alpha * z;

                tmp = matrixDecomposition.ReverseStep(r);
                tmp = matrix.MultiplyByVector(tmp);
                tmp = matrixDecomposition.ForwardStep(tmp);

                double beta = -((p * tmp) / (p * p));

                p = tmp + beta * p;
                tmp = matrixDecomposition.ReverseStep(r);
                z = tmp + beta * z;
            }

            return result;
        }
    }
}
