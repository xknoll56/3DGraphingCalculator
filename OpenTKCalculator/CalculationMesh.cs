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
        public CalculationMesh(float[] vertices, uint[] indices, MeshType meshType, RenderType renderType, BufferUsageHint bufferUsageHint, bool calculateNormals) : base(vertices, indices, meshType, renderType, bufferUsageHint, calculateNormals)
        {
        }

        public static CalculationMesh GenerateCalculationMesh(float xStart, float xEnd, float zStart, float zEnd) 
        {

            List<float> planeVerts = new List<float>();
            List<uint> planeIndices = new List<uint>();
            //float xStart = -5, xEnd = 5;
            //float zStart = -5, zEnd = 5;
            uint divisions = (uint)(0.5*(Math.Abs(xEnd - xStart)*10+ Math.Abs(zEnd - zStart) * 10));
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
    }
}
