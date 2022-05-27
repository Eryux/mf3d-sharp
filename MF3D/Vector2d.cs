using System;

namespace MF3D
{
    [Serializable]
    public struct Vector2d : IComparable<Vector2d>, IEquatable<Vector2d>
    {
        public double x; public double y;


        public Vector2d(double x, double y)
        {
            this.x = x; 
            this.y = y;
        }

        public Vector2d(Vector2d vect)
        {
            x = vect.x;
            y = vect.y;
        }


        static public readonly Vector2d Zero = new Vector2d(0, 0);
        static public readonly Vector2d One = new Vector2d(1, 1);
        static public readonly Vector2d AxisX = new Vector2d(1, 0);
        static public readonly Vector2d AxisY = new Vector2d(0, 1);
        static public readonly Vector2d MaxValue = new Vector2d(double.MaxValue, double.MaxValue);
        static public readonly Vector2d MinValue = new Vector2d(double.MinValue, double.MinValue);


        public double this[int i]
        {
            get { return (i == 0) ? x : y; }
            set { if (i == 0) x = value; else y = value; }
        }


        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double LengthSquared
        {
            get { return x * x + y * y; }
        }

        public double Length
        {
            get { return System.Math.Sqrt(x * x + y * y); }
        }

        public Vector2d Normalized
        {
            get { return new Vector2d(this).Normalize(); }
        }

        public bool IsNormalized
        {
            get { return System.Math.Abs(x * x + y * y - 1) < Constant.ZeroTolerance; }
        }

        public bool IsFinite
        {
            get 
            {
                double f = x + y;
                return !double.IsNaN(f) && !double.IsInfinity(f);
            }
        }


        public Vector2d Normalize(double epsilon = Constant.Epsilon)
        {
            double length = Length;

            if (length > epsilon)
            {
                length = 1.0 / length;
                x *= length;
                y *= length;
            }
            else
            {
                x = y = 0;
            }

            return this;
        }

        public double Dot(Vector2d vect)
        {
            return x * vect.x + y * vect.y;
        }

        public double Cross(Vector2d vect)
        {
            return x * vect.y - y * vect.x;
        }

        public double DistanceSquared(Vector2d vect)
        {
            double dx = vect.x - x;
            double dy = vect.y - y;
            return dx * dx + dy * dy;
        }

        public double Distance(Vector2d vect)
        {
            return System.Math.Sqrt(DistanceSquared(vect));
        }


        public static Vector2d Lerp(Vector2d a, Vector2d b, double t)
        {
            double s = 1 - t;
            return new Vector2d(
                s * a.x + t * b.x,
                s * a.y + t * b.y
            );
        }


        public static bool operator ==(Vector2d a, Vector2d b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2d a, Vector2d b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static Vector2d operator -(Vector2d vect)
        {
            return new Vector2d(vect.x * -1.0, vect.y * -1.0);
        }

        public static Vector2d operator +(Vector2d vect, double f)
        {
            return new Vector2d(vect.x + f, vect.y + f);
        }

        public static Vector2d operator +(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x + b.x, a.y + b.y);
        }

        public static Vector2d operator -(Vector2d vect, double f)
        {
            return new Vector2d(vect.x - f, vect.y - f);
        }

        public static Vector2d operator -(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x - b.x, a.y - b.y);
        }

        public static Vector2d operator *(Vector2d vect, double f)
        {
            return new Vector2d(vect.x * f, vect .y * f);
        }

        public static Vector2d operator *(double f, Vector2d vect)
        {
            return new Vector2d(vect.x * f, vect.y * f);
        }

        public static Vector2d operator /(Vector2d vect, double f)
        {
            return new Vector2d(vect.x / f, vect.y / f);
        }

        public static Vector2d operator /(double f, Vector2d vect)
        {
            return new Vector2d(vect.x / f, vect.y / f);
        }

        public static Vector2d operator *(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x * b.x, a.y * b.y);
        }

        public static Vector2d operator /(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.x / b.x, a.y / b.y);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ x.GetHashCode();
                hash = (hash * 16777619) ^ y.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return this == (Vector2d)obj;
        }

        public bool Equals(Vector2d other)
        {
            return (x == other.x && y == other.y);
        }

        public int CompareTo(Vector2d other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            return 0;
        }


        public override string ToString()
        {
            return string.Format("{0:F8} {1:F8}", x, y);
        }
    }
}