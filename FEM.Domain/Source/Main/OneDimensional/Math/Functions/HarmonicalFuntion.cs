

namespace FEM.Domain.Source.Main.OneDimensional.Math.Functions
{
    public class HarmonicalFuntion : IHarmonicalFunction
    {
        private readonly Function _cosinePart;
        private readonly Function _sinePart;

        public HarmonicalFuntion(Function cosinePart, Function sinePart)
        {
            _cosinePart = cosinePart;
            _sinePart = sinePart;
        }

        public double ValueInPointOnCosinePart(double point) => _cosinePart(point);

        public double ValueInPointOnSinePart(double point) => _sinePart(point);
    }
}
