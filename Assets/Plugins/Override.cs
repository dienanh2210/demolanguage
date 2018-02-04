using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Override : MonoBehaviour
{
    public static string noti;
#if !UNITY_EDITOR && UNITY_IOS

    [DllImport("__Internal")]
    public static extern void start_iBeacon();

    [DllImport("__Internal")]
     public static extern void setText(string message);

    [DllImport("__Internal")]
     public static extern void updateAvailableMinorID(string minorIDs);

    [DllImport("__Internal")]
     public static extern void updateAvailableMajorID(string majorIDs);
   
#endif

    public static void StartiBeacon()
    {
#if UNITY_IOS && !UNITY_EDITOR
         start_iBeacon();
#endif
    }

    public static void SetText(string message)
    {
#if UNITY_IOS && !UNITY_EDITOR
         setText(message);
#endif
    }


    public static void LoadMinor_id(string minorIDs)
    {
#if UNITY_IOS && !UNITY_EDITOR
         updateAvailableMinorID(minorIDs);
#endif
    }

    public static void LoadMajor_id(string majorIDs)
    {
#if UNITY_IOS && !UNITY_EDITOR
         updateAvailableMajorID(majorIDs);
#endif
    }
}


