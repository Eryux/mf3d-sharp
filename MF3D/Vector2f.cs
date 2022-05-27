using System;

namespace MF3D
{
    [Serializable]
    public struct Vector2f : IComparable<Vector2f>, IEquatable<Vector2f>
    {
        public float x; public float y;


        public Vector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2f(Vector2f vect)
        {
            x = vect.x;
            y = vect.y;
        }


        static public readonly Vector2f Zero = new Vector2f(0f, 0f);
        static public readonly Vector2f One = new Vector2f(1f, 1f);
        static public readonly Vector2f AxisX = new Vector2f(1f, 0f);
        static public readonly Vector2f AxisY = new Vector2f(0f, 1f);
        static public readonly Vector2f MaxValue = new Vector2f(float.MaxValue, float.MaxValue);
        static public readonly Vector2f MinValue = new Vector2f(float.MinValue, float.MinValue);


        public float this[int i]
        {
            get { return (i == 0) ? x : y; }
            set { if (i == 0) x = value; else y = value; }
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

        public float LengthSquared
        {
            get { return x * x + y * y; }
        }

        public float Length
        {
            get { return (float)System.Math.Sqrt(x * x + y * y); }
        }

        public Vector2f Normalized
        {
            get { return new Vector2f(this).Normalize(); }
        }

        public bool IsNormalized
        {
            get { return System.Math.Abs(x * x + y * y - 1) < Constant.ZeroTolerance; }
        }

        public bool IsFinite
        {
            get
            {
                float f = x + y;
                return !float.IsNaN(f) && !float.IsInfinity(f);
            }
        }


        public Vector2f Normalize(float epsilon = Constant.Epsilonf)
        {
            float length = Length;

            if (length > epsilon)
            {
                length = 1.0f / length;
                x *= length;
                y *= length;
            }
            else
            {
                x = y = 0;
            }

            return this;
        }

        public float Dot(Vector2f vect)
        {
            return x * vect.x + y * vect.y;
        }

        public float Cross(Vector2f vect)
        {
            return x * vect.y - y * vect.x;
        }

        public float DistanceSquared(Vector2f vect)
        {
            float dx = vect.x - x;
            float dy = vect.y - y;
            return dx * dx + dy * dy;
        }

        public float Distance(Vector2f vect)
        {
            return (float)System.Math.Sqrt(DistanceSquared(vect));
        }


        public static Vector2f Lerp(Vector2f a, Vector2f b, float t)
        {
            float s = 1 - t;
            return new Vector2f(
                s * a.x + t * b.x,
                s * a.y + t * b.y
            );
        }


        public static bool operator ==(Vector2f a, Vector2f b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2f a, Vector2f b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static Vector2f operator -(Vector2f vect)
        {
            return new Vector2f(vect.x * -1.0f, vect.y * -1.0f);
        }

        public static Vector2f operator +(Vector2f vect, float f)
        {
            return new Vector2f(vect.x + f, vect.y + f);
        }

        public static Vector2f operator +(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.x + b.x, a.y + b.y);
        }

        public static Vector2f operator -(Vector2f vect, float f)
        {
            return new Vector2f(vect.x - f, vect.y - f);
        }

        public static Vector2f operator -(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.x - b.x, a.y - b.y);
        }

        public static Vector2f operator *(Vector2f vect, float f)
        {
            return new Vector2f(vect.x * f, vect.y * f);
        }

        public static Vector2f operator *(float f, Vector2f vect)
        {
            return new Vector2f(vect.x * f, vect.y * f);
        }

        public static Vector2f operator /(Vector2f vect, float f)
        {
            return new Vector2f(vect.x / f, vect.y / f);
        }

        public static Vector2f operator /(float f, Vector2f vect)
        {
            return new Vector2f(vect.x / f, vect.y / f);
        }

        public static Vector2f operator *(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.x * b.x, a.y * b.y);
        }

        public static Vector2f operator /(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.x / b.x, a.y / b.y);
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
            return this == (Vector2f)obj;
        }

        public bool Equals(Vector2f other)
        {
            return (x == other.x && y == other.y);
        }

        public int CompareTo(Vector2f other)
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
