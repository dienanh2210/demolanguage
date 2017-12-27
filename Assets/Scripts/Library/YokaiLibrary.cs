

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class YokaiLibrary : MonoBehaviour
{

    public static YokaiLibrary instance;

    void Start()
    {
        instance = this;
    }

    void OnEnable()
    {
        ShowImages();
    }

    public void ShowImages()
    {
        var n = 0;
        foreach (Transform i in this.transform)
        {

            bool check = UserData.IsGotYokai(ApplicationData.YokaiData[n].id);

            if (!ApplicationData.YokaiData[n].CanShowOnYokaiList())
            {
                i.gameObject.SetActive(false);
            }
            else
            {
                i.gameObject.SetActive(true);
                if (i.name.Remove(0, 6) == ApplicationData.YokaiData[n].id.ToString())
                {
                    if (ApplicationData.YokaiData[n].isTermLimited)
                    {
                        i.transform.Find("Limit").gameObject.SetActive(true);
                        i.transform.Find("Limit").GetComponent<Image>().sprite = ApplicationData.GetLocaleImage(LocaleType.IconLimitedYokai);                     
                    }
                    if (!CheckId(ApplicationData.YokaiData[n].necessary_item_id))
                    {
                        i.transform.GetChild(0).GetComponent<Image>().sprite = ApplicationData.YokaiData[n].image;
                        if (UserData.GetLatestYokaiId () == ApplicationData.YokaiData [n].id)
                        {
                            i.transform.GetChild (1).gameObject.SetActive (true);
                            if (ApplicationData.YokaiData [n].isTermLimited)
                            {
                                i.transform.Find("Limit").gameObject.SetActive (false);
                            }
                        }
                        else
                        {
                            i.transform.GetChild (1).gameObject.SetActive (false);
                        }
                        if (check)
                        {
                            i.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                        }
                        else
                        {
                            i.transform.GetChild(0).GetComponent<Image>().color = Color.black;
                        }
                    }
                    else
                    {
                        if (!ApplicationData.YokaiData[n].HasItem() && ApplicationData.YokaiData[n].IsNeedItem())
                        {

                            i.transform.GetChild(0).GetComponent<Image>().sprite = ApplicationData.ItemData[0].image;
                        }
                        else if(ApplicationData.YokaiData[n].HasItem() && ApplicationData.YokaiData[n].IsNeedItem())
                        {
                            i.transform.GetChild(0).GetComponent<Image>().sprite = ApplicationData.YokaiData[n].image;
                            if (UserData.GetLatestYokaiId() == ApplicationData.YokaiData[n].id)
                            {
                                i.transform.GetChild(1).gameObject.SetActive(true);
                                if (ApplicationData.YokaiData [n].isTermLimited) {
                                    i.transform.Find("Limit").gameObject.SetActive (false);
                                }
                            }
                            else
                            {
                                i.transform.GetChild(1).gameObject.SetActive(false);
                            }
                            if (check)
                            {
                                i.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                            }
                            else
                            {
                                i.transform.GetChild(0).GetComponent<Image>().color = Color.black;
                            }
                        }

                    }

                }
            }
            n++;
        }
    }
    public bool CheckId(int id)
    {
        return ApplicationData.ItemData.Exists(s => s.id == id);
    }
}
