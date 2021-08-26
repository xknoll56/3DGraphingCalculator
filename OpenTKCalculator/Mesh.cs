using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKCalculator
{
    enum MeshType
    {
        TEXTURED = 1,
        COLORED = 2,
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

        public Texture texture { get; set; }
        public int shaderType { get; }
        public bool indexed { get; }
        public int numVerts { get; }
        public MeshType meshType { get; }
        public RenderType renderType;
        public Vector3 color { get; set; }

        public Mesh(float[] vertices, MeshType meshType, RenderType renderType)
        {
            this.vertices = vertices;


            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int stride = 8, textureOffset = 6, normalOffset = 3;
            int typeNum = (int)meshType;

            switch (renderType)
            {
                case RenderType.TRIANGLES:


                    if (typeNum == 3)
                    {
                        stride = 8;
                    }
                    else if (typeNum == 2)
                    {
                        stride = 6;
                        textureOffset = 0;
                    }
                    else if (typeNum == 1)
                    {
                        stride = 8;
                        textureOffset = 6;
                    }

                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
                    GL.EnableVertexAttribArray(0);

                    GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), normalOffset * sizeof(float));
                    GL.EnableVertexAttribArray(2);

                    if ((typeNum & (int)MeshType.TEXTURED) != 0)
                    {
                        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), textureOffset * sizeof(float));
                        GL.EnableVertexAttribArray(1);
                    }
                    break;
                case RenderType.LINES:
                    stride = 3;
                    textureOffset = 0;
                    normalOffset = 0;
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
                    GL.EnableVertexAttribArray(0);
                    break;
            }

            this.meshType = meshType;
            this.renderType = renderType;
            shaderType = typeNum;
            indexed = false;

            if (renderType == RenderType.LINES)
                numVerts = vertices.Length / 2;
            else if (renderType == RenderType.TRIANGLES)
                numVerts = vertices.Length / 3;

            color = new Vector3(1, 1, 1);
            
        }

        public Mesh(float[] vertices, uint[] indices, MeshType meshType, RenderType renderType)
        {
            this.vertices = vertices;
            this.indices = indices;


            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            int stride = 8, textureOffset = 6, normalOffset = 3;
            int typeNum = (int)meshType;

            switch (renderType)
            {
                case RenderType.TRIANGLES:


                    if (typeNum == 3)
                    {
                        stride = 8;
                    }
                    else if (typeNum == 2)
                    {
                        stride = 6;
                        textureOffset = 0;
                    }
                    else if (typeNum == 1)
                    {
                        stride = 8;
                        textureOffset = 6;
                    }

                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
                    GL.EnableVertexAttribArray(0);

                    GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), normalOffset * sizeof(float));
                    GL.EnableVertexAttribArray(2);

                    if ((typeNum & (int)MeshType.TEXTURED) != 0)
                    {
                        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride * sizeof(float), textureOffset * sizeof(float));
                        GL.EnableVertexAttribArray(1);
                    }
                    break;
                case RenderType.LINES:
                    stride = 3;
                    textureOffset = 0;
                    normalOffset = 0;
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
                    GL.EnableVertexAttribArray(0);
                    break;
            }

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            this.meshType = meshType;
            this.renderType = renderType;
            shaderType = typeNum;
            indexed = true;

            if (renderType == RenderType.LINES)
                numVerts = indices.Length / 2;
            else if (renderType == RenderType.TRIANGLES)
                numVerts = indices.Length / 3;

            color = new Vector3(1, 1, 1);
        }

        public void OnDelete()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
        }

    }
}
