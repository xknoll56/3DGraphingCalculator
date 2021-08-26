using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using System.Diagnostics;
using OpenTK.Graphics;

namespace OpenTKCalculator
{
    public partial class Form1 : Form
    {
        float[] vertices = {
             0.5f,  0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,  // top right
             0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f   // top left
            };
        float[] colVertices = {
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, 1.0f,  // top right
             0.5f, -0.5f, 0.0f, 1.0f, 1.0f, 1.0f,  // bottom right
            -0.5f, -0.5f, 0.0f, 1.0f, 1.0f, 1.0f,  // bottom left
            -0.5f,  0.5f, 0.0f, 1.0f, 1.0f, 1.0f,  // top left
            };
        float[] texVertices = {
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f,  // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f   // top left
            };
        uint[] indices = {  // note that we start from 0!
            0, 3, 1,   // first triangle
            1, 3, 2    // second triangle
            };

        float[] cubeVertices = {
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
             0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
             0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
             0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
             0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
             0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
             0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,-1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,-1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,-1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,-1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,-1.0f, 0.0f, 0.0f, 1.0f, 0.0f,

             0.5f,  0.5f,  0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
             0.5f,  0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
             0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
             0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
             0.5f, -0.5f,  0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
             0.5f,  0.5f,  0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,
             0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
             0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
             0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
             0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
             0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
             0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f
            };

        float[] cubeVerticesNoTexture = {
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 
             0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 
             0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 
             0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 
            -0.5f,  0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 
            -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 

            -0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 
             0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 
             0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 
             0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 
            -0.5f,  0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 
            -0.5f, -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 

            -0.5f,  0.5f,  0.5f, -1.0f, 0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,-1.0f, 0.0f, 0.0f, 
            -0.5f, -0.5f, -0.5f,-1.0f, 0.0f, 0.0f, 
            -0.5f, -0.5f, -0.5f,-1.0f, 0.0f, 0.0f, 
            -0.5f, -0.5f,  0.5f,-1.0f, 0.0f, 0.0f, 
            -0.5f,  0.5f,  0.5f,-1.0f, 0.0f, 0.0f, 

             0.5f,  0.5f,  0.5f, 1.0f, 0.0f, 0.0f, 
             0.5f,  0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 
             0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 
             0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 
             0.5f, -0.5f,  0.5f, 1.0f, 0.0f, 0.0f, 
             0.5f,  0.5f,  0.5f, 1.0f, 0.0f, 0.0f, 

            -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 
             0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 
             0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f, 
             0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f, 
            -0.5f, -0.5f,  0.5f, 0.0f, -1.0f, 0.0f, 
            -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 

            -0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
             0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
             0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
             0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            };

        Renderer renderer;

        public Form1()
        {
            InitializeComponent();


        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            renderer = new Renderer();
            renderer.Initialize(glControl1);

            Mesh cubeMesh = new Mesh(cubeVerticesNoTexture, MeshType.COLORED, RenderType.TRIANGLES, BufferUsageHint.StaticDraw);
            Entity entity = new Entity(cubeMesh, new Vector3(3, 1, 0), new Vector3(1, 1, 1), new Quaternion(new Vector3()));
            Texture texture = Texture.LoadFromFile("texture.png");
            entity.mesh.texture = texture;
            renderer.AddEntity(entity);


            List<float> gridVerts = new List<float>();
            int gridSize = 5;
            for(int i = -gridSize; i<= gridSize; i++)
            {
                gridVerts.Add(i);
                gridVerts.Add(0);
                gridVerts.Add(-gridSize);

                gridVerts.Add(i);
                gridVerts.Add(0);
                gridVerts.Add(gridSize);

                gridVerts.Add(-gridSize);
                gridVerts.Add(0);
                gridVerts.Add(i);

                gridVerts.Add(gridSize);
                gridVerts.Add(0);
                gridVerts.Add(i);
            }

            Mesh gridMesh = new Mesh(gridVerts.ToArray(), MeshType.COLORED, RenderType.LINES, BufferUsageHint.StaticDraw);
            gridMesh.color = new Vector3(1, 0, 0);
            renderer.AddMesh(gridMesh);

            List<float> planeVerts = new List<float>();
            List<uint> planeIndices = new List<uint>();
            float xStart = -5, xEnd = 5;
            float zStart = -5, zEnd = 5;
            uint divisions = 100;
            uint rows = 0, cols = 0;
            bool rowsSet = false;
            float dp = Math.Abs(xEnd - xStart) / divisions;
            float x = xStart, z = zStart;
            while(x < xEnd)
            {
                while(z < zEnd)
                {
                    planeVerts.Add(x);
                    planeVerts.Add(0.1f*x*z);
                    planeVerts.Add(z);
                    if(!rowsSet)
                        rows++;
                    z += dp;
                }
                planeVerts.Add(x);
                planeVerts.Add(0.1f * x * zEnd);
                planeVerts.Add(zEnd);
                if(!rowsSet)
                    rows++;
                rowsSet = true;
                z = zStart;
                x += dp;
                cols++;
            }
            while (z < zEnd)
            {
                planeVerts.Add(xEnd);
                planeVerts.Add(0.1f * xEnd * z);
                planeVerts.Add(z);
                z += dp;
            }
            planeVerts.Add(xEnd);
            planeVerts.Add(0.1f * xEnd * zEnd);
            planeVerts.Add(zEnd);
            cols++;

            for (uint row = 0; row<rows-1; row++)
            {
                for(uint col = 0; col<cols-1; col++)
                {
                    uint ind = row * cols + col;
                    uint nextLineInd = row * cols + col + rows;

                    planeIndices.Add(ind);
                    planeIndices.Add(ind+1);
                    planeIndices.Add(nextLineInd+1);

                    planeIndices.Add(ind);
                    planeIndices.Add(nextLineInd + 1);
                    planeIndices.Add(nextLineInd);
                }
            }

            Mesh dynMesh = new Mesh(planeVerts.ToArray(), planeIndices.ToArray(), MeshType.COLORED, RenderType.TRIANGLES, BufferUsageHint.StaticDraw, true);
            renderer.AddMesh(dynMesh);
        }

        private void GlControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            renderer.OnQuit();
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            Input.keyboardInput = Keyboard.GetState();
           

            if (Input.keyboardInput.IsKeyDown(Key.Escape))
            {
                Application.Exit();
            }

            Input.keys[e.KeyValue] = true;

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Input.keys[e.KeyValue] = false;

            base.OnKeyUp(e);
        }

        private void glControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Input.mouseInput = Mouse.GetState();
            if (e.Button == MouseButtons.Left)
            {
                Input.mouse[0] = true;
                Input.prevMousePos.X = Input.mouseInput.X;
                Input.prevMousePos.Y = Input.mouseInput.Y;
                Cursor.Hide();
                Input.initMousePos = Cursor.Position;
            }
            if (e.Button == MouseButtons.Right)
                Input.mouse[1] = true;
            if (e.Button == MouseButtons.Middle)
                Input.mouse[2] = true;
        }

        private void glControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Input.mouse[0] = false;
                Cursor.Show();
                Cursor.Position = Input.initMousePos;
            }
            if (e.Button == MouseButtons.Right)
                Input.mouse[1] = false;
            if (e.Button == MouseButtons.Middle)
                Input.mouse[2] = false;
        }
    }
}
