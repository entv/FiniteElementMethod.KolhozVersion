using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.OneDimensional.Physic.Parameters;

namespace FEM.Domain.Source.Main.OneDimensional.Math.FiniteElement
{
    public class BilinearHarmonicFiniteElement : IHarmonicalFiniteElement
    {
        private const int _countOfNodes = 2;
        private Matrix _baseMassMatrix = new(_countOfNodes);

        public BilinearHarmonicFiniteElement()
        {
            _baseMassMatrix[0, 0] = 2.0; _baseMassMatrix[0, 1] = 1.0;
            _baseMassMatrix[1, 0] = 1.0; _baseMassMatrix[1, 1] = 2.0;
        }

        public Matrix BaseMassMatrixOnElement(double length)
        {
            return (length / 6.0) * _baseMassMatrix;
        }

        public int CountOfNodes() => _countOfNodes;

        public Matrix MatrixOnElement(double length, IHarmonicalParameters harmonicParameters)
        {
            var lambda = harmonicParameters.Lambda();
            var omega = harmonicParameters.Omega();
            var sigma = harmonicParameters.Sigma();
            var chi = harmonicParameters.Chi();

            var p00 = (-lambda / (2.0 * length)) - (omega * omega * chi / 3.0);
            var p10 = (lambda / (2.0 * length)) - (omega * omega * chi / 6.0);

            var c00 = omega * sigma * length / 3.0;
            var c10 = omega * sigma * length / 6.0;

            var localMatrix = new Matrix(_countOfNodes * _countOfNodes);

            localMatrix[0, 0] = p00; localMatrix[0, 1] = -c00; localMatrix[0, 2] = p10; localMatrix[0, 3] = -c10;
            localMatrix[1, 0] = c00; localMatrix[1, 1] = p00; localMatrix[1, 2] = c10; localMatrix[1, 3] = p10;
            localMatrix[2, 0] = p10; localMatrix[2, 1] = -c10; localMatrix[2, 2] = p00; localMatrix[2, 3] = -c00;
            localMatrix[3, 0] = c10; localMatrix[3, 1] = p10; localMatrix[3, 2] = c00; localMatrix[3, 3] = p00;

            return localMatrix;
        }
    }
}
