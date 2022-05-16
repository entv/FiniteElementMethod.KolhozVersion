using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;
using FEM.Domain.Source.Main.TwoDimensional.Geometry.Grid;

namespace FEM.Domain.Source.Main.TwoDimensional.Math.TaskType
{
    public interface INonStationaryTask 
    {
        TFormattedMatrix MassMatrixOnGrid<TFormattedMatrix>(IFormattedMatrixFactory<TFormattedMatrix> factory, IGrid grid) where TFormattedMatrix : FormattedMatrix;
        TFormattedMatrix StiffnessMatrixOnGrid<TFormattedMatrix>(IFormattedMatrixFactory<TFormattedMatrix> factory, IGrid grid) where TFormattedMatrix : FormattedMatrix;
        Vector ForceVectorOnGrid(double time, IGrid grid); 
    }
}
