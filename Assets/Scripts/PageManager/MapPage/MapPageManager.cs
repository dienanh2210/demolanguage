using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MapPageManager : Page
{
    public static MapPageManager instance;

    public InputField itemID;
    public InputField yokaiID;
    public GameObject btnParkNavi;
    GameObject touchPad;

    [SerializeField]
    Transform getYokai;

    [SerializeField]
    Transform MapPage;

    GameObject camera;

    public GameObject map;
    GameObject _canvas;
    bool firstTime = false;
    public static bool _backToMappage;
    [SerializeField]
    GameObject DialogEnding;
    [SerializeField]
    GameObject MiddleEnding;
    [SerializeField]
    GameObject LastEnding;
    [SerializeField]
    GameObject DialogGuidePlay;
    int _next;
    [SerializeField]
    GameObject btnShowYokaiLibrary;
    [SerializeField]
    GameObject btnShowRewardList;
    [SerializeField]
    GameObject btnGuidePlay;
    [SerializeField]
    GameObject btnClose;
    [SerializeField]
    GameObject exitAlertBoard;
    //public Text test;
    iBeaconDetect iBeaconDetect;
    [SerializeField]
    List<Text> lstText_HowToPlay = new List<Text>();
    [SerializeField]
    Text txtOpenEsashinavi;

    public static bool _failGetYokai;
    void Awake()
    {
        instance = this;
        iBeaconDetect = GetComponent<iBeaconDetect>();
    }

    void Start()
    {
        map = GameObject.Find("Map").transform.Find("Map_Image").gameObject;
        _canvas = GameObject.Find("Canvas");
        SetMapPage(5, 90, -9, RenderMode.ScreenSpaceOverlay);
        ChangeLanguageExitAlert();
    }

    void OnEnable()
    {
        MapManager.SetupIcon();
        DOTween.Clear();
        GameObject.Find("TextCanvas").transform.Find("txtGz").gameObject.SetActive(false);
        GameObject.Find("TextCanvas").transform.Find("txtGzItem").gameObject.SetActive(false);
        GameObject.Find("TextCanvas").transform.Find("notification").gameObject.SetActive(false);
        GameObject.Find("TextCanvas").transform.Find("notification_ending").gameObject.SetActive(false);
        GameObject.Find("TextCanvas").transform.Find("sprEffect").gameObject.SetActive(false);
        GameObject.Find("TextCanvas").transform.Find("sprBall").gameObject.SetActive(false);
        itemID.text = "";
        yokaiID.text = "";

        MapManager.SetupIcon();

        StartCoroutine(_EnableBeacon());

        btnShowYokaiLibrary.transform.GetChild(0).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.ButtonYokaiLibrary);
        btnShowRewardList.transform.GetChild(0).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.ButtonBonus);
        btnGuidePlay.transform.GetChild(0).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.ButtonHowToPlay);
        btnGuidePlay.transform.GetChild(0).GetComponent<Text>().font = ChangeFont();
        btnGuidePlay.transform.GetChild(0).GetComponent<Text>().fontSize = ApplicationData.SetFontSize(LocaleType.ButtonHowToPlay);
        btnClose.transform.GetChild(0).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.ButtonClose);
        if (ApplicationData.SelectedLanguage == LanguageType.Thai)
        {
            btnClose.transform.GetChild(0).GetComponent<Text>().font = ApplicationData.GetFont(4);
            foreach(var t in lstText_HowToPlay) {
                t.font = ApplicationData.GetFont(4);
            }
        }
        else
        {
            btnClose.transform.GetChild(0).GetComponent<Text>().font = ApplicationData.GetFont(2);
            foreach(var t in lstText_HowToPlay) {
                t.font = ApplicationData.GetFont(2);
            }
        }

        txtOpenEsashinavi.text = ApplicationData.GetLocaleText(LocaleType.ButtonParkNavi);
        txtOpenEsashinavi.font = GetComponent<Text>().font = ChangeFont();
    }

    IEnumerator _EnableBeacon()
    {
        yield return new WaitForSeconds(1f);
        if (!iBeaconDetect.IsBeaconActive)
        {
            iBeaconDetect.btn_StartStop();
        }
    }

    void OnDisable()
    {
        if (iBeaconDetect.IsBeaconActive)
        {
            iBeaconDetect.btn_StartStop();
        }
        OffMap_Click();
    }

    void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            // iBeaconDetect.btn_StartStop ();
        }
        else
        {
            // iBeaconDetect.btn_StartStop ();
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowExitDialog(true);
        }

        if (this.gameObject.activeSelf)
        {
            SetMapImage(true);
            SetMapPage(5, 90, -9, RenderMode.ScreenSpaceOverlay);
        }

        // if(_backToMappage) {
        if (true)
        {
            _backToMappage = false;
            if (ApplicationLogic.IsShowMessageForMiddleEnding()
                && !UserData.IsShowedMessageForMiddleEndingBeforeBoss)
            {
                DialogEnding.SetActive(true);
                MiddleEnding.SetActive(true);
                LastEnding.SetActive(false);
                MiddleEnding.transform.GetChild(0).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.MiddleEnding2);
                MiddleEnding.transform.GetChild(0).GetComponent<Text>().font = ChangeFont();
                UserData.IsShowedMessageForMiddleEndingBeforeBoss = true;
                MapManager.SetupIcon();
            }
            else if (UserData.IsPassedBossOnce && !UserData.IsShowedMessageForMiddleEnding)
            {
                DialogEnding.SetActive(true);
                MiddleEnding.SetActive(true);
                LastEnding.SetActive(false);
                MiddleEnding.transform.GetChild(0).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.MiddleEnding1);
                UserData.IsShowedMessageForMiddleEnding = true;
                MapManager.SetupIcon();
            }
            else if (ApplicationLogic.IsShowMessageForLastEnding()
                   && !UserData.IsShowedMessageForLastEnding)
            {
                DialogEnding.SetActive(true);
                LastEnding.SetActive(true);
                MiddleEnding.SetActive(false);
                LastEnding.transform.GetChild(0).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.LastEnding);
                LastEnding.transform.GetChild(0).GetComponent<Text>().font = ChangeFont();
                UserData.IsShowedMessageForLastEnding = true;
                MapManager.SetupIcon();
            }

        }

        if (_failGetYokai)
        {
            _failGetYokai = false;
            DisplayFailCase();
        }
    }
    public void ShowExitDialog(bool isShown)
    {

        if (exitAlertBoard)
        {
            exitAlertBoard.SetActive(isShown);
        }

    }
    public void AgreeExit(bool agree)
    {
        if (agree)
            Application.Quit();
        else
            ShowExitDialog(false);
    }
    void ChangeLanguageExitAlert()
    {
        if (!exitAlertBoard)
            return;
      
        exitAlertBoard.transform.Find("txtContent").GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.ExitAlert);
        exitAlertBoard.transform.Find("btnNo").GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.ButtonCancelQuit);
        exitAlertBoard.transform.Find("btnYes").GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.ButtonQuit);
    }
    void DisplayFailCase()
    {
        DialogEnding.SetActive(true);
        LastEnding.SetActive(true);
        MiddleEnding.SetActive(false);
        LastEnding.transform.GetChild(0).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.WaitNextDetect);
    }

    public void SetMapPage(int posY, int ros, float posZ, RenderMode renderMode)
    {
        camera = GameObject.Find("Main Camera");
        camera.transform.position = new Vector3(camera.transform.position.x, posY, posZ);
        camera.transform.rotation = Quaternion.Euler(ros, camera.transform.rotation.y, camera.transform.rotation.z);
        _canvas.GetComponent<Canvas>().renderMode = renderMode;
    }

    void SetMapImage(bool ac)
    {
        map.SetActive(ac);
    }

    public void OffMap_Click()
    {
        map.SetActive(false);
        SetMapPage(0, 0, -9, RenderMode.ScreenSpaceCamera);
    }


    public void btnClose_Click()
    {
        getYokai.gameObject.SetActive(false);
        map.GetComponent<lb_drag>().enabled = true;
        map.GetComponent<MeshCollider>().enabled = true;
        map.GetComponent<PinchZoom>().enabled = true;

        if (!CircleController._success)
        {
            DisplayFailCase();
        }
        CircleController._success = false;
    }

    public void ChangeLibraryPage()
    {
        PageManager.Show(PageType.Library);
    }

    public void ChangeBonusPage()
    {
        PageManager.Show(PageType.BonusPage);
    }
    public void ChangeYokaiGetPage()
    {
        if (UserData.IsShowedYokaiTutorial && PageData.yokaiID != -1)
        {
            PageManager.Show(PageType.YokaiGetPage);
        }
        else if (UserData.IsShowItemTutorial && PageData.itemID != -1)
        {
            PageManager.Show(PageType.YokaiGetPage);
        }
        else
        {
            PageManager.Show(PageType.YokaiGetTutorialPage);
        }

    }
    public void btnGetYokai_Click()
    {
        //test.text = UserData.IsShowedYokaiTutorial.ToString() + "  " + PageData.yokaiID.ToString();
        _backToMappage = true;
        ChangeYokaiGetPage();
        getYokai.gameObject.SetActive(false);
        SetMapPage(0, 0, -6, RenderMode.ScreenSpaceCamera);
    }
    public void btnSuccess_Click()
    {
        CircleController._success = false;
        if (PageData.IsYokai)
        {
            PageData.ShowYokaiDetail();
        }
        PageManager.Show(PageType.Library);

        getYokai.gameObject.SetActive(false);
    }

    public void OnClickYokai()
    {
        if (yokaiID.text != "")
        {
            //PlayerPrefs.SetInt("yokaiID", int.Parse(yokaiID.text));
            //PageData.yokaiID = int.Parse(yokaiID.text);
            PageData.SetYokaiID(int.Parse(yokaiID.text));

        }
    }

    public void OnClickItem()
    {
        if (itemID != null)
        {
            //PlayerPrefs.SetInt("itemID", int.Parse(itemID.text));
            //PageData.itemID = int.Parse(itemID.text);
            PageData.SetItemID(int.Parse(itemID.text));
        }
    }

    public void btnNext_Click(int i)
    {
        _next++;
        if (_next < i)
        {
            if (_next == 1)
            {
                // MiddleEnding.transform.GetChild(0).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.MiddleEnding2);
            }
        }
        else
        {
            _next = 0;
            DialogEnding.SetActive(false);
        }
    }

    public void DebugGetArea34()
    {
        var area3s = ApplicationData.YokaiData.FindAll((obj) =>
        {
            var ibeacon = ApplicationData.IBeaconData.Find(
                (beacon) => beacon.iBeaconType == IBeaconType.Yokai
                && beacon.data_id == obj.id);
            return ibeacon.area == 3;
        });
        foreach (var y in area3s)
        {
            UserData.SuccessGetYokai(y.id, -1);
        }

        var area4s = ApplicationData.YokaiData.FindAll((obj) =>
        {
            var ibeacon = ApplicationData.IBeaconData.Find(
                (beacon) => beacon.iBeaconType == IBeaconType.Yokai
                && beacon.data_id == obj.id);
            return ibeacon.area == 4;
        });
        foreach (var y in area4s)
        {
            UserData.SuccessGetYokai(y.id, -1);
        }
        MapManager.SetupIcon();
    }

    public void DebugGetArea12()
    {
        var area1s = ApplicationData.YokaiData.FindAll((obj) =>
        {
            var ibeacon = ApplicationData.IBeaconData.Find(
                (beacon) => beacon.iBeaconType == IBeaconType.Yokai
                && beacon.data_id == obj.id);
            return ibeacon.area == 1;
        });
        foreach (var y in area1s)
        {
            UserData.SuccessGetYokai(y.id, -1);
        }

        var area2s = ApplicationData.YokaiData.FindAll((obj) =>
        {
            var ibeacon = ApplicationData.IBeaconData.Find(
                (beacon) => beacon.iBeaconType == IBeaconType.Yokai
                && beacon.data_id == obj.id);
            return ibeacon.area == 2;
        });
        foreach (var y in area2s)
        {
            UserData.SuccessGetYokai(y.id, -1);
        }
        MapManager.SetupIcon();
    }

    public void btnGuidePlay_Click()
    {
        DialogGuidePlay.SetActive(true);

        lstText_HowToPlay[0].text = ApplicationData.GetLocaleText(LocaleType.how_to_play_1);
        lstText_HowToPlay[1].text = ApplicationData.GetLocaleText(LocaleType.how_to_play_2);
        lstText_HowToPlay[2].text = ApplicationData.GetLocaleText(LocaleType.how_to_play_yokai);
        lstText_HowToPlay[3].text = ApplicationData.GetLocaleText(LocaleType.how_to_play_item);
        lstText_HowToPlay[4].text = ApplicationData.GetLocaleText(LocaleType.how_to_play_3);
        lstText_HowToPlay[5].text = ApplicationData.GetLocaleText(LocaleType.how_to_play_message);
        lstText_HowToPlay[6].text = ApplicationData.GetLocaleText(LocaleType.ButtonPrologue);

        foreach (var text in lstText_HowToPlay) {
            if (ApplicationData.SelectedLanguage == LanguageType.English) {
                text.lineSpacing = 0.5f;
            } else {
                text.lineSpacing = 0.7f;
            }
        }
        
        for(int i = 0; i < lstText_HowToPlay.Count; i++)
        {
            lstText_HowToPlay[i].font = ChangeFont();
        }
    }
    public void ChangeTutorialPage()
    {
        PageManager.Show(PageType.Tutorial);
        TutorialManager.count = 0;
        TutorialManager.Instance.ResetPage();
    }

    public void OpenEsashinavi()
    {
        TitlePage.instance.OpenURL();
    }
}
