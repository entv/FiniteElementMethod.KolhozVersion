using FEM.Domain.Source.Main.TwoDimensional.Geometry.Nodes;

namespace FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid
{
    public class UniformGrid : IGrid
    {
        private readonly double _stepByX;
        private readonly double _stepByY;
        private readonly int _countOfNodesInLine;
        private readonly int _countOfNodesOnEdgeOfElement;
        private readonly int _countOfElementsInLine;

        public UniformGrid(
                int countOfElementsInLine,
                int countOfNodesInLine,
                int countOfNodesOnEdgeOfElement,
                double stepByX,
                double stepByY
            )
        {
            _countOfElementsInLine = countOfElementsInLine;
            _countOfNodesInLine = countOfNodesInLine;
            _countOfNodesOnEdgeOfElement = countOfNodesOnEdgeOfElement;
            _stepByX = stepByX;
            _stepByY = stepByY;
        }

        public int CountOfElements() => _countOfElementsInLine * _countOfElementsInLine;
        public IEnumerable<Node> BorderNodes()
        {
            List<Node> nodes = new List<Node>();

            for (int i = 0; i < _countOfNodesInLine; i++)
            {
                for (int j = 0; j < _countOfNodesInLine; j++)
                {
                    if (i == 0 || j == 0 || i == _countOfNodesInLine - 1 || j == _countOfNodesInLine - 1)
                    {
                        double x = (double)j * _stepByX;
                        double y = (double)i * _stepByY;
                        int nodeId = i * _countOfNodesInLine + j;

                        nodes.Add(new Node(nodeId, x, y));
                    }
                }
            }

            return nodes;
        }

        public IEnumerable<Node> NodesOfElementByNumber(int number)
        {
            if (number >= CountOfElements() || number < 0)
            {
                //throw new ArgumentOutOfRangeException(nameof(number));
            }

            List<Node> nodes = new List<Node>();

            int lineOfElement = number / _countOfElementsInLine;
            int columnOfElement = number % _countOfElementsInLine;
            int firstNodeId = lineOfElement * _countOfNodesInLine * (_countOfNodesOnEdgeOfElement - 1) + columnOfElement * (_countOfNodesOnEdgeOfElement - 1);
            int row = firstNodeId / _countOfNodesInLine;
            int column = firstNodeId % _countOfNodesInLine;
            for (int i = 0; i < _countOfNodesOnEdgeOfElement; i++)
            {
                for (int j = 0; j < _countOfNodesOnEdgeOfElement; j++)
                {
                    double x = (double)(column + j) * _stepByX;
                    double y = (double)(row + i) * _stepByY;
                    int nodeId = firstNodeId + (i * _countOfNodesInLine + j);

                    nodes.Add(new Node(nodeId, x, y));
                }
            }

            return nodes;
        }

        public int CountOfNodes() => _countOfNodesInLine * _countOfNodesInLine;

        public double WidthOfElement() => (double)(_countOfNodesOnEdgeOfElement - 1) * _stepByX;

        public double HeightOfElement() => (double)(_countOfNodesInLine - 1) * _stepByY;
    }
}
