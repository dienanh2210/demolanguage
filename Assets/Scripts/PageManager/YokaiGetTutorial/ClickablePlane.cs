using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
public class ClickablePlane : MonoBehaviour,  IPointerClickHandler{
    public static bool isClick = false;
    public GameObject monster;
    public GameObject sprEffect;
    public GameObject sprBall;
    public GameObject yokaiBtn;
    public GameObject itemBtn;
    public GameObject sprBackLight;
    GameObject txtGz;
    #region IPointerClickHandler implementation

    public void OnPointerClick (PointerEventData eventData)
    {
        this.gameObject.SetActive (false);

        sprBall.SetActive (true);
        sprBall.transform.DOLocalMoveY (-132, .5f).SetEase (Ease.Linear);
        sprBall.transform.DOScale (new Vector3 (1, 1, 1), .5f).SetEase (Ease.Linear);
        Invoke ("SetVariable", .5f);
        if (this.transform.localScale.x > .8f) {
            Debug.Log ("fail");
            Invoke ("CatchingObject", .5f);
        }else{
            Debug.Log ("success");
            Invoke ("Effect", .5f);
        }

    }

    #endregion

    void CatchingObject(){
        isClick = false;
        this.gameObject.SetActive (true);

        sprBall.SetActive (false);
        sprBall.transform.localPosition = new Vector3 (-1.5f, -954, -1);
        sprBall.transform.localScale = new Vector3 (2, 2, 2);
    }

    void SetVariable(){
        isClick = true;
    }

    public void CircleScale(){
        this.transform.DOScale (new Vector3 (.6f, .6f, .6f), 1f).SetEase (Ease.Linear).SetLoops (-1);

    }

    void OnEnable(){
        this.transform.localScale = new Vector3 (1.2f, 1.2f, 1.2f);
    }


    void Effect(){
        Debug.Log ("effect");

        sprEffect.SetActive (true);
        sprEffect.transform.DOLocalRotateQuaternion (Quaternion.Euler (0, 0, -179), 0.5f).SetEase(Ease.Linear).SetLoops (-1);
        sprEffect.transform.DOScale (new Vector3 (1.2f, 1.2f, 1.2f), .25f).SetEase (Ease.Linear).OnComplete (() => {
            sprEffect.transform.DOScale (new Vector3 (.4f, .4f, .4f), .5f).SetEase (Ease.Linear);
        });
        Invoke ("Faded", .25f);

    }

    void Faded(){
        sprEffect.GetComponent<Image> ().DOFade (0, 1f).SetEase (Ease.Linear);
        monster.transform.DOScale (new Vector3 (.5f, .5f, .5f), .5f).SetEase (Ease.Linear);
        monster.GetComponent<Image> ().DOFade (0, 1f).SetEase (Ease.Linear).OnComplete(()=>{
            monster.SetActive(false);

        });

        if (YokaiGetTutorialManager.instance.objYokai.activeSelf) {
            txtGz = GameObject.Find ("TextCanvas").transform.Find ("txtGz").gameObject;
            txtGz.SetActive (true);

        }
        if (YokaiGetTutorialManager.instance.objItem.activeSelf) {
            txtGz = GameObject.Find ("TextCanvas").transform.Find ("txtGzItem").gameObject;
            txtGz.SetActive (true);
        }
        sprBackLight.SetActive (true);
        sprBackLight.GetComponent<Image> ().DOFade (.3f, .5f).SetEase (Ease.Linear).OnComplete (() => {
            sprBackLight.GetComponent<Image> ().DOFade (1f, .5f).SetEase (Ease.Linear);
        }).SetLoops (-1);

        txtGz.transform.localPosition = new Vector3 (0, 563, 1000);
        txtGz.GetComponent<Transform> ().DOScale (new Vector3 (1.3f, 1.3f, 1.3f), .3f).SetEase (Ease.Linear).OnComplete (() => {
            txtGz.GetComponent<Transform> ().DOScale (new Vector3 (1f, 1f, 1f), .5f).SetEase (Ease.Linear);
        });

        Invoke ("DisplayButton",1);
    }

    void DisplayButton(){
        if (YokaiGetTutorialManager.instance.objYokai.activeSelf) {
            yokaiBtn.SetActive (true);
        }
        if (YokaiGetTutorialManager.instance.objItem.activeSelf) {
            itemBtn.SetActive (true);
        }

    }

}
