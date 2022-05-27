using System;

namespace MF3D
{
    [Serializable]
    public struct Matrix4d : IEquatable<Matrix4d>
    {
        public Vector4d row0, row1, row2, row3;


        public Matrix4d(double m00, double m01, double m02, double m03,
            double m10, double m11, double m12, double m13,
            double m20, double m21, double m22, double m23,
            double m30, double m31, double m32, double m33)
        {
            row0 = new Vector4d(m00, m01, m02, m03);
            row1 = new Vector4d(m10, m11, m12, m13);
            row2 = new Vector4d(m20, m21, m22, m23);
            row3 = new Vector4d(m30, m31, m32, m33);
        }

        public Matrix4d(Vector4d a, Vector4d b, Vector4d c, Vector4d d, bool inColumn = false)
        {
            if (!inColumn)
            {
                row0 = a;
                row1 = b;
                row2 = c;
                row3 = d;
            }
            else
            {
                row0 = new Vector4d(a.x, b.x, c.x, d.x);
                row1 = new Vector4d(a.y, b.y, c.y, d.y);
                row2 = new Vector4d(a.z, b.z, c.z, d.z);
                row3 = new Vector4d(a.w, b.w, c.w, d.w);
            }
        }


        public static readonly Matrix4d Identity = new Matrix4d(
            1.0, 0.0, 0.0, 0.0,
            0.0, 1.0, 0.0, 0.0,
            0.0, 0.0, 1.0, 0.0,
            0.0, 0.0, 0.0, 1.0
        );

        public static readonly Matrix4d Zero = new Matrix4d(
            0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0,
            0.0, 0.0, 0.0, 0.0
        );


        public double[] ToArray()
        {
            return new double[16] {
                row0.x, row0.y, row0.z, row0.w,
                row1.x, row1.y, row1.z, row1.w,
                row2.x, row2.y, row2.z, row2.w,
                row3.x, row3.y, row3.z, row3.w,
            };
        }

        public void ToArray(double[] arr)
        {
            arr[0] = row0.x; arr[1] = row0.y; arr[2] = row0.z; arr[3] = row0.w;
            arr[4] = row1.x; arr[5] = row1.y; arr[6] = row1.z; arr[7] = row1.w;
            arr[8] = row2.x; arr[9] = row2.y; arr[10] = row2.z; arr[11] = row2.w;
            arr[12] = row3.x; arr[13] = row3.y; arr[14] = row3.z; arr[15] = row3.w;
        }


        public double this[int r, int c]
        {
            get
            {
                return (r == 0) ? Row0[c] : ((r == 1) ? Row1[c] : ((r == 2) ? Row2[c] : Row3[c]));
            }
            set
            {
                if (r == 0) row0[c] = value;
                else if (r == 1) row1[c] = value;
                else if (r == 2) row2[c] = value;
                else row3[c] = value;
            }
        }

        public double this[int i]
        {
            get
            {
                return (i > 11) ? Row3[i % 4] : ((i > 7) ? Row2[i % 4] : ((i > 3) ? Row1[i % 4] : Row0[i % 4]));
            }
            set
            {
                if (i > 11) row3[i % 4] = value;
                else if (i > 7) row2[i % 4] = value;
                else if (i > 3) row1[i % 4] = value;
                else row0[i % 4] = value;
            }
        }


        public Vector4d Row0
        {
            get { return row0; }
            set { row0 = value; }
        }

        public Vector4d Row1
        {
            get { return row1; }
            set { row1 = value; }
        }

        public Vector4d Row2
        {
            get { return row2; }
            set { row2 = value; }
        }

        public Vector4d Row3
        {
            get { return row3; }
            set { row3 = value; }
        }

        public Vector4d Column0
        {
            get { return new Vector4d(row0.x, row1.x, row2.x, row3.x); }
            set { row0.x = value.x; row1.x = value.y; row2.x = value.z; row3.x = value.w; }
        }

        public Vector4d Column1
        {
            get { return new Vector4d(row0.y, row1.y, row2.y, row3.y); }
            set { row0.y = value.x; row1.y = value.y; row2.y = value.z; row3.y = value.w; }
        }

        public Vector4d Column2
        {
            get { return new Vector4d(row0.z, row1.z, row2.z, row3.z); }
            set { row0.z = value.x; row1.z = value.y; row2.z = value.z; row3.z = value.w; }
        }

        public Vector4d Column3
        {
            get { return new Vector4d(row0.w, row1.w, row2.w, row3.w); }
            set { row0.w = value.x; row1.w = value.y; row2.w = value.z; row3.w = value.w; }
        }

        public double Determinant
        {
            get
            {
                double m00 = Row0.x; double m01 = Row0.y; double m02 = Row0.z; double m03 = Row0.w;
                double m10 = Row1.x; double m11 = Row1.y; double m12 = Row1.z; double m13 = Row1.w;
                double m20 = Row2.x; double m21 = Row2.y; double m22 = Row2.z; double m23 = Row2.w;
                double m30 = Row3.x; double m31 = Row3.y; double m32 = Row3.z; double m33 = Row3.w;

                return (m00 * m11 * m22 * m33) - (m00 * m11 * m23 * m32) + (m00 * m12 * m23 * m31) - (m00 * m12 * m21 * m33)
                    + (m00 * m13 * m21 * m32) - (m00 * m13 * m22 * m31) - (m01 * m12 * m23 * m30) + (m01 * m12 * m20 * m33)
                    - (m01 * m13 * m20 * m32) + (m01 * m13 * m22 * m30) - (m01 * m10 * m22 * m33) + (m01 * m10 * m23 * m32)
                    + (m02 * m13 * m20 * m31) - (m02 * m13 * m21 * m30) + (m02 * m10 * m21 * m33) - (m02 * m10 * m23 * m31)
                    + (m02 * m11 * m23 * m30) - (m02 * m11 * m20 * m33) - (m03 * m10 * m21 * m32) + (m03 * m10 * m22 * m31)
                    - (m03 * m11 * m22 * m30) + (m03 * m11 * m20 * m32) - (m03 * m12 * m20 * m31) + (m03 * m12 * m21 * m30);
            }
        }


        public Matrix4d Transpose()
        {
            return new Matrix4d(
                Row0.x, Row1.x, Row2.x, Row3.x,
                Row0.y, Row1.y, Row2.y, Row3.y,
                Row0.z, Row1.z, Row2.z, Row3.z,
                Row0.w, Row1.w, Row2.w, Row3.w
            );
        }

        public Matrix4d Inverse(double epsilon = Constant.Epsilon)
        {
            double m00 = Row0.x; double m01 = Row0.y; double m02 = Row0.z; double m03 = Row0.w;
            double m10 = Row1.x; double m11 = Row1.y; double m12 = Row1.z; double m13 = Row1.w;
            double m20 = Row2.x; double m21 = Row2.y; double m22 = Row2.z; double m23 = Row2.w;
            double m30 = Row3.x; double m31 = Row3.y; double m32 = Row3.z; double m33 = Row3.w;

            double i00 = m11 * m22 * m33 - m11 * m23 * m32 - m21 * m12 * m33 + m21 * m13 * m32 + m31 * m12 * m23 - m31 * m13 * m22;
            double i01 = -m01 * m22 * m33 + m01 * m23 * m32 + m21 * m02 * m33 - m21 * m03 * m32 - m31 * m02 * m23 + m31 * m03 * m22;
            double i02 = m01 * m12 * m33 - m01 * m13 * m32 - m11 * m02 * m33 + m11 * m03 * m32 + m31 * m02 * m13 - m31 * m03 * m12;
            double i03 = -m01 * m12 * m23 + m01 * m13 * m22 + m11 * m02 * m23 - m11 * m03 * m22 - m21 * m02 * m13 + m21 * m03 * m12;

            double i10 = -m10 * m22 * m33 + m10 * m23 * m32 + m20 * m12 * m33 - m20 * m13 * m32 - m30 * m12 * m23 + m30 * m13 * m22;
            double i11 = m00 * m22 * m33 - m00 * m23 * m32 - m20 * m02 * m33 + m20 * m03 * m32 + m30 * m02 * m23 - m30 * m03 * m22;
            double i12 = -m00 * m12 * m33 + m00 * m13 * m32 + m10 * m02 * m33 - m10 * m03 * m32 - m30 * m02 * m13 + m30 * m03 * m12;
            double i13 = m00 * m12 * m23 - m00 * m13 * m22 - m10 * m02 * m23 + m10 * m03 * m22 + m20 * m02 * m13 - m20 * m03 * m12;

            double i20 = m10 * m21 * m33 - m10 * m23 * m31 - m20 * m11 * m33 + m20 * m13 * m31 + m30 * m11 * m23 - m30 * m13 * m21;
            double i21 = -m00 * m21 * m33 + m00 * m23 * m31 + m20 * m01 * m33 - m20 * m03 * m31 - m30 * m01 * m23 + m30 * m03 * m21;
            double i22 = m00 * m11 * m33 - m00 * m13 * m31 - m10 * m01 * m33 + m10 * m03 * m31 + m30 * m01 * m13 - m30 * m03 * m11;
            double i23 = -m00 * m11 * m23 + m00 * m13 * m21 + m10 * m01 * m23 - m10 * m03 * m21 - m20 * m01 * m13 + m20 * m03 * m11;

            double i30 = -m10 * m21 * m32 + m10 * m22 * m31 + m20 * m11 * m32 - m20 * m12 * m31 - m30 * m11 * m22 + m30 * m12 * m21;
            double i31 = m00 * m21 * m32 - m00 * m22 * m31 - m20 * m01 * m32 + m20 * m02 * m31 + m30 * m01 * m22 - m30 * m02 * m21;
            double i32 = -m00 * m11 * m32 + m00 * m12 * m31 + m10 * m01 * m32 - m10 * m02 * m31 - m30 * m01 * m12 + m30 * m02 * m11;
            double i33 = m00 * m11 * m22 - m00 * m12 * m21 - m10 * m01 * m22 + m10 * m02 * m21 + m20 * m01 * m12 - m20 * m02 * m11;

            double det = m00 * i00 + m01 * i10 + m02 * i20 + m03 * i30;

            if (System.Math.Abs(det) < epsilon)
            {
                throw new Exception("Matrix4d.Inverse: matrix is not invertible");
            }

            det = 1.0f / det;

            return new Matrix4d(
                i00 * det, i01 * det, i02 * det, i03 * det,
                i10 * det, i11 * det, i12 * det, i13 * det,
                i20 * det, i21 * det, i22 * det, i23 * det,
                i30 * det, i31 * det, i32 * det, i33 * det
            );
        }


        public Matrix4d Normalize()
        {
            double d = Determinant;
            Row0 /= d; Row1 /= d; Row2 /= d; Row3 /= d;
            return this;
        }

        public Matrix4d Normalized()
        {
            Matrix4d m = this;
            return m.Normalize();
        }


        public static Matrix4d CreateTranslation(Vector3d v)
        {
            Matrix4d m = Identity;
            m.row3.x = v.x; m.row3.y = v.y; m.row3.z = v.z;
            return m;
        }

        public Vector3d ExtractTranslation()
        {
            return new Vector3d(Row3.x, Row3.y, Row3.z);
        }

        public Matrix4d ClearTranslation()
        {
            Matrix4d m = this;
            m.row3.x = 0; m.row3.y = 0; m.row3.z = 0;
            return m;
        }


        public static Matrix4d CreateScale(double scale)
        {
            Matrix4d m = Identity;
            m.row0.x = scale; m.row1.y = scale; m.row2.z = scale;
            return m;
        }

        public static Matrix4d CreateScale(Vector3d scale)
        {
            Matrix4d m = Identity;
            m.row0.x = scale.x; m.row1.y = scale.y; m.row2.z = scale.z;
            return m;
        }

        public Vector3d ExtractScale()
        {
            Vector3d Row0xyz = new Vector3d(Row0.x, Row0.y, Row0.z);
            Vector3d Row1xyz = new Vector3d(Row1.x, Row1.y, Row1.z);
            Vector3d Row2xyz = new Vector3d(Row2.x, Row2.y, Row2.z);
            return new Vector3d(Row0xyz.Length, Row1xyz.Length, Row2xyz.Length);
        }

        public Matrix4d ClearScale()
        {
            Matrix4d m = this;
            Vector3d Row0xyz = new Vector3d(Row0.x, Row0.y, Row0.z).Normalized;
            Vector3d Row1xyz = new Vector3d(Row1.x, Row1.y, Row1.z).Normalized;
            Vector3d Row2xyz = new Vector3d(Row2.x, Row2.y, Row2.z).Normalized;
            m.Row0 = new Vector4d(Row0xyz.x, Row0xyz.y, Row0xyz.z, Row0.w);
            m.Row1 = new Vector4d(Row1xyz.x, Row1xyz.y, Row1xyz.z, Row0.w);
            m.Row2 = new Vector4d(Row2xyz.x, Row2xyz.y, Row2xyz.z, Row0.w);
            return m;
        }


        public static Matrix4d CreateRotationFromAxisAngle(Vector3d axis, double angle)
        {
            axis.Normalize();

            double cos = System.Math.Cos(-angle);
            double sin = System.Math.Sin(-angle);
            double t = 1.0 - cos;

            double txx = t * axis.x * axis.x; double txy = t * axis.x * axis.y; double txz = t * axis.x * axis.z;
            double tyy = t * axis.y * axis.y; double tyz = t * axis.y * axis.z; double tzz = t * axis.z * axis.z;

            double sinx = sin * axis.x; double siny = sin * axis.y; double sinz = sin * axis.z;

            Matrix4d m = Identity;
            m.Row0 = new Vector4d(txx + cos, txy - sinz, txz + siny, 0);
            m.Row1 = new Vector4d(txy + sinz, tyy + cos, tyz - sinx, 0);
            m.Row2 = new Vector4d(txy - siny, tyz + sinx, tzz + cos, 0);
            m.Row3 = new Vector4d(0, 0, 0, 1);
            return m;
        }


        public static Matrix4d CreateRotationFromQuaternion(Quaterniond quat)
        {
            double angle;
            Vector3d axis = quat.ToAxisAngle(out angle);
            return CreateRotationFromAxisAngle(axis, angle);
        }

        public Quaterniond ExtractRotation()
        {
            Vector3d row0xyz = new Vector3d(Row0.x, Row0.y, Row0.z).Normalized;
            Vector3d row1xyz = new Vector3d(Row1.x, Row1.y, Row1.z).Normalized;
            Vector3d row2xyz = new Vector3d(Row2.x, Row2.y, Row2.z).Normalized;

            Quaterniond q = new Quaterniond();

            double trace = 0.25 * (row0xyz.x + row1xyz.y + row2xyz.z + 1.0);

            if (trace > 0)
            {
                double sq = System.Math.Sqrt(trace);

                q.w = sq;
                sq = 1.0 / (4.0 * sq);
                q.x = (row1xyz.z - row2xyz.y) * sq;
                q.y = (row2xyz.x - row0xyz.z) * sq;
                q.z = (row0xyz.y - row1xyz.x) * sq;
            }
            else if (row0xyz.x > row1xyz.y && row0xyz.x > row2xyz.z)
            {
                double sq = 2.0 * System.Math.Sqrt(1.0 + row0xyz.x - row1xyz.y - row2xyz.z);

                q.w = 0.25 * sq;
                sq = 1.0 / sq;
                q.x = (row2xyz.y - row1xyz.z) * sq;
                q.y = (row1xyz.x - row0xyz.y) * sq;
                q.z = (row2xyz.x - row0xyz.z) * sq;
            }
            else if (row1xyz.y > row2xyz.z)
            {
                double sq = 2.0 * System.Math.Sqrt(1.0 + row1xyz.y - row0xyz.x - row2xyz.z);

                q.w = 0.25 * sq;
                sq = 1.0 / sq;
                q.x = (row2xyz.x - row0xyz.z) * sq;
                q.y = (row1xyz.x - row0xyz.y) * sq;
                q.z = (row2xyz.y - row1xyz.z) * sq;
            }
            else
            {
                double sq = 2.0 * System.Math.Sqrt(1.0 + row2xyz.z - row0xyz.x - row1xyz.y);

                q.w = 0.25 * sq;
                sq = 1.0 / sq;
                q.x = (row1xyz.x - row0xyz.y) * sq;
                q.y = (row2xyz.x - row0xyz.z) * sq;
                q.z = (row2xyz.y - row1xyz.z) * sq;
            }

            q.Normalize();
            return q;
        }

        public Matrix4d ClearRotation()
        {
            Matrix4d m = this;

            Vector3d row0xyz = new Vector3d(Row0.x, Row0.y, Row0.z);
            Vector3d row1xyz = new Vector3d(Row1.x, Row1.y, Row1.z);
            Vector3d row2xyz = new Vector3d(Row2.x, Row2.y, Row2.z);

            m.row0.x = row0xyz.Length;
            m.row1.y = row1xyz.Length;
            m.row2.z = row2xyz.Length;

            return m;
        }


        public static bool operator ==(Matrix4d a, Matrix4d b)
        {
            return a.row0 == b.row0 && a.row1 == b.row1 && a.row2 == b.row2 && a.row3 == b.row3;
        }

        public static bool operator !=(Matrix4d a, Matrix4d b)
        {
            return a.row0 != b.row0 || a.row1 != b.row1 || a.row2 != b.row2 || a.row3 != b.row3;
        }

        public static Matrix4d operator +(Matrix4d mat1, Matrix4d mat2)
        {
            return new Matrix4d(
                mat1.Row0 + mat2.Row0, mat1.Row1 + mat2.Row1, mat1.Row2 + mat2.Row2, mat1.Row3 + mat2.Row3, true
            );
        }

        public static Matrix4d operator -(Matrix4d mat1, Matrix4d mat2)
        {
            return new Matrix4d(
                mat1.Row0 - mat2.Row0, mat1.Row1 - mat2.Row1, mat1.Row2 - mat2.Row2, mat1.Row3 - mat2.Row3, true
            );
        }

        public static Matrix4d operator *(Matrix4d mat, double f)
        {
            return new Matrix4d(
                mat.Row0.x * f, mat.Row0.y * f, mat.Row0.z * f, mat.Row0.w * f,
                mat.Row1.x * f, mat.Row1.y * f, mat.Row1.z * f, mat.Row1.w * f,
                mat.Row2.x * f, mat.Row2.y * f, mat.Row2.z * f, mat.Row2.w * f,
                mat.Row3.x * f, mat.Row3.y * f, mat.Row3.z * f, mat.Row3.w * f
            );
        }

        public static Matrix4d operator *(double f, Matrix4d mat)
        {
            return mat * f;
        }


        public static Vector4d operator *(Matrix4d mat, Vector4d v)
        {
            return new Vector4d(
                mat.Row0.x * v.x + mat.Row0.y * v.y + mat.Row0.z * v.z + mat.Row0.w * v.w,
                mat.Row1.x * v.x + mat.Row1.y * v.y + mat.Row1.z * v.z + mat.Row1.w * v.w,
                mat.Row2.x * v.x + mat.Row2.y * v.y + mat.Row2.z * v.z + mat.Row2.w * v.w,
                mat.Row3.x * v.x + mat.Row3.y * v.y + mat.Row3.z * v.z + mat.Row3.w * v.w
            );
        }

        public static Matrix4d operator *(Matrix4d mat1, Matrix4d mat2)
        {
            double m00 = mat1.Row0.x * mat2.Row0.x + mat1.Row0.y * mat2.Row1.x + mat1.Row0.z * mat2.Row2.x + mat1.Row0.w * mat2.Row3.x;
            double m01 = mat1.Row0.x * mat2.Row0.y + mat1.Row0.y * mat2.Row1.y + mat1.Row0.z * mat2.Row2.y + mat1.Row0.w * mat2.Row3.y;
            double m02 = mat1.Row0.x * mat2.Row0.z + mat1.Row0.y * mat2.Row1.z + mat1.Row0.z * mat2.Row2.z + mat1.Row0.w * mat2.Row3.z;
            double m03 = mat1.Row0.x * mat2.Row0.w + mat1.Row0.y * mat2.Row1.w + mat1.Row0.z * mat2.Row2.w + mat1.Row0.w * mat2.Row3.w;

            double m10 = mat1.Row1.x * mat2.Row0.x + mat1.Row1.y * mat2.Row1.x + mat1.Row1.z * mat2.Row2.x + mat1.Row1.w * mat2.Row3.x;
            double m11 = mat1.Row1.x * mat2.Row0.y + mat1.Row1.y * mat2.Row1.y + mat1.Row1.z * mat2.Row2.y + mat1.Row1.w * mat2.Row3.y;
            double m12 = mat1.Row1.x * mat2.Row0.z + mat1.Row1.y * mat2.Row1.z + mat1.Row1.z * mat2.Row2.z + mat1.Row1.w * mat2.Row3.z;
            double m13 = mat1.Row1.x * mat2.Row0.w + mat1.Row1.y * mat2.Row1.w + mat1.Row1.z * mat2.Row2.w + mat1.Row1.w * mat2.Row3.w;

            double m20 = mat1.Row2.x * mat2.Row0.x + mat1.Row2.y * mat2.Row1.x + mat1.Row2.z * mat2.Row2.x + mat1.Row2.w * mat2.Row3.x;
            double m21 = mat1.Row2.x * mat2.Row0.y + mat1.Row2.y * mat2.Row1.y + mat1.Row2.z * mat2.Row2.y + mat1.Row2.w * mat2.Row3.x;
            double m22 = mat1.Row2.x * mat2.Row0.z + mat1.Row2.y * mat2.Row1.z + mat1.Row2.z * mat2.Row2.z + mat1.Row2.w * mat2.Row3.x;
            double m23 = mat1.Row2.x * mat2.Row0.w + mat1.Row2.y * mat2.Row1.w + mat1.Row2.z * mat2.Row2.w + mat1.Row2.w * mat2.Row3.w;

            double m30 = mat1.Row3.x * mat2.Row0.x + mat1.Row3.y * mat2.Row1.x + mat1.Row3.z * mat2.Row2.x + mat1.Row3.w * mat2.Row3.x;
            double m31 = mat1.Row3.x * mat2.Row0.y + mat1.Row3.y * mat2.Row1.y + mat1.Row3.z * mat2.Row2.y + mat1.Row3.w * mat2.Row3.x;
            double m32 = mat1.Row3.x * mat2.Row0.z + mat1.Row3.y * mat2.Row1.z + mat1.Row3.z * mat2.Row2.z + mat1.Row3.w * mat2.Row3.x;
            double m33 = mat1.Row3.x * mat2.Row0.w + mat1.Row3.y * mat2.Row1.w + mat1.Row3.z * mat2.Row2.w + mat1.Row3.w * mat2.Row3.w;

            return new Matrix4d(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ row0.GetHashCode();
                hash = (hash * 16777619) ^ row1.GetHashCode();
                hash = (hash * 16777619) ^ row2.GetHashCode();
                hash = (hash * 16777619) ^ row3.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return this == (Matrix4d)obj;
        }

        public bool Equals(Matrix4d other)
        {
            return row0 == other.row0 && row1 == other.row1 && row2 == other.row2 && row3 == other.row3;
        }


        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}\n{3}", row0.ToString(), row1.ToString(), row2.ToString(), row3.ToString());
        }
    }
}
