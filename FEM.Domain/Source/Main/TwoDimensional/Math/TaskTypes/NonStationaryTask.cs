using FEM.Domain.Source.Main.Common.ArrayExtensions;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid;
using FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement;
using FEM.Domain.Source.Main.TwoDimensional.Physic.Force;
using FEM.Domain.Source.Main.TwoDimensional.Physic.Material;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.TaskType
{
    public class NonStationaryTask : INonStationaryTask
    {
        private readonly IRectangularFiniteElement _finiteElement;
        private readonly ITimedForce _force;
        private readonly IMaterial _material;

        public NonStationaryTask(
            IRectangularFiniteElement finiteElement,
            ITimedForce force,
            IMaterial material
            )
        {
            _finiteElement = finiteElement;
            _force = force;
            _material = material;
        }
        public Vector ForceVectorOnGrid(double time, IGrid grid)
        {
            var forceVector = new Vector(grid.CountOfNodes());

            for (int elementNumber = 0; elementNumber < grid.CountOfElements(); elementNumber++)
            {
                var nodesOfElement = grid.NodesOfElementByNumber(elementNumber);
                var localForceVector = new Vector(nodesOfElement.Count());
                
                for (int i = 0; i < nodesOfElement.Count(); i++)
                {
                    var node = nodesOfElement.ElementAt(i);
                    localForceVector[i] = _force.ValueInPoint(node.X, node.Y, time);
                }

                localForceVector = _finiteElement.MassMatrixWithoutMaterial(
                    grid.WidthOfElement(),
                    grid.HeightOfElement()
                    ) * localForceVector;
                
                for (int i = 0; i < nodesOfElement.Count(); i++)
                {
                    var node = nodesOfElement.ElementAt(i);
                    forceVector[node.Id] += localForceVector[i];
                }
            }

            return forceVector;
        }

        public TFormattedMatrix MassMatrixOnGrid<TFormattedMatrix>(IFormattedMatrixFactory<TFormattedMatrix> factory, IGrid grid) where TFormattedMatrix : FormattedMatrix
        {
            var connectivityList = ConnectivityListOnGrid(grid);
            var nonDiagonalElementsCount = connectivityList.Select(elementsInRow => elementsInRow.Count()).Sum();

            double[] diagonalElements = new double[grid.CountOfNodes()];
            double[] lowerElements = new double[nonDiagonalElementsCount];
            double[] upperElements = new double[nonDiagonalElementsCount];
            int[] offsets = new int[grid.CountOfNodes() + 1];
            int[] columns = new int[nonDiagonalElementsCount];

            for (int row = 0; row < grid.CountOfNodes(); row++)
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
                var localMassMatrix = _finiteElement.MassMatrixWithoutMaterial(
                        grid.WidthOfElement(),
                        grid.HeightOfElement()
                    );

                for (int i = 0; i < localMassMatrix.Size; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        var globalRow = nodesOfElement.ElementAt(i).Id;
                        var globalColumn = nodesOfElement.ElementAt(j).Id;

                        if (globalRow == globalColumn)
                        {
                            diagonalElements[globalRow] += localMassMatrix[i, j];
                        } 
                        else
                        {
                            lowerElements[columns.IndexOf(globalColumn, offsets[globalRow])] += localMassMatrix[i, j];
                            upperElements[columns.IndexOf(globalColumn, offsets[globalRow])] += localMassMatrix[i, j];
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

        public TFormattedMatrix StiffnessMatrixOnGrid<TFormattedMatrix>(IFormattedMatrixFactory<TFormattedMatrix> factory, IGrid grid) where TFormattedMatrix : FormattedMatrix
        {
            var connectivityList = ConnectivityListOnGrid(grid);
            var nonDiagonalElementsCount = connectivityList.Select(elementsInRow => elementsInRow.Count()).Sum();

            double[] diagonalElements = new double[grid.CountOfNodes()];
            double[] lowerElements = new double[nonDiagonalElementsCount];
            double[] upperElements = new double[nonDiagonalElementsCount];
            int[] offsets = new int[grid.CountOfNodes() + 1];
            int[] columns = new int[nonDiagonalElementsCount];

            for (int row = 0; row < grid.CountOfNodes(); row++)
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
                var localStiffnessMatrix = _finiteElement.StiffnessMatrixOnElement(
                        grid.WidthOfElement(),
                        grid.HeightOfElement(),
                        _material
                    );

                for (int i = 0; i < localStiffnessMatrix.Size; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        var globalRow = nodesOfElement.ElementAt(i).Id;
                        var globalColumn = nodesOfElement.ElementAt(j).Id;

                        if (globalRow == globalColumn)
                        {
                            diagonalElements[globalRow] += localStiffnessMatrix[i, j];
                        }
                        else
                        {
                            lowerElements[columns.IndexOf(globalColumn, offsets[globalRow])] += localStiffnessMatrix[i, j];
                            upperElements[columns.IndexOf(globalColumn, offsets[globalRow])] += localStiffnessMatrix[j, i];
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

        private IEnumerable<IOrderedEnumerable<int>> ConnectivityListOnGrid(IGrid grid)
        {
            var list = new List<HashSet<int>>();
            for (int i = 0; i < grid.CountOfNodes(); i++)
            {
                list.Add(new HashSet<int>());
            }

            for (int elementNumber = 0; elementNumber < grid.CountOfElements(); elementNumber++)
            {
                var nodesOfElement = grid.NodesOfElementByNumber(elementNumber);

                for (int i = 0; i < nodesOfElement.Count(); i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        int globalRow = nodesOfElement.ElementAt(i).Id;
                        int globalColumn = nodesOfElement.ElementAt(j).Id;

                        list[globalRow].Add(globalColumn);
                    }
                }
            }

            return list.Select(row => row.OrderBy(column => column));
        }
    }
}
