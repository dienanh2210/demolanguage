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
