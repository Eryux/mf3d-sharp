using System;
using System.Collections.Generic;

namespace MF3D
{
    [Serializable]
    public class Transformd : ICloneable
    {
        protected Transformd parent;

        protected List<Transformd> childs;


        Vector3d localPosition;

        Quaterniond localRotation;

        Vector3d localSize;


        Vector3d position;

        Quaterniond rotation;

        Vector3d size;


        public Transformd(Vector3d position, Quaterniond rotation, Vector3d scale, Transformd parent = null)
        {
            this.position = localPosition = position;
            this.rotation = localRotation = rotation;
            this.size = localSize = scale;

            Parent = parent;
        }

        public Transformd(Matrix4d mat, Transformd parent = null)
        {
            localPosition = mat.ExtractTranslation();
            localRotation = mat.ExtractRotation();
            localSize = mat.ExtractScale();

            Parent = parent;
        }

        public Vector3d LocalPosition
        {
            get
            {
                return localPosition;
            }
            set
            {
                localPosition = value;
                Refresh();
            }
        }

        public Quaterniond LocalRotation
        {
            get
            {
                return localRotation;
            }
            set
            {
                localRotation = value;
                Refresh();
            }
        }

        public Vector3d LocalSize
        {
            get
            {
                return localSize;
            }
            set
            {
                localSize = value;
                Refresh();
            }
        }


        public Vector3d Position
        {
            get
            {
                return position;
            }
            set
            {
                LocalPosition = (parent != null) ? value - parent.Position : value;
            }
        }

        public Quaterniond Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                LocalRotation = (parent != null) ? value * parent.Rotation.Inverse() : value;
            }
        }

        public Vector3d Size
        {
            get
            {
                return size;
            }
        }

        public Transformd Parent
        {
            get { return parent; }
            set
            {
                if (parent != null && parent.childs != null)
                {
                    localPosition = position;
                    localRotation = rotation;
                    localSize = size;

                    parent.childs.Remove(this);
                }

                parent = value;

                if (parent != null)
                {
                    if (parent.childs == null)
                    {
                        parent.childs = new List<Transformd>();
                    }

                    parent.childs.Add(this);

                    localRotation = rotation * parent.Rotation.Inverse();
                    localPosition = position - parent.Position;
                }
                else
                {
                    position = localPosition;
                    rotation = localRotation;
                    size = localSize;
                }

                Refresh();
            }
        }


        public void Rotate(Quaterniond rotation, bool world = false)
        {
            if (world)
            {
                Rotation *= rotation;
            }
            else
            {
                LocalRotation *= rotation;
            }
        }

        public void Translate(Vector3d translation, bool world = false)
        {
            if (world)
            {
                Position = Position * (translation * rotation);
            }
            else
            {
                LocalPosition = localPosition * (translation * localRotation);
            }
        }

        public void Scale(Vector3d scale)
        {
            LocalSize *= scale;
        }


        public void Refresh()
        {
            if (parent != null)
            {
                size = parent.size * localSize;
                rotation = parent.Rotation * localRotation;
                position = parent.position + (localPosition * parent.Rotation) * size;
            }
            else
            {
                rotation = localRotation;
                position = localPosition;
                size = localSize;
            }

            // Refresh childs
            if (childs != null)
            {
                for (int i = 0; i < childs.Count; ++i)
                {
                    childs[i].Refresh();
                }
            }
        }


        public Matrix4d GetLocalTranslationMatrix()
        {
            return new Matrix4d(
                1f, 0f, 0f, localPosition.x,
                0f, 1f, 0f, localPosition.y,
                0f, 0f, 1f, localPosition.z,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4d GetLocalRotationMatrix()
        {
            Matrix3d mat = localRotation.ToRotationMatrix();

            return new Matrix4d(
                mat.row0.X, mat.row0.Y, mat.row0.z, 0f,
                mat.row1.X, mat.row1.Y, mat.row1.z, 0f,
                mat.row2.X, mat.row2.Y, mat.row2.z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4d GetLocalScaleMatrix()
        {
            return new Matrix4d(
                localSize.x, 0f, 0f, 0f,
                0f, localSize.y, 0f, 0f,
                0f, 0f, localSize.z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4d GetLocalMatrix()
        {
            return GetLocalTranslationMatrix() * GetLocalRotationMatrix() * GetLocalScaleMatrix();
        }


        public Matrix4d GetTranslationMatrix()
        {
            return new Matrix4d(
                1f, 0f, 0f, position.x,
                0f, 1f, 0f, position.y,
                0f, 0f, 1f, position.z,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4d GetRotationMatrix()
        {
            Matrix3d mat = rotation.ToRotationMatrix();

            return new Matrix4d(
                mat.row0.X, mat.row0.Y, mat.row0.z, 0f,
                mat.row1.X, mat.row1.Y, mat.row1.z, 0f,
                mat.row2.X, mat.row2.Y, mat.row2.z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4d GetScaleMatrix()
        {
            return new Matrix4d(
                size.x, 0f, 0f, 0f,
                0f, size.y, 0f, 0f,
                0f, 0f, size.z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4d GetMatrix()
        {
            return GetTranslationMatrix() * GetRotationMatrix() * GetScaleMatrix();
        }


        public Transformd[] GetChilds()
        {
            return childs.ToArray();
        }


        public object Clone()
        {
            Transformd clone = new Transformd(position, rotation, size, parent);

            if (childs != null)
            {
                for (int i = 0; i < childs.Count; ++i)
                {
                    Transformd childClone = (Transformd)childs[i].Clone();
                    childClone.Parent = clone;
                }
            }

            return clone;
        }
    }
}