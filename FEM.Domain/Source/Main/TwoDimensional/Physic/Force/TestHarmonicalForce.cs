

using FEM.Domain.Source.Main.TwoDimensional.Math.Function;

namespace FEM.Domain.Source.Main.TwoDimensional.Physic.Force
{
    public class TestHarmonicalForce : IHarmonicalForce
    {
        private readonly IHarmonicalFunction _baseFunction;
        private readonly double _lambda;
        private readonly double _omega;
        private readonly double _sigma;
        private readonly double _chi;
        private const double _h = 1e-5;

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

        public double ValueInPointOnCosinePart(double x, double y)
        {
            var secondDerivativeByX = (-_baseFunction.ValueInPointOnCosinePart(x + 2 * _h, y) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(x + _h, y) -
                                   30 * _baseFunction.ValueInPointOnCosinePart(x, y) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(x - _h, y) -
                                   _baseFunction.ValueInPointOnCosinePart(x - 2 * _h, y)) / (12 * _h * _h);

            var secondDerivativeByY = (-_baseFunction.ValueInPointOnCosinePart(x, y + 2 * _h) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(x, y + _h) -
                                   30 * _baseFunction.ValueInPointOnCosinePart(x, y) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(x, y - _h) -
                                   _baseFunction.ValueInPointOnCosinePart(x, y - 2 * _h)) / (12 * _h * _h);

            return -_lambda * (secondDerivativeByX + secondDerivativeByY) +
                    _omega * _sigma * _baseFunction.ValueInPointOnSinePart(x, y) -
                    _omega * _omega * _chi * _baseFunction.ValueInPointOnCosinePart(x, y);
        }

        public double ValueInPointOnSinePart(double x, double y)
        {
            var secondDerivativeByX = (-_baseFunction.ValueInPointOnCosinePart(x + 2 * _h, y) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(x + _h, y) -
                                   30 * _baseFunction.ValueInPointOnCosinePart(x, y) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(x - _h, y) -
                                   _baseFunction.ValueInPointOnCosinePart(x - 2 * _h, y)) / (12 * _h * _h);

            var secondDerivativeByY = (-_baseFunction.ValueInPointOnCosinePart(x, y + 2 * _h) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(x, y + _h) -
                                   30 * _baseFunction.ValueInPointOnCosinePart(x, y) +
                                   16 * _baseFunction.ValueInPointOnCosinePart(x, y - _h) -
                                   _baseFunction.ValueInPointOnCosinePart(x, y - 2 * _h)) / (12 * _h * _h);


            return -_lambda * (secondDerivativeByX + secondDerivativeByY) -
                    _omega * _sigma * _baseFunction.ValueInPointOnCosinePart(x, y) -
                    _omega * _omega * _chi * _baseFunction.ValueInPointOnSinePart(x, y);
        }
    }
}
