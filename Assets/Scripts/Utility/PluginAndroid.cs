using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PluginAndroid : MonoBehaviour
{

    private AndroidJavaObject activityContext = null;
    private AndroidJavaObject pluginObject = null;

    private bool onFocus = false;
    private bool onForeground = true;

    void Start()
    {
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
        using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.fujiwaranosato.YokaiAR.ibeacon.IBeaconPlugin"))
        {
            if (pluginClass != null)
            {
                pluginObject = pluginClass.CallStatic<AndroidJavaObject>("instance");
                pluginObject.Call("setContext", activityContext);
            }
        }
    }

    string DataToJSON()
    {
        List<IBeaconData> list = ApplicationData.IBeaconData;
        var sb = new StringBuilder("[");
        foreach (var ib in list)
        {
            if (ib.IsShowOnMap())
            {
                sb.Append(JsonUtility.ToJson(ib));
                sb.Append(',');
            }
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append(']');
        string str = sb.ToString();
        string finalJSON = "{\"iBeacons\":" + str + "}";
        return finalJSON;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && !onFocus && onForeground)
        {
            pluginObject.Call("turnOnService", DataToJSON(), ApplicationData.GetLocaleText(LocaleType.DetectNotification), "");
            onForeground = false;
        }
        if (!pauseStatus && onFocus && !onForeground)
        {
            onForeground = true;
            pluginObject.Call("turnOffService");
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        onFocus = hasFocus;
    }
}
