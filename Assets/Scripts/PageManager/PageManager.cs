using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    [SerializeField]
    Transform contentTrn;

    [SerializeField]
    PageType initialPageType;

    [SerializeField]
    List<PageInfo> pagePrefabs;

    Dictionary<PageType, Page> pages = new Dictionary<PageType, Page> ();
    Page currentPage;

    static PageManager instance;
    [SerializeField]
    PageTransiton pageTransiton;

    void Awake ()
    {
        instance = this;
        //StartCoroutine(Show(initialPageType));
        Show (initialPageType);
    }


    public static void Show (PageType pageType)
    {
        if (pageType != PageType.TitlePage)
//            && pageType != PageType.YokaiGetPage
//            && pageType != PageType.YokaiGetTutorialPage)
        {
            instance.pageTransiton.Fade (() => ShowProcess(pageType));
        } else {
            ShowProcess (pageType);
        }
    }

    static void ShowProcess (PageType pageType)
    {
        if (instance.currentPage != null) {
            instance.currentPage.Hide ();
        }

        Page page;
        if (instance.pages.ContainsKey (pageType)) {
            page = instance.pages [pageType];
        } else {
            var newPage = instance.pagePrefabs.Find ((info) => info.pageType == pageType);
            page = Instantiate (newPage.page) as Page;
            instance.pages.Add (pageType, page);
        }

        page.transform.SetParent (instance.contentTrn, false);

        page.Show ();

        instance.currentPage = page;
    }
}



[System.Serializable]
public class PageInfo
{
    public PageType pageType;
    public Page page;
}

public enum PageType
{
    TitlePage,
    Tutorial,
    MapPage,
    Library,
    BonusPage,
    PhotoFrame,
    YokaiGetPage,
    YokaiGetTutorialPage
}
