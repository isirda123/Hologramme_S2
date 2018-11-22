using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroScope_Mvt : MonoBehaviour {

    Transform GravityTransform = null;
    Quaternion startOrientation;
    float XStart;
    float YStart;
    float ZStart;

    public bool invertAxis;

    public Vector3 fuckthis;


    // Use this for initialization
    IEnumerator Start () {
        GameObject o = new GameObject("GravityObject");
        GravityTransform = o.transform;
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.00001f;

        startOrientation = GyroToUnity(Input.gyro.attitude);
        startOrientation = Quaternion.Inverse(startOrientation);
        Application.targetFrameRate = 600000;

        yield return null;
        XStart = -Input.acceleration.normalized.x;
        YStart = -Input.acceleration.normalized.y;
        ZStart = -Input.acceleration.normalized.z;
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
<<<<<<< HEAD
	    fuckthis = fuckthis.normalized;
	    Vector3 normalizeThatPlease = Input.acceleration.normalized;
        normalizeThatPlease = normalizeThatPlease.normalized;
        if (!invertAxis)
	    {
            
            //eulerRotation = new Vector3(-normalizeThatPlease.y - YStart, normalizeThatPlease.z - ZStart, -normalizeThatPlease.x - XStart);
	        eulerRotation = new Vector3(normalizeThatPlease.y, 0, 0);
        }
	    else
	    {
	        eulerRotation = new Vector3(Input.acceleration.normalized.y - YStart, Input.acceleration.normalized.z - ZStart, Input.acceleration.normalized.x - XStart);
        }        
        //print(normalizeThatPlease);
=======
        eulerRotation = new Vector3(Input.acceleration.normalized.y, -Input.acceleration.normalized.z, -Input.acceleration.normalized.x);
        //eulerRotation = AccelToDeg(eulerRotation);
        print(Input.acceleration.normalized);
>>>>>>> 3a07aec981120528dd4fa0611b8cc84e1d591612
        //}
        /*GravityTransform.rotation = Quaternion.AngleAxis(-eulerRotation.x * Mathf.Rad2Deg * Time.deltaTime, Vector3.right) *
                                    Quaternion.AngleAxis(-eulerRotation.y * Mathf.Rad2Deg * Time.deltaTime, Vector3.up) *
                                    Quaternion.AngleAxis(-eulerRotation.z * Mathf.Rad2Deg * Time.deltaTime, Vector3.forward) *
                                    GravityTransform.rotation;*/
        //GravityTransform.Rotate((eulerRotation) * Mathf.Rad2Deg * Time.deltaTime, Space.Self);
<<<<<<< HEAD
	    eulerRotation.x = -180f + (eulerRotation.x - -1f) * (180f - -180f) / (1f - -1f);
	    //eulerRotation.y = -180f + (eulerRotation.y - -1f) * (180f - -180f) / (1f - -1f);
	    //eulerRotation.z = -180f + (eulerRotation.z - -1f) * (180f - -180f) / (1f - -1f);
	    Quaternion newRot = Input.gyro.attitude;
	    Vector3 rotEuler = newRot.eulerAngles;
	    Vector3 temp = rotEuler;
	    rotEuler.x = temp.y;
	    rotEuler.y = 0;
	    rotEuler.z = -temp.x;
	    newRot = Quaternion.Euler(rotEuler.x, rotEuler.y, rotEuler.z);
	    //newRot *= Quaternion.AngleAxis(-90, Vector3.forward);

        GravityTransform.rotation = newRot;//Quaternion.Euler(eulerRotation);
        transform.rotation = newRot;

	    //Physics.gravity = -GravityTransform.up;



        Vector3 dir = Vector3.zero;

	    // we assume that device is held parallel to the ground
	    // and Home button is in the right hand

	    // remap device acceleration axis to game coordinates:
	    //  1) XY plane of the device is mapped onto XZ plane
	    //  2) rotated 90 degrees around Y axis
	    dir.x = -Input.gyro.userAcceleration.y;
	    dir.z = Input.gyro.userAcceleration.x;

	    // clamp acceleration vector to unit sphere
	    if (dir.sqrMagnitude > 1)
	        dir.Normalize();

	    // Make it move 10 meters per second instead of 10 meters per frame...
	    dir *= Time.deltaTime;

	    // Move object
	    //transform.Translate(dir * 30);
        
        //GravityTransform.rotation = startOrientation * GyroToUnity(Input.gyro.attitude);
        //Physics.gravity = -GravityTransform.up;
=======
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



>>>>>>> 3a07aec981120528dd4fa0611b8cc84e1d591612
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
