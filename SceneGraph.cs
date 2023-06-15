using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Template;

namespace INFOGR2023TemplateP2
{
    internal class SceneGraph
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
    }

    //internal void Render()
    //{
    //
    //}
}
