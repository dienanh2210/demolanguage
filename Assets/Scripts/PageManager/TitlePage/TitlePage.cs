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
    Text txtRed, txtSelectLanguage, txtShowApp, txtCaution;

	public Image imgLogo;
	Text[] GetText;

    string iosID = "id1331763426";
    string iosScheme = "esashinavi://";
    string androidID = "com.fujiwaranosato.EsashiNavi";

    [DllImport ("__Internal")]
    private static extern bool isInstalled(string id);

    [DllImport ("__Internal")]
    private static extern void openByScheme (string scheme);

    public static TitlePage instance;
    #endregion

    #region Init
    void OnEnable (){
        switch (Application.systemLanguage.ToString()) {
		case "Japanese":
			SelectJapanese ();
			break;
		case "ChineseTraditional":
			SelectChinese1 ();
			break;
		case "ChineseSimplified":
			SelectChinese2 ();
			break;
		case "Thai":
			SelectThai ();
			break;
		default:
			SelectEnglish ();
			break;
		}

		imgLogo.sprite = ApplicationData.GetLogoImage (ApplicationData.SelectedLanguage).img;

        PageData.Initialize ();

        if (ApplicationLogic.IsShowTermLimitedYokai())
        {
            dialog.SetActive(true);

            txtTerm.text = ApplicationData.GetLocaleText(LocaleType.TermLimitedYokai);
			txtTerm.lineSpacing = ApplicationData.SetLineSpacing (LocaleType.TermLimitedYokai);
			txtTerm.fontSize = ApplicationData.SetFontSize (LocaleType.TermLimitedYokai);

        }
        else
        {
            dialog.SetActive(false);
        }

        txtSelectLanguage.text = ApplicationData.GetLocaleText(LocaleType.SelectLanguage);
        txtShowApp.text = ApplicationData.GetLocaleText(LocaleType.ButtonOpenEsashiApp);
        txtCaution.text = ApplicationData.GetLocaleText(LocaleType.ButtonOpenCautionDialog);

    }
    private void Start()
    {
        if (!UserData.IsShowedGuideNote)
        {
            OpenPopupGuide();
            UserData.IsShowedGuideNote = true;

        }
        else
        {
            PopupRed.SetActive(false);
        }
        instance = this;
    }
#endregion

    #region LaunchApp
        void IosLaunch()
        {
            if (isInstalled (iosScheme)) {
                openByScheme (iosScheme);
            } else {
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
        try
        {
            List<Button> lstTemp = lstButton.FindAll(x => x.name != name);
            Button bt = lstButton.Find(x => x.name == name);
            bt.GetComponentInChildren<Text>().color = Color.black;
            bt.image.sprite = lstSprite[1];
            foreach (var item in lstTemp)
            {
                item.interactable = true;
            }

            imgLogo.sprite = ApplicationData.GetLogoImage(ApplicationData.SelectedLanguage).img;

            txtSelectLanguage.text = ApplicationData.GetLocaleText(LocaleType.SelectLanguage);
            txtSelectLanguage.fontSize = ApplicationData.SetFontSize(LocaleType.SelectLanguage);

            txtShowApp.text = ApplicationData.GetLocaleText(LocaleType.ButtonOpenEsashiApp);
            txtShowApp.fontSize = ApplicationData.SetFontSize(LocaleType.ButtonOpenEsashiApp);
            txtShowApp.lineSpacing = ApplicationData.SetLineSpacing(LocaleType.ButtonOpenEsashiApp);

            txtCaution.text = ApplicationData.GetLocaleText(LocaleType.ButtonOpenCautionDialog);
            txtCaution.fontSize = ApplicationData.SetFontSize(LocaleType.ButtonOpenCautionDialog);
            txtCaution.lineSpacing = ApplicationData.SetLineSpacing(LocaleType.ButtonOpenCautionDialog);

            GetText = Text.FindObjectsOfType<Text>();

            if (ApplicationData.SelectedLanguage == LanguageType.Thai)
            {
                txtSelectLanguage.font = ApplicationData.GetFont(4);
                txtShowApp.font = ApplicationData.GetFont(4);
                txtCaution.font = ApplicationData.GetFont(4);
            }
            else
            {
                txtSelectLanguage.font = ApplicationData.GetFont(2);
                txtShowApp.font = ApplicationData.GetFont(2);
                txtCaution.font = ApplicationData.GetFont(2);
            }
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
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
        MapManager.SetupMapImage();
    }
    public void EnableBluetooth()
    {
        BluetoothState.EnableBluetooth();
    }
    public void OpenPopupGuide()
    {
        PopupRed.SetActive(true);
        txtRed.text = ApplicationData.GetLocaleText(LocaleType.Guide);
        txtRed.font = ChangeFont();
        txtRed.lineSpacing = ApplicationData.SetLineSpacing(LocaleType.Guide);
        txtRed.fontSize = ApplicationData.SetFontSize(LocaleType.Guide);
    }
    #endregion
}
