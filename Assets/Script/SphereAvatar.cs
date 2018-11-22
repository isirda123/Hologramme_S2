using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAvatar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Score"))
        {
            HitScore();
        }
    }

    private void HitScore()
    {
        Destroy(gameObject);
    }
}
