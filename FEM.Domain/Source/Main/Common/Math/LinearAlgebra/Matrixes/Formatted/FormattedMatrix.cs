using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes.Formatted;
using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public abstract class FormattedMatrix
    {
        protected abstract FormattedMatrix SumWith(FormattedMatrix other);
        protected abstract FormattedMatrix MultiplyBy(double coefficient);
        protected abstract FormattedMatrix FirstBoundary(IEnumerable<int> rows);
        public abstract Vector MultiplyByVector(Vector vector);

        public TFormattedMatrix WithFirstBoundaryCondition<TFormattedMatrix>(IEnumerable<int> rows) where TFormattedMatrix : FormattedMatrix
        {
            return (TFormattedMatrix)FirstBoundary(rows);
        }
        public static FormattedMatrix operator+(FormattedMatrix first, FormattedMatrix second)
        {
            return first.SumWith(second);
        }

        public static FormattedMatrix operator*(double coefficient, FormattedMatrix matrix)
        {
            return matrix.MultiplyBy(coefficient);
        }
    }
}
