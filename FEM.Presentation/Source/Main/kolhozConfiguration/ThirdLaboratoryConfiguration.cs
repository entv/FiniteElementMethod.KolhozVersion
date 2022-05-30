using FEM.Domain.Source.Main.OneDimensional.Physic.Parameters;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid;
using FEM.Domain.Source.Main.TwoDimensional.Math.Approximation;
using FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement;
using FEM.Domain.Source.Main.TwoDimensional.Math.Function;
using FEM.Domain.Source.Main.TwoDimensional.Math.TaskTypes;
using FEM.Domain.Source.Main.TwoDimensional.Physic.Force;

namespace FEM.Presentation.Source.Main.Configuration
{
    public static class ThirdLaboratoryConfiguration
    {
        public static HarmonicalApproximation ConfigureThirdLaboratory(
                double width,
                double height,
                int numberOfFiniteElementsInLine,
                double lambda,
                double omega,
                double sigma,
                double chi,
                Function uSinePart,
                Function uCosPart
            )
        {
            var gridFactory = new UniformGridFactory();
            var finiteElement = new HarmonicalFiniteElement();

            var grid = gridFactory.CreateGrid(
                    width,
                    height,
                    numberOfFiniteElementsInLine,
                    finiteElement.CountOfNodes()
                );

            var parameters = new HarmonicalParameters(
                    lambda,
                    omega,
                    sigma,
                    chi
                );

            var harmonicalFunction = new HarmonicalFunction(uCosPart, uSinePart);

            var force = new TestHarmonicalForce(harmonicalFunction, lambda, omega, sigma, chi);
            var task = new HarmonicalTask(finiteElement, parameters, force);

            Console.WriteLine($"count of elements: {grid.CountOfElements()}");
            Console.WriteLine($"count of nodes: {grid.CountOfNodes()}");
            Console.WriteLine($"lambda: {lambda:e2}, omega: {omega:e2}, sigma: {sigma:e2}, chi: {chi:e2}");

            return new HarmonicalApproximation(task, grid, harmonicalFunction);
        }
    }
}
