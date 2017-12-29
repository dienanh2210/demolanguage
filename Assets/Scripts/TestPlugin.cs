using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPlugin : MonoBehaviour {

    //[SerializeField]
    //private Text text;

    private AndroidJavaObject activityContext = null;
    private AndroidJavaObject pluginObject = null;

    private string log = null;

    // Use this for initialization
    void Start()
    {
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.yume.hhm.mylibrary.iBeaconLib"))
        {
            if (pluginClass != null)
            {
                pluginObject = pluginClass.CallStatic<AndroidJavaObject>("instance");
                pluginObject.Call("setContext", activityContext);
                log = pluginObject.Call<string>("turnOnService");
                if (log != null)
                {
                    //text.text = log;
                }
                //pluginObject.Call("turnOnService");
            }
        }
    }
}
