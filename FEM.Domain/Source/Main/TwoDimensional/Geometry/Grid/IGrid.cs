using FEM.Domain.Source.Main.TwoDimensional.Geometry.Nodes;

namespace FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid
{
    public interface IGrid
    {
        int CountOfElements();
        int CountOfNodes();
        IEnumerable<Node> BorderNodes();
        IEnumerable<Node> NodesOfElementByNumber(int number);
        double WidthOfElement();
        double HeightOfElement();
    }
}
