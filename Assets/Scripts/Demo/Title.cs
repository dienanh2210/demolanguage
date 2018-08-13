using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class Title : MonoBehaviour {

    #region Declare
    [SerializeField]
    List<Button> lstButton;
    [SerializeField]
    Text  txtLanguage;
    public static Title instance;
    #endregion

    #region Init
    void OnEnable()
    {
        switch (Application.systemLanguage.ToString())
        {
            case "Japanese":
                SelectJapanese();
                break;
            case "ChineseTraditional":
                SelectChinese1();
                break;
            case "ChineseSimplified":
                SelectChinese2();
                break;
            case "Thai":
                SelectThai();
                break;
            default:
                SelectEnglish();
                break;
        }
        txtLanguage.text = Appli.GetLocaleText(LocaleTyp.TermLimitedYokai); //key value language
    }
    #endregion

        // Use this for initialization
        void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    #region Language
    void DisalbeButton(string name)
    {
        try
        {
            List<Button> lstTemp = lstButton.FindAll(x => x.name != name);
            Button bt = lstButton.Find(x => x.name == name);
            txtLanguage.text = Appli.GetLocaleText(LocaleTyp.TermLimitedYokai); //key value language
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
    }
    public void SelectEnglish()
    {
        Appli.SelectedLanguage = LanguageTyp.English;
     
        DisalbeButton("btEnglish");

    }
    public void SelectJapanese()
    {
        Appli.SelectedLanguage = LanguageTyp.Japanese;
        DisalbeButton("btJapanese");

    }
    public void SelectChinese1()
    {
        Appli.SelectedLanguage = LanguageTyp.Chinese1;
        DisalbeButton("btChinese");

    }
    public void SelectChinese2()
    {
        Appli.SelectedLanguage = LanguageTyp.Chinese2;
        DisalbeButton("btChinese2");

    }
    public void SelectThai()
    {
        Appli.SelectedLanguage = LanguageTyp.Thai;
        DisalbeButton("btThai");

    }
  
#endregion
}
