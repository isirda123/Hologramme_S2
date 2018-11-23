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
	    Quaternion newRot;

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
            newRot = DeviceRotation.Get();

            //DeviceRotation.GetAngleByDeviceAxis(Vector3.right);
            //DeviceRotation.GetAngleByDeviceAxis(Vector3.forward);
            

            //Vector3 rotEuler = new Vector3(DeviceRotation.GetAngleByDeviceAxis(Vector3.right), DeviceRotation.GetAngleByDeviceAxis(Vector3.up), DeviceRotation.GetAngleByDeviceAxis(Vector3.forward));

            Vector3 rotEuler = newRot.eulerAngles;
            Vector3 temp = rotEuler;


            rotEuler.x = -temp.x;
            rotEuler.y = temp.z;
            rotEuler.z = -temp.y;

            temp = rotEuler;

            rotEuler.y = -temp.z;
            rotEuler.z = temp.y;

            /*rotEuler = new Vector3();

            rotEuler.x = DeviceRotation.GetAngleByDeviceAxis(Vector3.right);
            rotEuler.z = DeviceRotation.GetAngleByDeviceAxis(Vector3.forward);*/

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



public static class DeviceRotation
{
    private static bool gyroInitialized = false;

    public static bool HasGyroscope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }

    public static Quaternion Get()
    {
        if (!gyroInitialized)
        {
            InitGyro();
        }

        return HasGyroscope
            ? ReadGyroscopeRotation()
            : Quaternion.identity;
    }

    private static void InitGyro()
    {
        if (HasGyroscope)
        {
            Input.gyro.enabled = true;                // enable the gyroscope
            Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
        }
        gyroInitialized = true;
    }

    private static Quaternion ReadGyroscopeRotation()
    {
        return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0) * Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(180, Vector3.right);
    }



    /// <summary>
    /// Returns the rotation angle of given device axis. Use Vector3.right to obtain pitch, Vector3.up for yaw and Vector3.forward for roll.
    /// This is for landscape mode. Up vector is the wide side of the phone and forward vector is where the back camera points to.
    /// </summary>
    /// <returns>A scalar value, representing the rotation amount around specified axis.</returns>
    /// <param name="axis">Should be either Vector3.right, Vector3.up or Vector3.forward. Won't work for anything else.</param>
    public static float GetAngleByDeviceAxis(Vector3 axis)
    {
        Quaternion deviceRotation = DeviceRotation.Get();
        Quaternion eliminationOfOthers = Quaternion.Inverse(
            Quaternion.FromToRotation(axis, deviceRotation * axis)
        );
        Vector3 filteredEuler = (eliminationOfOthers * deviceRotation).eulerAngles;

        float result = filteredEuler.z;
        if (axis == Vector3.up)
        {
            result = filteredEuler.y;
        }
        if (axis == Vector3.right)
        {
            // incorporate different euler representations.
            result = (filteredEuler.y > 90 && filteredEuler.y < 270) ? 180 - filteredEuler.x : filteredEuler.x;
        }
        return result;
    }


}
