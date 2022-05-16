using FEM.Domain.Source.Main.OneDimensional.Geometry.Grid.Factories;
using FEM.Domain.Source.Main.OneDimensional.Math.Approximation;
using FEM.Domain.Source.Main.OneDimensional.Math.FiniteElement;
using FEM.Domain.Source.Main.OneDimensional.Math.Functions;
using FEM.Domain.Source.Main.OneDimensional.Math.TaskTypes;
using FEM.Domain.Source.Main.OneDimensional.Physic.Force;
using FEM.Domain.Source.Main.OneDimensional.Physic.Parameters;

namespace FEM.Presentation.Source.Main.Configuration
{
    public static class ThirdLaboratoryConfiguration
    {
        public static HarmonicalApproximation ConfigureThirdLaboratory(
                double length,
                int countOfElements,
                double lambda,
                double omega,
                double sigma,
                double chi,
                Function uSinePart,
                Function uCosPart
            )
        {
            var gridFactory = new UniformGridFactory();
            var finiteElement = new BilinearHarmonicFiniteElement();

            var grid = gridFactory.CreateGrid(
                    length,
                    finiteElement.CountOfNodes(),
                    countOfElements
                );

            for (int i = 0; i < countOfElements; i++)
            {
                foreach (var node in grid.NodesOfElementByNumber(i))
                {
                    //Console.WriteLine(node.Id + "->" + node.Value);
                }
            }

            var parameters = new HarmonicalParameters(
                    lambda,
                    omega,
                    sigma,
                    chi
                );

            var harmonicalFunction = new HarmonicalFuntion(uCosPart, uSinePart);

            var force = new TestHarmonicalForce(harmonicalFunction, lambda, omega, sigma, chi);

            var task = new HarmonicalTask(finiteElement, parameters, force);

            return new HarmonicalApproximation(task, grid, harmonicalFunction);
        }
    }
}
