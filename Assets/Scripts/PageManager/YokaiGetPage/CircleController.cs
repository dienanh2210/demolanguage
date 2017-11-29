using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CircleController : MonoBehaviour
{

    public static CircleController instance;
    public static bool _success;
    public Sprite red, blue;
    private bool canThrow = false;
    private bool isExist = false;
    private bool isCatch = false;
    private GameObject TextController;
    private GameObject txtGz;
    private GameObject backLight;
    private bool isMiddle = false;
    public Transform cam;
    public LayerMask mask;
    bool onFocus;
    bool isBallFlying = false;
    public bool ok = false;
    GameObject notification;
    GameObject notification_ending;
    int throwCount = 0;

    void Awake ()
    {
        instance = this;
    }
        
    void Update ()
    {

        if (this.transform.localScale.x < 3.7f) {

            this.GetComponent<Image> ().sprite = red;
            if (this.transform.localScale.x < 2.8f) {
                this.GetComponent<Image> ().DOFade (0, .0001f);
            }

        } else {
            this.GetComponent<Image> ().sprite = blue;
            this.GetComponent<Image> ().DOFade (1, .0001f);
        }

        if (GetPageManager.throwCount == 3) {
            this.gameObject.SetActive (false);
        }

    }

    #region Check: is Yokai in the middle of the circle
    void FixedUpdate ()
    {
        Vector3 fwd = transform.TransformDirection (Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast (cam.transform.position + new Vector3 (0, .8f, 0), cam.forward, out hit, 20, mask)) {
            Debug.Log (hit.collider.gameObject.name);
            if (!onFocus) {
//                DOTween.Clear ();
                CircleScale ();
                onFocus = true;
                canThrow = true;
            }
        } else {
            canThrow = false;
            this.transform.DOScale (new Vector3 (8.5f, 8.5f, 8.5f), .5f).SetEase (Ease.Linear);
            onFocus = false;
            this.GetComponent<Image> ().DOFade (1, 0.00001f).SetEase (Ease.Linear);

        }
    }
    #endregion

    void CircleScale ()
    {
        this.transform.DOScale (new Vector3 (2.7f, 2.7f, 2.7f), 1f).SetEase (Ease.Linear).SetLoops (-1);
    }

    public void OnClick ()
    {
        if (canThrow && !isBallFlying) {
            DOTween.Clear ();
            MovingBall ();

        }
    }

    void MovingBall ()
    {
        GameObject.FindGameObjectWithTag ("YokaiGetPage").transform.Find ("Button").GetComponent<Button> ().enabled = false;
        Debug.Log ("GetPageManager.throwCount " + GetPageManager.throwCount);
        isBallFlying = true;
        throwCount++;
        if (UserData.IsShowedMessageForMiddleEndingBeforeBoss && !UserData.IsPassedBossOnce) {
            isBallFlying = false;
            GetPageManager.HideBackButton ();
            GetPageManager.throwCount++;
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.SetActive (true);

            var sequence = DOTween.Sequence ();
            sequence.Append (

                GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.DOLocalMoveY (0, .5f).SetEase (Ease.Linear).OnComplete (() => {
                    GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.SetActive (false);
                })
            );
            sequence.Join (
                GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.DOScale (new Vector3 (1, 1, 1), .5f).SetEase (Ease.Linear)

            );

            this.gameObject.SetActive (true);
            this.transform.localScale = new Vector3 (8.5f, 8.5f, 8.5f);
            CircleScale ();
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.localPosition = new Vector3 (0, -4f, .2f);
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.localScale = new Vector3 (2, 2, 2);

            notification = GameObject.Find ("TextCanvas").transform.Find ("notification").gameObject;

            if (GetPageManager.throwCount == 1) {

                notification.SetActive (true);
                notification.GetComponentInChildren<Text> ().text = ApplicationData.GetLocaleText (LocaleType.MiddleBossCrawMessage1);

            }
            if (GetPageManager.throwCount == 2) {
                notification.GetComponentInChildren<Text> ().text = ApplicationData.GetLocaleText (LocaleType.MiddleBossCrawMessage2);

            }
            if (GetPageManager.throwCount == 3) {
                notification.GetComponentInChildren<Text> ().text = ApplicationData.GetLocaleText (LocaleType.MiddleBossCrawMessage3);
                Invoke ("FadeOut", 1);
                Invoke ("EndingAfterMiddleBoss", 2f);
            }


        } else {

            if (PageData.IsItem) {

                GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.SetActive (true);
                var sequence = DOTween.Sequence ();
                sequence.Append (

                    GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.DOLocalMoveY (0, .5f).SetEase (Ease.Linear).OnComplete (() => {
                        StartCoroutine (CatchYokai ());
                    })
                );
                sequence.Join (
                    GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.DOScale (new Vector3 (1, 1, 1), .5f).SetEase (Ease.Linear)

                );
            } else {

                GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.SetActive (true);
                var sequence = DOTween.Sequence ();
                sequence.Append (

                    GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.DOLocalMoveY (0, .5f).SetEase (Ease.Linear).OnComplete (() => {
                        StartCoroutine (CatchYokai ());
                    })
                );
                sequence.Join (
                    GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.DOScale (new Vector3 (1, 1, 1), .5f).SetEase (Ease.Linear)

                );
            }
        }


    }

    void EndingAfterMiddleBoss ()
    {
        EndingAfterBoss (ApplicationData.GetLocaleText (LocaleType.MiddleEndingAfterBoss));
    }

    void EndingAfterLastBoss ()
    {
        GetPageManager.HideBackButton ();
        GameObject.Find ("TextCanvas").transform.Find ("txtGz").gameObject.SetActive (false);
        GameObject.Find("TextCanvas").transform.Find ("sprEffect").gameObject.SetActive (false);
        GameObject.Find("TextCanvas").transform.Find ("sprBall").gameObject.SetActive (false);
        GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.SetActive (false);
        EndingAfterBoss (ApplicationData.GetLocaleText (LocaleType.LastEndingAfterBoss));
        isCatch = true;
    }

    void EndingAfterBoss (string message)
    {
        Debug.Log ("message : "+ message);
        throwCount = 0;
        notification = GameObject.Find ("TextCanvas").transform.Find ("notification").gameObject;
        notification.SetActive (false);

        GetPageManager.GetInstance ().ShowEnding (message);


        UserData.IsPassedBossOnce = true;
    }

    void FadeOut ()
    {
        GameObject.FindGameObjectWithTag ("Model").transform.GetChild (0).gameObject.GetComponent<MeshRenderer> ().material.DOFade (0, 1f);
    }

    IEnumerator CatchYokai ()
    {
        this.gameObject.SetActive (false);

        if (PageData.IsItem) {
            GetPageManager.GetInstance ().model.GetComponentsInChildren<MeshRenderer> (true) [2].gameObject.SetActive (false);
        } else {
            GetPageManager.GetInstance ().model.GetComponentsInChildren<MeshRenderer> (true) [1].gameObject.SetActive (false);
        }

        //if (ApplicationData.GetYokaiData (PlayerPrefs.GetInt ("yokaiID")).IsNeedItem () && !ApplicationData.GetYokaiData (PlayerPrefs.GetInt ("yokaiID")).HasItem ()) {
        if (ApplicationData.GetYokaiData (PageData.yokaiID).IsNeedItem () && !ApplicationData.GetYokaiData (PageData.yokaiID).HasItem ()) {
            this.gameObject.SetActive (true);
            this.transform.localScale = new Vector3 (8.5f, 8.5f, 8.5f);
            CircleScale ();
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.SetActive (false);

            GetPageManager.GetInstance ().model.GetComponentsInChildren<MeshRenderer> (true) [0].gameObject.SetActive (true);

            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.localPosition = new Vector3 (0, -4f, .2f);
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.localScale = new Vector3 (2, 2, 2);
            GameObject.FindGameObjectWithTag ("YokaiGetPage").transform.Find ("Button").GetComponent<Button> ().enabled = true;
            isBallFlying = false;
        } else {
            Catching ();
        }
        yield return new WaitForSeconds (1);
    }

    void Catching ()
    {
        if (this.transform.localScale.x > 3.7f) {

            Debug.Log ("fail");
            this.gameObject.SetActive (true);
            this.transform.localScale = new Vector3 (8.5f, 8.5f, 8.5f);
            CircleScale ();
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.SetActive (false);

            GetPageManager.GetInstance ().model.GetComponentsInChildren<MeshRenderer> (true) [0].gameObject.SetActive (true);

            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.localPosition = new Vector3 (0, -4f, .2f);
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.transform.localScale = new Vector3 (2, 2, 2);
            isBallFlying = false;
            GameObject.FindGameObjectWithTag ("YokaiGetPage").transform.Find ("Button").GetComponent<Button> ().enabled = true;
        } else {


            Debug.Log ("Succesful!");
            if (PageData.IsItem) {
                UserData.SuccessGetItem (PageData.itemID, -1);
            } else {
                UserData.SuccessGetYokai (PageData.yokaiID, -1);
                if (UserData.IsGotBoss) {
                    UserData.CreateTicket ();
                }

            }
            isCatch = true;

            //Active the effect and rotate it
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (1).gameObject.SetActive (true);
            Vector3 v3 = GameObject.FindGameObjectWithTag ("Model").transform.GetChild (1).gameObject.transform.localRotation.eulerAngles;
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (1).gameObject.transform.DOLocalRotateQuaternion (Quaternion.Euler (-90, 90, 90), 0.5f).SetEase (Ease.Linear).SetLoops (-1);
            GameObject.FindGameObjectWithTag ("Model").transform.GetChild (1).gameObject.transform.DOScale (new Vector3 (.3f, .3f, .3f), .125f).SetEase (Ease.Linear).OnComplete (() => {
                GetPageManager.GetInstance ().model.GetComponentsInChildren<MeshRenderer> (true) [0].transform.DOScale (new Vector3 (.1f, .1f, .1f), .375f).SetEase (Ease.Linear);
                GameObject.FindGameObjectWithTag ("Model").transform.GetChild (1).gameObject.transform.DOScale (new Vector3 (.15f, .15f, .15f), .375f).SetEase (Ease.Linear).OnComplete (() => {
                    Invoke ("Faded", 0.1875f);
                });

            });

            //            if (ApplicationData.YokaiData[(PlayerPrefs.GetInt("yokaiID"))-1].isBoss && UserData.IsPassedBossOnce) {
            //                       
            //                
            //            }
            if (UserData.IsShowedMessageForLastEnding && ApplicationData.GetYokaiData ((PageData.yokaiID)).isBoss) {
                

                GetPageManager.HideBackButton ();
                Invoke ("EndingAfterLastBoss", 3f);
            }

            _success = true;

            Invoke ("WaitForASecond", 1);
        }
    }

    void EndingNotification ()
    {
        GameObject.FindGameObjectWithTag ("Model").transform.GetChild (2).gameObject.SetActive (false);
        GameObject.FindGameObjectWithTag ("TextCanvas").transform.GetChild (2).gameObject.SetActive (false);
        notification_ending = GameObject.Find ("TextCanvas").transform.Find ("notification_ending").gameObject;
        notification_ending.SetActive (true);
        notification_ending.GetComponentInChildren<Text> ().text = ApplicationData.GetLocaleText (LocaleType.LastEndingAfterBoss);
        isCatch = true;
    }


    void WaitForASecond ()
    {
        GameObject.Find ("TextCanvas").transform.Find ("sprBall").gameObject.SetActive (true);
        TextController = GameObject.FindGameObjectWithTag ("TextCanvas");
        if (!PageData.IsItem) {
            txtGz = TextController.transform.GetChild (2).gameObject;
            txtGz.SetActive (true);
            txtGz.GetComponent<Image> ().sprite = ApplicationData.GetSuccessImage (ApplicationData.SelectedLanguage).yokaiText;
            txtGz.GetComponent<Transform> ().DOScale (new Vector3 (1.6f, 1.6f, 1.6f), .3f).SetEase (Ease.Linear).OnComplete (() => {
                txtGz.GetComponent<Transform> ().DOScale (new Vector3 (1.4f, 1.4f, 1.4f), .5f).SetEase (Ease.Linear);
            });

        } else {
            txtGz = TextController.transform.GetChild (3).gameObject;
            txtGz.SetActive (true);
            txtGz.GetComponent<Image> ().sprite = ApplicationData.GetSuccessImage (ApplicationData.SelectedLanguage).itemText;
            txtGz.GetComponent<Transform> ().DOScale (new Vector3 (1.6f, 1.6f, 1.6f), .3f).SetEase (Ease.Linear).OnComplete (() => {
                txtGz.GetComponent<Transform> ().DOScale (new Vector3 (1.4f, 1.4f, 1.4f), .5f).SetEase (Ease.Linear);
            });
        }
        backLight = TextController.transform.Find ("sprEffect").gameObject;
        backLight.SetActive (true);
        backLight.GetComponent<Image> ().DOFade (.3f, 1f).SetEase (Ease.Linear).OnComplete (() => {
            backLight.GetComponent<Image> ().DOFade (1f, 1f).SetEase (Ease.Linear);
        }).SetLoops (-1);
    }

    void Faded ()
    {

        GameObject.FindGameObjectWithTag ("Model").transform.GetChild (1).gameObject.GetComponent<MeshRenderer> ().material.DOFade (0, .1875f);

        GetPageManager.GetInstance ().model.GetComponentsInChildren<MeshRenderer> (true) [0].material.DOFade (0, .1875f).SetEase (Ease.Linear).OnComplete (() => {
            GetPageManager.GetInstance ().model.GetComponentsInChildren<MeshRenderer> (true) [0].gameObject.SetActive (false);
            GameObject.FindGameObjectWithTag ("Model").transform.Find("Ball").gameObject.SetActive (false);
            GameObject.FindGameObjectWithTag ("YokaiGetPage").transform.Find ("Button").GetComponent<Button> ().enabled = true;
        });
        isBallFlying = false;

    }

    void MoveSprite ()
    {
        GetPageManager.GetInstance ().imgBall.SetActive (true);
        GetPageManager.GetInstance ().imgBall.transform.DOLocalMoveY (350, .5f).SetEase (Ease.Linear);
        GetPageManager.GetInstance ().imgBall.transform.DOScale (new Vector3 (1, 1, 1), .5f).SetEase (Ease.Linear);
    }

    public void StopWaitForSuccessMessage ()
    {
        CancelInvoke ("WaitForASecond");
    }

}

public static class RendererExtensions
{
    public static bool isVisibleFrom (this Renderer renderer, Camera camera)
    {
        Plane [] planes = GeometryUtility.CalculateFrustumPlanes (camera);
        return GeometryUtility.TestPlanesAABB (planes, renderer.bounds);
    }
}


