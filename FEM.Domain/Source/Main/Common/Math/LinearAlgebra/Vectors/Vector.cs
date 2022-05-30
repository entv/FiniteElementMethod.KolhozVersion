using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM.Domain.Source.Main.Common.Math.LinearAlgebra.Vectors
{
    public class Vector
    {
        private double[] _elements;

        public Vector(int size)
        {
            _elements = new double[size];

            for (int i = 0; i < size; i++)
            {
                _elements[i] = 0.0;
            }
        }

        public int Size => _elements.Length;

        public double EuqlideanNorm => System.Math.Sqrt(_elements.Select(x => x * x).Sum());

        public double this[int index]
        {
            get
            {
                return _elements[index];
            }
            set
            {
                _elements[index] = value;
            }
        }

        public static Vector operator *(double constant, Vector vector)
        {
            var result = new Vector(vector.Size);

            for (var i = 0; i < vector.Size; i++)
            {
                result[i] = constant * vector[i];
            }

            return result;
        }

        public static Vector operator *(Vector vector, double constant)
        {
            var result = new Vector(vector.Size);

            for (var i = 0; i < vector.Size; i++)
            {
                result[i] = constant * vector[i];
            }

            return result;
        }

        public static Vector operator +(Vector first, Vector second)
        {
            if (first.Size != second.Size)
            {
                //throw new ArgumentException();
            }

            var result = new Vector(first.Size);

            for (var i = 0; i < first.Size; i++)
            {
                result[i] = first[i] + second[i];
            }

            return result;
        }

        public static double operator *(Vector first, Vector second)
        {
            if (first.Size != second.Size)
            {
                //throw new ArgumentException();
            }
            
            var result = 0.0;

            for (int i = 0; i < first.Size; i++)
            {
                result += first[i] * second[i];
            }

            return result;
        }

        public static Vector operator -(Vector first, Vector second)
        {
            if (first.Size != second.Size)
            {
                //throw new ArgumentException();
            }

            var result = new Vector(first.Size);

            for (var i = 0; i < first.Size; i++)
            {
                result[i] = first[i] - second[i];
            }

            return result;
        }

        public static bool operator==(Vector first, Vector second)
        {
            for (int i = 0; i < first.Size; i++)
            {
                if (first[i] != second[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(Vector first, Vector second)
        {
            return !(first == second);
        }
    }
}
