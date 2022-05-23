using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.TaskTypes
{
    public interface IHarmonicalTask
    {
        TFormattedMatrix MatrixOnGrid<TFormattedMatrix>(IGrid grid, IFormattedMatrixFactory<TFormattedMatrix> factory) where TFormattedMatrix : FormattedMatrix;
        Vector ForceVectorOnGrid(IGrid grid);
    }
}
