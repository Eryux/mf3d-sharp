using System;

namespace MF3D
{
    [Serializable]
    public struct Vector3d : IComparable<Vector3d>, IEquatable<Vector3d>
    {
        public double x; public double y; public double z;


        public Vector3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3d(Vector3d vect)
        {
            x = vect.x;
            y = vect.y;
            z = vect.z;
        }

        static public readonly Vector3d Zero = new Vector3d(0.0, 0.0, 0.0);
        static public readonly Vector3d One = new Vector3d(1.0, 1.0, 1.0);
        static public readonly Vector3d AxisX = new Vector3d(1.0, 0.0, 0.0);
        static public readonly Vector3d AxisY = new Vector3d(0.0, 1.0, 0.0);
        static public readonly Vector3d AxisZ = new Vector3d(0.0, 0.0, 1.0);
        static public readonly Vector3d MaxValue = new Vector3d(double.MaxValue, double.MaxValue, double.MaxValue);
        static public readonly Vector3d MinValue = new Vector3d(double.MinValue, double.MinValue, double.MinValue);


        public double this[int i]
        {
            get { return (i == 0) ? x : (i == 1) ? y : z; }
            set { if (i == 0) x = value; else if (i == 1) y = value; else z = value; }
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

        public double Z
        {
            get { return z; }
            set { z = value; }
        }


        public double LengthSquared
        {
            get { return x * x + y * y + z * z; }
        }

        public double Length
        {
            get { return System.Math.Sqrt(x * x + y * y + z * z); }
        }

        public Vector3d Normalized
        {
            get { return new Vector3d(this).Normalize(); }
        }

        public bool IsNormalized
        {
            get { return System.Math.Abs(x * x + y * y + z * z - 1) < Constant.ZeroTolerance; }
        }

        public bool IsFinite
        {
            get
            {
                double f = x + y + z;
                return !double.IsNaN(f) && !double.IsInfinity(f);
            }
        }


        public Vector3d Normalize(double epsilon = Constant.Epsilon)
        {
            double length = Length;

            if (length > epsilon)
            {
                length = 1.0 / length;
                x *= length;
                y *= length;
                z *= length;
            }
            else
            {
                x = y = z = 0;
            }

            return this;
        }

        public double Dot(Vector3d vect)
        {
            return x * vect.x + y * vect.y + z * vect.z;
        }

        public Vector3d Cross(Vector3d vect)
        {
            return new Vector3d(
                y * vect.z - z * vect.y,
                z * vect.x - x * vect.z,
                x * vect.y - y * vect.x
            );
        }

        public double DistanceSquared(Vector3d vect)
        {
            double dx = vect.x - x;
            double dy = vect.y - y;
            double dz = vect.z - z;
            return dx * dx + dy * dy + dz * dz;
        }

        public double Distance(Vector3d vect)
        {
            return System.Math.Sqrt(DistanceSquared(vect));
        }


        public static Vector3d Lerp(Vector3d a, Vector3d b, double t)
        {
            double s = 1 - t;
            return new Vector3d(
                s * a.x + t * b.x,
                s * a.y + t * b.y,
                s * a.z + t * b.z
            );
        }


        public static bool operator ==(Vector3d a, Vector3d b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vector3d a, Vector3d b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public static Vector3d operator -(Vector3d vect)
        {
            return new Vector3d(vect.x * -1.0, vect.y * -1.0, vect.z * -1.0);
        }

        public static Vector3d operator +(Vector3d vect, double f)
        {
            return new Vector3d(vect.x + f, vect.y + f, vect.z + f);
        }

        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3d operator -(Vector3d vect, double f)
        {
            return new Vector3d(vect.x - f, vect.y - f, vect.z - f);
        }

        public static Vector3d operator -(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3d operator *(Vector3d vect, double f)
        {
            return new Vector3d(vect.x * f, vect.y * f, vect.z * f);
        }

        public static Vector3d operator *(double f, Vector3d vect)
        {
            return new Vector3d(vect.x * f, vect.y * f, vect.z * f);
        }

        public static Vector3d operator /(Vector3d vect, double f)
        {
            return new Vector3d(vect.x / f, vect.y / f, vect.z / f);
        }

        public static Vector3d operator /(double f, Vector3d vect)
        {
            return new Vector3d(vect.x / f, vect.y / f, vect.z / f);
        }

        public static Vector3d operator *(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3d operator /(Vector3d a, Vector3d b)
        {
            return new Vector3d(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3d operator *(Vector3d vect, Quaterniond quat)
        {
            Vector3d quatvect = new Vector3d(quat.x, quat.y, quat.z);
            return 2.0 * quatvect.Dot(vect) * quatvect
                + (quat.w * quat.w - quatvect.Dot(quatvect)) * vect
                + 2.0 * quat.w * quatvect.Cross(vect);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ x.GetHashCode();
                hash = (hash * 16777619) ^ y.GetHashCode();
                hash = (hash * 16777619) ^ z.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return this == (Vector3d)obj;
        }

        public bool Equals(Vector3d other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public int CompareTo(Vector3d other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            else if (z != other.z)
                return z < other.z ? -1 : 1;
            return 0;
        }


        public override string ToString()
        {
            return string.Format("{0:F8} {1:F8} {2:F8}", x, y, z);
        }

        public string ToString(string fmt)
        {
            return string.Format("{0} {1} {2}", x.ToString(fmt), y.ToString(fmt), z.ToString(fmt));
        }
    }
}