using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.OneDimensional.Physic.Parameters;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Nodes;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement
{
    public class HarmonicalFiniteElement : IHarmonicalFiniteElement
    {
        private const int _countOfNodes = 4;
        private Matrix _baseStiffnessLeftPart = new(_countOfNodes);
        private Matrix _baseStiffnessRightPart = new(_countOfNodes);
        private Matrix _baseMass = new(_countOfNodes);

        public HarmonicalFiniteElement()
        {
            _baseMass[0, 0] = 4.0; _baseMass[0, 1] = 2.0; _baseMass[0, 2] = 2.0; _baseMass[0, 3] = 1.0;
            _baseMass[1, 0] = 2.0; _baseMass[1, 1] = 4.0; _baseMass[1, 2] = 1.0; _baseMass[1, 3] = 2.0;
            _baseMass[2, 0] = 2.0; _baseMass[2, 1] = 1.0; _baseMass[2, 2] = 4.0; _baseMass[2, 3] = 2.0;
            _baseMass[3, 0] = 1.0; _baseMass[3, 1] = 2.0; _baseMass[3, 2] = 2.0; _baseMass[3, 3] = 4.0;

            _baseStiffnessLeftPart[0, 0] = 2.0; _baseStiffnessLeftPart[0, 1] = -2.0; _baseStiffnessLeftPart[0, 2] = 1.0; _baseStiffnessLeftPart[0, 3] = -1.0;
            _baseStiffnessLeftPart[1, 0] = -2.0; _baseStiffnessLeftPart[1, 1] = 2.0; _baseStiffnessLeftPart[1, 2] = -1.0; _baseStiffnessLeftPart[1, 3] = 1.0;
            _baseStiffnessLeftPart[2, 0] = 1.0; _baseStiffnessLeftPart[2, 1] = -1.0; _baseStiffnessLeftPart[2, 2] = 2.0; _baseStiffnessLeftPart[2, 3] = -2.0;
            _baseStiffnessLeftPart[3, 0] = -1.0; _baseStiffnessLeftPart[3, 1] = 1.0; _baseStiffnessLeftPart[3, 2] = -2.0; _baseStiffnessLeftPart[3, 3] = 2.0;

            _baseStiffnessRightPart[0, 0] = 2.0; _baseStiffnessRightPart[0, 1] = 1.0; _baseStiffnessRightPart[0, 2] = -2.0; _baseStiffnessRightPart[0, 3] = -1.0;
            _baseStiffnessRightPart[1, 0] = 1.0; _baseStiffnessRightPart[1, 1] = 2.0; _baseStiffnessRightPart[1, 2] = -1.0; _baseStiffnessRightPart[1, 3] = -2.0;
            _baseStiffnessRightPart[2, 0] = -2.0; _baseStiffnessRightPart[2, 1] = -1.0; _baseStiffnessRightPart[2, 2] = 2.0; _baseStiffnessRightPart[2, 3] = 1.0;
            _baseStiffnessRightPart[3, 0] = -1.0; _baseStiffnessRightPart[3, 1] = -2.0; _baseStiffnessRightPart[3, 2] = 1.0; _baseStiffnessRightPart[3, 3] = 2.0;
        }

        public Matrix BaseMassMatrixOnElement(double width, double height)
        {
            return ((width * height) / 36.0) * _baseMass;
        }

        public int CountOfNodes() => _countOfNodes;

        public Matrix MatrixOnElement(double width, double height, IHarmonicalParameters harmonicParameters)
        {
            var matrix = new Matrix(2 * _countOfNodes);

            var lambda = harmonicParameters.Lambda();
            var omega = harmonicParameters.Omega();
            var sigma = harmonicParameters.Sigma();
            var chi = harmonicParameters.Chi();

            for (int i = 0; i < _countOfNodes; i++)
            {
                for (int j = 0; j < _countOfNodes; j++)
                {
                    var p = (lambda * height) / (6.0 * width) * _baseStiffnessLeftPart[i, j] + (lambda * width) / (6.0 * height) * _baseStiffnessRightPart[i, j] - (omega * omega * chi * width * height / 36.0) * _baseMass[i, j];
                    var c = ((omega * sigma * width * height) / 36.0) * _baseMass[i, j];

                    matrix[2 * i, 2 * j] = p;
                    matrix[2 * i, 2 * j + 1] = -c;
                    matrix[2 * i + 1, 2 * j] = c;
                    matrix[2 * i + 1, 2 * j + 1] = p;
                }
            }

            return matrix;
        }
    }
}


/*
 
 */