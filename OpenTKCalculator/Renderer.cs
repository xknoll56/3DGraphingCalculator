using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;


namespace OpenTKCalculator
{
    class Renderer
    {
        private GLControl glControl;
        private Stopwatch stopwatch;
        private Shader shader;
        private Camera mainCamera;
        private List<Mesh> meshes;
        private List<Entity> entities;
        Vector3 rotationTest = new Vector3();
        public bool Initialize(GLControl gLControl)
        {
            this.glControl = gLControl;

            // You can bind the events here or in the Designer.
            glControl.Resize += OnResize;
            glControl.Paint += OnPaint;
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.LineSmooth);
            GL.LineWidth(2.5f);
            //GL.Enable(EnableCap.CullFace);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //GL.Hint(HintTarget.)
            Console.WriteLine(GL.GetString(StringName.Renderer));
            String version = GL.GetString(StringName.ShadingLanguageVersion);
            Console.WriteLine(version);
            Console.WriteLine(GL.GetString(StringName.Vendor));




            //Initialize the shader
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();



            mainCamera = new Camera(new Vector3(0, 2, 5), glControl.AspectRatio);
            mainCamera.Yaw = 10.0f;
            //mainCamera.Yaw = 0.23f;
            Matrix4 model = Matrix4.Identity;
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", mainCamera.GetViewMatrix());
            shader.SetMatrix4("projection", mainCamera.GetProjectionMatrix());

            stopwatch = new Stopwatch();
            stopwatch.Start();
            meshes = new List<Mesh>();
            entities = new List<Entity>();
            shader.SetVec4("color", new Vector4(1, 0, 0, 1));

            return true;
        }



        private void OnResize(object? sender, EventArgs e)
        {
            IGraphicsContext graphicsContext = GraphicsContext.CurrentContext;
            if (graphicsContext == null || !graphicsContext.IsCurrent)
                glControl.MakeCurrent();    // Tell OpenGL to use MyGLControl.

            // Update OpenGL on the new size of the control.
            GL.Viewport(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);

            /*
                Usually you compute projection matrices here too, like this:

                float aspect_ratio = MyGLControl.ClientSize.Width / (float)MyGLControl.ClientSize.Height;
                Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);

                And then you load that into OpenGL with a call like GL.LoadMatrix() or GL.Uniform().
            */
        }

        private void OnPaint(object? sender, PaintEventArgs e)
        {
            //input = Keyboard.GetState();
            //stopwatch.Stop();
            float dt = stopwatch.ElapsedMilliseconds / 1000.0f;
            stopwatch.Restart();
            //stopwatch.Start();
            Input.mouseInput = Mouse.GetState();

            if (Input.keys[(int)Keys.A])
            {
                mainCamera.Position -= mainCamera.Right * dt * 10.0f;
            }
            if (Input.keys[(int)Keys.D])
            {
                mainCamera.Position += mainCamera.Right * dt * 10.0f;
            }
            if (Input.keys[(int)Keys.W])
            {
                mainCamera.Position += mainCamera.Front * dt * 10.0f;
            }
            if (Input.keys[(int)Keys.S])
            {
                mainCamera.Position -= mainCamera.Front * dt * 10.0f;
            }
            if (Input.mouse[0])
            {
                float dx = (Input.mouseInput.X - Input.prevMousePos.X);
                float dy = (Input.mouseInput.Y - Input.prevMousePos.Y);
                mainCamera.Yaw += dx * dt * 5.0f;
                mainCamera.Pitch -= dy * dt * 5.0f;
                Input.prevMousePos.X = Input.mouseInput.X;
                Input.prevMousePos.Y = Input.mouseInput.Y;

            }
            shader.SetMatrix4("view", mainCamera.GetViewMatrix());
            shader.SetMatrix4("projection", mainCamera.GetProjectionMatrix());
            IGraphicsContext graphicsContext = GraphicsContext.CurrentContext;
            if (graphicsContext == null || !graphicsContext.IsCurrent)
                glControl.MakeCurrent();    // Tell OpenGL to draw on MyGLControl.
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();

            foreach (Mesh mesh in meshes)
            {
                shader.SetMatrix4("model", Matrix4.Identity);
                shader.SetVec4("color", mesh.color);                
                if (mesh.renderType == RenderType.TRIANGLES)
                {
                    if (mesh.indexed)
                    {

                        shader.SetInt("type", mesh.shaderType);
                        if (mesh.meshType == MeshType.TEXTURED)
                            mesh.texture.Use(TextureUnit.Texture0);
                        GL.BindVertexArray(mesh.VertexArrayObject);
                        GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
                    }
                    else
                    {
                        shader.SetInt("type", mesh.shaderType);
                        if (mesh.meshType == MeshType.TEXTURED)
                            mesh.texture.Use(TextureUnit.Texture0);
                        GL.BindVertexArray(mesh.VertexArrayObject);
                        GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.vertices.Length / 3);
                    }
                }
                else if (mesh.renderType == RenderType.LINES)
                {
                    shader.SetInt("type", mesh.shaderType);
                    GL.BindVertexArray(mesh.VertexArrayObject);
                    GL.DrawArrays(PrimitiveType.Lines, 0, mesh.vertices.Length / 2);
                }
            }


            foreach (Entity entity in entities)
            {
                shader.SetMatrix4("model", entity.model);
                shader.SetVec4("color", entity.mesh.color);
                rotationTest.Y += dt;
                rotationTest.X += dt * 2;
                entity.Rotation = Quaternion.FromEulerAngles(rotationTest);
                if (entity.mesh.renderType == RenderType.TRIANGLES)
                {
                    if (entity.mesh.indexed)
                    {

                        shader.SetInt("type", entity.mesh.shaderType);
                        if(entity.mesh.meshType == MeshType.TEXTURED)
                            entity.mesh.texture.Use(TextureUnit.Texture0);
                        GL.BindVertexArray(entity.mesh.VertexArrayObject);
                        GL.DrawElements(PrimitiveType.Triangles, entity.mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
                    }
                    else
                    {
                        shader.SetInt("type", entity.mesh.shaderType);
                        if (entity.mesh.meshType == MeshType.TEXTURED)
                            entity.mesh.texture.Use(TextureUnit.Texture0);
                        GL.BindVertexArray(entity.mesh.VertexArrayObject);
                        GL.DrawArrays(PrimitiveType.Triangles, 0, entity.mesh.vertices.Length / 3);
                    }
                }
                else if (entity.mesh.renderType == RenderType.LINES)
                {
                    shader.SetInt("type", entity.mesh.shaderType);
                    GL.BindVertexArray(entity.mesh.VertexArrayObject);
                    GL.DrawArrays(PrimitiveType.Lines, 0, entity.mesh.vertices.Length / 2);
                }
            }

            glControl.SwapBuffers();    // Display the result.
            glControl.Invalidate();
        }

        public void AddMesh(Mesh mesh)
        {
            meshes.Add(mesh);
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void OnQuit()
        {
            shader.Dispose();
            foreach (Mesh mesh in meshes)
                mesh.OnDelete();
        }
    }
}
