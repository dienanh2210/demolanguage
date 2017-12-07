using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class UserData : MonoBehaviour
{
    const string PREFS_KEY = "user_data";

    static UserInfo userInfo;

    public static UserData instance;
    static Dictionary<string, DateTime> detectedIbeaconHistory = new Dictionary<string, DateTime> ();

    public static List<UserInfo> LstUserInfo {
        get {
            return UserData.instance.lstUserInfo;
        }
    }

    [SerializeField]
    List<UserInfo> lstUserInfo = new List<UserInfo> ();

    public static bool TakeTicket ()
    {
        return userInfo.isGotTicket = false;
    }
    public static UserInfo GetUserInfo()
    {
        return userInfo;
    }

    public static void CreateTicket ()
    {
        userInfo.isGotTicket = true;
    }

    public static bool IsGotTicket ()
    {
        return userInfo.isGotTicket;
    }

    public static void Reset ()
    {
        PlayerPrefs.SetString (PREFS_KEY, "");
    }

    public static bool IsGotYokai (int yokai_id)
    {
        if (userInfo.yokais == null) {
            return false;
        }
        return userInfo
            .yokais
            .Exists ((obj) => obj.yokai_id == yokai_id);
    }

    public static bool IsGotItem (int item_id)
    {
        if (userInfo.items == null) {
            return false;
        }
        return userInfo.items.Exists ((obj) => obj.item_id == item_id);
    }

    public static bool IsGotBoss {
        get {
            return userInfo.yokais.Exists ((obj) => {
                var yokai = ApplicationData.GetYokaiData (obj.yokai_id);
                return yokai.isBoss;
            });
        }
    }

    public static bool IsShowedGuideNote
    {
        get
        {
            return userInfo.IsShowedGuideNote;
        }
        set
        {
            userInfo.IsShowedGuideNote = value;
        }
    }

    public static bool IsShowedMessageForMiddleEndingBeforeBoss {
        get {
            return userInfo.isShowedMessageForMiddleEndingBeforeBoss;
        }
        set {
            userInfo.isShowedMessageForMiddleEndingBeforeBoss = value;
        }
    }

    public static bool IsShowedMessageForMiddleEnding {
        get {
            return userInfo.isShowedMessageForMiddleEnding;
        }
        set {
            userInfo.isShowedMessageForMiddleEnding = value;
        }
    }

    public static bool IsShowedMessageForLastEnding {
        get {
            return userInfo.isShowedMessageForLastEnding;
        }
        set {
            userInfo.isShowedMessageForLastEnding = value;
        }
    }

    public static bool IsShowedGameTutorial {
        get {
            return userInfo.isShowedGameTutorial;
        }
        set {
            userInfo.isShowedGameTutorial = value;
        }
    }

    public static bool IsShowedYokaiTutorial{
        get{ 
            return userInfo.isShowedYokaiTutorial;
        }
        set{ 
            userInfo.isShowedYokaiTutorial = value;
        }
    }

    public static bool IsShowItemTutorial{
        get{ 
            return userInfo.isShowedItemTutorial;
        }
        set{ 
            userInfo.isShowedItemTutorial = value;
        }
    }

    public static bool IsPassedBossOnce {
        get {
            return userInfo.isPassedBossOnce;
        }
        set {
            userInfo.isPassedBossOnce = value;
        }
    }

    static string GetBeaconKey (string minor_id, string major_id, string uuid)
    {
        return minor_id + "_" + major_id + "_" + uuid;
    }

    public static void DetectIBeacon (string minor_id, string major_id, string uuid)
    {
        var key = GetBeaconKey (minor_id, major_id, uuid);
        if (detectedIbeaconHistory.ContainsKey (key)) {
            detectedIbeaconHistory [key] = DateTime.Now;
        } else {
            detectedIbeaconHistory.Add (key, DateTime.Now);
        }
    }

    public static bool CanDetectIBeacon (string minor_id, string major_id, string uuid)
    {
        var key = GetBeaconKey (minor_id, major_id, uuid);
        if (detectedIbeaconHistory.ContainsKey (key) == false) {
            return true;
        }
        var lastDetectTime = detectedIbeaconHistory [key];
        return DateTime.Compare (lastDetectTime, DateTime.Now.AddMinutes (-5)) < 0;
    }
    public static bool DetectIBeaconExist (string minor_id, string major_id, string uuid)
    {
        var key = GetBeaconKey (minor_id, major_id, uuid);
        return detectedIbeaconHistory.ContainsKey (key);
    }
    public static void SuccessGetYokai (int yokai_id, int ibeacon_id)
    {
        userInfo.yokais.Add (new UserYokai () { yokai_id = yokai_id, ibeacon_id = ibeacon_id });
        userInfo.lastGetYokaiIdForLibrary = yokai_id;
    }

    public static void SuccessGetItem (int item_id, int ibeacon_id)
    {
        userInfo.items.Add (new UserItem { item_id = item_id, ibeacon_id = ibeacon_id });
        var yokai = ApplicationData.YokaiData.Find((obj) => obj.necessary_item_id == item_id);
        userInfo.lastGetYokaiIdForLibrary = yokai.id;
    }

    public static int GetLatestYokaiId()
    {
        return userInfo.lastGetYokaiIdForLibrary;
    }

    void Awake ()
    {
        instance = this;
        Restore ();
    }

    void OnApplicationPause ()
    {
        Save ();
    }

    void OnApplicationQuit ()
    {
        Save ();
    }

    void Save ()
    {
        Debug.Log ("SAVING DATA");
        string data_text = JsonMapper.ToJson (userInfo);
        PlayerPrefs.SetString (PREFS_KEY, data_text);
        PlayerPrefs.Save ();
    }

    static void Restore ()
    {
        var data_text = PlayerPrefs.GetString (PREFS_KEY);
        Debug.Log (data_text);
        if (string.IsNullOrEmpty (data_text)) {
            userInfo = new UserInfo ();
            userInfo.items = new List<UserItem> ();
            userInfo.yokais = new List<UserYokai> ();
            userInfo.isGotTicket = false;


        } else {
            userInfo = JsonMapper.ToObject<UserInfo> (data_text);
            if (userInfo.yokais == null) {
                userInfo.yokais = new List<UserYokai> ();
            }
            if (userInfo.items == null) {
                userInfo.items = new List<UserItem> ();
            }
        }
    }

}

public struct UserInfo
{
    public List<UserYokai> yokais;
    public List<UserItem> items;
    public bool isPassedBossOnce;
    public bool isGotTicket;
    public bool isShowedMessageForMiddleEndingBeforeBoss;
    public bool isShowedMessageForMiddleEnding;
    public bool isShowedMessageForLastEnding;
    public bool isShowedGameTutorial;
    public bool isShowedYokaiTutorial;
    public bool isShowedItemTutorial;
    public bool IsShowedGuideNote;
    public int lastGetYokaiIdForLibrary;
}

public struct UserYokai
{
    public int yokai_id;
    public int ibeacon_id;
}

public struct UserItem
{
    public int item_id;
    public int ibeacon_id;
}
