using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Page : MonoBehaviour
{

    public static Page instance;

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    private void Start()
    {
        instance = this;
    }

	public Font ChangeFont(){
		if (ApplicationData.SelectedLanguage == LanguageType.Thai) {
			return ApplicationData.GetFont (4);
			Debug.Log (1);
		} else {
			return ApplicationData.GetFont (2);
			Debug.Log (2);
		}
	}

	public void SetFont(Text[] txt){
		for (int i = 0; i < txt.Length; i++) {
			txt [i].font = ChangeFont ();
		}
	}
   
}
