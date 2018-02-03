using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusPage : Page {

    #region Declare
    [SerializeField]
    GameObject buttonTicket, btShowPhoto, btShowTicket;

    [SerializeField]
	Text txtBack, txtTitle, txtbtPhoto, txtbtTicket, txtbtTutorial, txtDialogConfirm1, txtDialogConfirm2, txtYes1, txtNo1, txtYes2, txtNo2, txtExchangeTicket;
    [SerializeField]
    Text txtTicketBack, txtTicketTitle, txtTicketNoticeForStaff, txtTicketNoticeDontTap, txtbtTicketStaff;


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
                ChangeColor(btShowTicket, lstImg, false);


            if (UserData.IsGotTicket())
                buttonTicket.SetActive(true);
            else
                buttonTicket.SetActive(false);

        txtBack.text = ApplicationData.GetLocaleText(LocaleType.ButtonBack);
        txtTitle.text = ApplicationData.GetLocaleText(LocaleType.TitleBonusPage);
        txtbtPhoto.text = ApplicationData.GetLocaleText(LocaleType.ButtonPhotoFrame);
        txtbtTicket.text = ApplicationData.GetLocaleText(LocaleType.ButtonTicket);
        txtbtTutorial.text = ApplicationData.GetLocaleText(LocaleType.ButtonPrologue);

		txtDialogConfirm1.text = ApplicationData.GetLocaleText (LocaleType.ConfirmationDialog1);
		txtDialogConfirm1.lineSpacing = ApplicationData.SetLineSpacing (LocaleType.ConfirmationDialog1);

		txtDialogConfirm2.text = ApplicationData.GetLocaleText (LocaleType.ConfirmationDialog2);

		txtYes1.text = ApplicationData.GetLocaleText (LocaleType.ButtonYes);
		txtNo1.text = ApplicationData.GetLocaleText (LocaleType.ButtonNo);
		txtYes2.text = ApplicationData.GetLocaleText (LocaleType.ButtonYes);
		txtNo2.text = ApplicationData.GetLocaleText (LocaleType.ButtonNo);
		txtExchangeTicket.text = ApplicationData.GetLocaleText (LocaleType.ButtonExchangeTicket);
        txtTicketBack.text = ApplicationData.GetLocaleText(LocaleType.ButtonBack);
        txtTicketTitle.text = ApplicationData.GetLocaleText(LocaleType.TitleTicketPage);

        txtTicketNoticeForStaff.text = ApplicationData.GetLocaleText(LocaleType.TicketNoticeForStaff);
		txtTicketNoticeForStaff.rectTransform.localPosition = ApplicationData.SetLinePosition (LocaleType.TicketNoticeForStaff);
		txtTicketNoticeForStaff.rectTransform.sizeDelta = new Vector2 (ApplicationData.SetLineWidth (LocaleType.TicketNoticeForStaff), 327);

        txtTicketNoticeDontTap.text = ApplicationData.GetLocaleText(LocaleType.TicketNoticeDontTap);
		txtTicketNoticeDontTap.rectTransform.localPosition = ApplicationData.SetLinePosition (LocaleType.TicketNoticeDontTap);
		txtTicketNoticeDontTap.rectTransform.sizeDelta = new Vector2 (ApplicationData.SetLineWidth (LocaleType.TicketNoticeDontTap), 327);

        txtbtTicketStaff.text = ApplicationData.GetLocaleText(LocaleType.ButtonTicketStaff);
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
