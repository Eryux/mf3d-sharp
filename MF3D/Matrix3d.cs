using System;

namespace MF3D
{
    [Serializable]
    public struct Matrix3d : IEquatable<Matrix3d>
    {
        public Vector3d row0, row1, row2;


        public Matrix3d(double m00, double m01, double m02, 
            double m10, double m11, double m12, 
            double m20, double m21, double m22)
        {
            row0 = new Vector3d(m00, m01, m02);
            row1 = new Vector3d(m10, m11, m12);
            row2 = new Vector3d(m20, m21, m22);
        }

        public Matrix3d(Vector3d a, Vector3d b, Vector3d c, bool inColumn = false)
        {
            if (!inColumn)
            {
                row0 = a;
                row1 = b;
                row2 = c;
            }
            else
            {
                row0 = new Vector3d(a.x, b.x, c.x);
                row1 = new Vector3d(a.y, b.y, c.y);
                row2 = new Vector3d(a.z, b.z, c.z);
            }
        }


        public static readonly Matrix3d Identity = new Matrix3d(1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0);
        public static readonly Matrix3d Zero = new Matrix3d(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);


        public double[] ToArray()
        {
            return new double[9] {
                row0.x, row0.y, row0.z,
                row1.x, row1.y, row1.z,
                row2.x, row2.y, row2.z
            };
        }

        public void ToArray(double[] arr)
        {
            arr[0] = row0.x; arr[1] = row0.y; arr[2] = row0.z;
            arr[3] = row1.x; arr[4] = row1.y; arr[5] = row1.z;
            arr[6] = row2.x; arr[7] = row2.y; arr[8] = row2.z;
        }


        public double this[int r, int c]
        {
            get
            {
                return (r == 0) ? row0[c] : ((r == 1) ? row1[c] : row2[c]);
            }
            set
            {
                if (r == 0) row0[c] = value;
                else if (r == 1) row1[c] = value;
                else row2[c] = value;
            }
        }

        public double this[int i]
        {
            get
            {
                return (i > 5) ? Row2[i % 3] : ((i > 2) ? Row1[i % 3] : Row0[i % 3]);
            }
            set
            {
                if (i > 5) row2[i % 3] = value;
                else if (i > 2) row1[i % 3] = value;
                else row0[i % 3] = value;
            }
        }


        public Vector3d Row0
        {
            get { return row0; }
            set { row0 = value; }
        }

        public Vector3d Row1
        {
            get { return row1; }
            set { row1 = value; }
        }

        public Vector3d Row2
        {
            get { return row2; }
            set { row2 = value; }
        }

        public Vector3d Column0
        {
            get { return new Vector3d(row0.x, row1.x, row2.x); }
            set { row0.x = value.x; row1.x = value.y; row2.x = value.z; }
        }

        public Vector3d Column1
        {
            get { return new Vector3d(row0.y, row1.y, row2.y); }
            set { row0.y = value.x; row1.y = value.y; row2.y = value.z; }
        }

        public Vector3d Column2
        {
            get { return new Vector3d(row0.z, row1.z, row2.z); }
            set { row0.z = value.x; row1.z = value.y; row2.z = value.z; }
        }

        public double Determinant
        {
            get
            {
                double a11 = row0.x, a12 = row0.y, a13 = row0.z;
                double a21 = row1.x, a22 = row1.y, a23 = row1.z;
                double a31 = row2.x, a32 = row2.y, a33 = row2.z;
                
                double i00 = a33 * a22 - a32 * a23;
                double i01 = -(a33 * a12 - a32 * a13);
                double i02 = a23 * a12 - a22 * a13;

                return a11 * i00 + a21 * i01 + a31 * i02;
            }
        }


        public Matrix3d Transpose()
        {
            return new Matrix3d(
               Row0.x, Row1.x, Row2.x,
               Row0.y, Row1.y, Row2.y,
               Row0.z, Row1.z, Row2.z
            );
        }

        public Matrix3d Inverse(double epsilon = Constant.Epsilon)
        {
            double a11 = Row0.x, a12 = Row0.y, a13 = Row0.z;
            double a21 = Row1.x, a22 = Row1.y, a23 = Row1.z;
            double a31 = Row2.x, a32 = Row2.y, a33 = Row2.z;

            double i00 = a33 * a22 - a32 * a23;
            double i01 = -(a33 * a12 - a32 * a13);
            double i02 = a23 * a12 - a22 * a13;

            double i10 = -(a33 * a21 - a31 * a23);
            double i11 = a33 * a11 - a31 * a13;
            double i12 = -(a23 * a11 - a21 * a13);

            double i20 = a32 * a21 - a31 * a22;
            double i21 = -(a32 * a11 - a31 * a12);
            double i22 = a22 * a11 - a21 * a12;

            double det = a11 * i00 + a21 * i01 + a31 * i02;

            if (System.Math.Abs(det) < epsilon)
                throw new Exception("Matrix3d.Inverse: matrix is not invertible");

            det = 1.0 / det;

            return new Matrix3d(
                i00 * det, i01 * det, i02 * det, 
                i10 * det, i11 * det, i12 * det, 
                i20 * det, i21 * det, i22 * det
            );
        }

        public static Matrix3d ToAxisAngle(Vector3d axis, double angle)
        {
            double cs = System.Math.Cos(angle);
            double sn = System.Math.Sin(angle);

            double oneMinusCos = 1.0 - cs;

            double x2 = axis[0] * axis[0];
            double y2 = axis[1] * axis[1];
            double z2 = axis[2] * axis[2];

            double xym = axis[0] * axis[1] * oneMinusCos;
            double xzm = axis[0] * axis[2] * oneMinusCos;
            double yzm = axis[1] * axis[2] * oneMinusCos;

            double xSin = axis[0] * sn;
            double ySin = axis[1] * sn;
            double zSin = axis[2] * sn;

            return new Matrix3d(
                x2 * oneMinusCos + cs, xym - zSin, xzm + ySin,
                xym + zSin, y2 * oneMinusCos + cs, yzm - xSin,
                xzm - ySin, yzm + xSin, z2 * oneMinusCos + cs
            );
        }


        public static bool operator ==(Matrix3d a, Matrix3d b)
        {
            return a.row0 == b.row0 && a.row1 == b.row1 && a.row2 == b.row2;
        }

        public static bool operator !=(Matrix3d a, Matrix3d b)
        {
            return a.row0 != b.row0 || a.row1 != b.row1 || a.row2 != b.row2;
        }

        public static Matrix3d operator +(Matrix3d mat1, Matrix3d mat2)
        {
            return new Matrix3d(mat1.Row0 + mat2.Row0, mat1.Row1 + mat2.Row1, mat1.Row2 + mat2.Row2, true);
        }

        public static Matrix3d operator -(Matrix3d mat1, Matrix3d mat2)
        {
            return new Matrix3d(mat1.Row0 - mat2.Row0, mat1.Row1 - mat2.Row1, mat1.Row2 - mat2.Row2, true);
        }

        public static Matrix3d operator *(Matrix3d mat, double f)
        {
            return new Matrix3d(
                mat.Row0.x * f, mat.Row0.y * f, mat.Row0.z * f,
                mat.Row1.x * f, mat.Row1.y * f, mat.Row1.z * f,
                mat.Row2.x * f, mat.Row2.y * f, mat.Row2.z * f
            );
        }
        public static Matrix3d operator *(double f, Matrix3d mat)
        {
            return new Matrix3d(
                mat.Row0.x * f, mat.Row0.y * f, mat.Row0.z * f,
                mat.Row1.x * f, mat.Row1.y * f, mat.Row1.z * f,
                mat.Row2.x * f, mat.Row2.y * f, mat.Row2.z * f
            );
        }

        public static Vector3d operator *(Matrix3d mat, Vector3d v)
        {
            return new Vector3d(
                mat.Row0.x * v.x + mat.Row0.y * v.y + mat.Row0.z * v.z,
                mat.Row1.x * v.x + mat.Row1.y * v.y + mat.Row1.z * v.z,
                mat.Row2.x * v.x + mat.Row2.y * v.y + mat.Row2.z * v.z
            );
        }

        public static Matrix3d operator *(Matrix3d mat1, Matrix3d mat2)
        {
            double m00 = mat1.Row0.x * mat2.Row0.x + mat1.Row0.y * mat2.Row1.x + mat1.Row0.z * mat2.Row2.x;
            double m01 = mat1.Row0.x * mat2.Row0.y + mat1.Row0.y * mat2.Row1.y + mat1.Row0.z * mat2.Row2.y;
            double m02 = mat1.Row0.x * mat2.Row0.z + mat1.Row0.y * mat2.Row1.z + mat1.Row0.z * mat2.Row2.z;

            double m10 = mat1.Row1.x * mat2.Row0.x + mat1.Row1.y * mat2.Row1.x + mat1.Row1.z * mat2.Row2.x;
            double m11 = mat1.Row1.x * mat2.Row0.y + mat1.Row1.y * mat2.Row1.y + mat1.Row1.z * mat2.Row2.y;
            double m12 = mat1.Row1.x * mat2.Row0.z + mat1.Row1.y * mat2.Row1.z + mat1.Row1.z * mat2.Row2.z;

            double m20 = mat1.Row2.x * mat2.Row0.x + mat1.Row2.y * mat2.Row1.x + mat1.Row2.z * mat2.Row2.x;
            double m21 = mat1.Row2.x * mat2.Row0.y + mat1.Row2.y * mat2.Row1.y + mat1.Row2.z * mat2.Row2.y;
            double m22 = mat1.Row2.x * mat2.Row0.z + mat1.Row2.y * mat2.Row1.z + mat1.Row2.z * mat2.Row2.z;

            return new Matrix3d(m00, m01, m02, m10, m11, m12, m20, m21, m22);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ row0.GetHashCode();
                hash = (hash * 16777619) ^ row1.GetHashCode();
                hash = (hash * 16777619) ^ row2.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return this == (Matrix3d)obj;
        }

        public bool Equals(Matrix3d other)
        {
            return row0 == other.row0 && row1 == other.row1 && row2 == other.row2;
        }


        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}", row0.ToString(), row1.ToString(), row2.ToString());
        }
    }
}
