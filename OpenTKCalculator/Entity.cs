using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKCalculator
{
    class Entity
    {
        public Vector3 position { get; set; }
        public Vector3 scale { get; set; }
        public Quaternion rotation { get; set; }
        public Matrix4 model { get; set; }
        private Matrix4 translationMatrix;
        private Matrix4 scaleMatrix;
        private Matrix4 rotationMatrix;

        public Mesh mesh { get; set; }

        public Entity(Vector3 position, Vector3 scale, Quaternion rotation)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            translationMatrix = Matrix4.CreateTranslation(position);
            scaleMatrix = Matrix4.CreateScale(scale);
            rotationMatrix = Matrix4.CreateFromQuaternion(rotation);
            model = translationMatrix * rotationMatrix * scaleMatrix;
        }

        public Entity(Mesh mesh, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            this.mesh = mesh;
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            translationMatrix = Matrix4.CreateTranslation(position);
            scaleMatrix = Matrix4.CreateScale(scale);
            rotationMatrix = Matrix4.CreateFromQuaternion(rotation);
            model = translationMatrix * rotationMatrix * scaleMatrix;
        }

        public void UpdateModel()
        {
            translationMatrix = Matrix4.CreateTranslation(position);
            scaleMatrix = Matrix4.CreateScale(scale);
            rotationMatrix = Matrix4.CreateFromQuaternion(rotation);
            model = translationMatrix * rotationMatrix * scaleMatrix;
        }

    }
}
