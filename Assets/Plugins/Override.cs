using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class Override : MonoBehaviour{

#if !UNITY_EDITOR && UNITY_IOS

  [DllImport("__Internal")]
  public static extern void start_iBeacon();
   
#endif

    public static void startiBeacon()
    {
        #if UNITY_IOS && !UNITY_EDITOR
         start_iBeacon();
        #endif
    }

}


