using System;
using System.Collections.Generic;

using static System.Math;

namespace SO
{
    public readonly struct Circle :
        IEquatable<Circle>,
        IFormattable
    {
        public Circle(double radius)
            : this(Point.Origin, radius)
        { }
        public Circle(Point center, double radius)
        {
            this.Center = center;
            this.Radius=radius;
        }

        public double Radius { get; }
        public Point Center { get; }

        public Point GetPoint(double t)
        {
            double x = Center.X + Radius * Cos(t), y = Center.Y + Radius * Sin(t);
            return new Point(x, y);
        }

        public Point GetClosestPoint(Point point)
        {
            double cx = point.X - Center.X, cy = point.Y - Center.Y;
            double t = Atan2(cy, cx);
            return GetPoint(t);
        }

        public double DistanceTo(Point point)
            => Center.DistanceTo(point) - Radius;
        public double DistanceTo(Circle circle)
            => Center.DistanceTo(circle.Center) - Radius - circle.Radius;

        #region Equality
        public override bool Equals(object obj) => obj is Circle circle && Equals(circle);

        public bool Equals(Circle other)
        {
            return Radius==other.Radius&&
                   Center.Equals(other.Center);
        }

        public override int GetHashCode()
        {
            var hashCode = -1545607722;
            hashCode=hashCode*-1521134295+Radius.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<Point>.Default.GetHashCode(Center);
            return hashCode;
        }

        public static bool operator ==(Circle circle1, Circle circle2)
        {
            return circle1.Equals(circle2);
        }

        public static bool operator !=(Circle circle1, Circle circle2)
        {
            return !(circle1==circle2);
        }
        #endregion

        #region Formatting
        public string ToString(string formatting, IFormatProvider provider)
        {
            string x_str = Center.X.ToString(formatting, provider);
            string y_str = Center.Y.ToString(formatting, provider);
            string r_str = Radius.ToString(formatting, provider);
            return $"Circle(x={x_str}, y={y_str}, r={r_str})";
        }
        public string ToString(string formatting)
            => ToString(formatting, null);
        public override string ToString()
            => ToString("g4");
        #endregion
    }
}
