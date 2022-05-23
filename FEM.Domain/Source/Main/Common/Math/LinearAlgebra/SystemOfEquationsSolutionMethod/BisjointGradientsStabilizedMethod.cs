﻿using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes.Formatted;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod
{
    public class BisjointGradientsStabilizedMethod<TFormattedMatrix> : ISystemOfEquationSolutionMethod<TFormattedMatrix> where TFormattedMatrix : FormattedMatrix, ITransposableFormattedMatrix
    {
        private const int _maxIterations = 10000;
        private const double _epsilon = 1e-12;
        
        public BisjointGradientsStabilizedMethod()
        {
            
        }
        public Vector SolutionOfSystemWith(TFormattedMatrix matrix, Vector vector)
        {
            var transposedMatrix = matrix.TransposedMatrix();

            var result = new Vector(vector.Size);

            var r = new Vector(vector.Size);
            var z = new Vector(vector.Size);
            var p = new Vector(vector.Size);
            var s = new Vector(vector.Size);
            for (int i = 0; i < vector.Size; i++)
            {
                r[i] = vector[i];
                p[i] = z[i] = s[i] = r[i];
            }

            for (int iteration = 0; iteration < _maxIterations; iteration++)
            {
                double alpha = (p * r) / (s * matrix.MultiplyByVector(z));

                result += alpha * z;
                var pr_k1 = p * r;
                r -= alpha * matrix.MultiplyByVector(z);
                p -= alpha * transposedMatrix.MultiplyByVector(s);

                if ((matrix.MultiplyByVector(result) - vector).EuqlideanNorm / vector.EuqlideanNorm < _epsilon)
                {
                    return result;
                }

                double beta = (p * r) / pr_k1;
                z = r + beta * z;
                s = p + beta * s;
            }

            return result;
        }
    }
}
