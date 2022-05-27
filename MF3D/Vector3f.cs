using System;

namespace MF3D
{
    [Serializable]
    public struct Vector3f : IComparable<Vector3f>, IEquatable<Vector3f>
    {
        public float x; public float y; public float z;


        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3f(Vector3f vect)
        {
            x = vect.x;
            y = vect.y;
            z = vect.z;
        }

        static public readonly Vector3f Zero = new Vector3f(0.0f, 0.0f, 0.0f);
        static public readonly Vector3f One = new Vector3f(1.0f, 1.0f, 1.0f);
        static public readonly Vector3f AxisX = new Vector3f(1.0f, 0.0f, 0.0f);
        static public readonly Vector3f AxisY = new Vector3f(0.0f, 1.0f, 0.0f);
        static public readonly Vector3f AxisZ = new Vector3f(0.0f, 0.0f, 1.0f);
        static public readonly Vector3f MaxValue = new Vector3f(float.MaxValue, float.MaxValue, float.MaxValue);
        static public readonly Vector3f MinValue = new Vector3f(float.MinValue, float.MinValue, float.MinValue);


        public float this[int i]
        {
            get { return (i == 0) ? x : (i == 1) ? y : z; }
            set { if (i == 0) x = value; else if (i == 1) y = value; else z = value; }
        }


        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }


        public float LengthSquared
        {
            get { return x * x + y * y + z * z; }
        }

        public float Length
        {
            get { return (float)System.Math.Sqrt(x * x + y * y + z * z); }
        }

        public Vector3f Normalized
        {
            get { return new Vector3f(this).Normalize(); }
        }

        public bool IsNormalized
        {
            get { return System.Math.Abs(x * x + y * y + z * z - 1) < Constant.ZeroTolerance; }
        }

        public bool IsFinite
        {
            get
            {
                float f = x + y + z;
                return !float.IsNaN(f) && !float.IsInfinity(f);
            }
        }


        public Vector3f Normalize(float epsilon = Constant.Epsilonf)
        {
            float length = Length;

            if (length > epsilon)
            {
                length = 1.0f / length;
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

        public float Dot(Vector3f vect)
        {
            return x * vect.x + y * vect.y + z * vect.z;
        }

        public Vector3f Cross(Vector3f vect)
        {
            return new Vector3f(
                y * vect.z - z * vect.y,
                z * vect.x - x * vect.z,
                x * vect.y - y * vect.x
            );
        }

        public float DistanceSquared(Vector3f vect)
        {
            float dx = vect.x - x;
            float dy = vect.y - y;
            float dz = vect.z - z;
            return dx * dx + dy * dy + dz * dz;
        }

        public float Distance(Vector3f vect)
        {
            return (float)System.Math.Sqrt(DistanceSquared(vect));
        }


        public static Vector3f Lerp(Vector3f a, Vector3f b, float t)
        {
            float s = 1 - t;
            return new Vector3f(
                s * a.x + t * b.x,
                s * a.y + t * b.y,
                s * a.z + t * b.z
            );
        }


        public static bool operator ==(Vector3f a, Vector3f b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vector3f a, Vector3f b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public static Vector3f operator -(Vector3f vect)
        {
            return new Vector3f(vect.x * -1.0f, vect.y * -1.0f, vect.z * -1.0f);
        }

        public static Vector3f operator +(Vector3f vect, float f)
        {
            return new Vector3f(vect.x + f, vect.y + f, vect.z + f);
        }

        public static Vector3f operator +(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3f operator -(Vector3f vect, float f)
        {
            return new Vector3f(vect.x - f, vect.y - f, vect.z - f);
        }

        public static Vector3f operator -(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3f operator *(Vector3f vect, float f)
        {
            return new Vector3f(vect.x * f, vect.y * f, vect.z * f);
        }

        public static Vector3f operator *(float f, Vector3f vect)
        {
            return new Vector3f(vect.x * f, vect.y * f, vect.z * f);
        }

        public static Vector3f operator /(Vector3f vect, float f)
        {
            return new Vector3f(vect.x / f, vect.y / f, vect.z / f);
        }

        public static Vector3f operator /(float f, Vector3f vect)
        {
            return new Vector3f(vect.x / f, vect.y / f, vect.z / f);
        }

        public static Vector3f operator *(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3f operator /(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3f operator *(Vector3f vect, Quaternionf quat)
        {
            Vector3f quatvect = new Vector3f(quat.x, quat.y, quat.z);
            return 2.0f * quatvect.Dot(vect) * quatvect
                + (quat.w * quat.w - quatvect.Dot(quatvect)) * vect
                + 2.0f * quat.w * quatvect.Cross(vect);
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
            return this == (Vector3f)obj;
        }

        public bool Equals(Vector3f other)
        {
            return (x == other.x && y == other.y && z == other.z);
        }

        public int CompareTo(Vector3f other)
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