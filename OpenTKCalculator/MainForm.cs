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
        Interpreter interpreter;
        Tokenizer tokenizer;
        //CalculationMesh[] dynMeshes;
        CalculationGrid grid;
        Mutex evaluationMutex;
        Stopwatch stopwatch;
        private bool appRunning;

        public MainForm()
        {
            InitializeComponent();


        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            appRunning = true;
            Renderer.instance = new Renderer();
            interpreter = new Interpreter();
            tokenizer = new Tokenizer();
            evaluationMutex = new Mutex();
            stopwatch = new Stopwatch();
            Renderer.instance.Initialize(glControl1);
            grid = CalculationGrid.GenerateCalculationGrid(BufferUsageHint.DynamicDraw, -5, -5, 5, 5);
            listBox1.Items.Add(grid);

            numericUpDownRotX.Scroll += NumericUpDownRotX_Scroll;
        }

        private void NumericUpDownRotX_Scroll(object sender, ScrollEventArgs e)
        {
            Console.WriteLine("Hello");
        }

        private void GlControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnFormClosing(System.Windows.Forms.FormClosingEventArgs e)
        {
            appRunning = false;
            base.OnClosing(e);
            Renderer.instance.Dispose();
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
                Renderer.instance.canControl = true;
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
                Renderer.instance.canControl = false;
            }
            if (e.Button == MouseButtons.Right)
                Input.mouse[1] = false;
            if (e.Button == MouseButtons.Middle)
                Input.mouse[2] = false;
        }

        private async void expressionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEvaluate_Click(sender, e);
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            float s = 10.0f * (float)(hScrollScale.Value) / hScrollScale.Maximum;
            grid.parent.Scale = new Vector3(s, s, s);
            this.Invalidate();
        }

        private void numericUpDownRotX_ValueChanged(object sender, EventArgs e)
        {
            grid.parent.Euler = new Vector3((float)numericUpDownRotX.Value, grid.parent.Euler.Y, grid.parent.Euler.Z);
        }

        private void numericUpDownRotY_ValueChanged(object sender, EventArgs e)
        {
            grid.parent.Euler = new Vector3(grid.parent.Euler.X, (float)numericUpDownRotY.Value, grid.parent.Euler.Z);
        }

        private void numericUpDownRotZ_ValueChanged(object sender, EventArgs e)
        {
            grid.parent.Euler = new Vector3(grid.parent.Euler.X, grid.parent.Euler.Y, (float)numericUpDownRotZ.Value);
        }

        private void numericUpDownRotX_DragOver(object sender, DragEventArgs e)
        {
            Console.WriteLine("drag");
        }

        private async void buttonEvaluate_Click(object sender, EventArgs e)
        {
            evaluationMutex.WaitOne();
            try
            {
                tokenizer.TokenizeExpression(expressionTextBox.Text);
                //try to interpret the expression to ensure no errors a had
                interpreter.EvaluateExpression(tokenizer.Tokens);
                grid.UpdateExpression(expressionTextBox.Text, tokenizer.Tokens, interpreter, (double)numericUpDownCentroidX.Value, (double)numericUpDownCentroidZ.Value);
                listBox1.SelectedItem = grid;
                Camera.instance.Position = new Vector3(Camera.instance.Position.X, grid.GetCentroidPosition().Y, Camera.instance.Position.Z);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            evaluationMutex.ReleaseMutex();
        }

        private void buttonResetCamera_Click(object sender, EventArgs e)
        {
            Camera.instance.Position = new Vector3(0, 2, 5);
            Camera.instance.Yaw = 0.0f;
            Camera.instance.Pitch = 0.0f;
        }
    }
}
