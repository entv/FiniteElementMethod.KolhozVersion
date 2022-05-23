

namespace FEM.Domain.Source.Main.TwoDimensional.Physic.Force
{
    public interface IHarmonicalForce
    {
        double ValueInPointOnCosinePart(double x, double y);
        double ValueInPointOnSinePart(double x, double y);
    }
}
