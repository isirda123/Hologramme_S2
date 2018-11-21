using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroScope_Mvt : MonoBehaviour {

    Transform GravityTransform = null;
    Quaternion startOrientation;
    float XStart;
    float YStart;
    float ZStart;
    // Use this for initialization
    void Start () {
        GameObject o = new GameObject("GravityObject");
        GravityTransform = o.transform;
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.00001f;

        startOrientation = GyroToUnity(Input.gyro.attitude);
        startOrientation = Quaternion.Inverse(startOrientation);
        Application.targetFrameRate = 600000;

        XStart = -Input.gyro.attitude.eulerAngles.x;
        YStart = -Input.gyro.attitude.eulerAngles.y;
        ZStart = -Input.gyro.attitude.eulerAngles.z;
        print(XStart +""+ YStart +""+ ZStart);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 eulerRotation;
        /*if (Input.gyro.attitude.eulerAngles.z - ZStart < 0)
        {
            eulerRotation = new Vector3(Input.gyro.attitude.eulerAngles.x, Input.gyro.attitude.eulerAngles.z, Input.gyro.attitude.eulerAngles.y);
        }
        else
        {*/
        //eulerRotation = new Vector3(-Input.gyro.attitude.eulerAngles.x, -Input.gyro.attitude.eulerAngles.z, -Input.gyro.attitude.eulerAngles.y);
        eulerRotation = new Vector3(Input.acceleration.normalized.y, -Input.acceleration.normalized.z, -Input.acceleration.normalized.x);
        //eulerRotation = AccelToDeg(eulerRotation);
        print(Input.acceleration.normalized);
        //}
        /*GravityTransform.rotation = Quaternion.AngleAxis(-eulerRotation.x * Mathf.Rad2Deg * Time.deltaTime, Vector3.right) *
                                    Quaternion.AngleAxis(-eulerRotation.y * Mathf.Rad2Deg * Time.deltaTime, Vector3.up) *
                                    Quaternion.AngleAxis(-eulerRotation.z * Mathf.Rad2Deg * Time.deltaTime, Vector3.forward) *
                                    GravityTransform.rotation;*/
        //GravityTransform.Rotate((eulerRotation) * Mathf.Rad2Deg * Time.deltaTime, Space.Self);
        GravityTransform.rotation = Quaternion.Euler(eulerRotation*Mathf.Rad2Deg);
        //Physics.gravity = -GravityTransform.up;
        //GravityTransform.rotation = startOrientation * GyroToUnity(Input.gyro.attitude);
        if (eulerRotation.y > 0)
        {
            Physics.gravity = -GravityTransform.up;
        }
        else
        {
            Physics.gravity = GravityTransform.up;
        }



        //print (Input.gyro.rotationRateUnbiased.x*Mathf.Rad2Deg);
        //print(Input.gyro.rotationRateUnbiased.z);
        //print(Input.gyro.attitude.eulerAngles);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return q;
        //return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
    

    private static Vector3 AccelToDeg(Vector3 value)
    {
        value.x = -Remap(-1, 1, -180, 180, value.x);
        value.y = -Remap(-1, 1, -180, 180, value.y);
        value.z = -Remap(-1, 1, -180, 180, value.z);

        return value;
    }

    public static float Remap(float minOld, float maxOld, float minNew, float maxNew, float value)
    {
        return minNew + (value - minOld) * (maxOld - maxNew) / (maxOld - minOld);
    }

}
