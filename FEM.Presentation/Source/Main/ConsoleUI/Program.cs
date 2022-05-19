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
            double width = 1.0;
            double height = 1.0;
            var countOfElementsInLine = 80;//5 10 20 40 80
            var countOfElements = countOfElementsInLine * countOfElementsInLine;

            double time = 1.0;
            double timeStep = .1;

            double lambda = 1.0;
            double gamma = 1.0;

            double sigma = 1.0;
            double chi = 1.0;

            TimedFunction u = (x, y, t) => Math.Exp(x);//Math.Pow(t, 2)

            var approximation = CourseWorkConfiguration.ConfigureCourseWork(
                    width,
                    height,
                    time,
                    timeStep,
                    u,
                    lambda,
                    gamma,
                    chi,
                    sigma,
                    countOfElementsInLine
                );

            foreach (var result in approximation.Solution(new SparseMatrixFactory(), new LocallyOptimalScheme()))
            {
                for (int i = 0; i < result.Size; i++)
                {
                    //Console.Write(result[i] + " ");
                }
                //Console.WriteLine();
                Console.WriteLine("<----------------------------------------------------->");
            }
        }
    }
}

/*
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
 */
