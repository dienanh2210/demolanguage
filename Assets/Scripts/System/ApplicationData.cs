using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ApplicationData : MonoBehaviour
{
    static ApplicationData applicationData;

    public static string updateNotificationText()
    {
        var listMinor_id = ApplicationData.IBeaconData.Where(s => s.IsShowOnMap()).ToList();
        string minor = "";
        string major = "";
        string uuid = "";
        foreach (IBeaconData i in listMinor_id)
        {
            minor = minor + "," + i.minor_id;
            major = major + "," + i.major_id;
            uuid = uuid + "," + i.uuid;
        }
        string notificationText = GetLocaleText(LocaleType.DetectNotification);

        Override.SetText(notificationText);
        Override.LoadMinor_id(minor);
        Override.LoadMajor_id(major);
        Override.LoadUUID(uuid);

        return "";
    }


    public static string GetLocaleText (LocaleType key)
    {
        var locale = applicationData.localeData.Find ((obj) => obj.key == key);
        if (locale.key != LocaleType.None) {
            return locale.localContents.Find ((obj) => obj.languageType == SelectedLanguage).text;
        } else {
            Debug.LogError ("cant find locale");
            return "";
        }
    }

	public static Font GetFont(int index){
		switch (index) {
		case 1:
			return applicationData.fontData.titleFont;
			break;
		case 2:
			return applicationData.fontData.contentFont;
			break;
		case 3:
			return applicationData.fontData.thaiTitleFont;
			break;
		case 4:
			return applicationData.fontData.thaiContentFont;
			break;
		}
		return null;
	}

    public static Sprite GetLocaleImage(LocaleType key)
    {
        var locale = applicationData.localeData.Find((obj) => obj.key == key);
        if (locale.key != LocaleType.None)
        {
            return locale.localContents.Find((obj) => obj.languageType == SelectedLanguage).image;
        }
        else
        {
            Debug.LogError("cant find locale");
            return null;
        }
    }
		

    public static List<YokaiData> YokaiData {
        get {
            return applicationData.yokaiData;
        }
    }


    public static SuccessImageData GetSuccessImage(LanguageType languageType)
    {
        return applicationData.successImageData.Find ((obj) => obj.type == languageType);
    }

	public static Logo GetLogoImage(LanguageType languageType)
	{
		return applicationData.logo.Find ((obj) => obj.type == languageType);
	}

    public static YokaiData GetYokaiData (int yokai_id)
    {
        return applicationData.yokaiData.Find ((obj) => obj.id == yokai_id);
    }

    public static YokaiData GetYokaiDataFromItemId (int item_id)
    {
        return applicationData.yokaiData.Find ((obj) => obj.necessary_item_id == item_id);
    }

    public static List<ItemData> ItemData {
        get {
            return applicationData.itemData;
        }
    }

    public static ItemData GetItemData (int item_id)
    {
        return applicationData.itemData.Find ((obj) => obj.id == item_id);
    }

    public static List<IBeaconData> IBeaconData {
        get {
            return applicationData.iBeaconData;
        }
    }

    public static IBeaconData GetIbeaconData (string minor_id)
    {
        return applicationData.iBeaconData.Find ((obj) => obj.minor_id == minor_id);
    }

    public static IBeaconData GetIbeaconData (int index)
    {
        return applicationData.iBeaconData.Find ((obj) => obj.index == index);
    }

    public static List<TutorialData> TutorialData {
        get {
            return applicationData.tutorialData;
        }
    }


    public static List<YokaiGetTutorialData> YokaiGetTutorialData {
        get {
            return applicationData.yokaiGetTutorialData;
        }
    }



    public static LanguageType SelectedLanguage {
        get {
            return applicationData.selectedLanguage;
        }
        set {
            applicationData.selectedLanguage = value;
        }
    }

    public static List<YokaiData> GetYokaisOnArea (int area_id, bool withTermLimitedYokai = false)
    {
        var yokais = YokaiData.FindAll (
            (yokai) => yokai.GetArea () == area_id);
        if (!withTermLimitedYokai) {
            return yokais.FindAll ((yokai) => yokai.isTermLimited == false);
        } else {
            return yokais;
        }
    }

	public static float SetLineSpacing(LocaleType key){
		var locale = applicationData.localeData.Find ((obj) => obj.key == key);
		if (locale.key != LocaleType.None) {
			return locale.localContents [(int)ApplicationData.SelectedLanguage].lineSpacing;
		} else {
			return 1 ;
		}
	}

	public static int SetFontSize(LocaleType key){
		var locale = applicationData.localeData.Find ((obj) => obj.key == key);
		if (locale.key != LocaleType.None) {
			return locale.localContents [(int)ApplicationData.SelectedLanguage].fontSize;
		} else {
			return 1 ;
		}
	}

	public static float SetLineWidth(LocaleType key){
		var locale = applicationData.localeData.Find ((obj) => obj.key == key);
		if (locale.key != LocaleType.None) {
			return locale.localContents [(int)ApplicationData.SelectedLanguage].lineWidth;
		} else {
			return 1 ;
		}
	}

	public static Vector2 SetLinePosition(LocaleType key){
		var locale = applicationData.localeData.Find ((obj) => obj.key == key);
		if (locale.key != LocaleType.None) {
			return locale.localContents [(int)ApplicationData.SelectedLanguage].linePosition;
		} else {
			return new Vector2(0,0) ;
		}
	}

    [SerializeField]
    LanguageType selectedLanguage = LanguageType.Japanese;

    [SerializeField]
    List<LocaleData> localeData = new List<LocaleData> ();

	[SerializeField]
	FontText fontData = new FontText();

    [SerializeField]
    List<Locale> local = new List<Locale>();

    [SerializeField]
    List<YokaiData> yokaiData = new List<YokaiData> ();

    [SerializeField]
    List<SuccessImageData> successImageData = new List<SuccessImageData>();

    [SerializeField]
    List<ItemData> itemData = new List<ItemData> ();

    [SerializeField]
    List<IBeaconData> iBeaconData = new List<IBeaconData> ();

    [SerializeField]
    List<TutorialData> tutorialData = new List<TutorialData> ();


    [SerializeField]
    List<YokaiGetTutorialData> yokaiGetTutorialData = new List<YokaiGetTutorialData> ();

	[SerializeField]
	List<Logo> logo = new List<Logo>();


    void Awake ()
    {
        applicationData = this;
    }
}

public enum LanguageType
{
    Japanese,
    English,
    Chinese1,
    Chinese2,
    Thai
}

public enum LocaleType
{
    None,
    TermLimitedYokai,
    MiddleEnding1,
    MiddleEnding2,
    LastEnding,
    NoItemMessage1,
    NoItemMessage2,
    ItemGetTutorial,
    MiddleBossCrawMessage1,
    MiddleBossCrawMessage2,
    MiddleBossCrawMessage3,
    MiddleEndingAfterBoss,
    LastEndingAfterBoss,
    WaitNextDetect,
    Guide,
    HowToPlay,
    TapOnPrologue,
    ButtonBack,
    TitleYokaiLibrary,
    IconLimitedYokai,
    ButtonHowToPlay,
    ButtonYokaiLibrary,
    ButtonBonus,
    ButtonSwitchCamera,
    TitleBonusPage,
    ButtonPhotoFrame,
    ButtonTicket,
    ButtonPrologue,
    TitleTicketPage,
    TicketNoticeForStaff,
    TicketNoticeDontTap,
    ButtonTicketStaff,
    SelectLanguage,
    ButtonOpenEsashiApp,
    ButtonOpenCautionDialog,
    ButtonClose,
    ButtonGetSuccessful,
    ButtonToSeal,
    MessageGetItem,
    MessageGetYokai,
    MessageFindYokai,
    MessageFindItem,
	ConfirmationDialog1,
	ConfirmationDialog2,
	ButtonYes,
    ButtonNo,
	ButtonExchangeTicket,
    ShareTwitterTitle,
    DetectNotification
}

[Serializable]
public struct LocaleData
{
    public LocaleType key;
    public List<Locale> localContents;
}
	

[Serializable]
public struct SuccessImageData
{
    public LanguageType type;
    public Sprite yokaiText;
    public Sprite itemText;
}

[Serializable]
public struct TutorialData
{
    public int index;
    public Sprite image;
    public List<Locale> localContents;
}


[Serializable]
public struct YokaiGetTutorialData
{
    public int index;
    public List<Locale> localContents;
}

[Serializable]
public struct Logo
{
	public LanguageType type;
	public Sprite img;
}

[Serializable]
public struct IBeaconData
{
    public int index;
    public IBeaconType iBeaconType;
    public string uuid; // this field will be removed
    public string major_id;
    public string minor_id;
    public string name;
    public int data_id; // if iBeaconType is Yokai, get YokaiData using data_id. if iBeacon is Item, get ItemData using data_id
    public int area;    // 1, 2, 3, 4

    public bool IsShowOnMap ()
    {
        switch (iBeaconType) {
        case IBeaconType.Yokai:
            if (UserData.IsGotYokai (data_id)) {
                return false;
            }
            var yokai = ApplicationData.GetYokaiData (data_id);
            if (yokai.isTermLimited){
                return ApplicationLogic.IsShowTermLimitedYokai ();
            }
            if (yokai.isBoss
                && (
                    (!UserData.IsShowedMessageForMiddleEndingBeforeBoss && !UserData.IsShowedMessageForLastEnding)
                    || (!UserData.IsShowedMessageForLastEnding && UserData.IsPassedBossOnce))) {
                return false;
            }
            break;

        case IBeaconType.Item:
            if (UserData.IsGotItem (data_id)) {
                return false;
            }
            break;
        }

        switch (area) {
        case 3:
        case 4:
            if (UserData.IsPassedBossOnce == false) {
                return false;
            }
            break;
        }
        return true;
    }
}

public enum IBeaconType
{
    Yokai,
    Item
}

[Serializable]
public struct YokaiData
{
    public int id;
    public string name;
    public string kana;
    public List<Locale> localNames;
    public Sprite image;
    public Sprite nameImage;
    public List<Locale> localContents;
    public int necessary_item_id;
    public bool isBoss;
    public bool isTermLimited;

    public bool IsNeedItem ()
    {
        return necessary_item_id > 0;
    }

    public bool HasItem ()
    {
        // player has the item or not.

        if (necessary_item_id == 0) {
            return true;
        }
        return UserData.IsGotItem (necessary_item_id);
    }

    public bool CanShowOnYokaiList ()
    {
        return !isBoss || (isBoss && UserData.IsGotBoss);
    }

    public int GetArea ()
    {
        var yokai_id = id;
        var iBeaconData = ApplicationData.IBeaconData.Find (
            (iBeacon) =>
                iBeacon.iBeaconType == IBeaconType.Yokai
                && iBeacon.data_id == yokai_id);
        return iBeaconData.area;
    }
}

[Serializable]
public struct ItemData
{
    public int id;
    public string name;
    public Sprite image;
}

[Serializable]
public struct Locale
{
    public LanguageType languageType;
    [Multiline]
    public string text;
    public Sprite image;
	public float lineSpacing;
	public Vector2 linePosition;
	public float lineWidth;
	public int fontSize;
}


[Serializable]
public struct FontText{
	public Font titleFont;
	public Font contentFont;
	public Font thaiTitleFont;
	public Font thaiContentFont;
}

