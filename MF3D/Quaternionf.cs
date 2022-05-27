using System;

namespace MF3D
{
    [Serializable]
    public struct Quaternionf : IEquatable<Quaternionf>
    {
        public float x, y, z, w;


        public Quaternionf(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quaternionf(Quaternionf quat)
        {
            x = quat.x;
            y = quat.y;
            z = quat.z;
            w = quat.w;
        }


        static public readonly Quaternionf Zero = new Quaternionf(0f, 0f, 0f, 0f);
        static public readonly Quaternionf Identity = new Quaternionf(0f, 0f, 0f, 1f);


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
            get
            {
                return x * x + y * y + z * z + w * w;
            }
        }

        public float Length
        {
            get
            {
                return (float)System.Math.Sqrt(LengthSquared);
            }
        }

        public Vector3f AxisX
        {
            get
            {
                float twoY = 2 * y; float twoZ = 2 * z;
                float twoWY = twoY * w; float twoWZ = twoZ * w;
                float twoXY = twoY * x; float twoXZ = twoZ * x;
                float twoYY = twoY * y; float twoZZ = twoZ * z;
                return new Vector3f(1 - (twoYY + twoZZ), twoXY + twoWZ, twoXZ - twoWY);
            }
        }

        public Vector3f AxisY
        {
            get
            {
                float twoX = 2 * x; float twoY = 2 * y; float twoZ = 2 * z;
                float twoWX = twoX * w; float twoWZ = twoZ * w; float twoXX = twoX * x;
                float twoXY = twoY * x; float twoYZ = twoZ * y; float twoZZ = twoZ * z;
                return new Vector3f(twoXY - twoWZ, 1 - (twoXX + twoZZ), twoYZ + twoWX);
            }
        }

        public Vector3f AxisZ
        {
            get
            {
                float twoX = 2 * x; float twoY = 2 * y; float twoZ = 2 * z;
                float twoWX = twoX * w; float twoWY = twoY * w; float twoXX = twoX * x;
                float twoXZ = twoZ * x; float twoYY = twoY * y; float twoYZ = twoZ * y;
                return new Vector3f(twoXZ + twoWY, twoYZ - twoWX, 1 - (twoXX + twoYY));
            }
        }


        public Quaternionf Normalize(float epsilon = 0)
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
                x = y = z = 0f;
                w = 1f;
            }

            return this;
        }

        public Quaternionf Normalized
        {
            get
            {
                return new Quaternionf(this).Normalize();
            }
        }


        public Quaternionf Inverse()
        {
            float norm = LengthSquared;

            if (norm > 0)
            {
                norm = 1.0f / norm;
                return new Quaternionf(
                    -x * norm,
                    -y * norm,
                    -z * norm,
                    w * norm
                );
            }
            else
            {
                return Zero;
            }
        }

        public static Quaternionf Inverse(Quaternionf q)
        {
            return q.Inverse();
        }


        public Quaternionf Conjugate()
        {
            return new Quaternionf(-x, -y, -z, w);
        }


        public Matrix3f ToRotationMatrix()
        {
            float twoX = 2 * x; float twoY = 2 * y; float twoZ = 2 * z;
            float twoWX = twoX * w; float twoWY = twoY * w; float twoWZ = twoZ * w;
            float twoXX = twoX * x; float twoXY = twoY * x; float twoXZ = twoZ * x;
            float twoYY = twoY * y; float twoYZ = twoZ * y; float twoZZ = twoZ * z;

            return new Matrix3f(
                1 - (twoYY + twoZZ), twoXY - twoWZ, twoXZ + twoWY,
                twoXY + twoWZ, 1 - (twoXX + twoZZ), twoYZ - twoWX,
                twoXZ - twoWY, twoYZ + twoWX, 1 - (twoXX + twoYY)
            );
        }

        public static Quaternionf FromRotationMatrix(Matrix3f rot)
        {
            Quaternionf q = Identity;

            float trace = rot.row0.x + rot.row1.y + rot.row2.z;

            if (trace > 0)
            {
                float s = (float)System.Math.Sqrt(trace + 1) * 2;
                float invs = 1.0f / s;

                q.w = s * 0.25f;
                q.x = (rot.row2.y - rot.row1.z) * invs;
                q.y = (rot.row0.z - rot.row2.x) * invs;
                q.z = (rot.row1.x - rot.row0.y) * invs;
            }
            else
            {
                float m00 = rot.Row0.x;
                float m11 = rot.Row1.y;
                float m22 = rot.Row2.z;

                if (m00 > m11 && m00 > m22)
                {
                    float s = (float)System.Math.Sqrt(1 + m00 - m11 - m22) * 2;
                    float invs = 1.0f / s;

                    q.w = (rot.row2.y - rot.row1.z) * invs;
                    q.x = s * 0.25f;
                    q.y = (rot.row0.y + rot.row1.x) * invs;
                    q.z = (rot.row0.z + rot.row2.x) * invs;
                }
                else if (m11 > m22)
                {
                    float s = (float)System.Math.Sqrt(1 + m22 - m00 - m22) * 2;
                    float invs = 1.0f / s;

                    q.w = (rot.row0.z - rot.row2.x) * invs;
                    q.x = (rot.row0.y + rot.row1.x) * invs;
                    q.y = s * 0.25f;
                    q.z = (rot.row1.z + rot.row2.y) * invs;
                }
                else
                {
                    float s = (float)System.Math.Sqrt(1 + m22 - m00 - m11) * 2;
                    float invs = 1.0f / s;

                    q.w = (rot.row1.x - rot.row0.y) * invs;
                    q.x = (rot.row0.z + rot.row2.x) * invs;
                    q.y = (rot.row1.z + rot.row2.y) * invs;
                    q.z = s * 0.25f;
                }
            }

            return q.Normalize();
        }

        public static Quaternionf FromRotationMatrix(Matrix4f rot)
        {
            Matrix3f mat = Matrix3f.Zero;
            mat.row0.x = rot.row0.X;
            mat.row1.y = rot.row1.y;
            mat.row2.z = rot.row2.z;
            return FromRotationMatrix(rot);
        }


        public Vector3f ToAxisAngle(out float angle)
        {
            Quaternionf quat = this;

            if (System.Math.Abs(quat.w) > 1.0)
            {
                quat.Normalize();
            }

            Vector3f r;

            float d = (float)System.Math.Sqrt(1.0 - quat.w * quat.w);
            r = (d > 0) ? new Vector3f(quat.x / d, quat.y / d, quat.z / d) : Vector3f.AxisX;

            angle = 2.0f * (float)System.Math.Acos(quat.w);
            return r;
        }

        public static Quaternionf FromAxisAngle(Vector3f axis, float angle)
        {
            Quaternionf quat = Zero;

            float halfAngle = 0.5f * angle;
            float sn = (float)System.Math.Sin(halfAngle);

            quat.w = (float)System.Math.Cos(halfAngle);
            quat.x = (float)(sn * axis.x);
            quat.y = (float)(sn * axis.y);
            quat.z = (float)(sn * axis.z);

            return quat;
        }


        public static Quaternionf FromTo(Vector3f vFrom, Vector3f vTo)
        {
            Quaternionf quat = Identity;

            Vector3f from = vFrom.Normalized, to = vTo.Normalized;
            Vector3f bisector = (from + to).Normalized;

            quat.w = from.Dot(bisector);

            if (quat.w != 0)
            {
                Vector3f cross = from.Cross(bisector);
                quat.x = cross.x;
                quat.y = cross.y;
                quat.z = cross.z;
            }
            else
            {
                float invLength;

                if (System.Math.Abs(from.x) >= System.Math.Abs(from.y))
                {
                    invLength = 1.0f / (float)System.Math.Sqrt(from.x * from.x + from.z * from.z);
                    quat.x = -from.z * invLength;
                    quat.y = 0f;
                    quat.z = +from.x * invLength;
                }
                else
                {
                    invLength = 1.0f / (float)System.Math.Sqrt(from.y * from.y + from.z * from.z);
                    quat.x = 0f;
                    quat.y = +from.z * invLength;
                    quat.z = -from.y * invLength;
                }
            }

            return quat.Normalize();
        }


        public Vector3f ToEuler()
        {
            const float SINGULARITY_THRESHOLD = 0.4999995f;

            float sqw = w * w; float sqx = x * x; float sqy = y * y; float sqz = z * z;
            float unit = sqx + sqy + sqz + sqw;
            float singularity = x * z + w * y;

            Vector3f r = new Vector3f();

            if (singularity > SINGULARITY_THRESHOLD * unit)
            {
                r.z = 2f * (float)System.Math.Atan2(x, w);
                r.y = Constant.HalfPIf;
                r.x = 0f;
            }
            else if (singularity < -SINGULARITY_THRESHOLD * unit)
            {
                r.z = -2f * (float)System.Math.Atan2(x, w);
                r.y = Constant.HalfPIf;
                r.x = 0f;
            }
            else
            {
                r.z = (float)System.Math.Atan2(2f * ((w * z) - (x * y)), sqw + sqx - sqy - sqz);
                r.y = (float)System.Math.Asin(2f * singularity / unit);
                r.x = (float)System.Math.Atan2(2f * ((w * x) - (y * z)), sqw - sqx - sqy + sqz);
            }

            if (r.x == float.NaN)
                r.x = 0f;
            if (r.y == float.NaN)
                r.y = 0f;
            if (r.z == float.NaN)
                r.z = 0f;

            return r;
        }


        public static Quaternionf FromEuler(Vector3f vect)
        {
            vect *= 0.5f;

            float c1 = (float)System.Math.Cos(vect.x); float c2 = (float)System.Math.Cos(vect.y); float c3 = (float)System.Math.Cos(vect.z);
            float s1 = (float)System.Math.Sin(vect.x); float s2 = (float)System.Math.Sin(vect.y); float s3 = (float)System.Math.Sin(vect.z);

            return new Quaternionf(
                (s1 * c2 * c3) + (c1 * s2 * s3),
                (c1 * s2 * c3) - (s1 * c2 * s3),
                (c1 * c2 * s3) + (s1 * s2 * c3),
                (c1 * c2 * c3) - (s1 * s2 * s3)
            ).Normalize();
        }


        public static Quaternionf Slerp(Quaternionf p, Quaternionf q, float t)
        {
            float cs = p.Dot(q);
            float angle = (float)System.Math.Acos(cs);

            if ((float)System.Math.Abs(angle) >= Constant.ZeroTolerance)
            {
                float sn = (float)System.Math.Sin(angle);
                float invSn = 1 / sn;
                float tAngle = t * angle;
                float coeff0 = (float)System.Math.Sin(angle - tAngle) * invSn;
                float coeff1 = (float)System.Math.Sin(tAngle) * invSn;

                Quaternionf r = Identity;

                r.x = coeff0 * p.x + coeff1 * q.x;
                r.y = coeff0 * p.y + coeff1 * q.y;
                r.z = coeff0 * p.z + coeff1 * q.z;
                r.w = coeff0 * p.w + coeff1 * q.w;

                return r;
            }

            return new Quaternionf(p);
        }


        public bool EpsilonEqual(Quaternionf quat, float epsilon)
        {
            return System.Math.Abs(x - quat.x) <= epsilon &&
                  System.Math.Abs(y - quat.y) <= epsilon &&
                  System.Math.Abs(z - quat.z) <= epsilon &&
                  System.Math.Abs(w - quat.w) <= epsilon;
        }


        public float Dot(Quaternionf quat)
        {
            return x * quat.x + y * quat.y + z * quat.z + w * quat.w;
        }


        public static bool operator ==(Quaternionf q1, Quaternionf q2)
        {
            return q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w;
        }

        public static bool operator !=(Quaternionf q1, Quaternionf q2)
        {
            return q1.x != q2.x || q1.y != q2.y || q1.z != q2.z || q1.w != q2.w;
        }

        public static Quaternionf operator +(Quaternionf q1, Quaternionf q2)
        {
            return new Quaternionf(q1.x + q2.x, q1.y + q2.y, q1.z + q2.z, q1.w + q2.w);
        }

        public static Quaternionf operator -(Quaternionf q2)
        {
            return new Quaternionf(-q2.x, -q2.y, -q2.z, -q2.w);
        }

        public static Quaternionf operator -(Quaternionf q1, Quaternionf q2)
        {
            return new Quaternionf(q1.x - q2.x, q1.y - q2.y, q1.z - q2.z, q1.w - q2.w);
        }

        public static Quaternionf operator *(Quaternionf a, Quaternionf b)
        {
            float w = a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z;
            float x = a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y;
            float y = a.w * b.y + a.y * b.w + a.z * b.x - a.x * b.z;
            float z = a.w * b.z + a.z * b.w + a.x * b.y - a.y * b.x;
            return new Quaternionf(x, y, z, w);
        }

        public static Quaternionf operator *(Quaternionf q1, float d)
        {
            return new Quaternionf(d * q1.x, d * q1.y, d * q1.z, d * q1.w);
        }

        public static Quaternionf operator *(float d, Quaternionf q1)
        {
            return new Quaternionf(d * q1.x, d * q1.y, d * q1.z, d * q1.w);
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
            return this == (Quaternionf)obj;
        }

        public bool Equals(Quaternionf other)
        {
            return (x == other.x && y == other.y && z == other.z && w == other.w);
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
