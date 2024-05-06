using System;
using System.Collections.Generic;

using static System.Math;

namespace SO
{
    public readonly struct Ellipse2 :
        IEquatable<Ellipse2>,
        IFormattable
    {
        public Ellipse2(double majorAxis, double minorAxis)
            : this(Point2.Origin, majorAxis, minorAxis)
        {
        }
        public Ellipse2(Point2 center, double majorAxis, double minorAxis)
        {
            this.Center = center;
            this.MajorAxis=majorAxis;
            this.MinorAxis=minorAxis;
        }

        public double MajorAxis { get; }
        public double MinorAxis { get; }
        public Point2 Center { get; }

        public Point2 GetPoint(double t)
        {
            double x = Center.X + MajorAxis * Cos(t), y = Center.Y + MinorAxis * Sin(t);
            return new Point2(x, y);
        }
        public Point2 GetClosestPoint(Circle2 circle, double tol)
            => GetClosestPoint(circle.Center, tol);

        public Point2 GetClosestPoint(Point2 point, double tol)
        {
            var delta = Center.VectorTo(point);
            var sign = Sign(delta.X);
            var Q = MajorAxis*MajorAxis-MinorAxis*MinorAxis;
            var A = 2*delta.X*MajorAxis/Q;
            var B = 2*delta.Y*MinorAxis/Q;
            double t;
            if (A!=0)
            {

                double IterFun(double z)
                {
                    return 1/A*(B + (sign)* 2*z/Sqrt(1+z*z));
                }

                var z_sol = NumericalMethods.GaussPointIteration(IterFun, 0, tol);

                t = sign == 1 ? (float)Atan(z_sol) : (float)Atan(z_sol) + (float)PI;

            }
            else
            {
                t = Sign(B)*(PI/2);
            }
            return GetPoint(t);
        }
        public Point2 GetClosestPoint(Line2 line, double tol)
        {
            var (a, b, c)= line.Coords;
            var (cx, cy) = Center.Coords;
            // transform line coordinates to being relative to ellipse center
            c -= -a*cx - b*cy;
            double rx = MajorAxis, ry = MinorAxis;

            double IterFun(double z)
            {
                return (b*ry-a*rx*z)*(a*rx+b*ry*z)/(a*c*rx*Sqrt(1+z*z))+(b*ry)/(a*rx);
            }

            double z_sol = NumericalMethods.GaussPointIteration(IterFun, 0, tol);

            double t = Atan(z_sol);

            return GetPoint(t);

        }

        public double DistanceTo(Point2 point, double tol)
            => GetClosestPoint(point, tol).DistanceTo(point);
        public double DistanceTo(Circle2 circle, double tol)
            => GetClosestPoint(circle.Center, tol).DistanceTo(circle);

        #region Equality
        public override bool Equals(object obj) => obj is Ellipse2 ellipse && Equals(ellipse);

        public bool Equals(Ellipse2 other)
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
            hashCode=hashCode*-1521134295+EqualityComparer<Point2>.Default.GetHashCode(Center);
            return hashCode;
        }

        public static bool operator ==(Ellipse2 ellipse1, Ellipse2 ellipse2)
        {
            return ellipse1.Equals(ellipse2);
        }

        public static bool operator !=(Ellipse2 ellipse1, Ellipse2 ellipse2)
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
