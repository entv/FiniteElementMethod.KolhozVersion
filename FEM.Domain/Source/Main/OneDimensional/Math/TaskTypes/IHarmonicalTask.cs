using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.OneDimensional.Geometry.Grid;

namespace FEM.Domain.Source.Main.OneDimensional.Math.TaskTypes
{
    public interface IHarmonicalTask
    {
        FormattedMatrix MatrixOnGrid(IGrid grid, IFormattedMatrixFactory<FormattedMatrix> factory);
        Vector ForceVectorOnGrid(IGrid grid);
    }
}
