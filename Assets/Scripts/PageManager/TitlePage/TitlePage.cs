using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class TitlePage : Page
{
    #region Declare
    [SerializeField]
    List<Button> lstButton;
    [SerializeField]
    List<Sprite> lstSprite;

    [SerializeField]
    GameObject dialog,PopupRed;
    [SerializeField]
    TextPic txtTerm;
    [SerializeField]
    Text txtRed;

    string iosID;
    string androidID;

    [DllImport ("__Internal")]
    private static extern int isInstalled();

    #endregion

    #region Init
    void OnEnable (){
        PageData.Initialize ();
        androidID = "com.fujiwaranosato.EsashiNavi";
        iosID = "youtube-watch-listen-stream/id544007664?mt=8";

        if (ApplicationLogic.IsShowTermLimitedYokai())
        {
            dialog.SetActive(true);

            txtTerm.text = ApplicationData.GetLocaleText(LocaleType.TermLimitedYokai);

        }
        else
        {
            dialog.SetActive(false);
        }
    }
    private void Start()
    {
        if (!UserData.IsShowedGuideNote)
        {
            PopupRed.SetActive(true);
            txtRed.text = ApplicationData.GetLocaleText(LocaleType.Guide);
            UserData.IsShowedGuideNote = true;

        }
        else
        {
            PopupRed.SetActive(false);
        }
        DisalbeButton("btJapanese");
    }
#endregion

    #region LaunchApp
        void IosLaunch()
        {

            int appStatus = 0; //Assign value we recieve to this
        
            // Calls the isInstalled function inside the plugin 
            appStatus = isInstalled(); //returns the status of app install in ObjC plugin
            // 0 is No, 1 is Yes

            if (appStatus != 1)
            {
                Application.OpenURL("itms-apps://itunes.apple.com/app/" + iosID);
            }
        }
        void LaunchIntent()
        {
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

            AndroidJavaObject launchIntent = null;
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", androidID);

            if (launchIntent == null)
            { 
                //open app in store
                Application.OpenURL("market://details?id=" + androidID);
            }
            else 
            {
                //open the app
                ca.Call("startActivity", launchIntent);
            }

            up.Dispose();
            ca.Dispose();
            packageManager.Dispose();
            launchIntent.Dispose();
        }
        public void OpenURL()
        {
    #if UNITY_ANDROID
            LaunchIntent();
    #elif UNITY_IOS
            IosLaunch();
    #endif
    }
    #endregion

    #region Language
    void DisalbeButton(string name)
        {
            List<Button> lstTemp = lstButton.FindAll(x => x.name != name);
            Button bt = lstButton.Find(x => x.name == name);
            Color myGray = new Color();
            ColorUtility.TryParseHtmlString("#9C9C9CFF", out myGray);
            bt.GetComponentInChildren<Text>().color = Color.black;
            bt.image.sprite = lstSprite[1];
            foreach (var item in lstTemp)
            {
                item.image.sprite = lstSprite[2];
                item.GetComponentInChildren<Text>().color = myGray;
                item.interactable = false;
            }
        }
        public void SelectEnglish()
        {
            ApplicationData.SelectedLanguage = LanguageType.English;
            DisalbeButton("btEnglish");
        }
        public void SelectJapanese()
        {
            ApplicationData.SelectedLanguage = LanguageType.Japanese;
            DisalbeButton("btJapanese");
        }
        public void SelectChinese1()
        {
            ApplicationData.SelectedLanguage = LanguageType.Chinese1;
            DisalbeButton("btChinese");
        }
        public void SelectChinese2()
        {
            ApplicationData.SelectedLanguage = LanguageType.Chinese2;
            DisalbeButton("btChinese2");
        }
        public void SelectThai()
        {
            ApplicationData.SelectedLanguage = LanguageType.Thai;
            DisalbeButton("btThai");
        }
    #endregion

    #region Utilities
    public void ChangeTutorialPage()
    {
        if (!UserData.IsShowedGameTutorial)
        {
            PageManager.Show(PageType.Tutorial);
        }
        else
        {
            PageManager.Show(PageType.MapPage);
        }
    }
    public void EnableBluetooth()
    {
        BluetoothState.EnableBluetooth();
    }
    public void OpenPopupGuide()
    {
        PopupRed.SetActive(true);
        txtRed.text = ApplicationData.GetLocaleText(LocaleType.Guide);
    }
    #endregion
}
