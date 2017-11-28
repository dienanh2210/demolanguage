using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCamera : MonoBehaviour {

    private Gyroscope gyro;
    private bool gyroSupported;
    private Quaternion rotFix;


    private GameObject worldObj;
    private float startY;

    private void OnEnable()
    {
        DebugConsole.Log("May choi hay nghi? " + SystemInfo.supportsGyroscope);
    }

    void Start(){
        gyroSupported = SystemInfo.supportsGyroscope;

        GameObject camParent = new GameObject ("Cam Parent");
        camParent.transform.position = transform.position;
        transform.parent = camParent.transform;

        if (gyroSupported) {
            gyro = Input.gyro;
            gyro.enabled = true;

            camParent.transform.rotation = Quaternion.Euler (90f,180f,0f);
            rotFix = new Quaternion (0,0,1,0);
        }

        worldObj = GameObject.Find ("World Object");
       
    }

    void Update(){
        if (gyroSupported && startY == 0)
        {
            ResetGyroRotation();
        }

        transform.localRotation = gyro.attitude * rotFix;
    }

    void ResetGyroRotation(){
        startY = transform.eulerAngles.y;
        if (worldObj!=null) {
            worldObj.transform.rotation = Quaternion.Euler (0f,startY,0f);
        }

    }
}
