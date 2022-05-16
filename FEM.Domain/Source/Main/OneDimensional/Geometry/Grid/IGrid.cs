using FEM.Domain.Source.Main.OneDimensional.Geometry.Nodes;

namespace FEM.Domain.Source.Main.OneDimensional.Geometry.Grid
{
    public interface IGrid
    {
        int CountOfNodes();
        int CountOfElements();

        IEnumerable<Node> BorderNodes();
        IEnumerable<Node> NodesOfElementByNumber(int number);

        double LengthOfElementByNumber(int number);
    }
}
