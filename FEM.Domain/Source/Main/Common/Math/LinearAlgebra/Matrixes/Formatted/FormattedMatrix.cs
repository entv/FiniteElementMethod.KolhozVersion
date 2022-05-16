using FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Matrixes
{
    public abstract class FormattedMatrix
    {
        protected abstract FormattedMatrix SumWith(FormattedMatrix other);
        protected abstract FormattedMatrix MultiplyBy(double coefficient);
        public abstract Vector MultiplyByVector(Vector vector);

        public abstract FormattedMatrix WithFirstBoundaryCondition(IEnumerable<int> rows);
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
