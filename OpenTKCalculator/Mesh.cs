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
        public float[] normals { get; }
        public uint[] indices { get; }
        public int VertexBufferObject { get; }
        public int NormalBufferObject { get; }
        public int VertexArrayObject { get; }
        public int ElementBufferObject { get; }

        public Texture texture { get; set; }
        public int shaderType { get; }
        public bool indexed { get; }
        public int numVerts { get; }
        public MeshType meshType { get; }
        public RenderType renderType { get; }
        public BufferUsageHint bufferUsageHint { get; }
        public Vector3 color { get; set; }

        public bool calculateNormals { get; }

        public Mesh()
        {

        }
        public Mesh(float[] vertices, MeshType meshType, RenderType renderType, BufferUsageHint bufferUsageHint)
        {
            this.vertices = vertices;


            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, bufferUsageHint);

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
            this.bufferUsageHint = bufferUsageHint;
            shaderType = typeNum;
            indexed = false;

            if (renderType == RenderType.LINES)
                numVerts = vertices.Length / 2;
            else if (renderType == RenderType.TRIANGLES)
                numVerts = vertices.Length / 3;

            color = new Vector3(1, 1, 1);
            
        }

        public Mesh(float[] vertices, uint[] indices, MeshType meshType, RenderType renderType, BufferUsageHint bufferUsageHint, bool calculateNormals)
        {
            this.vertices = vertices;
            this.indices = indices;


            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, bufferUsageHint);
            int stride = 8, textureOffset = 6, normalOffset = 3;
            int typeNum = (int)meshType;

            switch (renderType)
            {
                case RenderType.TRIANGLES:

                    if (!calculateNormals)
                    {
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
                    }
                    else
                    {
                        stride = 3;

                        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
                        GL.EnableVertexAttribArray(0);

                        normals = new float[vertices.Length];
                        for(uint i = 0; i<indices.Length; i+=3)
                        {
                            uint index1 = indices[i], index2 = indices[i + 1], index3 = indices[i + 2];
                            Vector3 v1 = new Vector3(vertices[index1 * 3], vertices[index1 * 3 + 1], vertices[index1 * 3 + 2]);
                            Vector3 v2 = new Vector3(vertices[index2 * 3], vertices[index2 * 3 + 1], vertices[index2 * 3 + 2]);
                            Vector3 v3 = new Vector3(vertices[index3 * 3], vertices[index3 * 3 + 1], vertices[index3 * 3 + 2]);

                            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1);
                            normal.Normalize();
                            normals[index1 * 3] = normal.X;
                            normals[index1 * 3 + 1] = normal.Y;
                            normals[index1 * 3 + 2] = normal.Z;
                            normals[index2 * 3] = normal.X;
                            normals[index2 * 3 + 1] = normal.Y;
                            normals[index2 * 3 + 2] = normal.Z;
                            normals[index3 * 3] = normal.X;
                            normals[index3 * 3 + 1] = normal.Y;
                            normals[index3 * 3 + 2] = normal.Z;
                        }

                        NormalBufferObject = GL.GenBuffer();
                        GL.BindBuffer(BufferTarget.ArrayBuffer, NormalBufferObject);
                        GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * sizeof(float), normals, bufferUsageHint);

                        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride * sizeof(float), 0);
                        GL.EnableVertexAttribArray(2);
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
            this.bufferUsageHint = bufferUsageHint;
            this.calculateNormals = calculateNormals;
            shaderType = typeNum;
            indexed = true;

            if (renderType == RenderType.LINES)
                numVerts = indices.Length / 2;
            else if (renderType == RenderType.TRIANGLES)
                numVerts = indices.Length / 3;

            color = new Vector3(1, 1, 1);
        }

        public Mesh(Mesh mesh): this(mesh.vertices, mesh.indices, mesh.meshType, mesh.renderType, mesh.bufferUsageHint, mesh.calculateNormals)
        {

        }

        public void OnDelete()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
        }

    }
}
