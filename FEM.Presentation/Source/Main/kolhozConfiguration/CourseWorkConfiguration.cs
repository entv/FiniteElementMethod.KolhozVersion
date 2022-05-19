using FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid;
using FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement;
using FEM.Domain.Source.Main.TwoDimensional.Math.TaskType;
using FEM.Domain.Source.Main.TwoDimensional.Math.Function;
using FEM.Domain.Source.Main.TwoDimensional.Physic.Force;
using FEM.Domain.Source.Main.TwoDimensional.Physic.Material;
using FEM.Domain.Source.Main.TwoDimensional.Math.Approximation;
using FEM.Domain.Source.Main.TwoDimensional.Physic.TimeLine;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;

namespace FEM.Presentation.Source.Main.Configuration
{
    public static class CourseWorkConfiguration
    {
        public static HyperbolicFourLayerTimeApproximation ConfigureCourseWork(
                 double width,
                double height,
                double timeLength,
                double timeStep,
                TimedFunction u,
                double lambda,
                double gamma,
                double chi,
                double sigma,
                int numberOfFiniteElementsInLine
            )
        {
            var gridFactory = new UniformGridFactory();
            var formattedMatrixFactory = new SparseMatrixFactory();

            var finiteElement = new BiquadraticRectangularFiniteElement();

            var grid = gridFactory.CreateGrid(width, height, numberOfFiniteElementsInLine, finiteElement.CountOfNodes());

            var material = new SolidMaterial(lambda, gamma);
            var force = new TestTimeForce(u, lambda, gamma, sigma, chi);

            var task = new NonStationaryTask(finiteElement, force, material);
            var time = new UniformTimeLine(timeLength, timeStep);
            return new HyperbolicFourLayerTimeApproximation(
                    task,
                    grid,
                    time,
                    chi,
                    sigma,
                    gamma,
                    u
                );
        }
    }
}
