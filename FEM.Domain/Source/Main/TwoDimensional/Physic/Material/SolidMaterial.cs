

namespace FEM.Domain.Source.Main.TwoDimensional.Physic.Material
{
    public class SolidMaterial : IMaterial
    {
        private readonly double _lambda;
        private readonly double _gamma;

        public SolidMaterial(double lambda, double gamma)
        {
            _lambda = lambda;
            _gamma = gamma;
        }

        public double Gamma() => _gamma;

        public double Lambda() => _lambda;
    }
}
