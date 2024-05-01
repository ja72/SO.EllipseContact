# EllipseContact

Sample code for [answer on SO](https://math.stackexchange.com/a/4908633/3301) which asks for finding a tangent circle to an ellipse, if the circle is constrained to be on an arc.

My solution involves two numerical methods, a bisection of the location of the tangent circle as an outer loop, and a single point iteration for each circe location to find the closest point to the ellipse.

![fig1](https://i.sstatic.net/f84ASK6t.png)

## Closest Point on an ellipse to outside point

The code snippet below shows the single-point iteration algorithm used for the ellipse-circle distance

```C#
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
```

## Search for circle-ellipse tangency

The code snipper below used a bisection method to find the circle's location that is tangent to the ellipse.

```C#
  Ellipse ellipse = new Ellipse(7, 1);
  Point pivot = new Point(10, -4);
  Circle arc = new Circle(pivot, 8);

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
```

## Sample Output

```text
Arc     = Circle(x=10, y=-4, r=8)
Target  = Ellipse(x=0, y=0, rx=7, ry=1)
Circle  = Circle(x=4.46565766050686, y=1.77676854905003, r=1)
Contact = Point(x=4.35294539705547, y=0.783136894362217)
distance= 3.95975714484464E-06
```
