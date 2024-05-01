using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static class NumericalMethods
    {
        public static int MaxIterations { get; } = 128;
        public static double LooseTolerance { get; } = 1e-6;
        public static double TightTolerance { get; } = 1e-10;

        public static double GaussPointIteration(this Func<double, double> f, double x_init, double tol)
        {
            double x = x_init, x_old;
            int iter = 0;
            do
            {
                iter++;
                x_old = x;
                x = f(x);
            } while (iter < MaxIterations &&  Abs(x-x_old)>tol);

            return x;
        }

        public static double BisectionRoot(this Func<double, double> f, double x_low, double x_high, double tol)
        {
            double f_low = f(x_low), f_high = f(x_high);

            if (Abs(f_low)<=tol) return x_low;
            if (Abs(f_high)<=tol) return x_high;

            int iter = 0;

            while (iter <= MaxIterations && f_low*f_high > 0)
            {
                iter++;
                (x_low, x_high) =((3*x_low-x_high)/2, (3*x_high-x_low)/2);
                (f_low, f_high) =(f(x_low), f(x_high));
            }

            if (iter>MaxIterations)
                throw new ArgumentException("Invalid Initial Conditions for Bisection.");

            iter = 0;
            double x_mid, f_mid;
            do
            {
                iter++;
                x_mid = (x_low + x_high)/2;
                f_mid = f(x_mid);
                if (Abs(f_mid) <= tol) return x_mid;
                if (f_low*f_mid<0)
                {
                    x_high = x_mid;
                    f_high = f_mid;
                }
                else if (f_high*f_mid<0)
                {
                    x_low = x_mid;
                    f_low = f_mid;
                }
                else
                    throw new InvalidOperationException();

            } while (iter <= MaxIterations && Abs(x_high-x_low)>2*tol);

            if (iter>MaxIterations)
                throw new ArgumentException("Tolerance is too small. Max iterations reached.");

            return x_mid;
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            Ellipse ellipse = new Ellipse(7, 1);
            Point pivot = new Point(10, -4);
            Circle arc = new Circle(pivot, 8);

            Console.WriteLine($"Arc     = {arc}");
            Console.WriteLine($"Target  = {ellipse}");

            double RootFun(double u)
            {
                Circle cen = new Circle(arc.GetPoint(u), 1);
                Point trgt = ellipse.GetClosestPoint(cen, NumericalMethods.LooseTolerance);
                return trgt.DistanceTo(cen);
            }

            double u_sol = NumericalMethods.BisectionRoot(RootFun, 0, PI/2, 1e-6);
            Circle circle = new Circle(arc.GetPoint(u_sol), 1);
            Point closest = ellipse.GetClosestPoint(circle, NumericalMethods.LooseTolerance);
            double distance = closest.DistanceTo(circle);

            Console.WriteLine($"Circle  = {circle}");
            Console.WriteLine($"Contact = {closest}");
            Console.WriteLine($"distance= {distance}");

            /*
            Arc     = Circle(x=10, y=-4, r=8)
            Target  = Ellipse(x=0, y=0, rx=7, ry=1)
            Circle  = Circle(x=4.46565766050686, y=1.77676854905003, r=1)
            Contact = Point(x=4.35294539705547, y=0.783136894362217)
            distance= 3.95975714484464E-06
            */

        }
    }
}
