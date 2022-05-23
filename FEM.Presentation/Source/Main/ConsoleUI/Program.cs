using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod;
using FEM.Domain.Source.Main.TwoDimensional.Math.Function;
using FEM.Presentation.Source.Main.Configuration;

namespace FEM.Presentation.Source.Main.ConsoleUI
{
    public class Program
    {
        public static void Main()
        {
            var width = 1.0;
            var height = 1.0;
            var countOfElementsInLine = 10;

            double lambda = 1;
            double omega = 1;
            double sigma = 1;
            double chi = 0;

            Function uSinePart = (x, y) => x;
            Function uCosPart = (x, y) => 1;

            var approximation = ThirdLaboratoryConfiguration.ConfigureThirdLaboratory(
                        width,
                        height,
                        countOfElementsInLine,
                        lambda,
                        omega,
                        sigma,
                        chi,
                        uSinePart,
                        uCosPart
                    );

            var result = approximation.Solution(new SparseMatrixFactory(), new LocallyOptimalScheme<SparseFormattedMatrix>());
            //var result = approximation.Solution(new SparseMatrixFactory(), new BisjointGradientsStabilizedMethod<SparseFormattedMatrix>());

            //Console.WriteLine("<-------------------------------------------------------------------------->");
            for (int i = 0; i < result.Size; i++)
            {
                //Console.Write(result[i] + " ");
            }
            //Console.WriteLine();
            //Console.WriteLine("<-------------------------------------------------------------------------->");

            for (int i = 0; i < result.Size / 2; i++)
            {
                //Console.Write(result[2 * i] + result[2 * i + 1] + " ");
            }
        }
    }
}

/*
  var result = approximation.Solution(new SparseMatrixFactory(), new LocallyOptimalScheme());

            for (int i = 0; i < result.Size; i++)
            {
                Console.WriteLine(result[i]);
            }

            Console.WriteLine("<-------------------------------------------------------------------------->");

            for (int i = 0; i < result.Size / 2; i++)
            {
                Console.WriteLine(result[2 * i] + result[2 * i + 1]);
            }
 */
