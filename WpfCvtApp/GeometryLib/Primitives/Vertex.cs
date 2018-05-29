using System;
using g3;

namespace GeometryLib.Primitives
{
    public class Vertex : IEquatable<Vertex> //, IVertex
    {

        public int Id { get; set; }

        public int I { get; set; }
        public int J { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public Vertex(double x, double y)
        {
            Point = new Vector2d(x, y);
            Position = new double[2];
            Position[0] = x;
            Position[1] = y;
        }

        public Vertex(int i, int j)
        {
            I = i;
            J = j;
        }


        public Vertex(double x, double y, int id) : this(x, y)
        {
            this.Id = id;
        }

        public Vertex(Vector2d point)
        {
            Point = point;
        }

        public double X => Point.x;

        public double Y => Point.y;

        public Vector2d Point { get; private set; }

        public override string ToString()
        {
            return $"({X:F4}, {Y:F4})";
        }


        public bool Equals(Vertex other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Point.Equals(other.Point);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vertex)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ Point.GetHashCode();
            }
        }

        public double[] Position { get; }
    }
}