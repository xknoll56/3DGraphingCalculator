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
             0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,  // top right
             0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f   // top left
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
            -0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,

            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,

             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,

            -0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f, -0.5f,

            -0.5f,  0.5f, -0.5f,
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f
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
            //Mesh mesh = new Mesh(vertices, indices, MeshType.COLORED | MeshType.TEXTURED);
            //Mesh mesh = new Mesh(texVertices, indices, MeshType.TEXTURED);
            Mesh mesh = new Mesh(colVertices, indices, MeshType.COLORED);
            Texture texture = Texture.LoadFromFile("texture.png");
            mesh.AddTexture(texture);
            renderer.AddMesh(mesh);

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

            Mesh gridMesh = new Mesh(gridVerts.ToArray(), MeshType.NONE);
            gridMesh.renderType = RenderType.LINES;
            renderer.AddMesh(gridMesh);



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
