using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace specular_lighting {

    class Shader {

        public int program, vertexShader, fragmentShader;

        public Shader(string vertexShaderPath, string fragmentShaderPath) {
            string vertexShaderSource = LoadShaderSource(vertexShaderPath),
                   fragmentShaderSource = LoadShaderSource(fragmentShaderPath);

            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(vertexShader);
            GL.CompileShader(fragmentShader);

            program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);
        }

        public void Use() {
            GL.UseProgram(program);
        }

        string LoadShaderSource(string path) {
            string source = null;;

            using (StreamReader r = new StreamReader(path, Encoding.UTF8)) {
                source = r.ReadToEnd();
            }

            return source;
        }
    }
}