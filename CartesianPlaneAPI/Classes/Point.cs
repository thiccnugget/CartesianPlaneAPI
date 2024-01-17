using System.Runtime.CompilerServices;

namespace CartesianPlaneAPI.Classes
{
    public class Point : IComparable<Point>
    {
        public double x { get; }
        public double y { get; }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }


        //Override the GetHashCode to consider unique a point combining it's coordinates
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        //Two points are equal when their coordinates are equal
        public override bool Equals(object obj)
        {
            if (obj is Point p)
            {
                return this.x == p.x && this.y == p.y;
            }
            return false;
        }

        //Implement the CompareTo method of the IComparable interface
        public int CompareTo(Point? other)
        {
            int xComparison = x.CompareTo(other.x);

            return xComparison == 0 ? y.CompareTo(other.y) : xComparison;
        }
    }
}
