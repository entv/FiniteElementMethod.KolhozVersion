using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.OneDimensional.Geometry.Grid;
using FEM.Domain.Source.Main.OneDimensional.Math.Functions;
using FEM.Domain.Source.Main.OneDimensional.Math.TaskTypes;

namespace FEM.Domain.Source.Main.OneDimensional.Math.Approximation
{
    public class HarmonicalApproximation : IApproximation
    {
        private IHarmonicalTask _task;
        private readonly IGrid _grid;
        private readonly IHarmonicalFunction _u;

        public HarmonicalApproximation(
                IHarmonicalTask task,
                IGrid grid,
                IHarmonicalFunction u
            )
        {
            _task = task;
            _grid = grid;
            _u = u;
        }
        public Vector Solution(IFormattedMatrixFactory<FormattedMatrix> factory, ISystemOfEquationSolutionMethod<FormattedMatrix> systemOfEquation)
        {
            var force = _task.ForceVectorOnGrid(_grid);
            var matrix = _task.MatrixOnGrid(_grid, factory);

            var firstConditionsNodes = _grid.BorderNodes()
                                       .Select(node => 2 * node.Id)
                                       .Concat(_grid.BorderNodes().Select(node => 2 * node.Id + 1))
                                       .OrderBy(x => x);

            foreach(var node in _grid.BorderNodes())
            {
                force[2 * node.Id] = _u.ValueInPointOnCosinePart(node.Value);
                force[2 * node.Id + 1] = _u.ValueInPointOnSinePart(node.Value);
            }
            var matrixWithBoundaryConditions = matrix.WithFirstBoundaryCondition(firstConditionsNodes);

            return systemOfEquation.SolutionOfSystemWith(matrixWithBoundaryConditions, force);
        } 
    }
}
