using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionParent : MonoBehaviour {

    public GameObject Enfant;

    public int numberOfChilds = 5;

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
            for (int i = 0; i < numberOfChilds; i++)
                CreateNewChild(Pos);
        }

        else if (collision.gameObject.CompareTag("Creatrice"))
        {

        }
    }

    private void CreateNewChild(Vector3 position)
    {
        Instantiate(Enfant, new Vector3(position.x, position.y, position.z), new Quaternion(0, 0, 0, 0), transform.parent);
    }
}
