using System;

namespace MF3D
{
    [Serializable]
    public struct Matrix2d : IEquatable<Matrix2d>
    {
        public double m00, m01, m10, m11;


        public Matrix2d(double m00, double m01, double m10, double m11)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m10 = m10;
            this.m11 = m11;
        }

        public Matrix2d(Vector2d a, Vector2d b, bool inColumn = false)
        {
            if (!inColumn)
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


        public static readonly Matrix2d Identity = new Matrix2d(1.0, 0.0, 0.0, 1.0);
        public static readonly Matrix2d Zero = new Matrix2d(0.0, 0.0, 0.0, 0.0);
        public static readonly Matrix2d One = new Matrix2d(1.0, 1.0, 1.0, 1.0);


        public static Matrix2d ToRotation(double angle)
        {
            return new Matrix2d(
                System.Math.Cos(angle), -System.Math.Sin(angle), System.Math.Sin(angle), System.Math.Cos(angle)
            );
        }

        public double[] ToArray()
        {
            return new double[4] {
                m00, m01, m10, m11
            };
        }

        public void ToArray(double[] arr)
        {
            arr[0] = m00; arr[1] = m01; arr[2] = m10; arr[3] = m11;
        }


        public double this[int r, int c]
        {
            get { return (r == 0) ? ((c == 0) ? m00 : m01) : ((c == 0) ? m10 : m11); }
        }


        public Vector2d Row0
        {
            get { return new Vector2d(m00, m10); }
            set { m00 = value.x; m10 = value.y; }
        }

        public Vector2d Row1
        {
            get { return new Vector2d(m01, m11); }
            set { m01 = value.x; m11 = value.y; }
        }

        public Vector2d Column0
        {
            get { return new Vector2d(m00, m01); }
            set { m00 = value.x; m01 = value.y; }
        }

        public Vector2d Column1
        {
            get { return new Vector2d(m10, m11); }
            set { m10 = value.x; m11 = value.y; }
        }

        public double Determinant
        {
            get { return m00 * m11 - m01 * m10; }
        }


        public Matrix2d Transpose()
        {
            return new Matrix2d(m00, m10, m01, m11);
        }

        public Matrix2d Inverse(double epsilon = Constant.Epsilon)
        {
            double det = Determinant;

            if (System.Math.Abs(det) > epsilon)
            {
                double invDet = 1.0 / det;
                return new Matrix2d(
                    m11 * invDet, -m01 * invDet,
                    -m10 * invDet, m00 * invDet
                );
            }
            else
            {
                return Zero;
            }
        }

        public Matrix2d Adjoint()
        {
            return new Matrix2d(m11, -m01, -m10, m00);
        }


        public static bool operator ==(Matrix2d a, Matrix2d b)
        {
            return a.m00 == b.m00 && a.m01 == b.m01 && a.m10 == b.m10 && a.m11 == b.m11;
        }

        public static bool operator !=(Matrix2d a, Matrix2d b)
        {
            return a.m00 != b.m00 || a.m01 != b.m01 || a.m10 != b.m10 || a.m11 != b.m11;
        }

        public static Matrix2d operator -(Matrix2d v)
        {
            return new Matrix2d(-v.m00, -v.m01, -v.m10, -v.m11);
        }

        public static Matrix2d operator +(Matrix2d a, Matrix2d o)
        {
            return new Matrix2d(a.m00 + o.m00, a.m01 + o.m01, a.m10 + o.m10, a.m11 + o.m11);
        }

        public static Matrix2d operator +(Matrix2d a, double f)
        {
            return new Matrix2d(a.m00 + f, a.m01 + f, a.m10 + f, a.m11 + f);
        }

        public static Matrix2d operator -(Matrix2d a, Matrix2d o)
        {
            return new Matrix2d(a.m00 - o.m00, a.m01 - o.m01, a.m10 - o.m10, a.m11 - o.m11);
        }

        public static Matrix2d operator -(Matrix2d a, double f)
        {
            return new Matrix2d(a.m00 - f, a.m01 - f, a.m10 - f, a.m11 - f);
        }

        public static Matrix2d operator *(Matrix2d a, double f)
        {
            return new Matrix2d(a.m00 * f, a.m01 * f, a.m10 * f, a.m11 * f);
        }

        public static Matrix2d operator *(double f, Matrix2d a)
        {
            return new Matrix2d(a.m00 * f, a.m01 * f, a.m10 * f, a.m11 * f);
        }

        public static Matrix2d operator /(Matrix2d a, double f)
        {
            return new Matrix2d(a.m00 / f, a.m01 / f, a.m10 / f, a.m11 / f);
        }

        public static Matrix2d operator *(Matrix2d a, Matrix2d o)
        {
            return new Matrix2d(
                a.m00 * o.m00 + a.m01 * o.m10,
                a.m00 * o.m01 + a.m01 * o.m11,
                a.m10 * o.m00 + a.m11 * o.m10,
                a.m10 * o.m01 + a.m11 * o.m11
            );
        }

        public static Vector2d operator *(Matrix2d m, Vector2d v)
        {
            return new Vector2d(
                m.m00 * v.x + m.m01 * v.y,
                m.m10 * v.x + m.m11 * v.y
            );
        }

        public static Vector2d operator *(Vector2d v, Matrix2d m)
        {
            return new Vector2d(
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
            return this == (Matrix2d)obj;
        }

        public bool Equals(Matrix2d other)
        {
            return m00 == other.m00 && m01 == other.m01 && m10 == other.m10 && m11 == other.m11;
        }


        public override string ToString()
        {
            return string.Format("{0:F8} {1:F8} {2:F8} {3:F8}", m00, m01, m10, m11);
        }
    }
}