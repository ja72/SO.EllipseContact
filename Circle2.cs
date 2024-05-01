using System;
using System.Collections.Generic;

using static System.Math;

namespace SO
{
    public readonly struct Circle2 :
        IEquatable<Circle2>,
        IFormattable
    {
        public Circle2(double radius)
            : this(Point2.Origin, radius)
        { }
        public Circle2(Point2 center, double radius)
        {
            this.Center = center;
            this.Radius=radius;
        }

        public double Radius { get; }
        public Point2 Center { get; }

        public Point2 GetPoint(double t)
        {
            double x = Center.X + Radius * Cos(t), y = Center.Y + Radius * Sin(t);
            return new Point2(x, y);
        }

        public Point2 GetClosestPoint(Point2 point)
        {
            double cx = point.X - Center.X, cy = point.Y - Center.Y;
            double t = Atan2(cy, cx);
            return GetPoint(t);
        }

        public double DistanceTo(Point2 point)
            => Center.DistanceTo(point) - Radius;
        public double DistanceTo(Circle2 circle)
            => Center.DistanceTo(circle.Center) - Radius - circle.Radius;

        #region Equality
        public override bool Equals(object obj) => obj is Circle2 circle && Equals(circle);

        public bool Equals(Circle2 other)
        {
            return Radius==other.Radius&&
                   Center.Equals(other.Center);
        }

        public override int GetHashCode()
        {
            var hashCode = -1545607722;
            hashCode=hashCode*-1521134295+Radius.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<Point2>.Default.GetHashCode(Center);
            return hashCode;
        }

        public static bool operator ==(Circle2 circle1, Circle2 circle2)
        {
            return circle1.Equals(circle2);
        }

        public static bool operator !=(Circle2 circle1, Circle2 circle2)
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
