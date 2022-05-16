

using FEM.Domain.Source.Main.OneDimensional.Geometry.Nodes;

namespace FEM.Domain.Source.Main.OneDimensional.Geometry.Grid
{
    public class UniformGrid : IGrid
    {
        private readonly double _step;
        private readonly int _countOfElements;
        private readonly int _countOfNodes;
        private readonly int _countOfNodesOnElement;
        public UniformGrid(double step, int countOfElements, int countOfNodes, int countOfNodesOnElement)
        {
            _step = step;
            _countOfElements = countOfElements;
            _countOfNodes = countOfNodes;
            _countOfNodesOnElement = countOfNodesOnElement;
        }
        public IEnumerable<Node> BorderNodes()
        {
            List<Node> nodes = new List<Node>();

            nodes.Add(new Node(0, 0));
            nodes.Add(new Node(_countOfNodes - 1, _step * (_countOfNodes - 1)));

            return nodes;
        }

        public int CountOfNodes() => _countOfNodes;

        public IEnumerable<Node> NodesOfElementByNumber(int number)
        {
            List<Node> nodes = new List<Node>();

            var firstNodeId = number * (_countOfNodesOnElement - 1);
            for (int i = 0; i < _countOfNodesOnElement; i++)
            {
                var value = (firstNodeId + i) * _step;
                var id = firstNodeId + i;
                nodes.Add(new Node(id, value));
            }

            return nodes;
        }

        public double LengthOfElementByNumber(int number)
        {
            if (number < 0 || number >= _countOfElements)
            {
                //throw ArgumentOutOfRangeException();
            }

            return _step;
        }

        public int CountOfElements() => _countOfElements;
    }
}
