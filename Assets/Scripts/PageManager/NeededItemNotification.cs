using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NeededItemNotification : MonoBehaviour {

    public void OnClick(){
        GetPageManager.GetInstance ().count++;
        if (GetPageManager.GetInstance ().count == 2)
        {
            if (ApplicationData.SelectedLanguage == LanguageType.Thai)
            {
                this.GetComponentInChildren<Text>().fontSize = 62;
            }
            this.GetComponentInChildren<Text>().text = ApplicationData.GetLocaleText(LocaleType.NoItemMessage2);
        }
        if (GetPageManager.GetInstance ().count == 3)
        {
            this.gameObject.SetActive(false);
        }
    }
}
