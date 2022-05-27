using System;

namespace MF3D
{
    [Serializable]
    public struct Vector4d : IComparable<Vector4d>, IEquatable<Vector4d>
    {
        public double x; public double y; public double z; public double w;


        public Vector4d(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4d(Vector4d vect)
        {
            x = vect.x;
            y = vect.y;
            z = vect.z;
            w = vect.w;
        }

        static public readonly Vector4d Zero = new Vector4d(0.0, 0.0, 0.0, 0.0);
        static public readonly Vector4d One = new Vector4d(1.0, 1.0, 1.0, 1.0);
        static public readonly Vector4d AxisX = new Vector4d(1.0, 0.0, 0.0, 0.0);
        static public readonly Vector4d AxisY = new Vector4d(0.0, 1.0, 0.0, 0.0);
        static public readonly Vector4d AxisZ = new Vector4d(0.0, 0.0, 1.0, 0.0);
        static public readonly Vector4d AxisW = new Vector4d(0.0, 0.0, 0.0, 1.0);
        static public readonly Vector4d MaxValue = new Vector4d(double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue);
        static public readonly Vector4d MinValue = new Vector4d(double.MinValue, double.MinValue, double.MinValue, double.MinValue);


        public double this[int i]
        {
            get { return (i == 0) ? x : (i == 1) ? y : (i == 2) ? z : w; }
            set { if (i == 0) x = value; else if (i == 1) y = value; else if (i == 2) z = value; else w = value; }
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

        public double W
        {
            get { return w; }
            set { w = value; }
        }


        public double LengthSquared
        {
            get { return x * x + y * y + z * z + w * w; }
        }

        public double Length
        {
            get { return System.Math.Sqrt(x * x + y * y + z * z + w * w); }
        }

        public Vector4d Normalized
        {
            get { return new Vector4d(this).Normalize(); }
        }

        public bool IsNormalized
        {
            get { return System.Math.Abs(x * x + y * y + z * z + w * w - 1) < Constant.ZeroTolerance; }
        }

        public bool IsFinite
        {
            get
            {
                double f = x + y + z + w;
                return !double.IsNaN(f) && !double.IsInfinity(f);
            }
        }


        public Vector4d Normalize(double epsilon = Constant.Epsilon)
        {
            double length = Length;

            if (length > epsilon)
            {
                length = 1.0 / length;
                x *= length;
                y *= length;
                z *= length;
                w *= length;
            }
            else
            {
                x = y = z = w = 0;
            }

            return this;
        }

        public double Dot(Vector4d vect)
        {
            return x * vect.x + y * vect.y + z * vect.z + w * vect.w;
        }

        public Vector4d Cross(Vector4d vect)
        {
            return new Vector4d(
                z * vect.w - w * vect.z,
                w * vect.x - x * vect.w,
                x * vect.y - y * vect.x,
                y * vect.z - z * vect.y
            );
        }

        public double DistanceSquared(Vector4d vect)
        {
            double dx = vect.x - x;
            double dy = vect.y - y;
            double dz = vect.z - z;
            double dw = vect.w - w;
            return dx * dx + dy * dy + dz * dz + dw * dw;
        }

        public double Distance(Vector4d vect)
        {
            return System.Math.Sqrt(DistanceSquared(vect));
        }


        public static Vector4d Lerp(Vector4d a, Vector4d b, double t)
        {
            double s = 1 - t;
            return new Vector4d(
                s * a.x + t * b.x,
                s * a.y + t * b.y,
                s * a.z + t * b.z,
                s * a.w + t * b.w
            );
        }


        public static bool operator ==(Vector4d a, Vector4d b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }

        public static bool operator !=(Vector4d a, Vector4d b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }

        public static Vector4d operator -(Vector4d vect)
        {
            return new Vector4d(vect.x * -1.0, vect.y * -1.0, vect.z * -1.0, vect.w * -1.0);
        }

        public static Vector4d operator +(Vector4d vect, double f)
        {
            return new Vector4d(vect.x + f, vect.y + f, vect.z + f, vect.w * f);
        }

        public static Vector4d operator +(Vector4d a, Vector4d b)
        {
            return new Vector4d(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Vector4d operator -(Vector4d vect, double f)
        {
            return new Vector4d(vect.x - f, vect.y - f, vect.z - f, vect.w - f);
        }

        public static Vector4d operator -(Vector4d a, Vector4d b)
        {
            return new Vector4d(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Vector4d operator *(Vector4d vect, double f)
        {
            return new Vector4d(vect.x * f, vect.y * f, vect.z * f, vect.w * f);
        }

        public static Vector4d operator *(double f, Vector4d vect)
        {
            return new Vector4d(vect.x * f, vect.y * f, vect.z * f, vect.w * f);
        }

        public static Vector4d operator /(Vector4d vect, double f)
        {
            return new Vector4d(vect.x / f, vect.y / f, vect.z / f, vect.w / f);
        }

        public static Vector4d operator /(double f, Vector4d vect)
        {
            return new Vector4d(vect.x / f, vect.y / f, vect.z / f, vect.w / f);
        }

        public static Vector4d operator *(Vector4d a, Vector4d b)
        {
            return new Vector4d(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }

        public static Vector4d operator /(Vector4d a, Vector4d b)
        {
            return new Vector4d(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ x.GetHashCode();
                hash = (hash * 16777619) ^ y.GetHashCode();
                hash = (hash * 16777619) ^ z.GetHashCode();
                hash = (hash * 16777619) ^ w.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return this == (Vector4d)obj;
        }

        public bool Equals(Vector4d other)
        {
            return (x == other.x && y == other.y && z == other.z && w == other.w);
        }

        public int CompareTo(Vector4d other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            else if (z != other.z)
                return z < other.z ? -1 : 1;
            else if (w != other.w)
                return w < other.w ? -1 : 1;
            return 0;
        }


        public override string ToString()
        {
            return string.Format("{0:F8} {1:F8} {2:F8} {3:F8}", x, y, z, w);
        }

        public string ToString(string fmt)
        {
            return string.Format("{0} {1} {2} {3}", x.ToString(fmt), y.ToString(fmt), z.ToString(fmt), w.ToString(fmt));
        }
    }
}