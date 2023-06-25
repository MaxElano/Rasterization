using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Template;
using OpenTK.Mathematics;

namespace INFOGR2023TemplateP2
{
    //Your task is to add a new class SceneGraph, which stores a hierarchy of meshes.
    //The mesh class needs to be expanded a bit as well; each mesh should have a local transform.
    //The SceneGraph class should implement a Render method, which takes a camera matrix as input.
    //This method then renders all meshes in the hierarchy.To determine the final transform for each mesh,
    //matrix concatenation should be used to combine all matrices, starting with the camera matrix, all the way down to each individual mesh.
    //Task list for the scene graph:
    //1.Add a model matrix to the Mesh class.
    //2.Add the SceneGraph: a data structure for storing a tree-structured hierarchy of meshes, where the position of each mesh in the scene will also be affected by the model matrices of all its ancestors.
    //3.Add a Render method for the Scene Graph that recursively processes the nodes in the tree, while combining matrices so that each mesh is drawn using the correct combined matrix.
    //4.Call the Render method of the Scene Graph from the Game class, using a camera matrix that is updated based on user input.

    public class Node
    {
        internal Matrix4 objectToParent;
        Mesh mesh;
        internal List<Node> children;
        bool isWorldNode;
        internal Node parent;
        internal Node(Mesh mesh = null, bool isWorldNode = false)
        {
            children = new List<Node>();
            if (isWorldNode)
                this.objectToParent = Matrix4.Identity;
            else if (!(this is Light))
                this.objectToParent = mesh.modelMatrix;
            this.mesh = mesh;
            this.isWorldNode = isWorldNode;
        }
        
        internal void Render(Matrix4 worldToScreen, Matrix4 parentToWorld, Shader shader, List<Light> lights, Vector3 cameraPosition)
        {
            Matrix4 objectToWorld = objectToParent * parentToWorld;
            Matrix4 objectToScreen = objectToWorld * worldToScreen; //object to world and world to screen
            if(!isWorldNode && mesh != null && mesh.texture != null)
                mesh.Render(shader, objectToScreen, objectToWorld, mesh.texture, lights, cameraPosition);
            foreach (Node child in children)
                child.Render(worldToScreen, objectToWorld, shader, lights, cameraPosition);
        }

        internal void AddChild(Node child)
        {
            child.parent = this;
            children.Add(child);
        }
    }

    public class Light : Node
    {
        internal Vector3 originalLightColor;
        internal Vector3 color;
        internal Vector4 location;
        internal float angleInDegrees;
        internal Vector3 shineAtDirection;
        bool lightOn = true;
        internal Light(Vector3 color, Vector3 location, List<Light> list, Mesh mesh = null, bool isWorldNode = false) : base(mesh, isWorldNode) 
        {
            list.Add(this);
            this.originalLightColor = color;
            this.color = originalLightColor;
            this.location = new Vector4(location.X, location.Y, location.Z, 0);
            angleInDegrees = 180; //Full rotation, because you can go 180` both ways
            shineAtDirection = new Vector3(1, 1, 1);
            this.shineAtDirection.Normalize();
        }
        internal void SetLocation()
        {
            Matrix4 finalMatrix = objectToParent;
            Node tempNode = this;
            while (tempNode.parent != null)
            {
                finalMatrix = tempNode.parent.objectToParent * finalMatrix;
                tempNode = tempNode.parent;
            }
            location = finalMatrix * location;
        }
        internal void SwitchOnOff()
        {
            if (lightOn)
                color = new Vector3(0, 0, 0);
            else
                color = originalLightColor;
            
            lightOn = !lightOn;
        }
    }

    public class Spotlight : Light
    {
        internal Spotlight(Vector3 color, Vector3 location, float angleInDegrees, Vector3 shineAtDirection, List<Light> list, Mesh mesh = null, bool isWorldNode = false) : base(color, location, list, mesh, isWorldNode)
        {
            this.angleInDegrees = angleInDegrees;
            this.shineAtDirection = shineAtDirection;
            this.shineAtDirection.Normalize();
        }
    }
    //Scene Graph Lecture Tips:
    //1. Follow the convention name+from+to (ex. teapotObjectToWorld)
}
