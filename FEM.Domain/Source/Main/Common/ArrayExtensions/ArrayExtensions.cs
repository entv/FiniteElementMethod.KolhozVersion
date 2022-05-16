

namespace FEM.Domain.Source.Main.Common.ArrayExtensions
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T item, int startIndex)
        {
            return Array.FindIndex(array, startIndex, value => value != null ? value.Equals(item) : false);
        }
    }
}
