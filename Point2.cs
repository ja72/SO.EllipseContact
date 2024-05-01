using System;

using static System.Math;

namespace SO
{
    public readonly struct Point2 :
        IEquatable<Point2>,
        IFormattable
    {
        public Point2(double x, double y) : this()
        {
            X=x;
            Y=y;
        }

        public static Point2 Origin { get; } = new Point2(0, 0);

        public double X { get; }
        public double Y { get; }

        public (double x, double y) Coords => (X, Y);

        public Vector2 AsVector() => new Vector2(X, Y);

        public double DistanceTo(Point2 other)
            => Sqrt((other.X-X)*(other.X-X) + (other.Y-Y)*(other.Y-Y));
        public double DistanceTo(Circle2 circle)
            => circle.DistanceTo(this);

        public Vector2 VectorTo(Point2 target)
            => Difference(target, this);

        #region Equality
        public override bool Equals(object obj)
        {
            return obj is Point2 point && Equals(point);
        }
        public bool Equals(Point2 other)
            => X == other.X && Y == other.Y;

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode=hashCode*-1521134295+X.GetHashCode();
            hashCode=hashCode*-1521134295+Y.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Point2 point1, Point2 point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Point2 point1, Point2 point2)
        {
            return !(point1==point2);
        }
        #endregion

        #region Algebra
        public static Point2 Add(Point2 a, Vector2 b)
            => new Point2(
                a.X+b.X,
                a.Y+b.Y);

        public static Point2 Subtract(Point2 a, Vector2 b)
            => new Point2(
                a.X-b.X,
                a.Y-b.Y);

        public static Vector2 Difference(Point2 target, Point2 reference)
        {
            double dx = target.X - reference.X;
            double dy = target.Y - reference.Y;
            return new Vector2(dx, dy);
        }

        public static double Dot(Point2 point, Line2 line)
            => line.A* point.X + line.B * point.Y + line.C;

        public static Point2 operator +(Point2 a, Vector2 b) => Add(a, b);
        public static Point2 operator -(Point2 a, Vector2 b) => Subtract(a, b);
        public static Vector2 operator -(Point2 a, Point2 b) => Difference(a, b);
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
