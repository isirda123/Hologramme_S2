using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroScope_Mvt : MonoBehaviour {

    public bool TypeRotation = false;
    Transform GravityTransform = null;
    Quaternion startOrientation;
    float XStart;
    float YStart;
    float ZStart;

    public bool invertAxis;

    public Vector3 fuckthis;

    private Rigidbody rb;

    Quaternion newRot;

    // Use this for initialization
    IEnumerator Start () {
        GameObject o = new GameObject("GravityObject");
        GravityTransform = o.transform;
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.00001f;

        startOrientation = GyroToUnity(Input.gyro.attitude);
        startOrientation = Quaternion.Inverse(startOrientation);
        Application.targetFrameRate = 600000;

        rb = GetComponent<Rigidbody>();

        yield return null;
        XStart = -Input.acceleration.normalized.x;
        YStart = -Input.acceleration.normalized.y;
        ZStart = -Input.acceleration.normalized.z;
        print(XStart +""+ YStart +""+ ZStart);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 eulerRotation;
	    Vector3 rotEuler = Vector3.zero;
        if (TypeRotation == true)
        {
 //           if (Input.acceleration.normalized.z < 0)
 //           {
                newRot = Quaternion.Euler(Input.acceleration.normalized.y*Mathf.Rad2Deg, 0, -Input.acceleration.normalized.x * Mathf.Rad2Deg);
 //           }
  //          else
 //           {
 //               newRot = Quaternion.Euler(-Input.acceleration.normalized.x * Mathf.Rad2Deg, -Input.acceleration.normalized.z * Mathf.Rad2Deg, Input.acceleration.normalized.y * Mathf.Rad2Deg);
 //           }
        }
        else
        {
            //eulerRotation.x = -180f + (eulerRotation.x - -1f) * (180f - -180f) / (1f - -1f);
            //eulerRotation.y = -180f + (eulerRotation.y - -1f) * (180f - -180f) / (1f - -1f);
            //eulerRotation.z = -180f + (eulerRotation.z - -1f) * (180f - -180f) / (1f - -1f);
            //newRot = Input.gyro.attitude;
            rotEuler = Input.gyro.attitude.eulerAngles;
            Vector3 temp = rotEuler;
            rotEuler.x = temp.y;
            rotEuler.y = temp.z;
            rotEuler.z = -temp.x;
            newRot = Quaternion.Euler(rotEuler.x, rotEuler.y, -rotEuler.z);
            rotEuler.x = -temp.x * Mathf.Sign(180 - rotEuler.y);
            rotEuler.y = 0;
            rotEuler.z = -temp.y ;
            newRot = Quaternion.Euler(rotEuler.x, rotEuler.y, rotEuler.z);

            //Vector3 localEuler = transform.eulerAngles;
            //newRot = Quaternion.AngleAxis(rotEuler.x - transform.eulerAngles.x, transform.right) * newRot;
            //newRot = Quaternion.AngleAxis(rotEuler.z - transform.eulerAngles.z, transform.forward) * newRot;


        }
	    //newRot *= Quaternion.AngleAxis(-90, Vector3.forward);

        GravityTransform.rotation = newRot;//Quaternion.Euler(eulerRotation);

        if (invertAxis == true)
        {
            transform.rotation = newRot;
        }
        else
        {
            Quaternion AngleDifference = Quaternion.FromToRotation(transform.up, (newRot * Vector3.up).normalized);

            float AngleToCorrect = Quaternion.Angle(newRot, rb.rotation);
            Vector3 Perpendicular = Vector3.Cross((newRot * Vector3.up).normalized, (newRot * Vector3.forward).normalized);
            if (Vector3.Dot(transform.forward, Perpendicular) < 0)
                AngleToCorrect *= -1;
            Quaternion Correction = Quaternion.AngleAxis(AngleToCorrect, (newRot * Vector3.up).normalized);

            Vector3 MainRotation = RectifyAngleDifference((AngleDifference).eulerAngles);
            Vector3 CorrectiveRotation = RectifyAngleDifference((Correction).eulerAngles);
            rb.AddTorque((MainRotation - CorrectiveRotation / 2) - rb.angularVelocity, ForceMode.VelocityChange);
        }
	   

        //rb.rotation = newRot;

        //rotEuler = transform.rotation.eulerAngles - rotEuler;
        //rb.AddTorque(rotEuler, ForceMode.VelocityChange);
        //Physics.gravity = -GravityTransform.up;
        //rb.angularVelocity = Mathf.Deg2Rad * rotEuler;



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

    private Vector3 RectifyAngleDifference(Vector3 angdiff)
    {
        if (angdiff.x > 180) angdiff.x -= 360;
        if (angdiff.y > 180) angdiff.y -= 360;
        if (angdiff.z > 180) angdiff.z -= 360;
        return angdiff;
    }

}
