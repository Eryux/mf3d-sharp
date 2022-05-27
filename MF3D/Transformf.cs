using System;
using System.Collections.Generic;

namespace MF3D
{
    [Serializable]
    public class Transformf : ICloneable
    {
        protected Transformf parent;

        protected List<Transformf> childs;


        Vector3f localPosition;

        Quaternionf localRotation;

        Vector3f localSize;


        Vector3f position;

        Quaternionf rotation;

        Vector3f size;


        public Transformf(Vector3f position, Quaternionf rotation, Vector3f scale, Transformf parent = null)
        {
            this.position = localPosition = position;
            this.rotation = localRotation = rotation;
            this.size = localSize = scale;
            
            Parent = parent;
        }

        public Transformf(Matrix4f mat, Transformf parent = null)
        {
            localPosition = mat.ExtractTranslation();
            localRotation = mat.ExtractRotation();
            localSize = mat.ExtractScale();

            Parent = parent;
        }

        public Vector3f LocalPosition
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

        public Quaternionf LocalRotation
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

        public Vector3f LocalSize
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


        public Vector3f Position
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

        public Quaternionf Rotation
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

        public Vector3f Size
        {
            get
            {
                return size;
            }
        }

        public Transformf Parent
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
                        parent.childs = new List<Transformf>();
                    }

                    parent.childs.Add(this);

                    localRotation = rotation * parent.Rotation.Inverse();
                    localPosition = position - parent.Position;
                }

                Refresh();
            }
        }


        public void Rotate(Quaternionf rotation, bool world = false)
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

        public void Translate(Vector3f translation, bool world = false)
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

        public void Scale(Vector3f scale)
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


        public Matrix4f GetLocalTranslationMatrix()
        {
            return new Matrix4f(
                1f, 0f, 0f, localPosition.x,
                0f, 1f, 0f, localPosition.y,
                0f, 0f, 1f, localPosition.z,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4f GetLocalRotationMatrix()
        {
            Matrix3f mat = localRotation.ToRotationMatrix();

            return new Matrix4f(
                mat.row0.X, mat.row0.Y, mat.row0.z, 0f,
                mat.row1.X, mat.row1.Y, mat.row1.z, 0f,
                mat.row2.X, mat.row2.Y, mat.row2.z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4f GetLocalScaleMatrix()
        {
            return new Matrix4f(
                localSize.x, 0f, 0f, 0f,
                0f, localSize.y, 0f, 0f,
                0f, 0f, localSize.z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4f GetLocalMatrix()
        {
            return GetLocalTranslationMatrix() * GetLocalRotationMatrix() * GetLocalScaleMatrix();
        }


        public Matrix4f GetTranslationMatrix()
        {
            return new Matrix4f(
                1f, 0f, 0f, position.x,
                0f, 1f, 0f, position.y,
                0f, 0f, 1f, position.z,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4f GetRotationMatrix()
        {
            Matrix3f mat = rotation.ToRotationMatrix();

            return new Matrix4f(
                mat.row0.X, mat.row0.Y, mat.row0.z, 0f,
                mat.row1.X, mat.row1.Y, mat.row1.z, 0f,
                mat.row2.X, mat.row2.Y, mat.row2.z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4f GetScaleMatrix()
        {
            return new Matrix4f(
                size.x, 0f, 0f, 0f,
                0f, size.y, 0f, 0f,
                0f, 0f, size.z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        public Matrix4f GetMatrix()
        {
            return GetTranslationMatrix() * GetRotationMatrix() * GetScaleMatrix();
        }


        public Transformf[] GetChilds()
        {
            return childs.ToArray();
        }


        public object Clone()
        {
            Transformf clone = new Transformf(position, rotation, size, parent);

            if (childs != null)
            {
                for (int i = 0; i < childs.Count; ++i)
                {
                    Transformf childClone = (Transformf)childs[i].Clone();
                    childClone.Parent = clone;
                }
            }

            return clone;
        }
    }
}
