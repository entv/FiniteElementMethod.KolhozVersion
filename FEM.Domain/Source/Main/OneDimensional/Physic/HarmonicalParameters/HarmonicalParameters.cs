

namespace FEM.Domain.Source.Main.OneDimensional.Physic.Parameters
{
    public class HarmonicalParameters : IHarmonicalParameters
    {
        private readonly double _lambda;
        private readonly double _omega;
        private readonly double _sigma;
        private readonly double _chi;

        public HarmonicalParameters(double lambda, double omega, double sigma, double chi)
        {
            _lambda = lambda;
            _omega = omega;
            _sigma = sigma;
            _chi = chi;
        }

        public double Chi() => _chi;

        public double Lambda() => _lambda;

        public double Omega() => _omega;

        public double Sigma() => _sigma;
    }
}
