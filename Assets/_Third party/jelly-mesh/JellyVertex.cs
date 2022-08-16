using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct JellyVertex
{
    public int index;
    public Vector3 originalPosition;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 force;

    public JellyVertex(int index, Vector3 position)
    {
        this.index = index;
        this.position = position;
        this.velocity = Vector3.zero;
        this.force = Vector3.zero;
        this.originalPosition = Vector3.zero;
    }

    public JellyVertex(Vector3 position, Vector3 originalPosition)  //position- world position of the original vector; originalPosition- local position of the original vector
    {
        this.index = 0;
        this.position = position;
        this.velocity = Vector3.zero;
        this.force = Vector3.zero;
        this.originalPosition = originalPosition;
    }

    public void Shake(Vector3 target, float mass, float stiffness, float dampen)//target- the originalPosition vector transformed to world space
    {
        force = (target - position) * stiffness;    //(target - position) = curPos of the vertex (world space) - prevPos of the vertex (world space) (assuming the position is different this return a value !=0)
        velocity = (velocity + force / mass) * dampen;  //some math
        position += velocity;   //the new position of the vertex (world space) based on the new transform offset (target)
        if ((velocity + force + force / mass).sqrMagnitude < 0.00001f)
        {
            position = target;
        }
    }
}