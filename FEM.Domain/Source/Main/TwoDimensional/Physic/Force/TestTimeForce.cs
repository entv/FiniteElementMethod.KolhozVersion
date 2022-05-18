using FEM.Domain.Source.Main.TwoDimensional.Math.Function;

namespace FEM.Domain.Source.Main.TwoDimensional.Physic.Force
{
    public class TestTimeForce : ITimedForce
    {
        private readonly TimedFunction _baseFunction;
        private const double _h = 1e-5;

        private readonly double _lambda;
        private readonly double _gamma;
        private readonly double _sigma;
        private readonly double _chi;

        public TestTimeForce(
            TimedFunction baseFunction,
            double lambda,
            double gamma,
            double sigma,
            double chi
            )
        {
            _baseFunction = baseFunction;
            _lambda = lambda;
            _gamma = gamma;
            _sigma = sigma;
            _chi = chi;
        }
        public double ValueInPoint(double x, double y, double time)
        {
            var firstDerivativeByTime = (-_baseFunction(x, y, time + 2 * _h) + 
                                        8 * _baseFunction(x, y, time + _h) -
                                        8 * _baseFunction(x, y, time - _h) +
                                        _baseFunction(x, y, time - 2 * _h)) / (12 * _h);

            var secondDerivativeByX = (-_baseFunction(x + 2 * _h, y, time) +
                                      16 * _baseFunction(x + _h, y, time) -
                                      30 * _baseFunction(x, y, time) +
                                      16 * _baseFunction(x - _h, y, time) -
                                      _baseFunction(x - 2 * _h, y, time)) / (12 * _h * _h);

            var secondDerivativeByY = (-_baseFunction(x, y + 2 * _h, time) +
                                      16 * _baseFunction(x, y + _h, time) -
                                      30 * _baseFunction(x, y, time) +
                                      16 * _baseFunction(x, y - _h, time) -
                                      _baseFunction(x, y - 2 * _h, time)) / (12 * _h * _h);

            var secondDerivativeByTime = (-_baseFunction(x, y, time + 2 * _h) +
                                      16 * _baseFunction(x, y, time + _h) -
                                      30 * _baseFunction(x, y, time) +
                                      16 * _baseFunction(x, y, time - _h) -
                                      _baseFunction(x, y, time - 2 * _h)) / (12 * _h * _h);
                
            return -_lambda * (secondDerivativeByX + secondDerivativeByY) +
                    _gamma * _baseFunction(x, y, time) +
                    _sigma * firstDerivativeByTime + _chi * secondDerivativeByTime;
        }
    }
}
