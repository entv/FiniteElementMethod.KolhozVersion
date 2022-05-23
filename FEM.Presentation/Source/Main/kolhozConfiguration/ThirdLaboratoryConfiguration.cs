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
            var finiteElement = new HarmonicalFiniteElement(new Domain.Source.Main.OneDimensional.Math.FiniteElement.BilinearHarmonicFiniteElement());

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

            return new HarmonicalApproximation(task, grid, harmonicalFunction);
        }
    }
}
