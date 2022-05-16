using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.TwoDimensional.Physic.Material;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.FiniteElement
{
    public class BiquadraticRectangularFiniteElement : IRectangularFiniteElement
    {
        private const int _countOfNodesOnEdge = 3;
        private Matrix _massMatrix1D = new(_countOfNodesOnEdge);
        private Matrix _stiffnessMatrix1D = new(_countOfNodesOnEdge);

        public BiquadraticRectangularFiniteElement()
        {
            _massMatrix1D[0, 0] = 4.0; _massMatrix1D[0, 1] = 2.0; _massMatrix1D[0, 2] = -1.0;
            _massMatrix1D[1, 0] = 2.0; _massMatrix1D[1, 1] = 16.0; _massMatrix1D[1, 2] = 2.0;
            _massMatrix1D[2, 0] = -1.0; _massMatrix1D[2, 1] = 2.0; _massMatrix1D[2, 2] = 4.0;

            _stiffnessMatrix1D[0, 0] = 7.0; _stiffnessMatrix1D[0, 1] = -8.0; _stiffnessMatrix1D[0, 2] = 1.0;
            _stiffnessMatrix1D[1, 0] = -8.0; _stiffnessMatrix1D[1, 1] = 16.0; _stiffnessMatrix1D[1, 2] = -8.0;
            _stiffnessMatrix1D[2, 0] = 1.0; _stiffnessMatrix1D[2, 1] = -8.0; _stiffnessMatrix1D[2, 2] = 7.0;
        }

        public int CountOfNodes() => _countOfNodesOnEdge * _countOfNodesOnEdge;

        public Matrix MassMatrixWithoutMaterial(double width, double height)
        {
            //readability of formulas
            double hx = width;
            double hy = height;
            Matrix M = _massMatrix1D;
            //-----------------------

            Matrix mass = new(CountOfNodes());

            for (int i = 0; i < CountOfNodes(); i++)
            {
                for (int j = 0; j < CountOfNodes(); j++)
                {
                    mass[i, j] += (hx * M[m(i), m(j)] / 30.0) * (hy * M[v(i), v(j)] / 30.0);
                }
            }

            return mass;
        }

        public Matrix MassMatrixOnElement(double width, double height, IMaterial material)
        {
            return material.Gamma() * MassMatrixWithoutMaterial(width, height);
        }

        public Matrix StiffnessMatrixOnElement(double width, double height, IMaterial material)
        {
            Matrix stiffness = new(CountOfNodes());

            //readability of formulas
            double hx = width;
            double hy = height;
            Matrix G = _stiffnessMatrix1D;
            Matrix M = _massMatrix1D;
            //-----------------------

            double lambda = material.Lambda();

            for (int i = 0; i < CountOfNodes(); i++)
            {
                for (int j = 0; j < CountOfNodes(); j++)
                {
                    stiffness[i, j] += lambda * ((G[m(i), m(j)] / (3.0 * hx)) * ((hy * M[v(i), v(j)]) / 30.0) + ((hx * M[m(i), m(j)]) / 30.0) * (G[v(i), v(j)] / (3.0 * hy)));
                }
            }

            return stiffness;
        }

        private int m(int index) => index % _countOfNodesOnEdge;
        private int v(int index) => index / _countOfNodesOnEdge;
    }
}
