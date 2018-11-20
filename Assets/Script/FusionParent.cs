using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionParent : MonoBehaviour {

    public GameObject Enfant;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.Contains("Parent"))
        {
            Vector3 Pos = collision.contacts[0].point;
            Instantiate(Enfant, new Vector3 (Pos.x, Pos.y, Pos.z), new Quaternion (0,0,0,0));
        }
    }
}
