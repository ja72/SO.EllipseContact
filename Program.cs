using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Math;

namespace SO
{

    static class Program
    {
        static void Main(string[] args)
        {
            Ellipse2 ellipse = new Ellipse2(7, 1);
            Point2 pivot = new Point2(10, -4);
            Circle2 arc = new Circle2(pivot, 8);

            Console.WriteLine($"Arc     = {arc}");
            Console.WriteLine($"Target  = {ellipse}");

            double RootFun(double u)
            {
                Circle2 cen = new Circle2(arc.GetPoint(u), 1);
                Point2 trgt = ellipse.GetClosestPoint(cen, NumericalMethods.LooseTolerance);
                return trgt.DistanceTo(cen);
            }

            double u_sol = NumericalMethods.BisectionRoot(RootFun, 0, PI/2, 1e-6);
            Circle2 circle = new Circle2(arc.GetPoint(u_sol), 1);
            Point2 closest = ellipse.GetClosestPoint(circle, NumericalMethods.LooseTolerance);
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
