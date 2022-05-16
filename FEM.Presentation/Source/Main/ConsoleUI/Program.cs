using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod;
using FEM.Domain.Source.Main.OneDimensional.Math.Functions;
using FEM.Presentation.Source.Main.Configuration;

namespace FEM.Presentation.Source.Main.ConsoleUI
{
    public class Program
    {
        public static void Main()
        {
            var length = 1000.0;
            var countOfElements = 1000;

            double lambda = 1;
            double omega = 1;
            double sigma = 0;
            double chi = 1;

            Function uSinePart = (x) => x / 2;
            Function uCosPart = (x) => x / 2;

            var approximation = ThirdLaboratoryConfiguration.ConfigureThirdLaboratory(
                        length, 
                        countOfElements,
                        lambda,
                        omega,
                        sigma,
                        chi,
                        uSinePart,
                        uCosPart
                    );

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
        }
    }
}

/*double width = 4.0;
            double height = 4.0;
            var countOfElementsInLine = 3;

            double lambda = 1.0;
            double gamma = 1.0;

            double sigma = 1.0;
            double chi = 1.0;

            TimedFunction u = (x, y, t) => t;

            var FiniteElementMethod = ConfigureCourseWork(
                    width,
                    height,
                    u,
                    lambda,
                    gamma,
                    chi,
                    sigma,
                    countOfElementsInLine
                );

            foreach(var result in FiniteElementMethod.Solution(new SparseMatrixFactory(), new LocallyOptimalScheme()))
            {
                for (int i = 0; i < result.Size; i++)
                {
                    Console.Write(result[i] + " ");
                }
                Console.WriteLine();
            }*/
