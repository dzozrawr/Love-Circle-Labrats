using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddForce : MonoBehaviour
{
    private Rigidbody rb;

    public Transform forceVector=null;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        rb.AddForce(forceVector.right*10f,ForceMode.VelocityChange);
    }


}
