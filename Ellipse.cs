using System;
using System.Collections.Generic;

using static System.Math;

namespace SO
{
    public readonly struct Ellipse :
        IEquatable<Ellipse>,
        IFormattable
    {
        public Ellipse(double majorAxis, double minorAxis)
            : this(Point.Origin, majorAxis, minorAxis)
        {
        }
        public Ellipse(Point center, double majorAxis, double minorAxis)
        {
            this.Center = center;
            this.MajorAxis=majorAxis;
            this.MinorAxis=minorAxis;
        }

        public double MajorAxis { get; }
        public double MinorAxis { get; }
        public Point Center { get; }

        public Point GetPoint(double t)
        {
            double x = Center.X + MajorAxis * Cos(t), y = Center.Y + MinorAxis * Sin(t);
            return new Point(x, y);
        }
        public Point GetClosestPoint(Circle circle, double tol)
            => GetClosestPoint(circle.Center, tol);

        public Point GetClosestPoint(Point point, double tol)
        {
            double cx = point.X - Center.X, cy = point.Y - Center.Y;
            double Q = MajorAxis*MajorAxis-MinorAxis*MinorAxis;
            double A = 2*cx*MajorAxis/Q;
            double B = 2*cy*MinorAxis/Q;

            double IterFun(double z)
            {
                return 1/A*(B + 2*z/Sqrt(1+z*z));
            }

            double z_sol = NumericalMethods.GaussPointIteration(IterFun, 0, tol);

            double t = Atan(z_sol);

            return GetPoint(t);
        }

        public double DistanceTo(Point point, double tol)
            => GetClosestPoint(point, tol).DistanceTo(point);
        public double DistanceTo(Circle circle, double tol)
            => GetClosestPoint(circle.Center, tol).DistanceTo(circle);

        #region Equality
        public override bool Equals(object obj) => obj is Ellipse ellipse && Equals(ellipse);

        public bool Equals(Ellipse other)
        {
            return MajorAxis==other.MajorAxis&&
                   MinorAxis==other.MinorAxis&&
                   Center.Equals(other.Center);
        }

        public override int GetHashCode()
        {
            var hashCode = -1545607722;
            hashCode=hashCode*-1521134295+MajorAxis.GetHashCode();
            hashCode=hashCode*-1521134295+MinorAxis.GetHashCode();
            hashCode=hashCode*-1521134295+EqualityComparer<Point>.Default.GetHashCode(Center);
            return hashCode;
        }

        public static bool operator ==(Ellipse ellipse1, Ellipse ellipse2)
        {
            return ellipse1.Equals(ellipse2);
        }

        public static bool operator !=(Ellipse ellipse1, Ellipse ellipse2)
        {
            return !(ellipse1==ellipse2);
        }
        #endregion

        #region Formatting
        public string ToString(string formatting, IFormatProvider provider)
        {
            string x_str = Center.X.ToString(formatting, provider);
            string y_str = Center.Y.ToString(formatting, provider);
            string a_str = MajorAxis.ToString(formatting, provider);
            string b_str = MinorAxis.ToString(formatting, provider);
            return $"Ellipse(x={x_str}, y={y_str}, rx={a_str}, ry={b_str})";
        }
        public string ToString(string formatting)
            => ToString(formatting, null);
        public override string ToString()
            => ToString("g4");
        #endregion

    }
}
