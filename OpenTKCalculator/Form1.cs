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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
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
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
            };
        int VertexBufferObject;
        int VertexArrayObject;
        Shader shader;
        private int ElementBufferObject;
        KeyboardState input;
        Camera mainCamera;
        bool[] keys = new bool[132];
        long time;
        Stopwatch stopwatch;

        public Form1()
        {
            InitializeComponent();
            input = Keyboard.GetState();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            // You can bind the events here or in the Designer.
            glControl1.Resize += MyGLControl_Resize;
            glControl1.Paint += MyGLControl_Paint;
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            // GL.FrontFace(FrontFaceDirection.Ccw);
            //GL.CullFace(CullFaceMode.Back);
            //GL.Hint(HintTarget.)
            Console.WriteLine(GL.GetString(StringName.Renderer));
            String version = GL.GetString(StringName.Version);
            Debug.Assert(version.Contains("4.0.0"));
            Console.WriteLine(GL.GetString(StringName.Vendor));



            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            //Load the image
            Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>("texture.png");

            //ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            //This will correct that, making the texture display properly.
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.
            var pixels = new List<byte>(4 * image.Width * image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                var row = image.GetPixelRowSpan(y);

                for (int x = 0; x < image.Width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.ActiveTexture(TextureUnit.Texture0); // activate the texture unit first before binding texture
            int texture = GL.GetUniformLocation(shader.GetHandle(), "texture0");
            GL.BindTexture(TextureTarget.Texture2D, texture);

            mainCamera = new Camera(new Vector3(0, 0, 5), glControl1.AspectRatio);
            mainCamera.Yaw = 10.0f;
            Console.WriteLine(mainCamera.Yaw);
            //mainCamera.Yaw = 0.23f;
            Matrix4 model = Matrix4.Identity;
            //Matrix4 view = Matrix4.LookAt(new Vector3(5, 2, -5), new Vector3(0, 0, 0), Vector3.UnitY);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI * 0.25f, (float)glControl1.Width / glControl1.Height, 0.1f, 100.0f);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", mainCamera.GetViewMatrix());
            shader.SetMatrix4("projection", mainCamera.GetProjectionMatrix());

            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            shader.Dispose();
        }
        


        private void MyGLControl_Resize(object? sender, EventArgs e)
        {
            IGraphicsContext graphicsContext = GraphicsContext.CurrentContext;
            if (graphicsContext == null || !graphicsContext.IsCurrent)
                glControl1.MakeCurrent();    // Tell OpenGL to use MyGLControl.

            // Update OpenGL on the new size of the control.
            GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);

            /*
                Usually you compute projection matrices here too, like this:

                float aspect_ratio = MyGLControl.ClientSize.Width / (float)MyGLControl.ClientSize.Height;
                Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);

                And then you load that into OpenGL with a call like GL.LoadMatrix() or GL.Uniform().
            */
        }

        private void MyGLControl_Paint(object? sender, PaintEventArgs e)
        {
            //input = Keyboard.GetState();
            //stopwatch.Stop();
            float dt = stopwatch.ElapsedMilliseconds / 1000.0f;
            stopwatch.Restart();
            //stopwatch.Start();

            if (keys[(int)Keys.A])
            {
                mainCamera.Yaw -= dt*45.0f;
            }
            if (keys[(int)Keys.D])
            {
                mainCamera.Yaw += dt*45.0f;
            }
            if (keys[(int)Keys.W])
            {
                mainCamera.Position += mainCamera.Front * dt*10.0f;
            }
            if (keys[(int)Keys.S])
            {
                mainCamera.Position -= mainCamera.Front * dt*10.0f;
            }
            shader.SetMatrix4("view", mainCamera.GetViewMatrix());
            shader.SetMatrix4("projection", mainCamera.GetProjectionMatrix());
            IGraphicsContext graphicsContext = GraphicsContext.CurrentContext;
            if(graphicsContext == null || !graphicsContext.IsCurrent)
                glControl1.MakeCurrent();    // Tell OpenGL to draw on MyGLControl.
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            // GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            glControl1.SwapBuffers();    // Display the result.
            glControl1.Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Application.Exit();
            }

            keys[e.KeyValue] = true;

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            keys[e.KeyValue] = false;

            base.OnKeyUp(e);
        }


    }
}
