using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod
{
    public class LocallyOptimalScheme : ISystemOfEquationSolutionMethod<FormattedMatrix>
    {
        private const int _maxIterations = 10000;
        private const double _epsilon = 1e-22;

        public LocallyOptimalScheme()
        {

        }
        public Vector SolutionOfSystemWith(FormattedMatrix matrix, Vector vector)
        {
            var result = new Vector(vector.Size);

            var r = new Vector(vector.Size);
            var z = new Vector(vector.Size);
            var p = new Vector(vector.Size);

            for (int i = 0; i < vector.Size; i++)
            {
                r[i] = vector[i];
                z[i] = r[i];
            }
            p = matrix.MultiplyByVector(z);

            for (int iteration = 0; iteration < _maxIterations; iteration++)
            {
                double alpha = (p * r) / (p * p);
                result += alpha * z;
                r -= alpha * p;
                
                if ((r.EuqlideanNorm / vector.EuqlideanNorm) < _epsilon)
                {
                    return result;
                }

                var Ar = matrix.MultiplyByVector(r);
                double beta = -((p * Ar) / (p * p));

                z = r + beta * z;
                p = Ar + beta * p;
            }

            return result;
        }
    }
}
