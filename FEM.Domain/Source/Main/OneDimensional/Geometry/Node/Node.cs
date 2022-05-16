

namespace FEM.Domain.Source.Main.OneDimensional.Geometry.Nodes
{
    public class Node
    {
        private readonly int _id;
        private readonly double _value;

        public Node(int id, double value)
        {
            _id = id;
            _value = value;
        }

        public int Id => _id;
        public double Value => _value;
    }
}
