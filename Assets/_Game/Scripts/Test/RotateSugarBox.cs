using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSugarBox : MonoBehaviour
{
    public float zRotLimit=120f;

    public float rotateSpeed=10f;
    public float backwardsRotateSpeed=20f;

    private float speedMultiplier=10f;

    private Vector3 startingUpVector;
    // Start is called before the first frame update
    void Start()
    {
        startingUpVector=transform.up;

        rotateSpeed*=speedMultiplier;
        backwardsRotateSpeed*=speedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            if(Vector3.Angle(startingUpVector,transform.up)<120f){
                transform.rotation= Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,0,rotateSpeed) *Time.deltaTime);
            }
          
           // Debug.Log(transform.rotation.eulerAngles.z);
        }else{
            if(Vector3.Angle(startingUpVector,transform.up)>0.5f){
                 transform.rotation= Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0,0,backwardsRotateSpeed) *Time.deltaTime);
                 
            }else{
                transform.rotation= Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,0));
            }
        }
    }
}
