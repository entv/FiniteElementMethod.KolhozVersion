

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public class IncompleteLuDecompositionMethod : IDecompositionMethod
    {
        public IMatrixDecomposition IncompleteDecomposition(IList<double> diagonalElements, IList<double> upperElements, IList<double> lowerElements, IList<int> columns, IList<int> offsets)
        {
            double[] factorizedDiagonal = new double[diagonalElements.Count];
            double [] factorizedLower = new double[lowerElements.Count];
            double[] factorizedUpper = new double[upperElements.Count];


            for (int i = 0; i < diagonalElements.Count; i++)
            {
                int currentRow = offsets[i];
                int nextRow = offsets[i + 1];

                double diagonalElementsSum = 0.0;
                for (int k = currentRow; k < nextRow; k++)
                {
                    int j = columns[k];
                    int ki = currentRow;
                    int kj = offsets[j];
                    int j1 = offsets[j + 1];

                    double lowerElementsSum = 0.0;
                    double upperElementsSum = 0.0;

                    while (ki < k && kj < j1)
                    {
                        if (columns[ki] == columns[kj])
                        {
                            lowerElementsSum += factorizedLower[ki] * factorizedUpper[kj];
                            upperElementsSum += factorizedUpper[ki] * factorizedLower[kj];
                            ki++;
                            kj++;
                        }
                        else
                        {
                            if (columns[ki] > columns[kj])
                            {
                                kj++;
                            }
                            else
                            {
                                ki++;
                            }
                        }
                    }

                    factorizedLower[k] = lowerElements[k] - lowerElementsSum;
                    factorizedUpper[k] = (upperElements[k] - upperElementsSum) / factorizedDiagonal[j];

                    diagonalElementsSum += factorizedUpper[k] * factorizedLower[k];
                }

                factorizedDiagonal[i] = diagonalElements[i] - diagonalElementsSum;
            }

            return new IncompleteLuDecomposition(factorizedDiagonal, factorizedUpper, factorizedLower, columns, offsets);
        }
    }
}
