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

    public void OnClickChangePage(string name)
    {

        PageType type = PageType.TitlePage;
        if (name == "A") {
            type = PageType.TitlePage;
        }
        else if (name == "B")
        {
            if(!UserData.IsShowedGameTutorial)
            {
                type = PageType.Tutorial;
            }
            else
            {
                type = PageType.MapPage;
            }

        }
        else if (name == "C") {
            type = PageType.MapPage;
        } 
        else if (name == "D") {
            type = PageType.Library;
        }
        else if (name == "BonusPage") {
            type = PageType.BonusPage;
        } 
        else if (name == "PhotoFrame") {
            type = PageType.PhotoFrame;
        } 
        else if (name == "E") {
            if (UserData.IsShowedYokaiTutorial && PageData.yokaiID != -1)
            {
                type = PageType.YokaiGetPage;
            }
            else if (UserData.IsShowItemTutorial && PageData.itemID != -1)
            {
                type = PageType.YokaiGetPage;
            }
            //else if (ApplicationData.YokaiData.Exists(s => s.isBoss == true)) {
            //    type = PageType.YokaiGetPage;
            //} 

            else {
                type = PageType.YokaiGetTutorialPage;
            }

        }
        
        PageManager.Show(type);

    }
}
