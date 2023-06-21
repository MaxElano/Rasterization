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

        public Vector3 X;
        public Vector3 Y;
        public Vector3 Z;

        // test yaw pithc and roll
        float yaw = 90;
        float pitch = 45;
        float roll = 0;
        float FieldOfView = 60;

        public float Yaw { get { return MathHelper.DegreesToRadians(yaw); } }
        public float Pitch { get { return MathHelper.DegreesToRadians(pitch); } }
        public float Roll { get { return MathHelper.DegreesToRadians(roll); } }
        public float FOV { get { return MathHelper.DegreesToRadians(FieldOfView); } }


        internal Camera()
        {
            X = new Vector3(1, 0, 0);
            Y = new Vector3(0, 1, 0);
            Z = new Vector3(0, 0, 1);
        }

        public void Update(GameWindow window)
        {
            float oldyaw = yaw;
            float oldpitch = pitch;
            float oldroll = roll;

            if(window.IsKeyDown(Keys.A))
            {
                yaw -= 2;
                if (yaw < 0)
                    yaw = 360;
            }
            if (window.IsKeyDown(Keys.D))
            {
                yaw += 2;
                if (yaw > 360)
                    yaw = 0;
            }
            if (window.IsKeyDown(Keys.S))
            {
                pitch += 2;
                if (pitch > 360)
                    pitch = 0;
            }
            if (window.IsKeyDown(Keys.W))
            {
                pitch -= 2;
                if (pitch < 0)
                    pitch = 360;
            }
            if (window.IsKeyDown(Keys.Q))
            {
                roll -= 2;
                if (roll < 0)
                    roll = 360;
            }
            if (window.IsKeyDown(Keys.E))
            {
                roll += 2;
                if (roll > 360)
                    roll = 0;
            }

            if(oldyaw != yaw) 
            {
                oldyaw = yaw;
                Quaternion Q = new Quaternion(yaw, 0, 0);
                Y = Vector3.Transform(Y, Q);
                Q = new Quaternion(0, pitch, 0);
                Y = Vector3.Transform(Y, Q);
            }
        }

    }
}
