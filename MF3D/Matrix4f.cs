using System;

namespace MF3D
{
    [Serializable]
    public struct Matrix4f : IEquatable<Matrix4f>
    {
        public Vector4f row0, row1, row2, row3;


        public Matrix4f(float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            row0 = new Vector4f(m00, m01, m02, m03);
            row1 = new Vector4f(m10, m11, m12, m13);
            row2 = new Vector4f(m20, m21, m22, m23);
            row3 = new Vector4f(m30, m31, m32, m33);
        }

        public Matrix4f(Vector4f a, Vector4f b, Vector4f c, Vector4f d, bool inColumn = false)
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
                row0 = new Vector4f(a.x, b.x, c.x, d.x);
                row1 = new Vector4f(a.y, b.y, c.y, d.y);
                row2 = new Vector4f(a.z, b.z, c.z, d.z);
                row3 = new Vector4f(a.w, b.w, c.w, d.w);
            }
        }


        public static readonly Matrix4f Identity = new Matrix4f(
            1.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 1.0f
        );

        public static readonly Matrix4f Zero = new Matrix4f(
            0.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f
        );


        public float[] ToArray()
        {
            return new float[16] {
                row0.x, row0.y, row0.z, row0.w,
                row1.x, row1.y, row1.z, row1.w,
                row2.x, row2.y, row2.z, row2.w,
                row3.x, row3.y, row3.z, row3.w,
            };
        }

        public void ToArray(float[] arr)
        {
            arr[0] = row0.x; arr[1] = row0.y; arr[2] = row0.z; arr[3] = row0.w;
            arr[4] = row1.x; arr[5] = row1.y; arr[6] = row1.z; arr[7] = row1.w;
            arr[8] = row2.x; arr[9] = row2.y; arr[10] = row2.z; arr[11] = row2.w;
            arr[12] = row3.x; arr[13] = row3.y; arr[14] = row3.z; arr[15] = row3.w;
        }


        public float this[int r, int c]
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

        public float this[int i]
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


        public Vector4f Row0
        {
            get { return row0; }
            set { row0 = value; }
        }

        public Vector4f Row1
        {
            get { return row1; }
            set { row1 = value; }
        }

        public Vector4f Row2
        {
            get { return row2; }
            set { row2 = value; }
        }

        public Vector4f Row3
        {
            get { return row3; }
            set { row3 = value; }
        }

        public Vector4f Column0
        {
            get { return new Vector4f(row0.x, row1.x, row2.x, row3.x); }
            set { row0.x = value.x; row1.x = value.y; row2.x = value.z; row3.x = value.w; }
        }

        public Vector4f Column1
        {
            get { return new Vector4f(row0.y, row1.y, row2.y, row3.y); }
            set { row0.y = value.x; row1.y = value.y; row2.y = value.z; row3.y = value.w; }
        }

        public Vector4f Column2
        {
            get { return new Vector4f(row0.z, row1.z, row2.z, row3.z); }
            set { row0.z = value.x; row1.z = value.y; row2.z = value.z; row3.z = value.w; }
        }

        public Vector4f Column3
        {
            get { return new Vector4f(row0.w, row1.w, row2.w, row3.w); }
            set { row0.w = value.x; row1.w = value.y; row2.w = value.z; row3.w = value.w; }
        }

        public float Determinant
        {
            get
            {
                float m00 = Row0.x; float m01 = Row0.y; float m02 = Row0.z; float m03 = Row0.w;
                float m10 = Row1.x; float m11 = Row1.y; float m12 = Row1.z; float m13 = Row1.w;
                float m20 = Row2.x; float m21 = Row2.y; float m22 = Row2.z; float m23 = Row2.w;
                float m30 = Row3.x; float m31 = Row3.y; float m32 = Row3.z; float m33 = Row3.w;

                return (m00 * m11 * m22 * m33) - (m00 * m11 * m23 * m32) + (m00 * m12 * m23 * m31) - (m00 * m12 * m21 * m33)
                    + (m00 * m13 * m21 * m32) - (m00 * m13 * m22 * m31) - (m01 * m12 * m23 * m30) + (m01 * m12 * m20 * m33)
                    - (m01 * m13 * m20 * m32) + (m01 * m13 * m22 * m30) - (m01 * m10 * m22 * m33) + (m01 * m10 * m23 * m32)
                    + (m02 * m13 * m20 * m31) - (m02 * m13 * m21 * m30) + (m02 * m10 * m21 * m33) - (m02 * m10 * m23 * m31)
                    + (m02 * m11 * m23 * m30) - (m02 * m11 * m20 * m33) - (m03 * m10 * m21 * m32) + (m03 * m10 * m22 * m31)
                    - (m03 * m11 * m22 * m30) + (m03 * m11 * m20 * m32) - (m03 * m12 * m20 * m31) + (m03 * m12 * m21 * m30);
            }
        }


        public Matrix4f Transpose()
        {
            return new Matrix4f(
                Row0.x, Row1.x, Row2.x, Row3.x,
                Row0.y, Row1.y, Row2.y, Row3.y,
                Row0.z, Row1.z, Row2.z, Row3.z,
                Row0.w, Row1.w, Row2.w, Row3.w
            );
        }

        public Matrix4f Inverse(float epsilon = Constant.Epsilonf)
        {
            float m00 = Row0.x; float m01 = Row0.y; float m02 = Row0.z; float m03 = Row0.w;
            float m10 = Row1.x; float m11 = Row1.y; float m12 = Row1.z; float m13 = Row1.w;
            float m20 = Row2.x; float m21 = Row2.y; float m22 = Row2.z; float m23 = Row2.w;
            float m30 = Row3.x; float m31 = Row3.y; float m32 = Row3.z; float m33 = Row3.w;

            float i00 = m11 * m22 * m33 - m11 * m23 * m32 - m21 * m12 * m33 + m21 * m13 * m32 + m31 * m12 * m23 - m31 * m13 * m22;
            float i01 = -m01 * m22 * m33 + m01 * m23 * m32 + m21 * m02 * m33 - m21 * m03 * m32 - m31 * m02 * m23 + m31 * m03 * m22;
            float i02 = m01 * m12 * m33 - m01 * m13 * m32 - m11 * m02 * m33 + m11 * m03 * m32 + m31 * m02 * m13 - m31 * m03 * m12;
            float i03 = -m01 * m12 * m23 + m01 * m13 * m22 + m11 * m02 * m23 - m11 * m03 * m22 - m21 * m02 * m13 + m21 * m03 * m12;

            float i10 = -m10 * m22 * m33 + m10 * m23 * m32 + m20 * m12 * m33 - m20 * m13 * m32 - m30 * m12 * m23 + m30 * m13 * m22;
            float i11 = m00 * m22 * m33 - m00 * m23 * m32 - m20 * m02 * m33 + m20 * m03 * m32 + m30 * m02 * m23 - m30 * m03 * m22;
            float i12 = -m00 * m12 * m33 + m00 * m13 * m32 + m10 * m02 * m33 - m10 * m03 * m32 - m30 * m02 * m13 + m30 * m03 * m12;
            float i13 = m00 * m12 * m23 - m00 * m13 * m22 - m10 * m02 * m23 + m10 * m03 * m22 + m20 * m02 * m13 - m20 * m03 * m12;

            float i20 = m10 * m21 * m33 - m10 * m23 * m31 - m20 * m11 * m33 + m20 * m13 * m31 + m30 * m11 * m23 - m30 * m13 * m21;
            float i21 = -m00 * m21 * m33 + m00 * m23 * m31 + m20 * m01 * m33 - m20 * m03 * m31 - m30 * m01 * m23 + m30 * m03 * m21;
            float i22 = m00 * m11 * m33 - m00 * m13 * m31 - m10 * m01 * m33 + m10 * m03 * m31 + m30 * m01 * m13 - m30 * m03 * m11;
            float i23 = -m00 * m11 * m23 + m00 * m13 * m21 + m10 * m01 * m23 - m10 * m03 * m21 - m20 * m01 * m13 + m20 * m03 * m11;

            float i30 = -m10 * m21 * m32 + m10 * m22 * m31 + m20 * m11 * m32 - m20 * m12 * m31 - m30 * m11 * m22 + m30 * m12 * m21;
            float i31 = m00 * m21 * m32 - m00 * m22 * m31 - m20 * m01 * m32 + m20 * m02 * m31 + m30 * m01 * m22 - m30 * m02 * m21;
            float i32 = -m00 * m11 * m32 + m00 * m12 * m31 + m10 * m01 * m32 - m10 * m02 * m31 - m30 * m01 * m12 + m30 * m02 * m11;
            float i33 = m00 * m11 * m22 - m00 * m12 * m21 - m10 * m01 * m22 + m10 * m02 * m21 + m20 * m01 * m12 - m20 * m02 * m11;

            float det = m00 * i00 + m01 * i10 + m02 * i20 + m03 * i30;

            if ((float)System.Math.Abs(det) < epsilon)
            {
                throw new Exception("Matrix4f.Inverse: matrix is not invertible");
            }

            det = 1.0f / det;

            return new Matrix4f(
                i00 * det, i01 * det, i02 * det, i03 * det,
                i10 * det, i11 * det, i12 * det, i13 * det,
                i20 * det, i21 * det, i22 * det, i23 * det,
                i30 * det, i31 * det, i32 * det, i33 * det
            );
        }


        public Matrix4f Normalize()
        {
            float d = Determinant;
            Row0 /= d; Row1 /= d; Row2 /= d; Row3 /= d;
            return this;
        }

        public Matrix4f Normalized()
        {
            Matrix4f m = this;
            return m.Normalize();
        }


        public static Matrix4f CreateTranslation(Vector3f v)
        {
            Matrix4f m = Identity;
            m.row3.x = v.x; m.row3.y = v.y; m.row3.z = v.z;
            return m;
        }

        public Vector3f ExtractTranslation()
        {
            return new Vector3f(Row3.x, Row3.y, Row3.z);
        }

        public Matrix4f ClearTranslation()
        {
            Matrix4f m = this;
            m.row3.x = 0; m.row3.y = 0; m.row3.z = 0;
            return m;
        }


        public static Matrix4f CreateScale(float scale)
        {
            Matrix4f m = Identity;
            m.row0.x = scale; m.row1.y = scale; m.row2.z = scale;
            return m;
        }

        public static Matrix4f CreateScale(Vector3f scale)
        {
            Matrix4f m = Identity;
            m.row0.x = scale.x; m.row1.y = scale.y; m.row2.z = scale.z;
            return m;
        }

        public Vector3f ExtractScale()
        {
            Vector3f Row0xyz = new Vector3f(Row0.x, Row0.y, Row0.z);
            Vector3f Row1xyz = new Vector3f(Row1.x, Row1.y, Row1.z);
            Vector3f Row2xyz = new Vector3f(Row2.x, Row2.y, Row2.z);
            return new Vector3f(Row0xyz.Length, Row1xyz.Length, Row2xyz.Length);
        }

        public Matrix4f ClearScale()
        {
            Matrix4f m = this;
            Vector3f Row0xyz = new Vector3f(Row0.x, Row0.y, Row0.z).Normalized;
            Vector3f Row1xyz = new Vector3f(Row1.x, Row1.y, Row1.z).Normalized;
            Vector3f Row2xyz = new Vector3f(Row2.x, Row2.y, Row2.z).Normalized;
            m.Row0 = new Vector4f(Row0xyz.x, Row0xyz.y, Row0xyz.z, Row0.w);
            m.Row1 = new Vector4f(Row1xyz.x, Row1xyz.y, Row1xyz.z, Row0.w);
            m.Row2 = new Vector4f(Row2xyz.x, Row2xyz.y, Row2xyz.z, Row0.w);
            return m;
        }


        public static Matrix4f CreateRotationFromAxisAngle(Vector3f axis, float angle)
        {
            axis.Normalize();

            float cos = (float)System.Math.Cos(-angle);
            float sin = (float)System.Math.Sin(-angle);
            float t = 1.0f - cos;

            float txx = t * axis.x * axis.x; float txy = t * axis.x * axis.y; float txz = t * axis.x * axis.z;
            float tyy = t * axis.y * axis.y; float tyz = t * axis.y * axis.z; float tzz = t * axis.z * axis.z;

            float sinx = sin * axis.x; float siny = sin * axis.y; float sinz = sin * axis.z;

            Matrix4f m = Identity;
            m.Row0 = new Vector4f(txx + cos, txy - sinz, txz + siny, 0);
            m.Row1 = new Vector4f(txy + sinz, tyy + cos, tyz - sinx, 0);
            m.Row2 = new Vector4f(txy - siny, tyz + sinx, tzz + cos, 0);
            m.Row3 = new Vector4f(0, 0, 0, 1);
            return m;
        }

        
        public static Matrix4f CreateRotationFromQuaternion(Quaternionf quat)
        {
            float angle;
            Vector3f axis = quat.ToAxisAngle(out angle);
            return CreateRotationFromAxisAngle(axis, angle);
        }

        public Quaternionf ExtractRotation()
        {
            Vector3f row0xyz = new Vector3f(Row0.x, Row0.y, Row0.z).Normalized;
            Vector3f row1xyz = new Vector3f(Row1.x, Row1.y, Row1.z).Normalized;
            Vector3f row2xyz = new Vector3f(Row2.x, Row2.y, Row2.z).Normalized;

            Quaternionf q = new Quaternionf();

            float trace = 0.25f * (row0xyz.x + row1xyz.y + row2xyz.z + 1.0f);

            if (trace > 0f)
            {
                float sq = (float)System.Math.Sqrt(trace);

                q.w = sq;
                sq = 1.0f / (4.0f * sq);
                q.x = (row1xyz.z - row2xyz.y) * sq;
                q.y = (row2xyz.x - row0xyz.z) * sq;
                q.z = (row0xyz.y - row1xyz.x) * sq;
            }
            else if (row0xyz.x > row1xyz.y && row0xyz.x > row2xyz.z)
            {
                float sq = 2.0f * (float)System.Math.Sqrt(1.0f + row0xyz.x - row1xyz.y - row2xyz.z);

                q.w = 0.25f * sq;
                sq = 1.0f / sq;
                q.x = (row2xyz.y - row1xyz.z) * sq;
                q.y = (row1xyz.x - row0xyz.y) * sq;
                q.z = (row2xyz.x - row0xyz.z) * sq;
            }
            else if (row1xyz.y > row2xyz.z)
            {
                float sq = 2.0f * (float)System.Math.Sqrt(1.0f + row1xyz.y - row0xyz.x - row2xyz.z);

                q.w = 0.25f * sq;
                sq = 1.0f / sq;
                q.x = (row2xyz.x - row0xyz.z) * sq;
                q.y = (row1xyz.x - row0xyz.y) * sq;
                q.z = (row2xyz.y - row1xyz.z) * sq;
            }
            else
            {
                float sq = 2.0f * (float)System.Math.Sqrt(1.0 + row2xyz.z - row0xyz.x - row1xyz.y);

                q.w = 0.25f * sq;
                sq = 1.0f / sq;
                q.x = (row1xyz.x - row0xyz.y) * sq;
                q.y = (row2xyz.x - row0xyz.z) * sq;
                q.z = (row2xyz.y - row1xyz.z) * sq;
            }

            return q.Normalize();
        }

        public Matrix4f ClearRotation()
        {
            Matrix4f m = this;

            Vector3f row0xyz = new Vector3f(Row0.x, Row0.y, Row0.z);
            Vector3f row1xyz = new Vector3f(Row1.x, Row1.y, Row1.z);
            Vector3f row2xyz = new Vector3f(Row2.x, Row2.y, Row2.z);

            m.row0.x = row0xyz.Length;
            m.row1.y = row1xyz.Length;
            m.row2.z = row2xyz.Length;

            return m;
        }


        public static bool operator ==(Matrix4f a, Matrix4f b)
        {
            return a.row0 == b.row0 && a.row1 == b.row1 && a.row2 == b.row2 && a.row3 == b.row3;
        }

        public static bool operator !=(Matrix4f a, Matrix4f b)
        {
            return a.row0 != b.row0 || a.row1 != b.row1 || a.row2 != b.row2 || a.row3 != b.row3;
        }

        public static Matrix4f operator +(Matrix4f mat1, Matrix4f mat2)
        {
            return new Matrix4f(
                mat1.Row0 + mat2.Row0, mat1.Row1 + mat2.Row1, mat1.Row2 + mat2.Row2, mat1.Row3 + mat2.Row3, true
            );
        }

        public static Matrix4f operator -(Matrix4f mat1, Matrix4f mat2)
        {
            return new Matrix4f(
                mat1.Row0 - mat2.Row0, mat1.Row1 - mat2.Row1, mat1.Row2 - mat2.Row2, mat1.Row3 - mat2.Row3, true
            );
        }

        public static Matrix4f operator *(Matrix4f mat, float f)
        {
            return new Matrix4f(
                mat.Row0.x * f, mat.Row0.y * f, mat.Row0.z * f, mat.Row0.w * f,
                mat.Row1.x * f, mat.Row1.y * f, mat.Row1.z * f, mat.Row1.w * f,
                mat.Row2.x * f, mat.Row2.y * f, mat.Row2.z * f, mat.Row2.w * f,
                mat.Row3.x * f, mat.Row3.y * f, mat.Row3.z * f, mat.Row3.w * f
            );
        }

        public static Matrix4f operator *(float f, Matrix4f mat)
        {
            return mat * f;
        }


        public static Vector4f operator *(Matrix4f mat, Vector4f v)
        {
            return new Vector4f(
                mat.Row0.x * v.x + mat.Row0.y * v.y + mat.Row0.z * v.z + mat.Row0.w * v.w,
                mat.Row1.x * v.x + mat.Row1.y * v.y + mat.Row1.z * v.z + mat.Row1.w * v.w,
                mat.Row2.x * v.x + mat.Row2.y * v.y + mat.Row2.z * v.z + mat.Row2.w * v.w,
                mat.Row3.x * v.x + mat.Row3.y * v.y + mat.Row3.z * v.z + mat.Row3.w * v.w
            );
        }

        public static Matrix4f operator *(Matrix4f mat1, Matrix4f mat2)
        {
            float m00 = mat1.Row0.x * mat2.Row0.x + mat1.Row0.y * mat2.Row1.x + mat1.Row0.z * mat2.Row2.x + mat1.Row0.w * mat2.Row3.x;
            float m01 = mat1.Row0.x * mat2.Row0.y + mat1.Row0.y * mat2.Row1.y + mat1.Row0.z * mat2.Row2.y + mat1.Row0.w * mat2.Row3.y;
            float m02 = mat1.Row0.x * mat2.Row0.z + mat1.Row0.y * mat2.Row1.z + mat1.Row0.z * mat2.Row2.z + mat1.Row0.w * mat2.Row3.z;
            float m03 = mat1.Row0.x * mat2.Row0.w + mat1.Row0.y * mat2.Row1.w + mat1.Row0.z * mat2.Row2.w + mat1.Row0.w * mat2.Row3.w;

            float m10 = mat1.Row1.x * mat2.Row0.x + mat1.Row1.y * mat2.Row1.x + mat1.Row1.z * mat2.Row2.x + mat1.Row1.w * mat2.Row3.x;
            float m11 = mat1.Row1.x * mat2.Row0.y + mat1.Row1.y * mat2.Row1.y + mat1.Row1.z * mat2.Row2.y + mat1.Row1.w * mat2.Row3.y;
            float m12 = mat1.Row1.x * mat2.Row0.z + mat1.Row1.y * mat2.Row1.z + mat1.Row1.z * mat2.Row2.z + mat1.Row1.w * mat2.Row3.z;
            float m13 = mat1.Row1.x * mat2.Row0.w + mat1.Row1.y * mat2.Row1.w + mat1.Row1.z * mat2.Row2.w + mat1.Row1.w * mat2.Row3.w;

            float m20 = mat1.Row2.x * mat2.Row0.x + mat1.Row2.y * mat2.Row1.x + mat1.Row2.z * mat2.Row2.x + mat1.Row2.w * mat2.Row3.x;
            float m21 = mat1.Row2.x * mat2.Row0.y + mat1.Row2.y * mat2.Row1.y + mat1.Row2.z * mat2.Row2.y + mat1.Row2.w * mat2.Row3.x;
            float m22 = mat1.Row2.x * mat2.Row0.z + mat1.Row2.y * mat2.Row1.z + mat1.Row2.z * mat2.Row2.z + mat1.Row2.w * mat2.Row3.x;
            float m23 = mat1.Row2.x * mat2.Row0.w + mat1.Row2.y * mat2.Row1.w + mat1.Row2.z * mat2.Row2.w + mat1.Row2.w * mat2.Row3.w;

            float m30 = mat1.Row3.x * mat2.Row0.x + mat1.Row3.y * mat2.Row1.x + mat1.Row3.z * mat2.Row2.x + mat1.Row3.w * mat2.Row3.x;
            float m31 = mat1.Row3.x * mat2.Row0.y + mat1.Row3.y * mat2.Row1.y + mat1.Row3.z * mat2.Row2.y + mat1.Row3.w * mat2.Row3.x;
            float m32 = mat1.Row3.x * mat2.Row0.z + mat1.Row3.y * mat2.Row1.z + mat1.Row3.z * mat2.Row2.z + mat1.Row3.w * mat2.Row3.x;
            float m33 = mat1.Row3.x * mat2.Row0.w + mat1.Row3.y * mat2.Row1.w + mat1.Row3.z * mat2.Row2.w + mat1.Row3.w * mat2.Row3.w;

            return new Matrix4f(m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
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
            return this == (Matrix4f)obj;
        }

        public bool Equals(Matrix4f other)
        {
            return row0 == other.row0 && row1 == other.row1 && row2 == other.row2 && row3 == other.row3;
        }


        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}\n{3}", row0.ToString(), row1.ToString(), row2.ToString(), row3.ToString());
        }
    }
}
