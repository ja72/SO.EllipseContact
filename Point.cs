using System;

using static System.Math;

namespace SO
{
    public readonly struct Point :
        IEquatable<Point>,
        IFormattable
    {
        public Point(double x, double y) : this()
        {
            X=x;
            Y=y;
        }

        public static Point Origin { get; } = new Point(0, 0);

        public double X { get; }
        public double Y { get; }

        public double DistanceTo(Point other)
            => Sqrt((other.X-X)*(other.X-X) + (other.Y-Y)*(other.Y-Y));
        public double DistanceTo(Circle circle)
            => circle.DistanceTo(this);

        #region Equality
        public override bool Equals(object obj)
        {
            return obj is Point point && Equals(point);
        }
        public bool Equals(Point other)
            => X == other.X && Y == other.Y;

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode=hashCode*-1521134295+X.GetHashCode();
            hashCode=hashCode*-1521134295+Y.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Point point1, Point point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1==point2);
        }
        #endregion

        #region Algebra
        public static Point Negate(Point a)
            => new Point(
                -a.X,
                -a.Y);
        public static Point Scale(double factor, Point a)
            => new Point(
                factor*a.X,
                factor*a.Y);
        public static Point Add(Point a, Point b)
            => new Point(
                a.X+b.X,
                a.Y+b.Y);
        public static Point Subtract(Point a, Point b)
            => new Point(
                a.X-b.X,
                a.Y-b.Y);

        public static Point operator +(Point a, Point b) => Add(a, b);
        public static Point operator -(Point a) => Negate(a);
        public static Point operator -(Point a, Point b) => Subtract(a, b);
        public static Point operator *(double f, Point a) => Scale(f, a);
        public static Point operator *(Point a, double f) => Scale(f, a);
        public static Point operator /(Point a, double d) => Scale(1/d, a);
        #endregion

        #region Formatting
        public string ToString(string formatting, IFormatProvider provider)
        {
            string x_str = X.ToString(formatting, provider);
            string y_str = Y.ToString(formatting, provider);
            return $"Point(x={x_str}, y={y_str})";
        }
        public string ToString(string formatting)
            => ToString(formatting, null);
        public override string ToString()
            => ToString("g4");
        #endregion

    }
}
