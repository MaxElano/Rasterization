using System.Diagnostics;
using INFOGR2023TemplateP2;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        Mesh? teapot, floor;                    // meshes to draw using OpenGL
        float a = 0;                            // teapot rotation angle
        readonly Stopwatch timer = new();       // timer for measuring frame duration
        Shader? shader;                         // shader to use for rendering
        Shader? postproc;                       // shader to use for post processing
        Texture? wood, brick, grass, sky;       // texture to use for rendering
        RenderTarget? target;                   // intermediate render target
        ScreenQuad? quad;                       // screen filling quad for post processing
        readonly bool useRenderTarget = true;   // required for post processing
        Node worldNode;
        Camera camera;
        GameWindow window;
        public List<Light> lights;
        readonly Stopwatch buttonTimer = new(); // timer for checking if a button was recently pressed
        int buttonPressInterval = 600;
        bool showHelp = true;
        int textInterval = 18;
        int distanceCounter = 1;
        Light selectedLight;
        float lightMoveSpeed = 1;
        float lightColorChangeSpeed = 0.01f;

        // constructor
        public MyApplication(Surface screen, OpenTKApp window)
        {
            //buttonTimer.Start();
            this.screen = screen;
            worldNode = new Node(null, true);
            camera = new Camera();
            this.window = window;
            lights= new List<Light>();
        }
        // initialize
        public void Init()
        {
            // load a texture
            wood = new Texture("../../../assets/wood.jpg");
            brick = new Texture("../../../assets/bricks.png");
            grass = new Texture("../../../assets/grass.png");
            sky = new Texture("../../../assets/wierd.png");


            // load teapot
            teapot = new Mesh("../../../assets/teapot.obj", Matrix4.CreateScale(0.5f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a), wood);
            floor = new Mesh("../../../assets/floor.obj", Matrix4.CreateScale(10.0f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a), wood);
            // initialize stopwatch
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../../shaders/vs.glsl", "../../../shaders/fs.glsl");
            postproc = new Shader("../../../shaders/vs_post.glsl", "../../../shaders/fs_post.glsl");
            
            // create the render target
            if (useRenderTarget) target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            worldNode.children.Add(new Node(teapot));
            worldNode.children.Add(new Node(floor));
            //worldNode.children.Add(new Light(new Vector3(2, 2, 2), new Vector3(5, 1, 5), lights));
            //worldNode.children.Add(new Light(new Vector3(2000, 0, 0), new Vector3(5, 1, -5), lights));
            //worldNode.children.Add(new Light(new Vector3(0, 2000, 0), new Vector3(-5, 1, 0), lights));
            //worldNode.children.Add(new Light(new Vector3(2000, 2000, 2000), new Vector3(5, 5, 0), lights));
            worldNode.children.Add(new Spotlight(new Vector3(2, 0, 0), new Vector3(-5, 20, 0), 10f, new Vector3(0, 1, 0), lights));
            worldNode.children.Add(new Spotlight(new Vector3(0, 2, 0), new Vector3(0, 20, 0), 10f, new Vector3(0, 1, 0), lights));
            worldNode.children.Add(new Spotlight(new Vector3(0, 0, 2), new Vector3(0, 20, 5), 10f, new Vector3(0, 1, 0), lights));
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);

            //Update Camera position and angle
            camera.Update(window);
            
            if (lights.Count > 0)
            {
                LightsUpdate();
            }
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            // prepare matrix for vertex shader

            //Matrix4 teapotObjectToWorld = Matrix4.CreateScale(0.5f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a); OLD
            //Matrix4 floorObjectToWorld = Matrix4.CreateScale(4.0f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);  OLD

            Matrix4 worldToCamera = Matrix4.CreateTranslation(camera.location) * (Matrix4.CreateFromAxisAngle(camera.Y, camera.Pitch) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), camera.Yaw));

            Matrix4 cameraToScreen = Matrix4.CreatePerspectiveFieldOfView(camera.FOV, (float)screen.width/screen.height, .1f, 1000);

            

            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * MathF.PI) a -= 2 * MathF.PI;

            if (useRenderTarget && target != null && quad != null)
            {
                // enable render target
                target.Bind();

                // render scene to render target
                if (shader != null /*&& wood != null*/)
                {
                    worldNode.Render(worldToCamera * cameraToScreen, Matrix4.Identity, shader, lights, camera.location);
                    //teapot?.Render(shader, teapotObjectToWorld * worldToCamera * cameraToScreen, teapotObjectToWorld, wood); OLD
                    //floor?.Render(shader, floorObjectToWorld * worldToCamera * cameraToScreen, floorObjectToWorld, wood); OLD
                }

                // render quad
                target.Unbind();
                if (postproc != null)
                    quad.Render(postproc, target.GetTextureID());
            }
            else
            {
                // render scene directly to the screen
                if (shader != null /*&& wood != null*/)
                {
                    worldNode.Render(worldToCamera * cameraToScreen, Matrix4.Identity, shader, lights, camera.location);
                    //teapot?.Render(shader, teapotObjectToWorld * worldToCamera * cameraToScreen, teapotObjectToWorld, wood); OLD
                    //floor?.Render(shader, floorObjectToWorld * worldToCamera * cameraToScreen, floorObjectToWorld, wood); OLD
                }
            }

        }

        internal void LightsUpdate()
        {
            buttonTimer.Stop();
            //Choosing different lights
            if (lights.Count > 0 && window.IsKeyDown(Keys.D1))
            {
                selectedLight = lights[0];
            }
            else if (lights.Count > 1 && window.IsKeyDown(Keys.D2))
            {
                selectedLight = lights[1];
            }
            else if (lights.Count > 2 && window.IsKeyDown(Keys.D3))
            {
                selectedLight = lights[2];
            }
            else if (lights.Count > 3 && window.IsKeyDown(Keys.D4))
            {
                selectedLight = lights[3];
            }

            //Changing aspects of the selected light
            if (selectedLight != null)
            {
                if (window.IsKeyDown(Keys.Space) && buttonTimer.ElapsedMilliseconds >= buttonPressInterval)
                {
                    selectedLight.SwitchOnOff();
                    buttonTimer.Reset();
                }
                else if (window.IsKeyDown(Keys.I)) //Forward (from starting)
                {
                    selectedLight.location.X -= lightMoveSpeed * camera.forwardVector.X;
                    selectedLight.location.Z -= lightMoveSpeed * camera.forwardVector.Z;
                }
                else if (window.IsKeyDown(Keys.K)) //Back (from starting)
                {
                    selectedLight.location.X += lightMoveSpeed * camera.forwardVector.X;
                    selectedLight.location.Z += lightMoveSpeed * camera.forwardVector.Z;
                }
                else if (window.IsKeyDown(Keys.O)) //Up
                {
                    selectedLight.location.Y += lightMoveSpeed;
                }
                else if (window.IsKeyDown(Keys.U)) //Down
                {
                    selectedLight.location.Y -= lightMoveSpeed;
                }
                else if (window.IsKeyDown(Keys.J)) //Left (from starting)
                {
                    selectedLight.location.X -= lightMoveSpeed * camera.leftVector.X;
                    selectedLight.location.Z -= lightMoveSpeed * camera.leftVector.Z;
                }
                else if (window.IsKeyDown(Keys.L)) //Right (from starting)
                {
                    selectedLight.location.X += lightMoveSpeed * camera.leftVector.X;
                    selectedLight.location.Z += lightMoveSpeed * camera.leftVector.Z;
                }
                else if (window.IsKeyDown(Keys.R)) //Red Brighter
                {
                    selectedLight.color.X += lightColorChangeSpeed;
                }
                else if (window.IsKeyDown(Keys.F)) //Red Weaker
                {
                    selectedLight.color.X -= lightColorChangeSpeed;
                }
                else if (window.IsKeyDown(Keys.T)) //Green Brighter
                {
                    selectedLight.color.Y += lightColorChangeSpeed;
                }
                else if (window.IsKeyDown(Keys.G)) //Green Weaker
                {
                    selectedLight.color.Y -= lightColorChangeSpeed;
                }
                else if (window.IsKeyDown(Keys.Y)) //Blue Brighter
                {
                    selectedLight.color.Z += lightColorChangeSpeed;
                }
                else if (window.IsKeyDown(Keys.H)) //Red Weaker
                {
                    selectedLight.color.Z -= lightColorChangeSpeed;
                }
            }
            buttonTimer.Start();
        }

        //Shows the controls on the side of the screen
        internal void ShowHelp()
        {
            distanceCounter = 1;
            screen.Print("Movement Controls", 2, distanceCounter * textInterval, 255 * 256 * 256 + 255 * 256 + 255);
            distanceCounter++;
            screen.Print("Move: W/A/S/D", 2, distanceCounter * textInterval, 255 * 256 * 256 + 255 * 256 + 255);
            distanceCounter++;
            screen.Print("Up: E", 2, distanceCounter * textInterval, 255 * 256 * 256 + 255 * 256 + 255);
            distanceCounter++;
            screen.Print("Down: Q", 2, distanceCounter * textInterval, 255 * 256 * 256 + 255 * 256 + 255);
            distanceCounter++;
            screen.Print("Look Around: Up/Left/Down/Right", 2, distanceCounter * textInterval, 255 * 256 * 256 + 255 * 256 + 255);
        }
    }
}