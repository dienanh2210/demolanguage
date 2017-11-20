using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageData {

    public static int yokaiID { get; private set; }
    public static int itemID { get; private set; }
    public static bool isShowYokaiDetail { get; private set; }

    public static bool IsItem {
        get {
            return itemID != -1;
        }
    }

    public static bool IsYokai {
        get {
            return yokaiID != -1;
        }
    }

    public static void Initialize()
    {
        yokaiID = -1;
        itemID = -1;
        isShowYokaiDetail = false;
    }

    public static void SetYokaiID(int yID)
    {
        Initialize();
        yokaiID = yID;
    }

    public static void SetItemID(int iID)
    {
        Initialize();
        itemID = iID;
    }

    public static void ShowYokaiDetail ()
    {
        isShowYokaiDetail = true;
    }
}
