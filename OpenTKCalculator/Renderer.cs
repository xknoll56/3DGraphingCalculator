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
    class Renderer: IDisposable
    {
        private GLControl glControl;
        private Stopwatch stopwatch;
        private Shader shader;
        private Shader gridShader;
        private Camera mainCamera;
        private List<CalculationMesh> calculationMeshes;
        private List<Mesh> meshes;
        private List<Entity> parentEntities;
        private Matrix4 IDENTITY = Matrix4.Identity;
        public bool canControl { get; set; }
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
            shader = new Shader("Shaders/ModelShader.vert", "Shaders/ModelShader.frag");
            gridShader = new Shader("Shaders/GridShader.vert", "Shaders/GridShader.frag");

            mainCamera = new Camera(new Vector3(0, 2, 5), glControl.AspectRatio);
            mainCamera.Yaw = 10.0f;
            //mainCamera.Yaw = 0.23f;
            Matrix4 model = Matrix4.Identity;
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", mainCamera.GetViewMatrix());
            shader.SetMatrix4("projection", mainCamera.GetProjectionMatrix());
            gridShader.SetMatrix4("model", Matrix4.Identity);

            stopwatch = new Stopwatch();
            stopwatch.Start();
            calculationMeshes = new List<CalculationMesh>();
            meshes = new List<Mesh>();
            parentEntities = new List<Entity>();
            canControl = false;

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
            if (canControl)
            {
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
            }
            shader.SetMatrix4("view", mainCamera.GetViewMatrix());
            shader.SetMatrix4("projection", mainCamera.GetProjectionMatrix());
            gridShader.SetMatrix4("view", mainCamera.GetViewMatrix());
            gridShader.SetMatrix4("projection", mainCamera.GetProjectionMatrix());
            IGraphicsContext graphicsContext = GraphicsContext.CurrentContext;
            if (graphicsContext == null || !graphicsContext.IsCurrent)
                glControl.MakeCurrent();    // Tell OpenGL to draw on MyGLControl.
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (Mesh mesh in meshes)
            {
                shader.SetMatrix4("model", Matrix4.Identity);
                DrawMesh(shader, mesh);

            }
            foreach (CalculationMesh mesh in calculationMeshes)
            {
                shader.SetMatrix4("model", Matrix4.Identity);
                DrawMesh(shader, mesh);

                if(mesh.gridMesh != null)
                {
                    gridShader.SetVec3("color", new Vector3(1,0,0));
                    GL.UseProgram(gridShader.GetHandle());
                    GL.BindVertexArray(mesh.gridMesh.VertexArrayObject);
                    GL.DrawArrays(PrimitiveType.Lines, 0, mesh.gridMesh.numVerts);
                }

            }

            // parentEntities[0].Rotation = new Quaternion(rotationTest);
            parentEntities[0].Euler += new Vector3(0.25f * dt, dt, 0.5f * dt);
            foreach (Entity entity in parentEntities)
            {
                DrawEntity(ref IDENTITY, entity);
            }

            glControl.SwapBuffers();    // Display the result.
            glControl.Invalidate();
        }

        private void DrawEntity(ref Matrix4 parentTransform, Entity entity)
        {
            Matrix4 trans = entity.model*parentTransform;
            shader.SetVec3("color", entity.color);
            shader.SetMatrix4("model", trans);
            if (entity.mesh != null)
                DrawMesh(shader, entity.mesh, entity.color);
            foreach (Entity child in entity.children)
                DrawEntity(ref trans, child);
        }

        private void DrawMesh(Shader shader, Mesh mesh)
        {
            shader.SetVec3("color", mesh.color);
            if (mesh.renderType == RenderType.TRIANGLES)
            {
                shader.Use();
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
                    GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.numVerts);
                }
            }
            else if (mesh.renderType == RenderType.LINES)
            {
                //gridShader.Use();
                gridShader.SetVec3("color", mesh.color);
                GL.UseProgram(gridShader.GetHandle());
                GL.BindVertexArray(mesh.VertexArrayObject);
                GL.DrawArrays(PrimitiveType.Lines, 0, mesh.numVerts);
            }
        }


        private void DrawMesh(Shader shader, Mesh mesh, Vector3 color)
        {
            shader.SetVec3("color", color);
            if (mesh.renderType == RenderType.TRIANGLES)
            {
                shader.Use();
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
                    GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.numVerts);
                }
            }
            else if (mesh.renderType == RenderType.LINES)
            {
                gridShader.SetVec3("color", color);
                GL.UseProgram(gridShader.GetHandle());
                GL.BindVertexArray(mesh.VertexArrayObject);
                GL.DrawArrays(PrimitiveType.Lines, 0, mesh.numVerts);
            }
        }

        public void AddCalculationMesh(CalculationMesh mesh)
        {
            calculationMeshes.Add(mesh);
        }

        public void AddMesh(Mesh mesh)
        {
            meshes.Add(mesh);
        }

        public void AddEntity(Entity entity)
        {
            parentEntities.Add(entity);
        }



        public void Dispose()
        {
            shader.Dispose();
            foreach (Mesh mesh in calculationMeshes)
                mesh.Dispose();
        }
    }
}
