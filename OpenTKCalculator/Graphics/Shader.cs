using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenTKCalculator
{
    class Shader: IDisposable
    {
        int handle;
        private readonly Dictionary<string, int> uniformLocations;

        public Shader(string vertexPath, string fragmentPath)
        {
            string VertexShaderSource;

            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            string FragmentShaderSource;

            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                FragmentShaderSource = reader.ReadToEnd();
            }

            int VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            int status;
            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out status);
            if(status ==0)
                if (infoLogVert != System.String.Empty)
                    System.Console.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);


            handle = GL.CreateProgram();

            GL.AttachShader(handle, VertexShader);
            GL.AttachShader(handle, FragmentShader);

            GL.LinkProgram(handle);

            GL.DetachShader(handle, VertexShader);
            GL.DetachShader(handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

            GL.UseProgram(handle);

            // First, we have to get the number of active uniforms in the shader.
            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            // Next, allocate the dictionary to hold the locations.
            uniformLocations = new Dictionary<string, int>();

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(handle, i, out _, out _);

                // get the location,
                var location = GL.GetUniformLocation(handle, key);

                // and then add it to the dictionary.
                uniformLocations.Add(key, location);
            }
        }

        public void Use()
        {
            GL.UseProgram(handle);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(handle);
            GL.UniformMatrix4(uniformLocations[name], false, ref data);
        }

        public void SetVec4(string name, Vector4 data)
        {
            GL.UseProgram(handle);
            GL.Uniform4(uniformLocations[name], ref data);
        }

        public void SetVec3(string name, Vector3 data)
        {
            GL.UseProgram(handle);
            GL.Uniform3(uniformLocations[name], ref data);
        }

        public void SetFloat(string name, float data)
        {
            GL.UseProgram(handle);
            GL.Uniform1(uniformLocations[name], data);
        }

        public void SetInt(string name, int data)
        {
            GL.UseProgram(handle);
            GL.Uniform1(uniformLocations[name], data);
        }

        public int GetHandle()
        {
            return handle;
        }
    }
}
