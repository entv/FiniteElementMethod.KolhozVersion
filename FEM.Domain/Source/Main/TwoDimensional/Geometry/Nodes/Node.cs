

namespace FEM.Domain.Source.Main.TwoDimensional.Geometry.Nodes
{
    public class Node
    {
        private readonly int _id;
        private readonly double _x;
        private readonly double _y;

        public Node(int id, double x, double y)
        {
            _id = id;
            _x = x;
            _y = y;
        }

        public int Id => _id;
        public double X => _x;
        public double Y => _y;
    }
}
