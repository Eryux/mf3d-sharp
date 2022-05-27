using System;

namespace MF3D
{
    [Serializable]
    public struct Quaterniond : IEquatable<Quaterniond>
    {
        public double x, y, z, w;


        public Quaterniond(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quaterniond(Quaterniond quat)
        {
            x = quat.x;
            y = quat.y;
            z = quat.z;
            w = quat.w;
        }


        static public readonly Quaterniond Zero = new Quaterniond(0.0, 0.0, 0.0, 0.0);
        static public readonly Quaterniond Identity = new Quaterniond(0.0, 0.0, 0.0, 1.0);


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
            get
            {
                return x * x + y * y + z * z + w * w;
            }
        }

        public double Length
        {
            get
            {
                return System.Math.Sqrt(LengthSquared);
            }
        }

        public Vector3d AxisX
        {
            get
            {
                double twoY = 2 * y; double twoZ = 2 * z;
                double twoWY = twoY * w; double twoWZ = twoZ * w;
                double twoXY = twoY * x; double twoXZ = twoZ * x;
                double twoYY = twoY * y; double twoZZ = twoZ * z;
                return new Vector3d(1 - (twoYY + twoZZ), twoXY + twoWZ, twoXZ - twoWY);
            }
        }

        public Vector3d AxisY
        {
            get
            {
                double twoX = 2 * x; double twoY = 2 * y; double twoZ = 2 * z;
                double twoWX = twoX * w; double twoWZ = twoZ * w; double twoXX = twoX * x;
                double twoXY = twoY * x; double twoYZ = twoZ * y; double twoZZ = twoZ * z;
                return new Vector3d(twoXY - twoWZ, 1 - (twoXX + twoZZ), twoYZ + twoWX);
            }
        }

        public Vector3d AxisZ
        {
            get
            {
                double twoX = 2 * x; double twoY = 2 * y; double twoZ = 2 * z;
                double twoWX = twoX * w; double twoWY = twoY * w; double twoXX = twoX * x;
                double twoXZ = twoZ * x; double twoYY = twoY * y; double twoYZ = twoZ * y;
                return new Vector3d(twoXZ + twoWY, twoYZ - twoWX, 1 - (twoXX + twoYY));
            }
        }


        public Quaterniond Normalize(double epsilon = 0)
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
                x = y = z = 0;
                w = 1;
            }

            return this;
        }

        public Quaterniond Normalized
        {
            get
            {
                return new Quaterniond(this).Normalize();
            }
        }


        public Quaterniond Inverse()
        {
            double norm = LengthSquared;

            if (norm > 0)
            {
                norm = 1.0 / norm;
                return new Quaterniond(
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

        public static Quaterniond Inverse(Quaterniond q)
        {
            return q.Inverse();
        }


        public Quaterniond Conjugate()
        {
            return new Quaterniond(-x, -y, -z, w);
        }


        public Matrix3d ToRotationMatrix()
        {
            double twoX = 2 * x; double twoY = 2 * y; double twoZ = 2 * z;
            double twoWX = twoX * w; double twoWY = twoY * w; double twoWZ = twoZ * w;
            double twoXX = twoX * x; double twoXY = twoY * x; double twoXZ = twoZ * x;
            double twoYY = twoY * y; double twoYZ = twoZ * y; double twoZZ = twoZ * z;

            return new Matrix3d(
                1 - (twoYY + twoZZ), twoXY - twoWZ, twoXZ + twoWY,
                twoXY + twoWZ, 1 - (twoXX + twoZZ), twoYZ - twoWX,
                twoXZ - twoWY, twoYZ + twoWX, 1 - (twoXX + twoYY)
            );
        }

        public static Quaterniond FromRotationMatrix(Matrix3d rot)
        {
            Quaterniond q = Identity;

            double trace = rot.row0.x + rot.row1.y + rot.row2.z;

            if (trace > 0)
            {
                double s = System.Math.Sqrt(trace + 1) * 2;
                double invs = 1.0 / s;

                q.w = s * 0.25;
                q.x = (rot.row2.y - rot.row1.z) * invs;
                q.y = (rot.row0.z - rot.row2.x) * invs;
                q.z = (rot.row1.x - rot.row0.y) * invs;
            }
            else
            {
                double m00 = rot.Row0.x;
                double m11 = rot.Row1.y;
                double m22 = rot.Row2.z;

                if (m00 > m11 && m00 > m22)
                {
                    double s = System.Math.Sqrt(1 + m00 - m11 - m22) * 2;
                    double invs = 1.0 / s;

                    q.w = (rot.row2.y - rot.row1.z) * invs;
                    q.x = s * 0.25;
                    q.y = (rot.row0.y + rot.row1.x) * invs;
                    q.z = (rot.row0.z + rot.row2.x) * invs;
                }
                else if (m11 > m22)
                {
                    double s = System.Math.Sqrt(1 + m22 - m00 - m22) * 2;
                    double invs = 1.0 / s;

                    q.w = (rot.row0.z - rot.row2.x) * invs;
                    q.x = (rot.row0.y + rot.row1.x) * invs;
                    q.y = s * 0.25;
                    q.z = (rot.row1.z + rot.row2.y) * invs;
                }
                else
                {
                    double s = System.Math.Sqrt(1 + m22 - m00 - m11) * 2;
                    double invs = 1.0 / s;

                    q.w = (rot.row1.x - rot.row0.y) * invs;
                    q.x = (rot.row0.z + rot.row2.x) * invs;
                    q.y = (rot.row1.z + rot.row2.y) * invs;
                    q.z = s * 0.25;
                }
            }

            return q.Normalize();
        }

        public static Quaterniond FromRotationMatrix(Matrix4d rot)
        {
            Matrix3d mat = Matrix3d.Zero;
            mat.row0.x = rot.row0.X;
            mat.row1.y = rot.row1.y;
            mat.row2.z = rot.row2.z;
            return FromRotationMatrix(rot);
        }


        public Vector3d ToAxisAngle(out double angle)
        {
            Quaterniond quat = this;

            if (System.Math.Abs(quat.w) > 1.0)
            {
                quat.Normalize();
            }

            Vector3d r;

            double d = System.Math.Sqrt(1.0 - quat.w * quat.w);
            r = (d > 0) ? new Vector3d(quat.x / d, quat.y / d, quat.z / d) : Vector3d.AxisX;

            angle = 2.0 * System.Math.Acos(quat.w);
            return r;
        }

        public static Quaterniond FromAxisAngle(Vector3d axis, double angle)
        {
            Quaterniond quat = Zero;

            double halfAngle = 0.5 * angle;
            double sn = System.Math.Sin(halfAngle);

            quat.w = (float)System.Math.Cos(halfAngle);
            quat.x = (float)(sn * axis.x);
            quat.y = (float)(sn * axis.y);
            quat.z = (float)(sn * axis.z);

            return quat;
        }


        public static Quaterniond FromTo(Vector3d vFrom, Vector3d vTo)
        {
            Quaterniond quat = Identity;

            Vector3d from = vFrom.Normalized, to = vTo.Normalized;
            Vector3d bisector = (from + to).Normalized;

            quat.w = from.Dot(bisector);

            if (quat.w != 0)
            {
                Vector3d cross = from.Cross(bisector);
                quat.x = cross.x;
                quat.y = cross.y;
                quat.z = cross.z;
            }
            else
            {
                double invLength;

                if (System.Math.Abs(from.x) >= System.Math.Abs(from.y))
                {
                    invLength = 1.0 / System.Math.Sqrt(from.x * from.x + from.z * from.z);
                    quat.x = -from.z * invLength;
                    quat.y = 0;
                    quat.z = +from.x * invLength;
                }
                else
                {
                    invLength = 1.0 / System.Math.Sqrt(from.y * from.y + from.z * from.z);
                    quat.x = 0;
                    quat.y = +from.z * invLength;
                    quat.z = -from.y * invLength;
                }
            }

            return quat.Normalize();
        }


        public Vector3d ToEuler()
        {
            const double SINGULARITY_THRESHOLD = 0.4999995f;

            double sqw = w * w; double sqx = x * x; double sqy = y * y; double sqz = z * z;
            double unit = sqx + sqy + sqz + sqw;
            double singularity = x * z + w * y;

            Vector3d r = new Vector3d();

            if (singularity > SINGULARITY_THRESHOLD * unit)
            {
                r.z = 2 * System.Math.Atan2(x, w);
                r.y = Constant.HalfPI;
                r.x = 0;
            }
            else if (singularity < -SINGULARITY_THRESHOLD * unit)
            {
                r.z = -2 * System.Math.Atan2(x, w);
                r.y = Constant.HalfPI;
                r.x = 0;
            }
            else
            {
                r.z = System.Math.Atan2(2 * ((w * z) - (x * y)), sqw + sqx - sqy - sqz);
                r.y = System.Math.Asin(2 * singularity / unit);
                r.x = System.Math.Atan2(2 * ((w * x) - (y * z)), sqw - sqx - sqy + sqz);
            }

            if (r.x == double.NaN)
                r.x = 0;
            if (r.y == double.NaN)
                r.y = 0;
            if (r.z == double.NaN)
                r.z = 0;

            return r;
        }


        public static Quaterniond FromEuler(Vector3d vect)
        {
            vect *= 0.5f;

            double c1 = System.Math.Cos(vect.x); double c2 = System.Math.Cos(vect.y); double c3 = System.Math.Cos(vect.z);
            double s1 = System.Math.Sin(vect.x); double s2 = System.Math.Sin(vect.y); double s3 = System.Math.Sin(vect.z);

            return new Quaterniond(
                (s1 * c2 * c3) + (c1 * s2 * s3),
                (c1 * s2 * c3) - (s1 * c2 * s3),
                (c1 * c2 * s3) + (s1 * s2 * c3),
                (c1 * c2 * c3) - (s1 * s2 * s3)
            ).Normalize();
        }


        public static Quaterniond Slerp(Quaterniond p, Quaterniond q, double t)
        {
            double cs = p.Dot(q);
            double angle = System.Math.Acos(cs);

            if (System.Math.Abs(angle) >= Constant.ZeroTolerance)
            {
                double sn = System.Math.Sin(angle);
                double invSn = 1 / sn;
                double tAngle = t * angle;
                double coeff0 = System.Math.Sin(angle - tAngle) * invSn;
                double coeff1 = System.Math.Sin(tAngle) * invSn;
                
                Quaterniond r = Identity;

                r.x = coeff0 * p.x + coeff1 * q.x;
                r.y = coeff0 * p.y + coeff1 * q.y;
                r.z = coeff0 * p.z + coeff1 * q.z;
                r.w = coeff0 * p.w + coeff1 * q.w;

                return r;
            }

            return new Quaterniond(p);
        }


        public bool EpsilonEqual(Quaterniond quat, double epsilon)
        {
            return System.Math.Abs(x - quat.x) <= epsilon &&
                  System.Math.Abs(y - quat.y) <= epsilon &&
                  System.Math.Abs(z - quat.z) <= epsilon &&
                  System.Math.Abs(w - quat.w) <= epsilon;
        }


        public double Dot(Quaterniond quat)
        {
            return x * quat.x + y * quat.y + z * quat.z + w * quat.w;
        }


        public static bool operator ==(Quaterniond q1, Quaterniond q2)
        {
            return q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w;
        }

        public static bool operator !=(Quaterniond q1, Quaterniond q2)
        {
            return q1.x != q2.x || q1.y != q2.y || q1.z != q2.z || q1.w != q2.w;
        }

        public static Quaterniond operator +(Quaterniond q1, Quaterniond q2)
        {
            return new Quaterniond(q1.x + q2.x, q1.y + q2.y, q1.z + q2.z, q1.w + q2.w);
        }

        public static Quaterniond operator -(Quaterniond q2)
        {
            return new Quaterniond(-q2.x, -q2.y, -q2.z, -q2.w);
        }

        public static Quaterniond operator -(Quaterniond q1, Quaterniond q2)
        {
            return new Quaterniond(q1.x - q2.x, q1.y - q2.y, q1.z - q2.z, q1.w - q2.w);
        }

        public static Quaterniond operator *(Quaterniond a, Quaterniond b)
        {
            double w = a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z;
            double x = a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y;
            double y = a.w * b.y + a.y * b.w + a.z * b.x - a.x * b.z;
            double z = a.w * b.z + a.z * b.w + a.x * b.y - a.y * b.x;
            return new Quaterniond(x, y, z, w);
        }

        public static Quaterniond operator *(Quaterniond q1, double d)
        {
            return new Quaterniond(d * q1.x, d * q1.y, d * q1.z, d * q1.w);
        }

        public static Quaterniond operator *(double d, Quaterniond q1)
        {
            return new Quaterniond(d * q1.x, d * q1.y, d * q1.z, d * q1.w);
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
            return this == (Quaterniond)obj;
        }

        public bool Equals(Quaterniond other)
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
