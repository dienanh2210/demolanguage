using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusPage : Page {

    #region Declare
    [SerializeField]
    GameObject buttonTicket, btShowPhoto, btShowTicket;

    [SerializeField]
	Text[] BonusPageText = new Text[17];

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
                ChangeColor(btShowPhoto, lstImg, true);

            if (ApplicationLogic.IsShowTicket())
                ChangeColor(btShowTicket, lstImg, true);
            else
                ChangeColor(btShowTicket, lstImg, true);


            if (UserData.IsGotTicket())
                buttonTicket.SetActive(true);
            else
                buttonTicket.SetActive(true);

		BonusPageText [0].text = ApplicationData.GetLocaleText (LocaleType.ButtonBack);
		BonusPageText [1].text = ApplicationData.GetLocaleText(LocaleType.TitleBonusPage);
		BonusPageText [2].text = ApplicationData.GetLocaleText(LocaleType.ButtonPhotoFrame);

		BonusPageText [3].text = ApplicationData.GetLocaleText(LocaleType.ButtonTicket);
		BonusPageText [4].text = ApplicationData.GetLocaleText(LocaleType.ButtonPrologue);
		BonusPageText [5].text = ApplicationData.GetLocaleText (LocaleType.ConfirmationDialog1);
		BonusPageText [5].lineSpacing = ApplicationData.SetLineSpacing (LocaleType.ConfirmationDialog1);
		BonusPageText [6].text = ApplicationData.GetLocaleText (LocaleType.ConfirmationDialog2);
		BonusPageText [7].text = ApplicationData.GetLocaleText (LocaleType.ButtonYes);
		BonusPageText [8].text = ApplicationData.GetLocaleText (LocaleType.ButtonNo);
		BonusPageText [9].text = ApplicationData.GetLocaleText (LocaleType.ButtonYes);
		BonusPageText [10].text = ApplicationData.GetLocaleText (LocaleType.ButtonNo);
		BonusPageText [11].text = ApplicationData.GetLocaleText (LocaleType.ButtonExchangeTicket);
		BonusPageText [12].text = ApplicationData.GetLocaleText(LocaleType.ButtonBack);
		BonusPageText [13].text = ApplicationData.GetLocaleText(LocaleType.TitleTicketPage);

		BonusPageText [14].text = ApplicationData.GetLocaleText(LocaleType.TicketNoticeForStaff);
		BonusPageText [14].rectTransform.localPosition = ApplicationData.SetLinePosition (LocaleType.TicketNoticeForStaff);
		BonusPageText [14].rectTransform.sizeDelta = new Vector2 (ApplicationData.SetLineWidth (LocaleType.TicketNoticeForStaff), 327);
		BonusPageText [14].fontSize = ApplicationData.SetFontSize (LocaleType.TicketNoticeForStaff);
		BonusPageText [14].lineSpacing = ApplicationData.SetLineSpacing (LocaleType.TicketNoticeForStaff);

		BonusPageText [15].text = ApplicationData.GetLocaleText(LocaleType.TicketNoticeDontTap);
		BonusPageText [15].rectTransform.localPosition = ApplicationData.SetLinePosition (LocaleType.TicketNoticeDontTap);
		BonusPageText [15].rectTransform.sizeDelta = new Vector2 (ApplicationData.SetLineWidth (LocaleType.TicketNoticeDontTap), 327);

		BonusPageText [16].text = ApplicationData.GetLocaleText(LocaleType.ButtonTicketStaff);

		SetFont (BonusPageText);
		BonusPageText [1].font = ApplicationData.GetFont (3);
		BonusPageText [1].fontSize = ApplicationData.SetFontSize (LocaleType.TitleBonusPage);
		BonusPageText [2].font = ApplicationData.GetFont (3);
		BonusPageText [2].fontSize = ApplicationData.SetFontSize (LocaleType.ButtonPhotoFrame);
		BonusPageText [3].font = ApplicationData.GetFont (3);
		BonusPageText [3].fontSize = ApplicationData.SetFontSize (LocaleType.ButtonTicket);
		BonusPageText [4].font = ApplicationData.GetFont (3);
		BonusPageText [4].fontSize = ApplicationData.SetFontSize (LocaleType.ButtonPrologue);
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
