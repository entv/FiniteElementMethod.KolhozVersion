

using FEM.Domain.Source.Main.OneDimensional.Geometry.Nodes;

namespace FEM.Domain.Source.Main.OneDimensional.Geometry.Grid
{
    public class NonUniformGrid : IGrid
    {
        public IEnumerable<Node> BorderNodes()
        {
            throw new NotImplementedException();
        }

        public int CountOfNodes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Node> NodesOfElementByNumber(int number)
        {
            throw new NotImplementedException();
        }

        public double LengthOfElementByNumber(int number)
        {
            throw new NotImplementedException();
        }

        public int CountOfElements()
        {
            throw new NotImplementedException();
        }
    }
}
