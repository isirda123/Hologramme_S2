﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.Contains("Enfant"))
        {
            Destroy(collision.gameObject);
        }
    }
}
