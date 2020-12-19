using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

using System;

namespace specular_lighting {
    class Camera {

        float speed = 2;
        float xRotation, yRotation;
        public Vector3 position = new Vector3(0,10,10),
                       lookEye = new Vector3(0,0, -1),
                       up = new Vector3(0,1,0);

        GameScene game;
        Vector2 lastPosition;
        bool canRotate = false;

        public Camera(GameScene game) {
            this.game = game;
            game.UpdateFrame += Update;

            // mouse
            game.MouseMove += MouseMove;
            game.MouseDown += MouseDown;
            game.MouseUp   += MouseUp;
        }

        void Update(FrameEventArgs e) {

            xRotation = Clamp(xRotation, -89.9f, 89.9f);
            lookEye.X = (float)Math.Cos(MathHelper.DegreesToRadians(xRotation)) * (float)Math.Cos(MathHelper.DegreesToRadians(yRotation));
            lookEye.Y = (float)Math.Sin(MathHelper.DegreesToRadians(xRotation));
            lookEye.Z = (float)Math.Cos(MathHelper.DegreesToRadians(xRotation)) * (float)Math.Sin(MathHelper.DegreesToRadians(yRotation));

            lookEye = Vector3.Normalize(lookEye);

            if (game.IsKeyDown(Keys.W)) position += lookEye * speed;
            else if (game.IsKeyDown(Keys.S)) position -= lookEye * speed;

            Console.WriteLine(position.X + " " + position.Y + " " + position.Z);

            Vector3 right = Vector3.Normalize(Vector3.Cross(lookEye, up));

            if (game.IsKeyDown(Keys.A)) position -= right * speed;
            if (game.IsKeyDown(Keys.D)) position += right * speed;
        }

        void MouseMove(MouseMoveEventArgs e) {
            if (canRotate)
            {
                xRotation += (lastPosition.Y - e.Y) * .5f;
                yRotation -= (lastPosition.X - e.X) * .5f;
            }
            lastPosition = new Vector2(e.X, e.Y);
        }

        void MouseDown(MouseButtonEventArgs e) {
            if (e.Button == MouseButton.Right) canRotate = true;
        }
        void MouseUp(MouseButtonEventArgs e) {
            if (e.Button == MouseButton.Right) canRotate = false;
        }

        float Clamp(float value, float min, float max) {

            if (value > max) value = max;
            if (value < min) value = min;
            return value;
        }
    }
}