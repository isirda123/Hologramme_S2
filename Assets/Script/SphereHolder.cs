using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHolder : MonoBehaviour
{
    public BoxCollider boundaries;
	// Use this for initialization

    public void Start()
    {

        boundaries = BoundariesScript.instance.collider;
    }
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    Vector3 currentPos = transform.position;
	    
	    if (!boundaries.bounds.Contains(currentPos))
	    {
	        transform.position = boundaries.ClosestPointOnBounds(currentPos);
	    }

    }

}
