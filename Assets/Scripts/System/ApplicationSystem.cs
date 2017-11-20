using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSystem : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {

        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 30;

    }
    public static bool IsIphoneX()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Screen.height == 2436 && Screen.width == 1125)
            {
                return true;
            }
        }
        return false;
    }
}
