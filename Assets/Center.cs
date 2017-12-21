using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour {
    float name;
    float kana;
	// Use this for initialization
	void Start () {
        name = this.transform.Find("name").GetComponent<RectTransform>().rect.width;
        kana = this.transform.Find("kana").GetComponent<RectTransform>().rect.width;
        this.transform.localPosition = new Vector3((name+kana)/15, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
