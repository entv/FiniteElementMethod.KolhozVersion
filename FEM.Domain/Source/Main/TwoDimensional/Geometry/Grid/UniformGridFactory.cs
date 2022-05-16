

namespace FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid
{
    public class UniformGridFactory : IGridFactory
    {
        public IGrid CreateGrid(double width, double height, int accuracy, int countOfNodesOfElement)
        {
            var nodesOnEdgeOfElement = (int)System.Math.Sqrt(countOfNodesOfElement);
            var countOfNodesInLine = nodesOnEdgeOfElement;
            for (int i = 1; i < accuracy; i++)
            {
                countOfNodesInLine += nodesOnEdgeOfElement - 1;
            }

            double stepByX = width / (countOfNodesInLine - 1);
            double stepByY = height / (countOfNodesInLine - 1);

            return new UniformGrid(accuracy, countOfNodesInLine, nodesOnEdgeOfElement, stepByX, stepByY);
        }
    }
}
