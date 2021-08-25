using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKCalculator
{
    enum MeshType
    {
        NONE = 0,
        TEXTURED = 1,
        COLORED = 2
    }
    enum RenderType
    {
        TRIANGLES = 0,
        LINES = 1,
        POINTS = 2
    }
    class Mesh
    {
        public float[] vertices { get; }
        public uint[] indices { get; }
        public int VertexBufferObject { get; }
        public int VertexArrayObject { get; }
        public int ElementBufferObject { get; }

        private List<Texture> textures;
        public int shaderType { get; }
        public bool indexed { get; }
        public int numVerts { get; }
        MeshType meshType;
        public RenderType renderType = RenderType.TRIANGLES;

        public Mesh(float[] vertices, MeshType meshType)
        {
            this.vertices = vertices;


            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int stride = 8, colorOffset = 3, textureOffset = 6;
            int typeNum = (int)meshType;
            if (typeNum == 3)
            {
                stride = 8;
            }
            else if (typeNum == 2)
            {
                stride = 6;
                colorOffset = 3;
                textureOffset = 0;
            }
            else if (typeNum == 1)
            {
                stride = 5;
                colorOffset = 0;
                textureOffset = 3;
            }
            else if(typeNum == 0)
            {
                stride = 3;
                colorOffset = 0;
                textureOffset = 0;
            }

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            if ((typeNum & (int)MeshType.COLORED) != 0)
            {
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), colorOffset * sizeof(float));
                GL.EnableVertexAttribArray(1);
            }
            if ((typeNum & (int)MeshType.TEXTURED) != 0)
            {
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), textureOffset * sizeof(float));
                GL.EnableVertexAttribArray(2);
            }

            textures = new List<Texture>();
            this.meshType = meshType;
            shaderType = typeNum;
            indexed = false;

            if (renderType == RenderType.LINES)
                numVerts = vertices.Length / 2;
            else if (renderType == RenderType.TRIANGLES)
                numVerts = vertices.Length / 3;
        }

        public Mesh(float[] vertices, uint[] indices, MeshType meshType)
        {
            this.vertices = vertices;
            this.indices = indices;


            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int stride = 8, colorOffset = 3, textureOffset = 6;
            int typeNum = (int)meshType;
            if (typeNum == 3)
            {
                stride = 8;
            }
            else if (typeNum == 2)
            {
                stride = 6;
                colorOffset = 3;
                textureOffset = 0;
            }
            else if (typeNum == 1)
            {
                stride = 5;
                colorOffset = 0;
                textureOffset = 3;
            }
            else if (typeNum == 0)
            {
                stride = 3;
                colorOffset = 0;
                textureOffset = 0;
            }

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            if ((typeNum & (int)MeshType.COLORED)!=0)
            {
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), colorOffset * sizeof(float));
                GL.EnableVertexAttribArray(1);
            }
            if ((typeNum & (int)MeshType.TEXTURED) != 0)
            {
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), textureOffset * sizeof(float));
                GL.EnableVertexAttribArray(2);
            }

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            textures = new List<Texture>();
            this.meshType = meshType;
            shaderType = typeNum;
            indexed = true;

            if (renderType == RenderType.LINES)
                numVerts = indices.Length / 2;
            else if (renderType == RenderType.TRIANGLES)
                numVerts = indices.Length / 3;
        }

        public void OnDelete()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
        }

        public List<Texture> GetTextures()
        {
            return textures;
        }

        public void AddTexture(Texture texture)
        {
            textures.Add(texture);
        }

    }
}
