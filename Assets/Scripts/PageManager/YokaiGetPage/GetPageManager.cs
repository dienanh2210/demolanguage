using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class GetPageManager : Page
{
    [SerializeField]
    YokaiGetEnding yokaiGetEnding;

    [SerializeField]
    GameObject backButton;

    public GameObject modelPrefab;
    public GameObject itemPrefab;

    public Material redFire, greenFire, itemMat;

    public GameObject backgroundCam;
    public GameObject imgBall;
    public GameObject model;

    public List<Material> lstMaterial = new List<Material> ();

    GameObject notification;
    GameObject notification_ending;

    GameObject map;
    GameObject worldObj;
    GameObject ball;
    GameObject yokaiCam;
    GameObject catchCircle;
    GameObject neededItemNoti;
    public int count = 1;
    public static int throwCount = 0;
    GameObject mapEffect;
    GameObject sprFire;

    private static GetPageManager instance;

    private GetPageManager ()
    {
        instance = this;
    }

    public static GetPageManager GetInstance ()
    {
        if (instance == null) {
            instance = new GetPageManager ();
        }
        return instance;
    }


    void OnEnable ()
    {
        
        Reset ();
        Camera.main.fieldOfView = 60;
        MapPageManager.instance.SetMapPage (0, 0, -6, RenderMode.ScreenSpaceCamera);
        mapEffect = GameObject.FindGameObjectWithTag ("MapEffect");
        sprFire = mapEffect.transform.GetChild (0).transform.GetChild (0).gameObject;
        mapEffect.transform.GetChild (0).gameObject.SetActive (true);
        if (ApplicationData.GetYokaiData(PageData.yokaiID).isTermLimited) {
            sprFire.GetComponent<MeshRenderer> ().material = greenFire;
        } else {
            sprFire.GetComponent<MeshRenderer> ().material = redFire;
        }


        FireEffect (true);

        yokaiGetEnding.Hide ();
        notification = GameObject.FindGameObjectWithTag ("TextCanvas").transform.Find("notification").gameObject;
        notification_ending = GameObject.FindGameObjectWithTag ("TextCanvas").transform.Find("notification_ending").gameObject;
        neededItemNoti = GameObject.Find ("TextCanvas").transform.Find ("neededItemNoti").gameObject;
        backButton.SetActive (true);

        if (ApplicationData.GetYokaiData (PageData.yokaiID).IsNeedItem ()) {
            if (!ApplicationData.GetYokaiData (PageData.yokaiID).HasItem ()) {

                neededItemNoti.SetActive (true);
                neededItemNoti.GetComponentInChildren<Text> ().text = ApplicationData.GetLocaleText (LocaleType.NoItemMessage1);
                count = 1;
            }

        }

        catchCircle = GameObject.FindGameObjectWithTag ("TextCanvasContent").transform.GetChild (0).gameObject;
        catchCircle.SetActive (true);

        StartCoroutine (TurnTheRawImage());


        #region Map

        yokaiCam = GameObject.FindGameObjectWithTag ("YokaiCamera");
        yokaiCam.transform.GetChild (0).gameObject.SetActive (true);
        map = GameObject.FindGameObjectWithTag ("Map");
        map.transform.GetChild (1).gameObject.SetActive (true);
        map.transform.GetChild (1).gameObject.GetComponent<PinchZoom> ().enabled = false;
        map.transform.GetChild (1).gameObject.GetComponent<lb_drag> ().enabled = false;
        //map.transform.GetChild(1).gameObject.transform.localPosition = new Vector3(0,0,-9);
        #endregion

        Camera.main.GetComponent<GyroCamera> ().enabled = true;

    
        if (PageData.yokaiID != -1) {

            StartCoroutine (ObjectAppear (modelPrefab));

        } else if (PageData.itemID != -1) {
            StartCoroutine (ObjectAppear (itemPrefab));
        }
    }

    IEnumerator TurnTheRawImage(){
        yield return new WaitForSeconds (.5f);
        if (GameObject.FindGameObjectWithTag ("main").transform.childCount >= 2) {
            backgroundCam.SetActive (false);
        } else {
            backgroundCam.GetComponent<CameraAsBackground> ().CameraStart ();
            yield return new WaitForSeconds (.25f);
            backgroundCam.SetActive (true);

        }

    }

    IEnumerator ObjectAppear (GameObject prefab)
    {

        yield return new WaitForSeconds (.1f);


        worldObj = new GameObject ("World Object");
        Vector3 v3Up = new Vector3 (Camera.main.transform.up.x, 0.6f, Camera.main.transform.up.z);
        Vector3 v3Pos = Camera.main.transform.position + v3Up * 7 + Camera.main.transform.forward * 7;
        model = Instantiate (prefab, v3Pos, Quaternion.Euler (0, 0, 0), worldObj.transform);

        YokaiData yokai;
        if (PageData.IsItem) {
            yokai = ApplicationData.GetYokaiDataFromItemId (PageData.itemID);
            model.GetComponentsInChildren<MeshRenderer> (true) [1].material = lstMaterial.Find (x => x.name == yokai.name);
            model.GetComponentsInChildren<MeshRenderer> (true) [1].material.color = Color.black;
            sprFire.GetComponent<MeshRenderer> ().material = itemMat;
            sprFire.transform.localScale = new Vector3 (.4f,.2f,.4f);
        } else {
            yokai = ApplicationData.GetYokaiData (PageData.yokaiID);
            model.GetComponentsInChildren<MeshRenderer> (true) [0].material = lstMaterial.Find (x => x.name == yokai.name);
            sprFire.transform.localScale = new Vector3 (.2f,.2f,.4f);
        }

        if (ApplicationData.GetYokaiData (PageData.yokaiID).isBoss) {
//            model.GetComponentsInChildren<MeshRenderer> (true) [0].material = Resources.Load ("Materials/YokaiMaterials/Boss", typeof (Material)) as Material;
            model.GetComponentsInChildren<MeshRenderer> (true) [0].material = lstMaterial.Find (x => x.name == "syuten-douji");
        }

        model.transform.LookAt (Camera.main.transform);

    }

    void OnDisable ()
    {
        mapEffect.transform.GetChild (0).gameObject.SetActive (false);

        Reset ();
        DOTween.Clear ();

        if (catchCircle != null) {
            catchCircle.SetActive (false);
        }

        map.transform.GetChild (1).gameObject.GetComponent<PinchZoom> ().enabled = true;
        map.transform.GetChild (1).gameObject.GetComponent<lb_drag> ().enabled = true;
        imgBall.SetActive (false);
        yokaiCam.transform.GetChild (0).gameObject.SetActive (false);
        map.transform.GetChild (1).gameObject.SetActive (false);
        Camera.main.GetComponent<GyroCamera> ().enabled = false;
        if (worldObj != null) {
            Destroy (worldObj);
        }
        GameObject.Find ("TextCanvas").transform.Find ("sprBall").gameObject.SetActive (false);
        MirrorFlipCamera.instance.flipHorizontal = false;
        notification.SetActive (false);
        notification_ending.SetActive (false);
        neededItemNoti.SetActive (false);
        throwCount = 0;
        CircleController.instance.StopWaitForSuccessMessage ();

    }

    void FireEffect (bool isDown)
    {
        //on map
        if (isDown) {
            mapEffect.GetComponentsInChildren<Transform> (true) [2].DOLocalMoveZ (-.5f, .5f).SetEase (Ease.Linear).OnComplete (() => {
                FireEffect (!isDown);
            });
        } else {
            mapEffect.GetComponentsInChildren<Transform> (true) [2].DOLocalMoveZ (.3f, .5f).SetEase (Ease.Linear).OnComplete (() => {
                FireEffect (!isDown);
            });
        }
    }
    public void ChangeMapPage()
    {
        PageManager.Show(PageType.MapPage);
    }
    public void ShowEnding (string message)
    {
        Debug.Log ("ShowEnding");
        yokaiGetEnding.Show (message);
        yokaiCam.transform.GetChild (0).gameObject.SetActive (false);
    }

    public static void HideBackButton ()
    {
        instance.backButton.SetActive (false);
    }

    void Reset ()
    {
        imgBall.transform.localPosition = new Vector3 (0, -406, 0);
        imgBall.transform.localScale = new Vector3 (2, 2, 2);
        GameObject txtGz = GameObject.Find ("TextCanvas").transform.Find ("txtGz").gameObject;
        txtGz.SetActive (false);
        txtGz.transform.localPosition = new Vector3 (0,645,0);
        GameObject txtGzItem = GameObject.Find ("TextCanvas").transform.Find ("txtGzItem").gameObject;
        txtGzItem.SetActive (false);
        txtGzItem.transform.localPosition = new Vector3 (0,645,0);
    }

    public void Fail ()
    {
        if (!CircleController._success) {
            MapPageManager._failGetYokai = true;
        }
    }
}
