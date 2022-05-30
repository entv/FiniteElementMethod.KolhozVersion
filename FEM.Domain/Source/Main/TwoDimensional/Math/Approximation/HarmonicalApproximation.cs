using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid;
using FEM.Domain.Source.Main.TwoDimensional.Math.Function;
using FEM.Domain.Source.Main.TwoDimensional.Math.TaskTypes;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.Approximation
{
    public class HarmonicalApproximation : IApproximation
    {
        private IHarmonicalTask _task;
        private readonly IGrid _grid;
        private readonly IHarmonicalFunction _u;

        public HarmonicalApproximation(IHarmonicalTask task, IGrid grid, IHarmonicalFunction u)
        {
            _task = task;
            _grid = grid;
            _u = u;
        }

        public Vector Solution<TFormattedMatrix>(IFormattedMatrixFactory<TFormattedMatrix> factory, ISystemOfEquationSolutionMethod<TFormattedMatrix> systemOfEquation) where TFormattedMatrix : FormattedMatrix
        {
            var force = _task.ForceVectorOnGrid(_grid);
            var matrix = _task.MatrixOnGrid(_grid, factory);

            var firstConditionsNodes = _grid.BorderNodes()
                                       .Select(node => 2 * node.Id)
                                       .Concat(_grid.BorderNodes().Select(node => 2 * node.Id + 1))
                                       .OrderBy(x => x).ToList();

            var test = new Vector(force.Size);
            for (int elementNumber = 0; elementNumber < _grid.CountOfElements(); elementNumber++)
            {
                foreach (var node in _grid.NodesOfElementByNumber(elementNumber))
                {
                    test[2 * node.Id] = _u.ValueInPointOnSinePart(node.X, node.Y);
                    test[2 * node.Id + 1] = _u.ValueInPointOnCosinePart(node.X, node.Y);
                }
            }

            foreach (var node in _grid.BorderNodes())
            {
                force[2 * node.Id] = _u.ValueInPointOnSinePart(node.X, node.Y);
                force[2 * node.Id + 1] = _u.ValueInPointOnCosinePart(node.X, node.Y);
            }
            var matrixWithBoundaryConditions = matrix.WithFirstBoundaryCondition<TFormattedMatrix>(firstConditionsNodes);

            var testRes = matrixWithBoundaryConditions.MultiplyByVector(test);

            var solution = systemOfEquation.SolutionOfSystemWith(matrixWithBoundaryConditions, force);

            var error = new Vector(solution.Size);
            for (int elementNumber = 0; elementNumber < _grid.CountOfElements(); elementNumber++)
            {
                foreach(var node in _grid.NodesOfElementByNumber(elementNumber))
                {
                    error[2 * node.Id] = solution[2 * node.Id] - _u.ValueInPointOnSinePart(node.X, node.Y);
                    error[2 * node.Id + 1] = solution[2 * node.Id + 1] - _u.ValueInPointOnCosinePart(node.X, node.Y);
                }
            }
            Console.WriteLine($"error: {error.EuqlideanNorm / error.Size:#.##e+0}");

            return solution;
        }
    }
}
