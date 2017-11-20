using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ApplicationLogic : MonoBehaviour
{
    static int startMonth = 7;
    static int endMonth = 8;

    public static bool IsShowMessageForMiddleEnding ()
    {
        var yokaisInArea1 = ApplicationData.GetYokaisOnArea (1);
        var yokaisInArea2 = ApplicationData.GetYokaisOnArea (2);
        return !yokaisInArea1.Exists ((obj) => UserData.IsGotYokai (obj.id) == false)
                             && !yokaisInArea2.Exists ((obj) => UserData.IsGotYokai (obj.id) == false);
    }

    public static bool IsShowMessageForLastEnding ()
    {
        var yokaisInArea1 = ApplicationData.GetYokaisOnArea (1);
        var yokaisInArea2 = ApplicationData.GetYokaisOnArea (2);
        var yokaisInArea3 = ApplicationData.GetYokaisOnArea (3);
        var yokaisInArea4 = ApplicationData.GetYokaisOnArea (4);
        return !yokaisInArea1.Exists ((obj) => UserData.IsGotYokai (obj.id) == false)
                             && !yokaisInArea2.Exists ((obj) => UserData.IsGotYokai (obj.id) == false)
                             && !yokaisInArea3.Exists ((obj) => UserData.IsGotYokai (obj.id) == false)
                             && !yokaisInArea4.Exists ((obj) => UserData.IsGotYokai (obj.id) == false);
    }

    public static bool IsShowPhotoFrame ()
    {
        return UserData.IsPassedBossOnce;
    }

    public static bool IsShowTicket ()
    {
        return UserData.IsGotBoss;
    }

    public static bool IsShowTermLimitedYokai ()
    {
        return DateTime.Now.Month >= startMonth
                      && DateTime.Now.Month <= endMonth;
    }
}

