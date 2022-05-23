

namespace FEM.Domain.Source.Main.TwoDimensional.Math.Function
{
    public interface IHarmonicalFunction
    {
        double ValueInPointOnCosinePart(double x, double y);
        double ValueInPointOnSinePart(double x, double y);
    }
}
