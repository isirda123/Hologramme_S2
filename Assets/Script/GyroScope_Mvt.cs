using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroScope_Mvt : MonoBehaviour {

    Transform GravityTransform = null;
    Quaternion startOrientation;
    // Use this for initialization
    void Start () {
        GameObject o = new GameObject("GravityObject");
        GravityTransform = o.transform;
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.00001f;

        startOrientation = GyroToUnity(Input.gyro.attitude);
        startOrientation = Quaternion.Inverse(startOrientation);
        Application.targetFrameRate = 600000;

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 eulerRotation = new Vector3(-Input.gyro.attitude.eulerAngles.x, Input.gyro.attitude.eulerAngles.z, -Input.gyro.attitude.eulerAngles.y);
        /*GravityTransform.rotation = Quaternion.AngleAxis(-eulerRotation.x * Mathf.Rad2Deg * Time.deltaTime, Vector3.right) *
                                    Quaternion.AngleAxis(-eulerRotation.y * Mathf.Rad2Deg * Time.deltaTime, Vector3.up) *
                                    Quaternion.AngleAxis(-eulerRotation.z * Mathf.Rad2Deg * Time.deltaTime, Vector3.forward) *
                                    GravityTransform.rotation;*/
        //GravityTransform.Rotate((eulerRotation) * Mathf.Rad2Deg * Time.deltaTime, Space.Self);
        GravityTransform.rotation = Quaternion.Euler(eulerRotation);
        //GravityTransform.rotation = startOrientation * GyroToUnity(Input.gyro.attitude);
        Physics.gravity = -GravityTransform.up;
        //print (Input.gyro.rotationRateUnbiased.x*Mathf.Rad2Deg);
        //print(Input.gyro.rotationRateUnbiased.z);
        print(Input.gyro.attitude.eulerAngles);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return q;
        //return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
    
}
