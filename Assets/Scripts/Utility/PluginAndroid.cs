﻿
﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class PluginAndroid : MonoBehaviour
{
#if UNITY_ANDROID
    public static System.Action<string> OnGetBeacon;

    private AndroidJavaObject activityContext = null;
    private AndroidJavaObject pluginObject = null;
    private bool onFocus = false;
    private bool onForeground = true;
    public Text textDebug;
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
                pluginClass.CallStatic("SetInstance");
                pluginObject = pluginClass.CallStatic<AndroidJavaObject>("instance");
                pluginObject.Call("setContext", activityContext);
            }
        }
        Scan();

    }
    void Scan()
    {
        try
        {
            pluginObject.Call("isNotRunningBackground");
            pluginObject.Call("turnOnService", DataToJSON(), ApplicationData.GetLocaleText(LocaleType.DetectNotification), "");

        }
        catch (Exception e)
        {
            textDebug.text = e.Message;
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
                IbeaconCanShown newData = new IbeaconCanShown();
                newData.uuId = ib.uuid;
                newData.majorId = ib.major_id;
                newData.minorId = ib.minor_id;
                newData.timeLastShown = UserData.GetLastTimeDetectBeacon(ib.minor_id, ib.major_id, ib.uuid).ToString("yyyy/MM/dd HH:mm:ss");
                sb.Append(JsonUtility.ToJson(newData));
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
        if (pauseStatus)
        {
            pluginObject.Call("changeJsonValue", DataToJSON(), ApplicationData.GetLocaleText(LocaleType.DetectNotification));
            pluginObject.Call("isRunningBackground");

        }
        else if (!pauseStatus)
        {
            pluginObject.Call("isNotRunningBackground");
        }
    }
    public void UpdateIbeaconInfo()
    {
        pluginObject.Call("changeJsonValue", DataToJSON(), ApplicationData.GetLocaleText(LocaleType.DetectNotification));
    }
    void OnApplicationFocus(bool hasFocus)
    {
        onFocus = hasFocus;
    }
    public void ReceiveMessage(string ibeacon)
    {
        if (OnGetBeacon != null)
            OnGetBeacon(ibeacon);
    }
    private void OnApplicationQuit()
    {
        pluginObject.Call("turnOffService");
    }
    private void OnDestroy()
    {
        pluginObject.Call("turnOffService");
    }
#endif
}
public struct IbeaconCanShown
{
    public string uuId;
    public string majorId;
    public string minorId;
    public string timeLastShown;
}