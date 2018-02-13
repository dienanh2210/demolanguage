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
    GameObject _kana;
    [SerializeField]
    GameObject _description;

    [SerializeField]
    GameObject _library;

    [SerializeField]
    GameObject _detail;

    [SerializeField]
    GameObject _backLibrary;

    private void Start()
    {
        instane = this;
    }

    void OnEnable()
    {
        Display();
    }
    // Use this for initialization
    public void Detail()
    {
        var yokai = ApplicationData.YokaiData.Where(s => s.id == ButtonYokai.yokaiName).First();
        this.transform.GetChild(0).GetComponent<Image>().sprite = yokai.image;

        if (yokai.kana != "" && ApplicationData.SelectedLanguage == LanguageType.Japanese)
        {
			_name.GetComponent<Text>().text = yokai.localNames[(int)ApplicationData.SelectedLanguage].text.ToString();
            _kana.GetComponent<Text>().text = yokai.kana.ToString();
        }
        else
        {
			_name.GetComponent<Text>().text = yokai.localNames[(int)ApplicationData.SelectedLanguage].text.ToString();
            _kana.GetComponent<Text>().text = "";
        }

        for (int a = 0; a < ApplicationData.YokaiData[a].localContents.Count; a++)
        {
			_description.GetComponent<Text>().text = yokai.localContents[(int)ApplicationData.SelectedLanguage].text.ToString();
			if (ApplicationData.SelectedLanguage == LanguageType.Thai) {
				_description.GetComponent<Text> ().font = ApplicationData.GetFont (4);
			} else {
				_description.GetComponent<Text> ().font = ApplicationData.GetFont (2);
			}
        }
    }

    void Display()
    {
        if (PageData.isShowYokaiDetail)
        {
            _library.SetActive(false);
            _detail.SetActive(true);
            _backLibrary.SetActive(true);

            if (ApplicationData.YokaiData.Exists(s => s.id == UserData.GetUserInfo().yokais.Last().yokai_id))
            {
                var d = ApplicationData.YokaiData.Where(s => s.id == UserData.GetUserInfo().yokais.Last().yokai_id).First();
                _image.GetComponent<Image>().sprite = d.image;
                if (d.kana != "" && ApplicationData.SelectedLanguage == LanguageType.Japanese)
                {
					_name.GetComponent<Text>().text = d.localNames[(int)ApplicationData.SelectedLanguage].text;
                    _kana.GetComponent<Text>().text = d.kana;
                }
                else
                {
					_name.GetComponent<Text>().text = d.localNames[(int)ApplicationData.SelectedLanguage].text;
                    _kana.GetComponent<Text>().text = "";
                }
				_description.GetComponent<Text>().text = d.localContents[(int)ApplicationData.SelectedLanguage].text;
				if (ApplicationData.SelectedLanguage == LanguageType.Thai) {
					_description.GetComponent<Text> ().font = ApplicationData.GetFont (4);
				} else {
					_description.GetComponent<Text> ().font = ApplicationData.GetFont (2);
				}
            }
        }
        PageData.Initialize();
    }
}
