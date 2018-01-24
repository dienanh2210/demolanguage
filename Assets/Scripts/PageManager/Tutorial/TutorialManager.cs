using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class TutorialManager : Page
{

    [SerializeField]
    private GameObject[] page = new GameObject[8];

    [SerializeField]
    private GameObject[] txt = new GameObject[4];

    public static bool isBonusPage = false;
    public static TutorialManager Instance;

    private void Start()
    {
        Instance = this;
		foreach (Transform i in this.transform)
		{
			string name = i.name.Remove(0, 5);
			TutorialData obj = ApplicationData.TutorialData.SingleOrDefault(s => s.index.ToString() == name.Trim());

			if (!obj.Equals(new TutorialData()))
			{
				i.transform.GetChild(0).GetComponent<Image>().sprite = obj.image;

				for (int a = 0; a < obj.localContents.Count; a++)
				{
					if (obj.localContents[a].languageType == ApplicationData.SelectedLanguage)
					{
						i.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = obj.localContents[a].text.ToString();
					}
					i.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = ApplicationData.GetLocaleText(LocaleType.TapOnPrologue);
				}
			}
		}
    }

    public static int count = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (!txt[count].GetComponent<TW_Regular>().checkRunText)
            {
                count++;
            }
            else
            {
                txt[count].GetComponent<TW_Regular>().enabled = false;
                txt[count].GetComponent<Text>().text = txt[count].GetComponent<TW_Regular>().ORIGINAL_TEXT;
                txt[count].GetComponent<TW_Regular>().checkRunText = false;
            }

            if (count == page.Length)
            {
                count = page.Length-1;
                UserData.IsShowedGameTutorial = true;
                if (isBonusPage)
                {
                    PageManager.Show(PageType.BonusPage);
                }
                else
                {
                    PageManager.Show(PageType.MapPage);
                }
                
            }
            for (int i = 0; i < page.Length; i++)
            {
                if (i == count)
                {
                    page[i].SetActive(true);
                }
                else
                {
                    page[i].SetActive(false);
                }
            }
        }
    }
   
    public void ResetPage()
    {
        count = 0;
        for (int i = 0; i < page.Length; i++)
        {
            if (i == count)
            {
                page[i].SetActive(true);
            }
            else
            {
                page[i].SetActive(false);
            }
        }
    }
}
