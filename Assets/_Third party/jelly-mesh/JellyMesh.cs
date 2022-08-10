using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyMesh : MonoBehaviour
{
    public float intensity = 1f;
    public float mass = 1f;
    public float stiffness = 1f;
    public float damping = 0.75f;

    Mesh clonedMesh;
    JellyVertex[] jellyVertices;
    Vector3[] modifiedVertices;
    int verticesCount;

    Vector3 boundsMax;
    Vector3 boundsSize;
 Vector3 boundsMaxLocal;
    void Start()
    {
        var renderer = GetComponent<MeshRenderer>();
        var meshFilter = GetComponent<MeshFilter>();

        //swap the original mesh with one that will be modified
        var originalMesh = meshFilter.sharedMesh;
        clonedMesh = Instantiate(originalMesh);
        meshFilter.sharedMesh = clonedMesh;

        //create jelly vertices
        verticesCount = clonedMesh.vertices.Length;
        modifiedVertices = new Vector3[verticesCount];
        jellyVertices = new JellyVertex[verticesCount];

        for (int i = 0; i < verticesCount; i++)
        {
            modifiedVertices[i] = clonedMesh.vertices[i];
            jellyVertices[i] = new JellyVertex(transform.TransformPoint(clonedMesh.vertices[i]), clonedMesh.vertices[i]);   //saves the coordinates in world space and in local space
        }

        //save the bounds
        boundsMax = renderer.bounds.max;
        boundsSize = renderer.bounds.size;

        boundsMaxLocal=transform.InverseTransformPoint(boundsMax);
    }


    void FixedUpdate()
    {
        for (int i = 0; i < verticesCount; i++)
        {
            modifiedVertices[i] = jellyVertices[i].originalPosition;    //originalPosition is a local space coordinate
            Vector3 target = transform.TransformPoint(modifiedVertices[i]); //transforming the same coordinate to world space, this is where the change in value is happening when moving the transform
           
            float intensity = (1-((boundsMaxLocal.y - modifiedVertices[i].y) / boundsSize.y)) * this.intensity;   //(boundsMax.y - target.y) = distance from the highest point in the model to the current vertex (IN THE WORLD SPACE!) 
                                                                                                //(boundsMax.y - target.y) / boundsSize.y = boundsSize.y is always bigger than boundsMax.y (the highest point), this value is between (-inf,+inf)
                                                                                                //(1 - (boundsMax.y - target.y) / boundsSize.y) = this value is 1 for the highest vertex in the model and 0 for the lowest vertex
/*             Debug.Log("boundsMaxLocal.y= "+boundsMaxLocal.y);     
            Debug.Log("modifiedVertices["+i+"].y= "+modifiedVertices[i].y);     
            Debug.Log("boundsSize.y= "+boundsSize.y);     
            Debug.Log("intensity= "+intensity);     */


            jellyVertices[i].Shake(target, mass, stiffness, damping);   //target- the vertex in world space 
            target = transform.InverseTransformPoint(jellyVertices[i].position); // assigning the newly calculated position of the vertex based on the transform offset transformed to local space
            modifiedVertices[i] = Vector3.Lerp(modifiedVertices[i], target, intensity); //lerping this vertex overtime through fixed update

        }
        clonedMesh.vertices = modifiedVertices;
    }
}
