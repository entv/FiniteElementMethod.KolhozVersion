using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.SystemOfEquationsSolutionMethod;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid;
using FEM.Domain.Source.Main.TwoDimensional.Math.Function;
using FEM.Domain.Source.Main.TwoDimensional.Math.TaskType;
using FEM.Domain.Source.Main.TwoDimensional.Physic.TimeLine;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.Approximation
{
    public class HyperbolicFourLayerImplicitTimeApproximation : ITimeApproximation
    {
        private readonly INonStationaryTask _nonStationaryTask;
        private readonly IGrid _grid;
        private readonly ITimeLine _timeLine;
        private double _chi;
        private double _sigma;
        private double _gamma;
        private TimedFunction _u;

        public HyperbolicFourLayerImplicitTimeApproximation(
                INonStationaryTask nonStationaryTask,
                IGrid grid,
                ITimeLine timeLine,
                double chi,
                double sigma,
                double gamma,
                TimedFunction u
            )
        {
            _nonStationaryTask = nonStationaryTask;
            _grid = grid;
            _timeLine = timeLine;
            _chi = chi;
            _sigma = sigma;
            _gamma = gamma;
            _u = u;
        }
        public IEnumerable<Vector> Solution(IFormattedMatrixFactory<FormattedMatrix> factory, ISystemOfEquationSolutionMethod<FormattedMatrix> systemOfEquation)
        {
            var mass = _nonStationaryTask.MassMatrixOnGrid(factory, _grid);
            var stiffness = _nonStationaryTask.StiffnessMatrixOnGrid(factory, _grid);

            var q1 = new Vector(_grid.CountOfNodes());
            var q2 = new Vector(_grid.CountOfNodes());
            var q3 = new Vector(_grid.CountOfNodes());

            for (var elementNumber = 0; elementNumber < _grid.CountOfElements(); elementNumber++)
            {
                foreach (var node in _grid.NodesOfElementByNumber(elementNumber))
                {
                    q1[node.Id] = _u(node.X, node.Y, _timeLine.TimeOnLayerByNumber(2));
                    q2[node.Id] = _u(node.X, node.Y, _timeLine.TimeOnLayerByNumber(1));
                    q3[node.Id] = _u(node.X, node.Y, _timeLine.TimeOnLayerByNumber(0));
                }
            }

            yield return q3;
            yield return q2;
            yield return q1;

            for (var layer = 3; layer < _timeLine.CountOfLayers(); layer++)
            {
                var t0 = _timeLine.TimeOnLayerByNumber(layer);
                var t1 = _timeLine.TimeOnLayerByNumber(layer - 1);
                var t2 = _timeLine.TimeOnLayerByNumber(layer - 2);
                var t3 = _timeLine.TimeOnLayerByNumber(layer - 3);

                var t03 = t0 - t3;
                var t02 = t0 - t2;
                var t01 = t0 - t1;

                var t10 = t1 - t0;
                var t12 = t1 - t2;
                var t13 = t1 - t3;

                var t32 = t3 - t2;
                var t31 = t3 - t1;
                var t30 = t3 - t0;

                var t23 = t2 - t3;
                var t21 = t2 - t1;
                var t20 = t2 - t0;

                var matrixApproximationCoefficient = 2 * _chi * (t01 + t02 + t03) / (t03 * t02 * t01) +
                                                     _sigma * (1.0 / t01 + 1.0 / t02 + 1.0 / t03) + _gamma;

                var approximatedMatrix = matrixApproximationCoefficient * mass + stiffness;

                var force = _nonStationaryTask.ForceVectorOnGrid(t0, _grid);

                var approximatedForce = force -
                                        mass.MultiplyByVector((2 * _chi * (t01 + t02) / (t32 * t31 * t30) + _sigma * (t01 * t02) / (t32 * t31 * t30)) * q3) -
                                        mass.MultiplyByVector((2 * _chi * (t01 + t03) / (t23 * t21 * t20) + _sigma * (t01 * t03) / (t23 * t21 * t20)) * q2) -
                                        mass.MultiplyByVector((2 * _chi * (t02 + t03) / (t13 * t12 * t10) + _sigma * (t02 * t03) / (t13 * t12 * t10)) * q1);

                foreach (var node in _grid.BorderNodes())
                {
                    approximatedForce[node.Id] = _u(node.X, node.Y, t0);
                }
                var approximatedMatrixWithBoundaryConditions = approximatedMatrix.WithFirstBoundaryCondition(
                                                                                    _grid.BorderNodes()
                                                                                    .Select(node => node.Id)
                                                                                  );

                var q = systemOfEquation.SolutionOfSystemWith(approximatedMatrixWithBoundaryConditions, approximatedForce);

                q3 = q2;
                q2 = q1;
                q1 = q;

                yield return q;
            }
        }
    }
}
