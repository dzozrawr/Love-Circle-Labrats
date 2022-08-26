using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Spill : MonoBehaviour
{
    ParticleSystem particleSystem;
    public float startAngle = 120f;
    public bool isSpilling=false;
    public GameObject sugarPile=null;
    public SugarProgressBar sugarProgressBar = null;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector3.Angle(Vector3.down, transform.forward) <= startAngle))
        {
            particleSystem.Play();
            isSpilling=true;
/*             sugarPile.transform.localScale=new Vector3(sugarPile.transform.localScale.x>1?1:(sugarPile.transform.localScale.x+Time.deltaTime/4),sugarPile.transform.localScale.y>1?1:(sugarPile.transform.localScale.y+Time.deltaTime/4),sugarPile.transform.localScale.z>1?1:(sugarPile.transform.localScale.z+Time.deltaTime/4));
            sugarProgressBar.SetFill(sugarPile.transform.localScale.x); */
        }
        else
        {
            isSpilling=false;
            particleSystem.Stop();
        }
    }

    
}
