using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace specular_lighting
{
    class Cube {

        Shader shader;
        int vao, vbo;
        static int size = 10;

        Vector3 position = new Vector3(0,0,0);
        Camera camera;

        float[] vertices = {
            -0.5f*size, -0.5f*size, -0.5f*size,  0.0f,  0.0f, -1.0f,
             0.5f*size, -0.5f*size, -0.5f*size,  0.0f,  0.0f, -1.0f,
             0.5f*size,  0.5f*size, -0.5f*size,  0.0f,  0.0f, -1.0f,
             0.5f*size,  0.5f*size, -0.5f*size,  0.0f,  0.0f, -1.0f,
            -0.5f*size,  0.5f*size, -0.5f*size,  0.0f,  0.0f, -1.0f,
            -0.5f*size, -0.5f*size, -0.5f*size,  0.0f,  0.0f, -1.0f,

            -0.5f*size, -0.5f*size,  0.5f*size,  0.0f,  0.0f,  1.0f,
             0.5f*size, -0.5f*size,  0.5f*size,  0.0f,  0.0f,  1.0f,
             0.5f*size,  0.5f*size,  0.5f*size,  0.0f,  0.0f,  1.0f,
             0.5f*size,  0.5f*size,  0.5f*size,  0.0f,  0.0f,  1.0f,
            -0.5f*size,  0.5f*size,  0.5f*size,  0.0f,  0.0f,  1.0f,
            -0.5f*size, -0.5f*size,  0.5f*size,  0.0f,  0.0f,  1.0f,

            -0.5f*size,  0.5f*size,  0.5f*size, -1.0f,  0.0f,  0.0f,
            -0.5f*size,  0.5f*size, -0.5f*size, -1.0f,  0.0f,  0.0f,
            -0.5f*size, -0.5f*size, -0.5f*size, -1.0f,  0.0f,  0.0f,
            -0.5f*size, -0.5f*size, -0.5f*size, -1.0f,  0.0f,  0.0f,
            -0.5f*size, -0.5f*size,  0.5f*size, -1.0f,  0.0f,  0.0f,
            -0.5f*size,  0.5f*size,  0.5f*size, -1.0f,  0.0f,  0.0f,

             0.5f*size,  0.5f*size,  0.5f*size,  1.0f,  0.0f,  0.0f,
             0.5f*size,  0.5f*size, -0.5f*size,  1.0f,  0.0f,  0.0f,
             0.5f*size, -0.5f*size, -0.5f*size,  1.0f,  0.0f,  0.0f,
             0.5f*size, -0.5f*size, -0.5f*size,  1.0f,  0.0f,  0.0f,
             0.5f*size, -0.5f*size,  0.5f*size,  1.0f,  0.0f,  0.0f,
             0.5f*size,  0.5f*size,  0.5f*size,  1.0f,  0.0f,  0.0f,

            -0.5f*size, -0.5f*size, -0.5f*size,  0.0f, -1.0f,  0.0f,
             0.5f*size, -0.5f*size, -0.5f*size,  0.0f, -1.0f,  0.0f,
             0.5f*size, -0.5f*size,  0.5f*size,  0.0f, -1.0f,  0.0f,
             0.5f*size, -0.5f*size,  0.5f*size,  0.0f, -1.0f,  0.0f,
            -0.5f*size, -0.5f*size,  0.5f*size,  0.0f, -1.0f,  0.0f,
            -0.5f*size, -0.5f*size, -0.5f*size,  0.0f, -1.0f,  0.0f,

            -0.5f*size,  0.5f*size, -0.5f*size,  0.0f,  1.0f,  0.0f,
             0.5f*size,  0.5f*size, -0.5f*size,  0.0f,  1.0f,  0.0f,
             0.5f*size,  0.5f*size,  0.5f*size,  0.0f,  1.0f,  0.0f,
             0.5f*size,  0.5f*size,  0.5f*size,  0.0f,  1.0f,  0.0f,
            -0.5f*size,  0.5f*size,  0.5f*size,  0.0f,  1.0f,  0.0f,
            -0.5f*size,  0.5f*size, -0.5f*size,  0.0f,  1.0f,  0.0f
        };

        public Cube(Camera camera) {

            this.camera = camera;

            shader = new Shader("Shaders/vertexShader.glsl", "Shaders/fragmentShader.glsl");

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.VertexAttribPointer(
                0,
                3,
                VertexAttribPointerType.Float,
                false,
                6 * sizeof(float),
                0
            );

            GL.VertexAttribPointer(
                1,
                3,
                VertexAttribPointerType.Float,
                false,
                6 * sizeof(float),
                3 * sizeof(float)
            );

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            shader.Use();

            int modelUniform = GL.GetUniformLocation(shader.program, "model"),
                viewUniform = GL.GetUniformLocation(shader.program, "view"),
                projectionUniform = GL.GetUniformLocation(shader.program, "projection");

            Matrix4 worldMatrix = new Matrix4(),
                    viewMatrix =  new Matrix4(),
                    projMatrix =  new Matrix4();

            worldMatrix = Matrix4.Identity;
            viewMatrix = Matrix4.LookAt(camera.position, camera.position + camera.lookEye, camera.up);
            projMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80), 1000/720, 0.01f, 2000f);


            GL.UniformMatrix4(modelUniform, false, ref worldMatrix);
            GL.UniformMatrix4(viewUniform, false, ref viewMatrix);
            GL.UniformMatrix4(projectionUniform, false, ref projMatrix);
        }

        public void Render(float elapsedTime) {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0,0,0,1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();

            // ------------------------------------------------- //
            // MATRICES
            // ------------------------------------------------- //

            int modelUniform = GL.GetUniformLocation(shader.program, "model"),
                viewUniform = GL.GetUniformLocation(shader.program, "view"),
                projectionUniform = GL.GetUniformLocation(shader.program, "projection");

            Matrix4 worldMatrix = new Matrix4(),
                    viewMatrix =  new Matrix4(),
                    projMatrix =  new Matrix4();

            worldMatrix = Matrix4.Identity;
            viewMatrix = Matrix4.LookAt(camera.position, camera.position + camera.lookEye, camera.up);
            projMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(80), 1000/720, 0.01f, 2000f);

            GL.UniformMatrix4(modelUniform, false, ref worldMatrix);
            GL.UniformMatrix4(viewUniform, false, ref viewMatrix);
            GL.UniformMatrix4(projectionUniform, false, ref projMatrix);

            // ------------------------------------------------- //
            // LIGHT PROPERTIES
            // ------------------------------------------------- //

            int viewPosition =  GL.GetUniformLocation(shader.program, "cameraPosition"),
                objectColor =   GL.GetUniformLocation(shader.program, "color"),
                lightPosition = GL.GetUniformLocation(shader.program, "lightPosition"),
                lightColor =    GL.GetUniformLocation(shader.program, "lightColor");

            Vector3 color = new Vector3((float)System.Math.Sin(elapsedTime)/2 + .5f,1,0), light = new Vector3(1, 1, 1);
            System.Console.WriteLine(elapsedTime);

            GL.Uniform3(viewPosition, camera.position);
            GL.Uniform3(lightPosition, camera.position);
            GL.Uniform3(objectColor, color);
            GL.Uniform3(lightColor, light);

            // ------------------------------------------------- //
            // RENDERING
            // ------------------------------------------------- //

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(vao);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}