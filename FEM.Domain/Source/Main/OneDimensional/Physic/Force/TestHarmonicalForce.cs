using FEM.Domain.Source.Main.OneDimensional.Math.Functions;

namespace FEM.Domain.Source.Main.OneDimensional.Physic.Force
{
    public class TestHarmonicalForce : IHarmonicalForce
    {
        private readonly IHarmonicalFunction _baseFunction;
        private readonly double _lambda;
        private readonly double _omega;
        private readonly double _sigma;
        private readonly double _chi;
        private const double _h = 1e-3;

        public TestHarmonicalForce(
                IHarmonicalFunction baseFunction,
                double lambda,
                double omega,
                double sigma,
                double chi
            )
        {
            _baseFunction = baseFunction;
            _lambda = lambda;
            _omega = omega;
            _sigma = sigma;
            _chi = chi;
        }
        public double ValueInPointOnCosinePart(double point)
        {
            var secondDerivative = (-_baseFunction.ValueInPointOnCosinePart(point + 2 * _h) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(point + _h) -
                                   30 * _baseFunction.ValueInPointOnCosinePart(point) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(point - _h) -
                                   _baseFunction.ValueInPointOnCosinePart(point - 2 * _h)) / (12 * _h * _h);

            return -_lambda * secondDerivative +
                    _omega * _sigma * _baseFunction.ValueInPointOnSinePart(point) -
                    _omega * _omega * _chi * _baseFunction.ValueInPointOnCosinePart(point);
        }

        public double ValueInPointOnSinePart(double point)
        {
            var secondDerivative = (-_baseFunction.ValueInPointOnSinePart(point + 2 * _h) +
                                   16 * _baseFunction.ValueInPointOnSinePart(point + _h) -
                                   30 * _baseFunction.ValueInPointOnSinePart(point) +
                                   16 * _baseFunction.ValueInPointOnSinePart(point - _h) -
                                   _baseFunction.ValueInPointOnSinePart(point - 2 * _h)) / (12 * _h * _h);

            return -_lambda * secondDerivative -
                    _omega * _sigma * _baseFunction.ValueInPointOnCosinePart(point) -
                    _omega * _omega * _chi * _baseFunction.ValueInPointOnSinePart(point);
        }
    }
}
