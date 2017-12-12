using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusPage : Page {
    
    #region Declare
        [SerializeField]
        GameObject buttonTicket,btShowPhoto,btShowTicket;

        Color myGray = new Color();
        Color myWhite = new Color();
        List<Image> lstImg = new List<Image>();
    #endregion

    #region Init
        private void Awake()
        {
            ColorUtility.TryParseHtmlString("#9C9C9CFF", out myGray);
            ColorUtility.TryParseHtmlString("#FFFFFFFF", out myWhite);
        }
        void OnEnable()
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
    #endregion

    #region Utility
        void ChangeColor(GameObject bt, List<Image> lstImg, bool Active)
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
        public void Click()
        {
            UserData.TakeTicket();
            buttonTicket.SetActive(false);
        }
    #endregion
    
    #region ChangePage
        public void ChangePhotoPage()
        {
            PageManager.Show(PageType.PhotoFrame);
        }
        public void ChangeMapPage()
        {
            PageManager.Show(PageType.MapPage);
        }
        public void ChangeTutorialPage()
        {
            PageManager.Show(PageType.Tutorial);
            TutorialManager.isBonusPage = true;
            TutorialManager.count = 0;
            TutorialManager.Instance.ResetPage();
        }
    #endregion


}
