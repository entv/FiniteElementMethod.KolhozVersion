using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.OneDimensional.Physic.Parameters;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Nodes;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement
{
    public class HarmonicalFiniteElement : IHarmonicalFiniteElement
    {
        private readonly OneDimensional.Math.FiniteElement.IHarmonicalFiniteElement _baseFiniteElement;

        public HarmonicalFiniteElement(OneDimensional.Math.FiniteElement.IHarmonicalFiniteElement baseFiniteElement)
        {
            _baseFiniteElement = baseFiniteElement;
        }

        public Matrix BaseMassMatrixOnElement(double width, double height)
        {
            var mass = new Matrix(CountOfNodes());
            var massX = _baseFiniteElement.BaseMassMatrixOnElement(width);
            var massY = _baseFiniteElement.BaseMassMatrixOnElement(height);

            for (int i = 0; i < mass.Size; i++)
            {
                for (int j = 0; j < mass.Size; j++)
                {
                    mass[i, j] += massX[i % massX.Size, j % massX.Size] * massY[i / massY.Size, j / massY.Size];
                }
            }

            return mass;
        }

        public int CountOfNodes() => _baseFiniteElement.CountOfNodes() * _baseFiniteElement.CountOfNodes();

        public Matrix MatrixOnElement(double width, double height, IHarmonicalParameters harmonicParameters)
        {
            var matrix = new Matrix(CountOfNodes() * CountOfNodes());
            var matrixX = _baseFiniteElement.MatrixOnElement(width, harmonicParameters);
            var matrixY = _baseFiniteElement.MatrixOnElement(height, harmonicParameters);

            for (int i = 0; i < matrix.Size; i++)
            {
                for (int j = 0; j < matrix.Size; j++)
                {
                    matrix[i, j] += matrixX[i % matrixX.Size, j % matrixX.Size] * matrixY[i / matrixY.Size, j / matrixY.Size];
                }
            }

            return matrix;
        }
    }
}
