using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.OneDimensional.Physic.Parameters;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid;
using FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement;
using FEM.Domain.Source.Main.TwoDimensional.Physic.Force;
using FEM.Domain.Source.Main.Common.ArrayExtensions;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.TaskTypes
{
    public class HarmonicalTask : IHarmonicalTask
    {
        private readonly IHarmonicalFiniteElement _finiteElement;
        private readonly IHarmonicalParameters _parameters;
        private readonly IHarmonicalForce _force;
        private const int _blockSize = 2;

        public HarmonicalTask(IHarmonicalFiniteElement finiteElement, IHarmonicalParameters parameters, IHarmonicalForce force)
        {
            _finiteElement = finiteElement;
            _parameters = parameters;
            _force = force;
        }
        public TFormattedMatrix MatrixOnGrid<TFormattedMatrix>(IGrid grid, IFormattedMatrixFactory<TFormattedMatrix> factory) where TFormattedMatrix : FormattedMatrix 
        {
            var connectivityList = ConnectivityListOnGrid(grid);
            var nonDiagonalElementsCount = connectivityList.Select(elementsInRow => elementsInRow.Count()).Sum();

            double[] diagonalElements = new double[_blockSize * grid.CountOfNodes()];
            double[] lowerElements = new double[nonDiagonalElementsCount];
            double[] upperElements = new double[nonDiagonalElementsCount];
            int[] offsets = new int[_blockSize * grid.CountOfNodes() + 1];
            int[] columns = new int[nonDiagonalElementsCount];

            for (int row = 0; row < _blockSize * grid.CountOfNodes(); row++)
            {
                offsets[row + 1] = offsets[row];

                foreach (var column in connectivityList.ElementAt(row))
                {
                    columns[offsets[row + 1]] = column;
                    offsets[row + 1] += 1;
                }
            }

            for (int elementNumber = 0; elementNumber < grid.CountOfElements(); elementNumber++)
            {
                var nodesOfElement = grid.NodesOfElementByNumber(elementNumber);
                var localMatrixOnElement = _finiteElement.MatrixOnElement(
                            grid.WidthOfElement(),
                            grid.HeightOfElement(),
                            _parameters
                        );

                for (int i = 0; i < nodesOfElement.Count(); i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        var rowOfBlock = nodesOfElement.ElementAt(i).Id;
                        var columnOfBlock = nodesOfElement.ElementAt(j).Id;

                        for (int row = 0; row < _blockSize; row++)
                        {
                            for (int column = 0; column < _blockSize; column++)
                            {
                                var globalRow = row + rowOfBlock * _blockSize;
                                var globalColumn = column + columnOfBlock * _blockSize;

                                if (globalRow == globalColumn)
                                {
                                    diagonalElements[globalRow] += localMatrixOnElement[_blockSize * i + row, _blockSize * j + column];
                                }
                                else if (globalColumn < globalRow)
                                {
                                    lowerElements[columns.IndexOf(globalColumn, offsets[globalRow])] += localMatrixOnElement[_blockSize * i + row, _blockSize * j + column];
                                    upperElements[columns.IndexOf(globalColumn, offsets[globalRow])] += localMatrixOnElement[_blockSize * j + row, _blockSize * i + column];
                                }
                            }
                        }
                    }
                }
            }

            

            return factory.CreateFormattedMatrix(
                    diagonalElements,
                    upperElements,
                    lowerElements,
                    columns,
                    offsets
                );
        }

        public Vector ForceVectorOnGrid(IGrid grid)
        {
            var forceVector = new Vector(_blockSize * grid.CountOfNodes());

            for (int elementNumber = 0; elementNumber < grid.CountOfElements(); elementNumber++)
            {
                var nodesOfElement = grid.NodesOfElementByNumber(elementNumber);
                var widthOfElement = grid.WidthOfElement();
                var heightOfElement = grid.HeightOfElement();

                var forceSinePart = new Vector(nodesOfElement.Count());
                var forceCosinePart = new Vector(nodesOfElement.Count());
                for (int i = 0; i < nodesOfElement.Count(); i++)
                {
                    var node = nodesOfElement.ElementAt(i);
                    forceSinePart[i] = _force.ValueInPointOnSinePart(node.X, node.Y);
                    forceCosinePart[i] = _force.ValueInPointOnCosinePart(node.X, node.Y);
                }

                var forceSinePartWithMass = _finiteElement.BaseMassMatrixOnElement(widthOfElement, heightOfElement) * forceSinePart;
                var forceCosinePartWithMass = _finiteElement.BaseMassMatrixOnElement(widthOfElement, heightOfElement) * forceCosinePart;

                for (int i = 0; i < nodesOfElement.Count(); i++)
                {
                    var node = nodesOfElement.ElementAt(i);
                    forceVector[_blockSize * node.Id] += forceSinePartWithMass[i];
                    forceVector[_blockSize * node.Id + 1] += forceCosinePartWithMass[i];
                }
            }

            return forceVector;
        }

        private IEnumerable<IOrderedEnumerable<int>> ConnectivityListOnGrid(IGrid grid)
        {
            var list = new List<HashSet<int>>();
            for (int i = 0; i < _blockSize * grid.CountOfNodes(); i++)
            {
                list.Add(new HashSet<int>());
            }

            for (int elementNumber = 0; elementNumber < grid.CountOfElements(); elementNumber++)
            {
                var nodesOfElement = grid.NodesOfElementByNumber(elementNumber);

                for (int i = 0; i < nodesOfElement.Count(); i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        int rowOfBlock = nodesOfElement.ElementAt(i).Id;
                        int columnOfBlock = nodesOfElement.ElementAt(j).Id;

                        if (rowOfBlock == columnOfBlock)
                        {
                            list[rowOfBlock * _blockSize + 1].Add(columnOfBlock * _blockSize);
                        }
                        else
                        {
                            for (int k = 0; k < _blockSize; k++)
                            {
                                for (int l = 0; l < _blockSize; l++)
                                {
                                    list[rowOfBlock * _blockSize + k].Add(columnOfBlock * _blockSize + l);
                                }
                            }
                        }
                    }
                }
            }

            return list.Select(row => row.OrderBy(column => column));
        }
    }
}
