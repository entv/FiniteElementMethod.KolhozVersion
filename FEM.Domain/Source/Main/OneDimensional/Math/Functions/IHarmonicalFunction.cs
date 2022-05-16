

namespace FEM.Domain.Source.Main.OneDimensional.Math.Functions
{
    public interface IHarmonicalFunction
    {
        double ValueInPointOnCosinePart(double point);
        double ValueInPointOnSinePart(double point);
    }
}
