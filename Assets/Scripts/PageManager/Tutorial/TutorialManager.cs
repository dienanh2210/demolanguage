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

    private void Start()
    {
        foreach (Transform i in this.transform)
        {
            string name = i.name.Remove(0, 5);
            var obj = ApplicationData.TutorialData.Where(s => s.index.ToString() == name.Trim()).ToList();
            if (obj.Count() > 0)
            {
                i.transform.GetChild(0).GetComponent<Image>().sprite = obj[0].image;

                for (int a = 0; a < ApplicationData.TutorialData[a].localContents.Count; a++)
                {
                    i.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = obj[0].localContents[a].text.ToString();

                }
            }
        }
    }

    int count = 0;

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
   
}
