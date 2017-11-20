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

    PageType page; 
    public void ChangeTutorialPage()
    {
        if (!UserData.IsShowedGameTutorial)
        {
            page = PageType.Tutorial;
        }
        else
        {
            page = PageType.MapPage;
        }
        PageManager.Show(page);
    }
   
    public void ChangeMapPage()
    {
        page = PageType.MapPage;
        PageManager.Show(page);
    }

    public void ChangeLibraryPage()
    {
        page = PageType.Library;
        PageManager.Show(page);
    }

    public void ChangeBonusPage()
    {
        page = PageType.BonusPage;
        PageManager.Show(page);
    }

    public void ChangePhotoPage()
    {
        page = PageType.PhotoFrame;
        PageManager.Show(page);
    }

    public void ChangeYokaiGetPage()
    {
        if (UserData.IsShowedYokaiTutorial && PageData.yokaiID != -1)
        {
            page = PageType.YokaiGetPage;
        }
        else if (UserData.IsShowItemTutorial && PageData.itemID != -1)
        {
            page = PageType.YokaiGetPage;
        }
        //else if (ApplicationData.YokaiData.Exists(s => s.isBoss == true)) {
        //    type = PageType.YokaiGetPage;
        //} 
        else
        {
            page = PageType.YokaiGetTutorialPage;
        }
        PageManager.Show(page);
    }
   /* public void ClickChangePage(string name)
    {

        PageType type = PageType.TitlePage;
        if (name == "Title") {
            type = PageType.TitlePage;
        }
        else if (name == "map")
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
        else if (name == "library") {
            type = PageType.Library;
        }
        else if (name == "bonus") {
            type = PageType.BonusPage;
        } 
        else if (name == "photo") {
            type = PageType.PhotoFrame;
        } 
        else if (name == "yokaiGet") {
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
   */
}
