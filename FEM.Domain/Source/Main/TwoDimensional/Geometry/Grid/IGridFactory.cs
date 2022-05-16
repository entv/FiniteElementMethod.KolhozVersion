

namespace FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid
{
    public interface IGridFactory
    {
        IGrid CreateGrid(double width, double height, int accuracy, int countOfNodesOfElement);
    }
}
