

namespace FEM.Domain.Source.Main.OneDimensional.Geometry.Grid.Factories
{
    public interface IGidFactory<TGrid> where TGrid : IGrid
    {
        TGrid CreateGrid(double length, int countOfNodesOfElement, int countOfElements);
    }
}
