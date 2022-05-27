using System;

namespace MF3D
{
    [Serializable]
    public struct Vector4f : IComparable<Vector4f>, IEquatable<Vector4f>
    {
        public float x; public float y; public float z; public float w;


        public Vector4f(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4f(Vector4f vect)
        {
            x = vect.x;
            y = vect.y;
            z = vect.z;
            w = vect.w;
        }

        static public readonly Vector4f Zero = new Vector4f(0.0f, 0.0f, 0.0f, 0.0f);
        static public readonly Vector4f One = new Vector4f(1.0f, 1.0f, 1.0f, 1.0f);
        static public readonly Vector4f AxisX = new Vector4f(1.0f, 0.0f, 0.0f, 0.0f);
        static public readonly Vector4f AxisY = new Vector4f(0.0f, 1.0f, 0.0f, 0.0f);
        static public readonly Vector4f AxisZ = new Vector4f(0.0f, 0.0f, 1.0f, 0.0f);
        static public readonly Vector4f AxisW = new Vector4f(0.0f, 0.0f, 0.0f, 1.0f);
        static public readonly Vector4f MaxValue = new Vector4f(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);
        static public readonly Vector4f MinValue = new Vector4f(float.MinValue, float.MinValue, float.MinValue, float.MinValue);


        public float this[int i]
        {
            get { return (i == 0) ? x : (i == 1) ? y : (i == 2) ? z : w; }
            set { if (i == 0) x = value; else if (i == 1) y = value; else if (i == 2) z = value; else w = value; }
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

        public float W
        {
            get { return w; }
            set { w = value; }
        }


        public float LengthSquared
        {
            get { return x * x + y * y + z * z + w * w; }
        }

        public float Length
        {
            get { return (float)System.Math.Sqrt(x * x + y * y + z * z + w * w); }
        }

        public Vector4f Normalized
        {
            get { return new Vector4f(this).Normalize(); }
        }

        public bool IsNormalized
        {
            get { return System.Math.Abs(x * x + y * y + z * z + w * w - 1) < Constant.ZeroTolerance; }
        }

        public bool IsFinite
        {
            get
            {
                float f = x + y + z + w;
                return !float.IsNaN(f) && !float.IsInfinity(f);
            }
        }


        public Vector4f Normalize(float epsilon = Constant.Epsilonf)
        {
            float length = Length;

            if (length > epsilon)
            {
                length = 1.0f / length;
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

        public float Dot(Vector4f vect)
        {
            return x * vect.x + y * vect.y + z * vect.z + w * vect.w;
        }

        public Vector4f Cross(Vector4f vect)
        {
            return new Vector4f(
                z * vect.w - w * vect.z,
                w * vect.x - x * vect.w,
                x * vect.y - y * vect.x,
                y * vect.z - z * vect.y
            );
        }

        public float DistanceSquared(Vector4f vect)
        {
            float dx = vect.x - x;
            float dy = vect.y - y;
            float dz = vect.z - z;
            float dw = vect.w - w;
            return dx * dx + dy * dy + dz * dz + dw * dw;
        }

        public float Distance(Vector4f vect)
        {
            return (float)System.Math.Sqrt(DistanceSquared(vect));
        }


        public static Vector4f Lerp(Vector4f a, Vector4f b, float t)
        {
            float s = 1 - t;
            return new Vector4f(
                s * a.x + t * b.x,
                s * a.y + t * b.y,
                s * a.z + t * b.z,
                s * a.w + t * b.w
            );
        }


        public static bool operator ==(Vector4f a, Vector4f b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }

        public static bool operator !=(Vector4f a, Vector4f b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z || a.w != b.w;
        }

        public static Vector4f operator -(Vector4f vect)
        {
            return new Vector4f(vect.x * -1.0f, vect.y * -1.0f, vect.z * -1.0f, vect.w * -1.0f);
        }

        public static Vector4f operator +(Vector4f vect, float f)
        {
            return new Vector4f(vect.x + f, vect.y + f, vect.z + f, vect.w * f);
        }

        public static Vector4f operator +(Vector4f a, Vector4f b)
        {
            return new Vector4f(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Vector4f operator -(Vector4f vect, float f)
        {
            return new Vector4f(vect.x - f, vect.y - f, vect.z - f, vect.w - f);
        }

        public static Vector4f operator -(Vector4f a, Vector4f b)
        {
            return new Vector4f(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Vector4f operator *(Vector4f vect, float f)
        {
            return new Vector4f(vect.x * f, vect.y * f, vect.z * f, vect.w * f);
        }

        public static Vector4f operator *(float f, Vector4f vect)
        {
            return new Vector4f(vect.x * f, vect.y * f, vect.z * f, vect.w * f);
        }

        public static Vector4f operator /(Vector4f vect, float f)
        {
            return new Vector4f(vect.x / f, vect.y / f, vect.z / f, vect.w / f);
        }

        public static Vector4f operator /(float f, Vector4f vect)
        {
            return new Vector4f(vect.x / f, vect.y / f, vect.z / f, vect.w / f);
        }

        public static Vector4f operator *(Vector4f a, Vector4f b)
        {
            return new Vector4f(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        }

        public static Vector4f operator /(Vector4f a, Vector4f b)
        {
            return new Vector4f(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);
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
            return this == (Vector4f)obj;
        }

        public bool Equals(Vector4f other)
        {
            return (x == other.x && y == other.y && z == other.z && w == other.w);
        }

        public int CompareTo(Vector4f other)
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