

namespace FEM.Domain.Source.Main.OneDimensional.Physic.Force
{
    public interface IHarmonicalForce
    {
        double ValueInPointOnCosinePart(double point);
        double ValueInPointOnSinePart(double point);
    }
}
