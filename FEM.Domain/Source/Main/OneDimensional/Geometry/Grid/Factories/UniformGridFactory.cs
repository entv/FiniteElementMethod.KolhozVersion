

namespace FEM.Domain.Source.Main.OneDimensional.Geometry.Grid.Factories
{
    public class UniformGridFactory : IGidFactory<UniformGrid>
    {
        public UniformGrid CreateGrid(double length, int countOfNodesOnElement, int countOfElements)
        {
            var countOfNodes = countOfNodesOnElement;
            for (int elementNumber = 1; elementNumber < countOfElements; elementNumber++)
            {
                countOfNodes += countOfNodesOnElement - 1;
            }

            var step = length / (countOfNodes - 1);

            return new UniformGrid(
                    step,
                    countOfElements,
                    countOfNodes,
                    countOfNodesOnElement
                );
        }
    }
}
