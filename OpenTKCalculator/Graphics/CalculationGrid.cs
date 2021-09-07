using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKCalculator
{
    class CalculationGrid
    {
        public CalculationMesh[] calcMeshes;
        public Entity[] calcEntity;
        public Entity parent;
        public Entity unitDirs;
        private string expression;
        private BufferUsageHint gridType;


        public CalculationGrid(uint fidelity = 10, int xStart = -10, int zStart = -10, int xEnd = 10, int zEnd = 10)
        {
            calcMeshes = CalculationMesh.GenerateCalculationMeshGrid(fidelity, xStart, zStart, xEnd, zEnd);
            parent = new Entity();
            calcEntity = new Entity[calcMeshes.Length*2];
            for(int i =0; i<calcMeshes.Length;i++)
            {
                calcEntity[i] = new Entity();
                calcEntity[i].mesh = calcMeshes[i];
                calcEntity[i + calcMeshes.Length] = new Entity();
                calcEntity[i + calcMeshes.Length].mesh = calcMeshes[i].gridMesh;
                calcEntity[i + calcMeshes.Length].color = new Vector3(1, 0, 0);
                parent.AddChild(calcEntity[i]);
                parent.AddChild(calcEntity[i + calcMeshes.Length]);
            }


            unitDirs = new Entity(new Vector3(), new Vector3(1, 1, 1), new Quaternion(new Vector3()));

            Mesh cyl = new Mesh(StaticVertices.cylinderVertices, MeshType.COLORED, RenderType.TRIANGLES, BufferUsageHint.StaticDraw, true);
            Entity entity = new Entity(new Vector3(0, 0, 0.5f), new Vector3(0.1f, 1, 0.1f), new Quaternion(new Vector3((float)Math.PI * 0.5f, 0, 0)));
            entity.mesh = cyl;
            entity.color = new Vector3(0, 0, 1);
            unitDirs.AddChild(entity);

            Entity entity2 = new Entity(new Vector3(0, 0.5f, 0), new Vector3(0.1f, 1, 0.1f), new Quaternion(new Vector3(0, 0, 0)));
            entity2.mesh = cyl;
            entity2.color = new Vector3(0, 1, 0);
            unitDirs.AddChild(entity2);

            Entity entity3 = new Entity(new Vector3(0.5f, 0, 0), new Vector3(0.1f, 1, 0.1f), new Quaternion(new Vector3(0, 0, (float)Math.PI * 0.5f)));
            entity3.mesh = cyl;
            entity3.color = new Vector3(1, 0, 0);
            unitDirs.AddChild(entity3);


            Mesh cone = new Mesh(StaticVertices.coneVertices, MeshType.COLORED, RenderType.TRIANGLES, BufferUsageHint.StaticDraw, true);

            Entity coneE1 = new Entity(new Vector3(0, 1.0f, 0), new Vector3(0.25f, 0.25f, 0.25f), new Quaternion(new Vector3()));
            coneE1.mesh = cone;
            coneE1.color = new Vector3(0, 1, 0);
            unitDirs.AddChild(coneE1);

            Entity coneE2 = new Entity(new Vector3(1.0f, 0, 0), new Vector3(0.25f, 0.25f, 0.25f), new Quaternion(new Vector3(0, 0, -(float)Math.PI * 0.5f)));
            coneE2.mesh = cone;
            coneE2.color = new Vector3(1, 0, 0);

            Entity coneE3 = new Entity(new Vector3(0, 0, 1.0f), new Vector3(0.25f, 0.25f, 0.25f), new Quaternion(new Vector3((float)Math.PI * 0.5f, 0, 0)));
            coneE3.mesh = cone;
            coneE3.color = new Vector3(0, 0, 1);
            unitDirs.AddChild(coneE1);
            unitDirs.AddChild(coneE2);
            unitDirs.AddChild(coneE3);
            parent.AddChild(unitDirs);
            Renderer.instance.AddEntity(parent);
            expression = "0";
        }

        public static CalculationGrid GenerateCalculationGrid(BufferUsageHint bufferUsageHint, int xStart = -10, int zStart = -10, int xEnd = 10, int zEnd = 10)
        {
            switch(bufferUsageHint)
            {
                case BufferUsageHint.DynamicDraw:
                    return new CalculationGrid(10, xStart, zStart, xEnd, zEnd);
                case BufferUsageHint.StreamDraw:
                    return new CalculationGrid(1, xStart, zStart, xEnd, zEnd);
            }
            throw new Exception();
        }

        public async void UpdateExpression(string expression, List<Token> tokens, Interpreter interpreter)
        {
            var watch = new Stopwatch();
            watch.Start();
            await Task.WhenAll(calcMeshes.Select(data => Task.Run(() => data.UpdateExpression(tokens))));
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            float z = interpreter.EvaluateExpression(tokens, 0, 0);
            unitDirs.Position = new Vector3(0, z, 0);
            for (int i = 0; i <calcMeshes.Length; i++)
            {
                calcMeshes[i].UpdateBuffers();
            }
            this.expression = expression;
        }

        public async void UpdateExpression(string expression, List<Token> tokens, Interpreter interpreter, double centroidX, double centroidZ)
        {
            var watch = new Stopwatch();
            watch.Start();
            await Task.WhenAll(calcMeshes.Select(data => Task.Run(() => data.UpdateExpression(tokens, centroidX, centroidZ))));
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            float z = interpreter.EvaluateExpression(tokens, centroidX, centroidZ);
            unitDirs.Position = new Vector3(0, z, 0);
            for (int i = 0; i < calcMeshes.Length; i++)
            {
                calcMeshes[i].UpdateBuffers();
            }
            this.expression = expression;
        }

        public Vector3 GetCentroidPosition()
        {
            return unitDirs.Position;
        }

        public override string ToString()
        {
            return expression;
        }
    }
}
