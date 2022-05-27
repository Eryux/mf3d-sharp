using System;

namespace MF3D
{
    [Serializable]
    public struct Matrix2f : IEquatable<Matrix2f>
    {
        public float m00, m01, m10, m11;


        public Matrix2f(float m00, float m01, float m10, float m11)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m10 = m10;
            this.m11 = m11;
        }

        public Matrix2f(Vector2f a, Vector2f b, bool inColumn = false)
        {
            if (inColumn)
            {
                m00 = a.x;
                m01 = a.y;
                m10 = b.x;
                m11 = b.y;
            }
            else
            {
                m00 = a.x;
                m01 = b.x;
                m10 = a.y;
                m11 = b.y;
            }
        }


        public static readonly Matrix2f Identity = new Matrix2f(1.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Matrix2f Zero = new Matrix2f(0.0f, 0.0f, 0.0f, 0.0f);
        public static readonly Matrix2f One = new Matrix2f(1.0f, 1.0f, 1.0f, 1.0f);


        public static Matrix2f ToRotation(float angle)
        {
            return new Matrix2f(
                (float)System.Math.Cos(angle), (float)-System.Math.Sin(angle), (float)System.Math.Sin(angle), (float)System.Math.Cos(angle)
            );
        }

        public float[] ToArray()
        {
            return new float[4] {
                m00, m01, m10, m11
            };
        }

        public void ToArray(float[] arr)
        {
            arr[0] = m00; arr[1] = m01; arr[2] = m10; arr[3] = m11;
        }


        public float this[int r, int c]
        {
            get { return (r == 0) ? ((c == 0) ? m00 : m01) : ((c == 0) ? m10 : m11); }
        }


        public Vector2f Row0
        {
            get { return new Vector2f(m00, m10); }
            set { m00 = value.x; m10 = value.y; }
        }

        public Vector2f Row1
        {
            get { return new Vector2f(m01, m11); }
            set { m01 = value.x; m11 = value.y; }
        }

        public Vector2f Column0
        {
            get { return new Vector2f(m00, m01); }
            set { m00 = value.x; m01 = value.y; }
        }

        public Vector2f Column1
        {
            get { return new Vector2f(m10, m11); }
            set { m10 = value.x; m11 = value.y; }
        }

        public float Determinant
        {
            get { return m00 * m11 - m01 * m10; }
        }


        public Matrix2f Transpose()
        {
            return new Matrix2f(m00, m10, m01, m11);
        }

        public Matrix2f Inverse(float epsilon = Constant.Epsilonf)
        {
            float det = Determinant;

            if (System.Math.Abs(det) > epsilon)
            {
                float invDet = 1.0f / det;
                return new Matrix2f(
                    m11 * invDet, -m01 * invDet,
                    -m10 * invDet, m00 * invDet
                );
            }
            else
            {
                return Zero;
            }
        }

        public Matrix2f Adjoint()
        {
            return new Matrix2f(m11, -m01, -m10, m00);
        }


        public static bool operator ==(Matrix2f a, Matrix2f b)
        {
            return a.m00 == b.m00 && a.m01 == b.m01 && a.m10 == b.m10 && a.m11 == b.m11;
        }

        public static bool operator !=(Matrix2f a, Matrix2f b)
        {
            return a.m00 != b.m00 || a.m01 != b.m01 || a.m10 != b.m10 || a.m11 != b.m11;
        }

        public static Matrix2f operator -(Matrix2f v)
        {
            return new Matrix2f(-v.m00, -v.m01, -v.m10, -v.m11);
        }

        public static Matrix2f operator +(Matrix2f a, Matrix2f o)
        {
            return new Matrix2f(a.m00 + o.m00, a.m01 + o.m01, a.m10 + o.m10, a.m11 + o.m11);
        }

        public static Matrix2f operator +(Matrix2f a, float f)
        {
            return new Matrix2f(a.m00 + f, a.m01 + f, a.m10 + f, a.m11 + f);
        }

        public static Matrix2f operator -(Matrix2f a, Matrix2f o)
        {
            return new Matrix2f(a.m00 - o.m00, a.m01 - o.m01, a.m10 - o.m10, a.m11 - o.m11);
        }

        public static Matrix2f operator -(Matrix2f a, float f)
        {
            return new Matrix2f(a.m00 - f, a.m01 - f, a.m10 - f, a.m11 - f);
        }

        public static Matrix2f operator *(Matrix2f a, float f)
        {
            return new Matrix2f(a.m00 * f, a.m01 * f, a.m10 * f, a.m11 * f);
        }

        public static Matrix2f operator *(float f, Matrix2f a)
        {
            return new Matrix2f(a.m00 * f, a.m01 * f, a.m10 * f, a.m11 * f);
        }

        public static Matrix2f operator /(Matrix2f a, float f)
        {
            return new Matrix2f(a.m00 / f, a.m01 / f, a.m10 / f, a.m11 / f);
        }

        public static Matrix2f operator *(Matrix2f a, Matrix2f o)
        {
            return new Matrix2f(
                a.m00 * o.m00 + a.m01 * o.m10,
                a.m00 * o.m01 + a.m01 * o.m11,
                a.m10 * o.m00 + a.m11 * o.m10,
                a.m10 * o.m01 + a.m11 * o.m11
            );
        }

        public static Vector2f operator *(Matrix2f m, Vector2f v)
        {
            return new Vector2f(
                m.m00 * v.x + m.m01 * v.y,
                m.m10 * v.x + m.m11 * v.y
            );
        }

        public static Vector2f operator *(Vector2f v, Matrix2f m)
        {
            return new Vector2f(
                v.x * m.m00 + v.y * m.m10,
                v.x * m.m01 + v.y * m.m11
            );
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ m00.GetHashCode();
                hash = (hash * 16777619) ^ m01.GetHashCode();
                hash = (hash * 16777619) ^ m10.GetHashCode();
                hash = (hash * 16777619) ^ m11.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return this == (Matrix2f)obj;
        }

        public bool Equals(Matrix2f other)
        {
            return m00 == other.m00 && m01 == other.m01 && m10 == other.m10 && m11 == other.m11;
        }


        public override string ToString()
        {
            return string.Format("{0:F8} {1:F8} {2:F8} {3:F8}", m00, m01, m10, m11);
        }
    }
}