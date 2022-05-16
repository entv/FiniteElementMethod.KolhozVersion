

namespace FEM.Domain.Source.Main.TwoDimensional.Physic.TimeLine
{
    public interface ITimeLine
    {
        int CountOfLayers();
        double TimeOnLayerByNumber(int number);
    }
}
