using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class YokaiDetail : MonoBehaviour
{

    public static YokaiDetail instane;
    [SerializeField]
    GameObject _image;
    [SerializeField]
    GameObject _name;
    [SerializeField]
    GameObject _description;

    [SerializeField]
    GameObject _library;

    [SerializeField]
    GameObject _detail;

    [SerializeField]
    GameObject _backLibrary;

    private void Start ()
    {
        instane = this;

    }

    void OnEnable ()
    {
        Display ();
    }
    // Use this for initialization
    public void Detail ()
    {

        this.transform.GetChild (0).GetComponent<Image> ().sprite = ApplicationData.YokaiData [ButtonYokai.yokaiName - 1].image;

        for (int j = 0; j < ApplicationData.YokaiData [ButtonYokai.yokaiName - 1].localNames.Count; j++) {

            this.transform.GetChild (0).transform.GetChild (0).GetChild (1).GetComponent<Text> ().text = ApplicationData.YokaiData [ButtonYokai.yokaiName - 1].localNames [j].text.ToString ();
        }

        for (int a = 0; a < ApplicationData.YokaiData [a].localContents.Count; a++) {
            this.transform.GetChild (0).transform.GetChild (0).GetChild (2).GetComponent<Text> ().text = ApplicationData.YokaiData [ButtonYokai.yokaiName - 1].localContents [a].text.ToString ();

        }
    }

    void Display ()
    {
        if (PageData.isShowYokaiDetail) {
            _library.SetActive (false);
            _detail.SetActive (true);
            _backLibrary.SetActive (true);

            if (ApplicationData.YokaiData.Exists (s => s.id == UserData.GetUserInfo ().yokais.Last ().yokai_id)) {
                var d = ApplicationData.YokaiData.Where (s => s.id == UserData.GetUserInfo ().yokais.Last ().yokai_id).First ();
                _image.GetComponent<Image> ().sprite = d.image;
                _name.GetComponent<Text> ().text = d.localNames [0].text;
                _description.GetComponent<Text> ().text = d.localContents [0].text;
            }
        }
        PageData.Initialize ();
    }
}
