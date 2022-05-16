using FEM.Domain.Source.Main.Common.ArrayExtensions;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.OneDimensional.Geometry.Grid;
using FEM.Domain.Source.Main.OneDimensional.Math.FiniteElement;
using FEM.Domain.Source.Main.OneDimensional.Physic.Force;
using FEM.Domain.Source.Main.OneDimensional.Physic.Parameters;

namespace FEM.Domain.Source.Main.OneDimensional.Math.TaskTypes
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
        public FormattedMatrix MatrixOnGrid(IGrid grid, IFormattedMatrixFactory<FormattedMatrix> factory)
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
                            grid.LengthOfElementByNumber(elementNumber),
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
                                    var test = columns.IndexOf(globalColumn, offsets[globalRow]);
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

        public Matrix testMatrix(IGrid grid)
        {
            var matrix = new Matrix(_blockSize * grid.CountOfNodes());

            for (int elementNumber = 0; elementNumber < grid.CountOfElements(); elementNumber++)
            {
                var nodesOfElement = grid.NodesOfElementByNumber(elementNumber);
                var localMatrixOnElement = _finiteElement.MatrixOnElement(
                            grid.LengthOfElementByNumber(elementNumber),
                            _parameters
                        );

                for (int i = 0; i < nodesOfElement.Count(); i++)
                {
                    for (int j = 0; j < nodesOfElement.Count(); j++)
                    {
                        var globalRow = nodesOfElement.ElementAt(i).Id;
                        var globalColumn = nodesOfElement.ElementAt(j).Id;

                        matrix[2 * globalRow, 2 * globalColumn] += localMatrixOnElement[2 * i, 2 * j];
                        matrix[2 * globalRow + 1, 2 * globalColumn] += localMatrixOnElement[2 * i + 1, 2 * j];
                        matrix[2 * globalRow, 2 * globalColumn + 1] += localMatrixOnElement[2 * i, 2 * j + 1];
                        matrix[2 * globalRow + 1, 2 * globalColumn + 1] += localMatrixOnElement[2 * i + 1, 2 * j + 1];
                    }
                }
            }

            return matrix;
        }

        public Vector ForceVectorOnGrid(IGrid grid)
        {
            var forceVector = new Vector(_blockSize * grid.CountOfNodes());

            for (int elementNumber = 0; elementNumber < grid.CountOfElements(); elementNumber++)
            {
                var nodesOfElement = grid.NodesOfElementByNumber(elementNumber);
                var lengthOfElement = grid.LengthOfElementByNumber(elementNumber);

                var forceSinePart = new Vector(nodesOfElement.Count());
                var forceCosinePart = new Vector(nodesOfElement.Count());
                for (int i = 0; i < nodesOfElement.Count(); i++)
                {
                    var point = nodesOfElement.ElementAt(i).Value;
                    forceSinePart[i] = _force.ValueInPointOnSinePart(point);
                    forceCosinePart[i] = _force.ValueInPointOnCosinePart(point);
                }

                var forceSinePartWithMass = _finiteElement.BaseMassMatrixOnElement(lengthOfElement) * forceSinePart;
                var forceCosinePartWithMass = _finiteElement.BaseMassMatrixOnElement(lengthOfElement) * forceCosinePart;

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
