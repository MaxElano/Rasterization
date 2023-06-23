using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Template
{
    public class Shader
    {
        // data members
        public int programID, vsID, fsID;
        public int in_vertexPositionObject;
        public int in_vertexNormalObject;
        public int in_vertexUV;
        public int uniform_objectToScreen;
        public int uniform_objectToWorld;
        public int uniform_cameraPosition;
        
        //Everything for the 4 lights (which could all be spotlights)
        public int uniform_lightPosition1;
        public int uniform_lightColor1;
        public int uniform_lightAngle1;
        public int uniform_lightShineAt1;

        public int uniform_lightPosition2;
        public int uniform_lightColor2;
        public int uniform_lightAngle2;
        public int uniform_lightShineAt2;

        public int uniform_lightPosition3;
        public int uniform_lightColor3;
        public int uniform_lightAngle3;
        public int uniform_lightShineAt3;

        public int uniform_lightPosition4;
        public int uniform_lightColor4;
        public int uniform_lightAngle4;
        public int uniform_lightShineAt4;



        // constructor
        public Shader(string vertexShader, string fragmentShader)
        {
            // compile shaders
            programID = GL.CreateProgram();
            GL.ObjectLabel(ObjectLabelIdentifier.Program, programID, -1, vertexShader + " + " + fragmentShader);
            Load(vertexShader, ShaderType.VertexShader, programID, out vsID);
            Load(fragmentShader, ShaderType.FragmentShader, programID, out fsID);
            GL.LinkProgram(programID);
            string infoLog = GL.GetProgramInfoLog(programID);
            if (infoLog.Length != 0) Console.WriteLine(infoLog);

            // get locations of shader parameters
            in_vertexPositionObject = GL.GetAttribLocation(programID, "vertexPositionObject");
            in_vertexNormalObject = GL.GetAttribLocation(programID, "vertexNormalObject");
            in_vertexUV = GL.GetAttribLocation(programID, "vertexUV");
            uniform_objectToScreen = GL.GetUniformLocation(programID, "objectToScreen");
            uniform_objectToWorld = GL.GetUniformLocation(programID, "objectToWorld");
            uniform_cameraPosition = GL.GetUniformLocation(programID, "cameraPosition");


            //Lighting stuff
            uniform_lightPosition1 = GL.GetUniformLocation(programID, "lightPosition1");
            uniform_lightColor1 = GL.GetUniformLocation(programID, "lightColor1");
            uniform_lightAngle1 = GL.GetUniformLocation(programID, "lightAngle1");
            uniform_lightShineAt1 = GL.GetUniformLocation(programID, "lightShineAt1");

            uniform_lightPosition2 = GL.GetUniformLocation(programID, "lightPosition2");
            uniform_lightColor2 = GL.GetUniformLocation(programID, "lightColor2");
            uniform_lightAngle2 = GL.GetUniformLocation(programID, "lightAngle2");
            uniform_lightShineAt2 = GL.GetUniformLocation(programID, "lightShineAt2");

            uniform_lightPosition3 = GL.GetUniformLocation(programID, "lightPosition3");
            uniform_lightColor3 = GL.GetUniformLocation(programID, "lightColor3");
            uniform_lightAngle3 = GL.GetUniformLocation(programID, "lightAngle3");
            uniform_lightShineAt3 = GL.GetUniformLocation(programID, "lightShineAt3");

            uniform_lightPosition4 = GL.GetUniformLocation(programID, "lightPosition4");
            uniform_lightColor4 = GL.GetUniformLocation(programID, "lightColor4");
            uniform_lightAngle4 = GL.GetUniformLocation(programID, "lightAngle4");
            uniform_lightShineAt4 = GL.GetUniformLocation(programID, "lightShineAt4");
        }

        // loading shaders
        void Load(String filename, ShaderType type, int program, out int ID)
        {
            // source: http://neokabuto.blogspot.nl/2013/03/opentk-tutorial-2-drawing-triangle.html
            ID = GL.CreateShader(type);
            GL.ObjectLabel(ObjectLabelIdentifier.Shader, ID, -1, filename);
            using (StreamReader sr = new StreamReader(filename)) GL.ShaderSource(ID, sr.ReadToEnd());
            GL.CompileShader(ID);
            GL.AttachShader(program, ID);
            string infoLog = GL.GetShaderInfoLog(ID);
            if (infoLog.Length != 0) Console.WriteLine(infoLog);
        }
    }
}
