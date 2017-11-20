using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ButtonYokai : Page {

    public static int yokaiName;

    [SerializeField]
    GameObject library;

    [SerializeField]
    GameObject detail;

    [SerializeField]
    GameObject backLibrary;
    [SerializeField]
    GameObject content;

    static int count;

    public static ButtonYokai bnYokai;

    void Awake()
    {
        if (ApplicationSystem.IsIphoneX())
        {
            detail.GetComponent<GridLayoutGroup>().padding.top = 500;
        }
    }
    private void Start()
    {
        count = 1;
        bnYokai = this;
    }

    public void ClickYokai(int name)
    {
        yokaiName = name;

        if(content.transform.Find("square"+name).GetChild(0).GetComponent<Image>().color==Color.white && content.transform.Find("square" + name).GetChild(0).GetComponent<Image>().sprite.name !="item")
        {
            YokaiDetail.instane.Detail();
            library.SetActive(false);
            backLibrary.SetActive(true);
        }
            
           
        

    }
  

    public void BackMapPage()
    {
        Page.instance.ChangeMapPage();
    }

    public void BackLibrary()
    {
        //YokaiDetail.instane.BackLibrary();
        library.SetActive(true);
        backLibrary.SetActive(false);
        
    }



}
