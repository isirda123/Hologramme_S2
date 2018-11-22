using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCreatrice : MonoBehaviour
{
    public GameObject roomCenter;
    public Material neutralMat;

    private bool isNeutral;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision other)
    {
        if (!isNeutral && other.collider.CompareTag("Avatar"))
        {
            HitByAvatar();
        }

        else if (isNeutral && other.collider.CompareTag("Score"))
        {
            Destroy(gameObject);
        }
    }

    private void HitByAvatar()
    {
        Instantiate(gameObject, GetRandomPosInRoom(), Quaternion.identity, transform.parent);

        GetComponent<Renderer>().material = neutralMat;
        isNeutral = true;
    }

    private Vector3 GetRandomPosInRoom()
    {
        return roomCenter.transform.position + new Vector3(Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f));
    }
}
