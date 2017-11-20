using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ClickController : Page, IPointerClickHandler {
    public static bool isBoss = false;

    #region IPointerClickHandler implementation

    public void OnPointerClick (PointerEventData eventData)
    {
//        PlayerPrefs.SetInt ("yokaiID", 21);
//        OnClickChangePage ("E");
//        MapPageManager.instance.SetMapPage (0,0,-6,RenderMode.ScreenSpaceCamera);

    }
    #endregion


	
}
