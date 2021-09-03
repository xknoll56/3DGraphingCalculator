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
using System.Threading;

namespace OpenTKCalculator
{
    public partial class MainForm : Form
    {

        Renderer renderer;
        Interpreter interpreter;
        Tokenizer tokenizer;
        CalculationMesh[] dynMeshes;
        Entity unitDirs;
        Mutex evaluationMutex;
        public MainForm()
        {
            InitializeComponent();


        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            StaticVertices.SetVertices();
            renderer = new Renderer();
            interpreter = new Interpreter();
            tokenizer = new Tokenizer();
            evaluationMutex = new Mutex();
            renderer.Initialize(glControl1);


            List<float> gridVerts = new List<float>();
            int gridSize = 50;
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

            //dynMeshes = CalculationMesh.GenerateCalculationMeshGrid(10, -20, -20, 20, 20);
            //for (int i = 0; i < dynMeshes.Length; i++)
            //{
            //    renderer.AddCalculationMesh(dynMeshes[i]);
            //}



            Mesh mesh = new Mesh(StaticVertices.cylinderVertices, MeshType.COLORED, RenderType.TRIANGLES, BufferUsageHint.StaticDraw, true);

            unitDirs = new Entity(new Vector3(), new Vector3(1, 1, 1), new Quaternion(new Vector3()));

            Entity entity = new Entity(new Vector3(0, 0, 2.5f), new Vector3(1, 5, 1), new Quaternion(new Vector3((float)Math.PI * 0.5f, 0, 0)));
            entity.mesh = mesh;
            entity.color = new Vector3(0, 0, 1);
            unitDirs.AddChild(entity);

            Entity entity2 = new Entity(new Vector3(0, 2.5f, 0), new Vector3(1, 5, 1), new Quaternion(new Vector3(0, 0, 0)));
            entity2.mesh = mesh;
            entity2.color = new Vector3(0, 1, 0);
            unitDirs.AddChild(entity2);

            Entity entity3 = new Entity(new Vector3(2.5f, 0, 0), new Vector3(1, 5, 1), new Quaternion(new Vector3(0, 0, (float)Math.PI * 0.5f)));
            entity3.mesh = mesh;
            entity3.color = new Vector3(1, 0, 0);
            unitDirs.AddChild(entity3);

            renderer.AddEntity(unitDirs);

            //renderer.AddMesh(mesh);
        }

        private void GlControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            renderer.Dispose();
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
                renderer.canControl = true;
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
                renderer.canControl = false;
            }
            if (e.Button == MouseButtons.Right)
                Input.mouse[1] = false;
            if (e.Button == MouseButtons.Middle)
                Input.mouse[2] = false;
        }

        private async void expressionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                evaluationMutex.WaitOne();
                try
                {
                    tokenizer.TokenizeExpression(expressionTextBox.Text);
                    //try to interpret the expression to ensure no errors a had
                    interpreter.EvaluateExpression(tokenizer.Tokens);
                    var watch = new Stopwatch();
                    watch.Start();
                    await Task.WhenAll(dynMeshes.Select(data => Task.Run(() => data.UpdateExpression(tokenizer.Tokens))));
                    watch.Stop();
                    Console.WriteLine(watch.ElapsedMilliseconds);

                    float z = interpreter.EvaluateExpression(tokenizer.Tokens, 0, 0);
                    unitDirs.Position = new Vector3(0, z, 0);
                    for (int i = 0; i < dynMeshes.Length; i++)
                    {
                        dynMeshes[i].UpdateBuffers();
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                evaluationMutex.ReleaseMutex();
            }
        }
    }
}
