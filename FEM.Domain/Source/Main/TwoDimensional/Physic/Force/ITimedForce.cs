

namespace FEM.Domain.Source.Main.TwoDimensional.Physic.Force
{
    public interface ITimedForce
    {
        double ValueInPoint(double x, double y, double time);
    }
}
