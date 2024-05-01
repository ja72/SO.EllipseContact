﻿using System;
using System.Security.Cryptography;

namespace SO
{
    public readonly struct Vector2 :
        IEquatable<Vector2>,
        IFormattable,
        System.Collections.ICollection,
        System.Collections.Generic.IReadOnlyList<double>
    {
        readonly (double x, double y) data;

        public Vector2(double x, double y) => data = (x, y);

        public static implicit operator Vector2(System.Numerics.Vector2 vector)
            => new Vector2(vector.X, vector.Y);

        public static Vector2 Zero { get; } = new Vector2(0, 0);
        public static Vector2 UnitX { get; } = new Vector2(1, 0);
        public static Vector2 UnitY { get; } = new Vector2(0, 1);
        public static Vector2 One { get; } = new Vector2(1, 1);

        public static Vector2 Cartesian(double x, double y) => new Vector2(x, y);
        public static Vector2 Polar(double r, double θ) 
            => new Vector2(
            r * Math.Cos(θ),
            r * Math.Sin(θ));
        public static Vector2 Elliptical(double a, double b, double θ) 
            => new Vector2(
            a * Math.Cos(θ),
            b * Math.Sin(θ));

        public (double r, double θ) ToPolar()
            => (Magnitude(), Math.Atan2(Y, X));

        public double X => data.x;
        public double Y => data.y;
        public (double x, double y) Coords => data;
        public double SumSquares() => data.x * data.x + data.y * data.y;
        public double Magnitude() => Math.Sqrt(SumSquares());

        public bool IsZero { get => data.x == 0 && data.y == 0; }

        public System.Numerics.Vector2 ToSngVector2()
            => new System.Numerics.Vector2((float)X, (float)Y);

        #region Algebra
        public static Vector2 Negate(Vector2 a)
            => new Vector2(
                -a.data.x,
                -a.data.y);
        public static Vector2 Scale(double factor, Vector2 a)
            => new Vector2(
                factor * a.data.x,
                factor * a.data.y);
        public static Vector2 Add(Vector2 a, Vector2 b)
            => new Vector2(
                a.data.x + b.data.x,
                a.data.y + b.data.y);
        public static Vector2 Subtract(Vector2 a, Vector2 b)
            => new Vector2(
                a.data.x - b.data.x,
                a.data.y - b.data.y);

        public static Vector2 operator +(Vector2 a, Vector2 b) => Add(a, b);
        public static Vector2 operator -(Vector2 a) => Negate(a);
        public static Vector2 operator -(Vector2 a, Vector2 b) => Subtract(a, b);
        public static Vector2 operator *(double f, Vector2 a) => Scale(f, a);
        public static Vector2 operator *(Vector2 a, double f) => Scale(f, a);
        public static Vector2 operator /(Vector2 a, double d) => Scale(1 / d, a);

        #endregion

        #region Vector Algebra        

        /// <summary>
        /// Scale the vector such that the magnitude is one (if possible).
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>A normalized vector.</returns>
        public static Vector2 Normalize(Vector2 vector)
        {
            double m = vector.Magnitude();

            if (m > 0)
            {
                return vector/m;
            }
            return vector;
        }

        /// <summary>
        /// Calculate the Eucledian distance between two points.
        /// </summary>
        /// <param name="fromPoint">From point.</param>
        /// <param name="toPoint">To point.</param>
        public static double Distance(Vector2 fromPoint, Vector2 toPoint)
            => (toPoint-fromPoint).Magnitude();

        /// <summary>
        /// Get the direction vector between two points.
        /// </summary>
        /// <param name="fromPoint">From point.</param>
        /// <param name="toPoint">To point.</param>
        public static Vector2 Direction(Vector2 fromPoint, Vector2 toPoint)
            => Normalize(toPoint-fromPoint);
        
        /// <summary>
        /// Dot product of two vectors.
        /// </summary>
        public static double Dot(Vector2 a, Vector2 b)
            => a.data.x*b.data.x + a.data.y*b.data.y;

        /// <summary>
        /// Cross product of two vectors.
        /// </summary>
        /// <returns>An out of plane value</returns>
        public static double Cross(Vector2 a, Vector2 b)
            => a.data.x * b.data.y - a.data.y * b.data.x;
        /// <summary>
        /// Cross product of a vector with an out-of-plane value.
        /// </summary>
        /// <returns>A vector</returns>
        public static Vector2 Cross(Vector2 a, double b)
            => new Vector2(a.data.y * b, -a.data.x * b);
        /// <summary>
        /// Cross product of a out-of-plane value with a vector.
        /// </summary>
        /// <returns>A vector</returns>
        public static Vector2 Cross(double a, Vector2 b)
            => Cross(b, -a);

        #endregion

        #region Equality
        public static bool operator ==(Vector2 vector1, Vector2 vector2)
        {
            return vector1.Equals(vector2);
        }

        public static bool operator !=(Vector2 vector1, Vector2 vector2)
        {
            return !(vector1 == vector2);
        }
        public override bool Equals(object obj)
        {
            return obj is Vector2 vector && Equals(vector);
        }
        public bool Equals(Vector2 other) => data.Equals(other.data);

        public override int GetHashCode()
        {
            return 1768953197 + data.GetHashCode();
        }
        #endregion

        #region Collections
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return data.x;
                    case 1: return data.y;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public double[] ToArray() => new[] { data.x, data.y };
        public void CopyTo(Array array, int index) => ToArray().CopyTo(array, index);
        public int Count { get => 2; }
        object System.Collections.ICollection.SyncRoot { get => null; }
        bool System.Collections.ICollection.IsSynchronized { get => false; }
        public System.Collections.IEnumerator GetEnumerator() => ToArray().GetEnumerator();
        public System.Collections.Generic.IEnumerable<double> AsEnumerable() => this as System.Collections.Generic.IEnumerable<double>;
        System.Collections.Generic.IEnumerator<double> System.Collections.Generic.IEnumerable<double>.GetEnumerator()
        {
            yield return data.x;
            yield return data.y;
        }
        #endregion

        #region Formatting
        public string ToString(string formatting, IFormatProvider provider)
        {
            string x = data.x.ToString(formatting, provider);
            string y = data.y.ToString(formatting, provider);
            return $"({x},{y})";
        }
        public string ToString(string formatting)
            => ToString(formatting, null);
        public override string ToString()
            => ToString("g");

        #endregion
    }
}
