using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKCalculator
{
    class CalculationMesh : Mesh
    {
        public float xStart { get; private set; }
        public float zStart { get; private set; }
        public float xEnd { get; private set; }
        public float zEnd { get; private set; }
        public string expression { get; private set; }

        public Mesh gridMesh = null;

        private static float gridLineOffset = 0.03f;

        public CalculationMesh(float[] vertices, uint[] indices, MeshType meshType, RenderType renderType, BufferUsageHint bufferUsageHint, bool calculateNormals) : base(vertices, indices, meshType, renderType, bufferUsageHint, calculateNormals)
        {
        }

        public static CalculationMesh GenerateCalculationMesh(float xStart, float xEnd, float zStart, float zEnd)
        {

            List<float> planeVerts = new List<float>();
            List<uint> planeIndices = new List<uint>();
            //float xStart = -5, xEnd = 5;
            //float zStart = -5, zEnd = 5;
            uint divisions = (uint)(0.5 * (Math.Abs(xEnd - xStart) * 10 + Math.Abs(zEnd - zStart) * 10));
            uint rows = 0, cols = 0;
            bool rowsSet = false;
            float dp = Math.Abs(xEnd - xStart) / divisions;
            float x = xStart, z = zStart;
            while (x < xEnd)
            {
                while (z < zEnd)
                {
                    planeVerts.Add(x);
                    planeVerts.Add(0);
                    planeVerts.Add(z);
                    if (!rowsSet)
                        rows++;
                    z += dp;
                }
                planeVerts.Add(x);
                planeVerts.Add(0);
                planeVerts.Add(zEnd);
                if (!rowsSet)
                    rows++;
                rowsSet = true;
                z = zStart;
                x += dp;
                cols++;
            }
            while (z < zEnd)
            {
                planeVerts.Add(xEnd);
                planeVerts.Add(0);
                planeVerts.Add(z);
                z += dp;
            }
            planeVerts.Add(xEnd);
            planeVerts.Add(0);
            planeVerts.Add(zEnd);
            cols++;

            for (uint col = 0; col < cols - 1; col++)
            {
                for (uint row = 0; row < rows - 1; row++)
                {
                    uint ind = rows * col + row;
                    uint nextLineInd = rows * col + row + rows;

                    planeIndices.Add(ind);
                    planeIndices.Add(ind + 1);
                    planeIndices.Add(nextLineInd + 1);

                    planeIndices.Add(ind);
                    planeIndices.Add(nextLineInd + 1);
                    planeIndices.Add(nextLineInd);
                }
            }

            CalculationMesh cMesh = new CalculationMesh(planeVerts.ToArray(), planeIndices.ToArray(), MeshType.COLORED, RenderType.TRIANGLES, BufferUsageHint.DynamicDraw, true);
            cMesh.xStart = xStart;
            cMesh.xEnd = xEnd;
            cMesh.zStart = zStart;
            cMesh.zEnd = zEnd;
            return cMesh;
        }


        public static CalculationMesh GenerateCalculationMeshWithGrid(float xStart, float xEnd, float zStart, float zEnd)
        {

            List<float> planeVerts = new List<float>();
            List<uint> planeIndices = new List<uint>();
            List<float> gridVerts = new List<float>();
            //float xStart = -5, xEnd = 5;
            //float zStart = -5, zEnd = 5;
            uint divisions = (uint)(0.5 * (Math.Abs(xEnd - xStart) * 10 + Math.Abs(zEnd - zStart) * 10));
            uint rows = 0, cols = 0;
            bool rowsSet = false;
            float dp = Math.Abs(xEnd - xStart) / divisions;
            float x = xStart, z = zStart;
            bool xBegin = true;
            bool zBegin = true;
            while (x < xEnd)
            {
                while (z < zEnd)
                {
                    planeVerts.Add(x);
                    planeVerts.Add(0);
                    planeVerts.Add(z);
                    if (!rowsSet)
                        rows++;
                    z += dp;
                }
                planeVerts.Add(x);
                planeVerts.Add(0);
                planeVerts.Add(zEnd);
                if (!rowsSet)
                    rows++;
                rowsSet = true;
                z = zStart;
                x += dp;
                cols++;
            }
            while (z < zEnd)
            {
                planeVerts.Add(xEnd);
                planeVerts.Add(0);
                planeVerts.Add(z);
                z += dp;
            }
            planeVerts.Add(xEnd);
            planeVerts.Add(0);
            planeVerts.Add(zEnd);
            cols++;

            for (uint col = 0; col < cols - 1; col++)
            {
                for (uint row = 0; row < rows - 1; row++)
                {
                    uint ind = rows * col + row;
                    uint nextLineInd = rows * col + row + rows;

                    planeIndices.Add(ind);
                    planeIndices.Add(ind + 1);
                    planeIndices.Add(nextLineInd + 1);

                    planeIndices.Add(ind);
                    planeIndices.Add(nextLineInd + 1);
                    planeIndices.Add(nextLineInd);
                }
            }


            for (uint row = 0; row < rows - 1; row++)
            {
                int ind = (int)row*3;

                gridVerts.Add(planeVerts[ind]);
                gridVerts.Add(planeVerts[ind + 1]+ gridLineOffset);
                gridVerts.Add(planeVerts[ind + 2]);

                ind = ((int)row + 1) * 3;

                gridVerts.Add(planeVerts[ind]);
                gridVerts.Add(planeVerts[ind + 1]+ gridLineOffset);
                gridVerts.Add(planeVerts[ind + 2]);
            }

            for (uint row = 0; row < rows - 1; row++)
            {
                int ind = (int)row * 3 + (int)(cols-1) * (int)(rows) * 3;

                gridVerts.Add(planeVerts[ind]);
                gridVerts.Add(planeVerts[ind + 1] + gridLineOffset);
                gridVerts.Add(planeVerts[ind + 2]);

                ind = ((int)row + 1) * 3 + (int)(cols-1)* (int)(rows) * 3;

                gridVerts.Add(planeVerts[ind]);
                gridVerts.Add(planeVerts[ind + 1] + gridLineOffset);
                gridVerts.Add(planeVerts[ind + 2]);
            }

            for (uint col = 0; col < cols - 1; col++)
            {
                int ind = (int)(rows * col) * 3;

                gridVerts.Add(planeVerts[ind]);
                gridVerts.Add(planeVerts[ind + 1] + gridLineOffset);
                gridVerts.Add(planeVerts[ind + 2]);

                ind = ((int)(rows * (col + 1))) * 3;

                gridVerts.Add(planeVerts[ind]);
                gridVerts.Add(planeVerts[ind + 1] + gridLineOffset);
                gridVerts.Add(planeVerts[ind + 2]);
            }

            for (uint col = 0; col < cols - 1; col++)
            {
                int ind = (int)(rows * col) * 3+(int)(rows-1)*3;

                gridVerts.Add(planeVerts[ind]);
                gridVerts.Add(planeVerts[ind + 1] + gridLineOffset);
                gridVerts.Add(planeVerts[ind + 2]);

                ind = ((int)(rows * (col + 1))) * 3 + (int)(rows - 1) * 3;

                gridVerts.Add(planeVerts[ind]);
                gridVerts.Add(planeVerts[ind + 1] + gridLineOffset);
                gridVerts.Add(planeVerts[ind + 2]);
            }

            CalculationMesh cMesh = new CalculationMesh(planeVerts.ToArray(), planeIndices.ToArray(), MeshType.COLORED, RenderType.TRIANGLES, BufferUsageHint.DynamicDraw, true);
            cMesh.xStart = xStart;
            cMesh.xEnd = xEnd;
            cMesh.zStart = zStart;
            cMesh.zEnd = zEnd;

            cMesh.gridMesh = new Mesh(gridVerts.ToArray(), MeshType.COLORED, RenderType.LINES, BufferUsageHint.DynamicDraw);
            return cMesh;
        }

        public static CalculationMesh[] GenerateCalculationMeshGrid(uint fidelity = 10, int xStart = -50, int zStart = -50, int xEnd = 50, int zEnd = 50)
        {


            CalculationMesh[] calculationMeshes = new CalculationMesh[(xEnd - xStart) * (zEnd - zStart)];
            uint index = 0;
            for (int xi = xStart; xi < xEnd; xi++)
            {
                for (int zi = zStart; zi < zEnd; zi++)
                {
                    calculationMeshes[index++] = GenerateCalculationMeshWithGrid(xi, xi + 1, zi, zi + 1);
                }
            }

            return calculationMeshes;
        }

        public static CalculationMesh GenerateCalculationMesh(float xStart, float xEnd, float zStart, float zEnd, Interpreter interpreter, string expression)
        {

            List<float> planeVerts = new List<float>();
            List<uint> planeIndices = new List<uint>();
            //float xStart = -5, xEnd = 5;
            //float zStart = -5, zEnd = 5;
            uint divisions = (uint)(0.5 * (Math.Abs(xEnd - xStart) * 10 + Math.Abs(zEnd - zStart) * 10));
            uint rows = 0, cols = 0;
            bool rowsSet = false;
            float dp = Math.Abs(xEnd - xStart) / divisions;
            float x = xStart, z = zStart;
            while (x < xEnd)
            {
                while (z < zEnd)
                {
                    planeVerts.Add(x);
                    planeVerts.Add((float)interpreter.EvaluateExpression(expression, x, z));
                    planeVerts.Add(z);
                    if (!rowsSet)
                        rows++;
                    z += dp;
                }
                planeVerts.Add(x);
                planeVerts.Add((float)interpreter.EvaluateExpression(expression, x, zEnd));
                planeVerts.Add(zEnd);
                if (!rowsSet)
                    rows++;
                rowsSet = true;
                z = zStart;
                x += dp;
                cols++;
            }
            while (z < zEnd)
            {
                planeVerts.Add(xEnd);
                planeVerts.Add((float)interpreter.EvaluateExpression(expression, xEnd, z));
                planeVerts.Add(z);
                z += dp;
            }
            planeVerts.Add(xEnd);
            planeVerts.Add((float)interpreter.EvaluateExpression(expression, xEnd, zEnd));
            planeVerts.Add(zEnd);
            cols++;

            for (uint col = 0; col < cols - 1; col++)
            {
                for (uint row = 0; row < rows - 1; row++)
                {
                    uint ind = rows * col + row;
                    uint nextLineInd = rows * col + row + rows;

                    planeIndices.Add(ind);
                    planeIndices.Add(ind + 1);
                    planeIndices.Add(nextLineInd + 1);

                    planeIndices.Add(ind);
                    planeIndices.Add(nextLineInd + 1);
                    planeIndices.Add(nextLineInd);
                }
            }

            CalculationMesh cMesh = new CalculationMesh(planeVerts.ToArray(), planeIndices.ToArray(), MeshType.COLORED, RenderType.TRIANGLES, BufferUsageHint.DynamicDraw, true);
            cMesh.xStart = xStart;
            cMesh.xEnd = xEnd;
            cMesh.zStart = zStart;
            cMesh.zEnd = zEnd;
            cMesh.expression = expression;
            return cMesh;

        }

        public void UpdateExpression(string expression, Interpreter interpreter)
        {
            for (uint ind = 0; ind < vertices.Length; ind += 3)
            {
                vertices[ind + 1] = (float)interpreter.EvaluateExpression(expression, vertices[ind], vertices[ind + 2]);
            }
            UpdateBuffers();
            if(gridMesh != null)
            {
                for (uint ind = 0; ind < gridMesh.vertices.Length; ind += 3)
                {
                    gridMesh.vertices[ind + 1] = (float)interpreter.EvaluateExpression(expression, gridMesh.vertices[ind], gridMesh.vertices[ind + 2]) + gridLineOffset;
                }
                gridMesh.UpdateBuffers(false);
            }
        }
    }
}
