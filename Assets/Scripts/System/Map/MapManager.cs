using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {

    [SerializeField]
    List<IBeaconIcon> icons;

    [SerializeField]
    Sprite greenFireIcon;

    [SerializeField]
    Sprite redFireIcon;

    [SerializeField]
    Sprite pageIcon;

    static MapManager instance;

	// Use this for initialization
	void Awake () {
        instance = this;
	}

    void Start ()
    {
        SetupIcon ();
       // PlayerPrefs.DeleteAll();
    }

    public static void SetupIcon ()
    {
        instance.icons.ForEach ((icon) => {
            var iBeaconData = ApplicationData.GetIbeaconData (icon.IbeaconIndex);
            switch (iBeaconData.iBeaconType) {
            case IBeaconType.Item:
                icon.SetIconImage (instance.pageIcon);
                break;

            case IBeaconType.Yokai:
                var yokai = ApplicationData.GetYokaiData (iBeaconData.data_id);
                if (yokai.isTermLimited) {
                    icon.SetIconImage (instance.greenFireIcon);
                } else {
                    icon.SetIconImage (instance.redFireIcon);
                }
                break;
            }

            icon.SetActive (iBeaconData.IsShowOnMap ());
        });
    }

    public static IBeaconIcon GetIBeaconIcon (int index)
    {
        return instance.icons.Find ((icon) => icon.IbeaconIndex == index);
    }
}
