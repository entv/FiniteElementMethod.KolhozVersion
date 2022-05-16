using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.TwoDimensional.Physic.Material;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement
{
    public class BilinearRectangularFiniteElement : IRectangularFiniteElement
    {
        private const int _countOfNodes = 4;
        private Matrix _baseStiffnessLeftPart = new(_countOfNodes);
        private Matrix _baseStiffnessRightPart = new(_countOfNodes);
        private Matrix _baseMass = new(_countOfNodes);

        public BilinearRectangularFiniteElement()
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
        public int CountOfNodes() => _countOfNodes;

        public Matrix MassMatrixOnElement(double width, double height, IMaterial material) => (material.Gamma() * width * height) / 36.0 * _baseMass;

        public Matrix MassMatrixWithoutMaterial(double width, double height) => (width * height) / 36.0 * _baseMass;

        public Matrix StiffnessMatrixOnElement(double width, double height, IMaterial material) => (material.Lambda() * height) / (6.0 * width) * _baseStiffnessLeftPart +
                                                                                                   (material.Lambda() * width) / (6.0 * height) * _baseStiffnessRightPart;
    }
}
