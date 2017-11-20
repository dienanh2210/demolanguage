using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusPage : Page {
    
    [SerializeField]
    GameObject buttonTicket,btShowPhoto,btShowTicket;

    Color myGray = new Color();
    Color myWhite = new Color();
    List<Image> lstImg = new List<Image>();

    

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#9C9C9CFF", out myGray);
        ColorUtility.TryParseHtmlString("#FFFFFFFF", out myWhite);
    }

    void OnEnable ()
    {
        
        if (ApplicationLogic.IsShowPhotoFrame())
            ChangeColor(btShowPhoto, lstImg, true);
        else
            ChangeColor(btShowPhoto, lstImg, false);

        if (ApplicationLogic.IsShowTicket())
            ChangeColor(btShowTicket, lstImg, true);
        else
            ChangeColor(btShowTicket, lstImg, false);


        if (UserData.IsGotTicket())
            buttonTicket.SetActive(true);
        else
            buttonTicket.SetActive(false);
    }

    void ChangeColor(GameObject bt , List<Image> lstImg , bool Active)
    {
        if (lstImg.Count > 0)
            lstImg.Clear();

        if (Active)
        {
            bt.GetComponent<Button>().enabled = true;
            bt.GetComponentsInChildren<Image>(true, lstImg);
            foreach (var item in lstImg)
            {
                item.color = myWhite;
            }
        }
        else
        {
            bt.GetComponent<Button>().enabled = false;
            bt.GetComponentsInChildren<Image>(true, lstImg);
            foreach (var item in lstImg)
            {
                item.color = myGray;
            }
        }
    }

    // Use this for initialization
    public void Click ()
    {
        UserData.TakeTicket();
        buttonTicket.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
