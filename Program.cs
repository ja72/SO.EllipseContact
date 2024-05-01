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
