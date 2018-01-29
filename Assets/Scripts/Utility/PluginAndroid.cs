using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PluginAndroid : MonoBehaviour
{

    private AndroidJavaObject activityContext = null;
    private AndroidJavaObject pluginObject = null;

    private bool onFocus = false;
    private bool onForeground = true;

    private string titleMsg;
    private string detailMsg;

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

    void SetMessages()
    {
        switch (ApplicationData.SelectedLanguage)
        {
            case LanguageType.English:
                titleMsg = "Yokai detected!";
                detailMsg = "Tap to show";
                break;
            case LanguageType.Japanese:
                titleMsg = "『日本語の歴史1 民族のことばの誕生』";
                detailMsg = "日本語の歴史1 民族のことばの誕生";
                break;
            case LanguageType.Chinese1:
                titleMsg = "我能幫你嗎?";
                detailMsg = "我能幫你嗎";
                break;
            case LanguageType.Chinese2:
                titleMsg = "我別無選擇";
                detailMsg = "我別無選擇";
                break;
            case LanguageType.Thai:
                titleMsg = "คุณช่วยอะไรฉันหน่อยได้ไหม?";
                detailMsg = "คุณช่วยอะไรฉันหน่อยได้ไหม";
                break;
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && !onFocus && onForeground)
        {
            SetMessages();
            pluginObject.Call("turnOnService", DataToJSON(), titleMsg, detailMsg);
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
