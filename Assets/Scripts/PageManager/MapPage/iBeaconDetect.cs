using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public enum BroadcastMode
{
    send = 0,
    receive = 1,
    unknown = 2
}
public enum BroadcastState
{
    inactive = 0,
    active = 1
}

internal class iBeaconDetect : MonoBehaviour
{

    [SerializeField]
    private Button _bluetoothButton;

    [SerializeField]
    private Button _stateButton;

    private Text _bluetoothText;


    private BroadcastMode bm_Mode;

    // Beacon BroadcastState (Start, Stop)
    [SerializeField]
    private Image img_ButtonBroadcastState;

    private BroadcastState bs_State;
    public bool IsBeaconActive {
        get {
            return bs_State == BroadcastState.active;
        }
    }

    // GameObject for found Beacons
    [SerializeField]
    private GameObject go_ScrollViewContent;

    [SerializeField]
    private GameObject go_FoundBeacon;
    List<GameObject> go_FoundBeaconCloneList = new List<GameObject> ();
    GameObject go_FoundBeaconClone;
    private float f_ScrollViewContentRectWidth;
    private float f_ScrollViewContentRectHeight;
    private int i_BeaconCounter = 0;

    // Receive
    private List<Beacon> mybeacons = new List<Beacon> ();

    [SerializeField]
    GameObject GetYokai;
    [SerializeField]
    GameObject _camera;
    Vector3 posCamera;
    [SerializeField]
    GameObject MapImage;
    int _dataID;
    [SerializeField]
    Text titleDialog;

    [SerializeField]
    GameObject btnSuccess;

    [SerializeField]
    GameObject btnGetYokai;


    void Awake ()
    {
        bs_State = BroadcastState.inactive;
        BluetoothState.EnableBluetooth ();
    }

    private void Start ()
    {
        _bluetoothButton.onClick.AddListener (delegate () {
            BluetoothState.EnableBluetooth ();
        });
        _bluetoothText = _bluetoothButton.GetComponentInChildren<Text> ();
        BluetoothState.BluetoothStateChangedEvent += delegate (BluetoothLowEnergyState state) {
            switch (state) {
            case BluetoothLowEnergyState.TURNING_OFF:
            case BluetoothLowEnergyState.TURNING_ON:
                break;
            case BluetoothLowEnergyState.UNKNOWN:
            case BluetoothLowEnergyState.RESETTING:
                break;
            case BluetoothLowEnergyState.UNAUTHORIZED:
                break;
            case BluetoothLowEnergyState.UNSUPPORTED:
                break;
            case BluetoothLowEnergyState.POWERED_OFF:
                _bluetoothButton.interactable = true;
                break;
            case BluetoothLowEnergyState.POWERED_ON:
                _bluetoothButton.interactable = false;
                break;
            default:
                break;
            }
        };
        BluetoothState.Init ();

        _camera = GameObject.FindGameObjectWithTag ("MainCamera");//
        posCamera = new Vector3 (0, 0, -9);
        MapImage = GameObject.Find ("Map").transform.Find ("Map_Image").gameObject;
    }

    void OnDetectBeacon ()
    {
        MapImage.GetComponent<lb_drag> ().enabled = false;
        MapImage.GetComponent<MeshCollider>().enabled = false;
        MapImage.GetComponent<PinchZoom> ().enabled = false;
        var dis = posCamera - MapManager.GetIBeaconIcon (_dataID).transform.position;
        dis.y = 0;
        // MapImage.transform.position = Vector3.MoveTowards (MapImage.transform.position, MapImage.transform.position + dis, 5f);
        //_camera.GetComponent<Camera>().fieldOfView = _camera.GetComponent<Camera>().fieldOfView - 10;
        MapImage.transform.DOMove (MapImage.transform.position + dis, 0.3f);
    }

    void Update ()
    {
        if (CircleController._success && GetYokai.gameObject.activeSelf == false) {

            GetYokai.SetActive (true);
            if (PageData.yokaiID == -1) {
                titleDialog.text = "アイテムを獲得しました！";
            } else {
                titleDialog.text = "妖怪を封印しました！";
            }
            btnSuccess.SetActive (true);
            btnGetYokai.SetActive (false);
        }

    }
    // BroadcastState
    public void btn_StartStop ()
    {
        Debug.Log ("btn_startstop");
        if (!IsBeaconActive) {
            iBeaconReceiver.BeaconRangeChangedEvent += OnBeaconRangeChanged;

            var regions = new List<iBeaconRegion> ();
            foreach (var beacon in ApplicationData.IBeaconData) {
                Debug.Log (beacon.uuid);
                regions.Add (new iBeaconRegion ("vn.javis.yokaiget" + beacon.index.ToString(), new Beacon (beacon.uuid, Convert.ToInt32 (beacon.major_id), Convert.ToInt32 (beacon.minor_id))));
            }
            iBeaconReceiver.regions = regions.ToArray ();
            Debug.Log ("ibeacon before scan");
            iBeaconReceiver.Scan ();
            Debug.Log ("Listening for beacons");
            bs_State = BroadcastState.active;
        } else {
            iBeaconReceiver.Stop ();
            iBeaconReceiver.BeaconRangeChangedEvent -= OnBeaconRangeChanged;
            bs_State = BroadcastState.inactive;
        }
    }


    private void OnBeaconRangeChanged (Beacon [] beacons)
    {
        foreach (Beacon b in beacons) {
            var index = mybeacons.IndexOf (b);
            if (index == -1) {
                mybeacons.Add (b);
            } else {
                mybeacons [index] = b;
            }
        }
        for (int i = mybeacons.Count - 1; i >= 0; --i) {
            if (mybeacons [i].lastSeen.AddSeconds (10) < DateTime.Now) {
                mybeacons.RemoveAt (i);
            }
        }
        DisplayOnBeaconFound ();
    }

    IEnumerator Vibrate()
    {
        for (int i = 0; i < 6; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void DisplayOnBeaconFound ()
    {
        RectTransform rt_Content = (RectTransform)go_ScrollViewContent.transform;
        foreach (Beacon b in mybeacons) {
            if (!UserData.CanDetectIBeacon (b.minor.ToString (), b.major.ToString(), b.UUID.ToUpper())) {
                continue;
            }

            if (ApplicationData.IBeaconData.Exists ((obj) => 
                                                        obj.minor_id == b.minor.ToString ()
                                                    && obj.major_id == b.major.ToString()
                                                    && obj.uuid.ToUpper() == b.UUID.ToUpper())) {
                var beaconData = ApplicationData.IBeaconData.Find ((obj) => 
                                                                   obj.minor_id == b.minor.ToString ()
                                                                   && obj.major_id == b.major.ToString()
                                                                   && obj.uuid.ToUpper() == b.UUID.ToUpper());

                if (!beaconData.IsShowOnMap ()) {
                    continue;
                }

                UserData.DetectIBeacon (b.minor.ToString (), b.major.ToString(), b.UUID.ToUpper());
                _dataID = beaconData.index;
                GetYokai.SetActive (true);
                if (GetYokai.activeSelf)
                {
                    StartCoroutine(Vibrate());
                }
                btnSuccess.SetActive (false);
                btnGetYokai.SetActive (true);
                // StartCoroutine(Complete());

                if (beaconData.iBeaconType == IBeaconType.Item) {
                    //PlayerPrefs.SetInt ("itemID", beaconData.data_id);
                    //PageData.itemID = beaconData.data_id;
                    PageData.SetItemID (beaconData.data_id);
                } else {
                    //PlayerPrefs.SetInt ("yokaiID", beaconData.data_id);
                    //PageData.yokaiID = beaconData.data_id;
                    PageData.SetYokaiID (beaconData.data_id);
                }

                if (beaconData.iBeaconType == IBeaconType.Yokai) {
                    titleDialog.text = "妖 怪 を 見 つけました。封 印 しますか?";
                } else if (beaconData.iBeaconType == IBeaconType.Item) {
                    titleDialog.text = "アイテム を 見 つけました。封 印 しますか?";
                }

                OnDetectBeacon ();
                break;
            }

            go_FoundBeaconClone.transform.SetParent (go_ScrollViewContent.transform);
            //go_FoundBeaconClone.transform.localPosition = new Vector3(0, 0, 0);//

            Debug.Log ("fond Beacon: " + b.ToString ());
        }
    }

}