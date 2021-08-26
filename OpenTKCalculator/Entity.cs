using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKCalculator
{
    class Entity
    {
        private Vector3 position;
        public Vector3 Position 
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                UpdateModel();
            }
        }
        private Vector3 scale;
        public Vector3 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                UpdateModel();
            }
        }
        private Quaternion rotation;
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                UpdateModel();
            }
        }
        public Vector4 color { get; set; }
        public Matrix4 model { get; private set; }
        private Matrix4 translationMatrix;
        private Matrix4 scaleMatrix;
        private Matrix4 rotationMatrix;

        public Mesh mesh { get; set; }

        public Entity(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            UpdateModel();
        }

        public Entity(Mesh mesh, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            this.mesh = mesh;
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            UpdateModel();
        }

        public void UpdateModel()
        {
            translationMatrix = Matrix4.CreateTranslation(position);
            scaleMatrix = Matrix4.CreateScale(scale);
            rotationMatrix = Matrix4.CreateFromQuaternion(rotation);
            model = scaleMatrix*rotationMatrix*translationMatrix;
        }

    }
}
