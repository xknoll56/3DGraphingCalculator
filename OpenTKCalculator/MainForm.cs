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
    public partial class MainForm : Form
    {

        Renderer renderer;
        //Tokenizer tokenizer;
        Interpreter interpreter;
        CalculationMesh dynMesh;

        public MainForm()
        {
            InitializeComponent();


        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            renderer = new Renderer();
            interpreter = new Interpreter();
            renderer.Initialize(glControl1);


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

            dynMesh = CalculationMesh.GenerateCalculationMesh(-5, 5, -5, 5, interpreter, "0.05*x*x + 0.05*y*y");
            //renderer.AddMesh(dynMesh);
            Entity dynMeshEntity = new Entity(new Vector3(0, 0, 0), new Vector3(5, 0, 5), new Quaternion(new Vector3(0.35f, 0, 0)));
            dynMeshEntity.mesh = dynMesh;
            dynMesh.color = new Vector3(0, 1, 1);
            // renderer.AddEntity(dynMeshEntity);
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

        private void expressionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                //tokenizer.TokenizeExpression(expressionTextBox.Text);
                //tokenizer.PrintTokens();
                //double res = interpreter.EvaluateExpression(expressionTextBox.Text, 5, 0);
                //expressionTextBox.Text = res.ToString();
                dynMesh.UpdateExpression(expressionTextBox.Text, interpreter);
            }
        }
    }
}
