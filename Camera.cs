using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2023TemplateP2
{
    internal class Camera
    {
        Matrix4 modelMatrix;

        public Vector3 location;

        public Vector3 Y;

        float camSpeed = 1;

        // test yaw pithc and roll
        public float yaw = 0;
        public float pitch = 90;
        public float FieldOfView = 60;

        public float Yaw { get { return MathHelper.DegreesToRadians(yaw); } }
        public float Pitch { get { return MathHelper.DegreesToRadians(pitch); } }
        public float FOV { get { return MathHelper.DegreesToRadians(FieldOfView); } }


        internal Camera()
        {
            location = new Vector3(0, -14.5f, 0);
        }

        public void Update(GameWindow window)
        {
            Rotation(window);

            Movement(window);
        }

        public void Rotation(GameWindow window)
        {
            if (window.IsKeyDown(Keys.Left))
            {
                yaw -= 2;
                if (yaw < -180)
                    yaw = 180;
            }
            if (window.IsKeyDown(Keys.Right))
            {
                yaw += 2;
                if (yaw > 180)
                    yaw = -180;
            }
            if (window.IsKeyDown(Keys.Down))
            {
                pitch += 2;
                if (pitch > 360)
                    pitch = 0;
            }
            if (window.IsKeyDown(Keys.Up))
            {
                pitch -= 2;
                if (pitch < 0)
                    pitch = 360;
            }

            float Yx = 0;
            float Yz = 0;

            if (Math.Abs(yaw) <= 90)
            {
                Yx = 1 - ((Math.Abs(yaw)) / 90);
                Yz = ((Math.Abs(yaw)) / 90);
            }
            else if (Math.Abs(yaw) > 90)
            {
                Yx = 1 - ((Math.Abs(90 - (Math.Abs(yaw) - 90))) / 90);
                Yz = ((Math.Abs(90 - (Math.Abs(yaw) - 90))) / 90);
            }
            if (yaw < 0)
            {
                Yz = -Yz;
            }
            if (Math.Abs(yaw) > 90)
            {
                Yx = -Yx;
            }

            Y = new Vector3(Yx, 0, Yz);
        }

        public void Movement(GameWindow window)
        {
            if (window.IsKeyDown(Keys.A))
            {
                Vector3 M = new Vector3(camSpeed * (float)Math.Cos(Yaw), 0, camSpeed * (float)Math.Sin(Yaw));
                location += M;
            }
            if (window.IsKeyDown(Keys.D))
            {
                Vector3 M = new Vector3(camSpeed * (float)Math.Cos(Yaw), 0, camSpeed * (float)Math.Sin(Yaw));
                location -= M;
            }
            if (window.IsKeyDown(Keys.W))
            {
                Vector3 M = new Vector3(camSpeed * (float)Math.Sin(-Yaw), 0, camSpeed * (float)Math.Cos(-Yaw));
                location += M;
            }
            if (window.IsKeyDown(Keys.S))
            {
                Vector3 M = new Vector3(camSpeed * (float)Math.Sin(-Yaw), 0, camSpeed * (float)Math.Cos(-Yaw));
                location -= M;
            }
            if (window.IsKeyDown(Keys.Q))
            {
                location.Y += camSpeed;
            }
            if (window.IsKeyDown(Keys.E))
            {
                location.Y -= camSpeed;
            }
        }
    }
}
