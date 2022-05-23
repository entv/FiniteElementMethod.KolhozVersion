

namespace FEM.Domain.Source.Main.TwoDimensional.Math.Function
{
    public class HarmonicalFunction : IHarmonicalFunction
    {
        private readonly Function _cosinePart;
        private readonly Function _sinePart;

        public HarmonicalFunction(Function cosinePart, Function sinePart)
        {
            _cosinePart = cosinePart;
            _sinePart = sinePart;
        }

        public double ValueInPointOnCosinePart(double x, double y) => _cosinePart(x, y);

        public double ValueInPointOnSinePart(double x, double y) => _sinePart(x, y);
    }
}
