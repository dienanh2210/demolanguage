using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YokaiGetEnding : MonoBehaviour {

    [SerializeField]
    Text text;

    public void Show (string message)
    {
        gameObject.SetActive (true);
        text.text = message;
		if (ApplicationData.SelectedLanguage == LanguageType.Thai) {
			text.font = ApplicationData.GetFont (4);
		} else {
			text.font = ApplicationData.GetFont (2);
		}

        if (ApplicationData.SelectedLanguage == LanguageType.English) {
            text.lineSpacing = 0.7f;
        } else {
            text.lineSpacing = 0.9f;
        }
    }

    public void Hide ()
    {
        gameObject.SetActive (false);
    }

    public void GoMapPage ()
    {
        Debug.Log ("GoMapPage");
        PageManager.Show (PageType.MapPage);
    }

}
