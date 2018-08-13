using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Appli : MonoBehaviour
{
    static Appli appli;
    public static string GetLocaleText(LocaleTyp key)
    {
        var localee = appli.localeData.Find((obj) => obj.key == key);
        if (localee.key != LocaleTyp.None)
        {
            if (Appli.SelectedLanguage == LanguageTyp.Thai)
            {
                try
                {
                    return ThaiFontAdjuster.Adjust(localee.localContents.Find((obj) => obj.languageTyp == SelectedLanguage).text);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    return "error !";
                }
            }
            else
            {
                return localee.localContents.Find((obj) => obj.languageTyp == SelectedLanguage).text;
            }
      
        }
        else
        {
            Debug.LogError("cant find locale");
            return "";
        }
    }
    public static LanguageTyp SelectedLanguage
    {
        get
        {
            return appli.selectedLanguage;
        }
        set
        {
            appli.selectedLanguage = value;
        }
    }

    [SerializeField]
    LanguageTyp selectedLanguage = LanguageTyp.Japanese;

    [SerializeField]
    List<LocaleDat> localeData = new List<LocaleDat>();
    void Awake()
    {
        appli = this;
    }
}
public enum LanguageTyp
{
    Japanese,
    English,
    Chinese1,
    Chinese2,
    Thai
}
public enum LocaleTyp
{
    None,
    TermLimitedYokai,//1
    MiddleEnding1,//2
}

[Serializable]
public struct LocaleDat
{
    public LocaleTyp key;
    public List<Localee> localContents;
}

[Serializable]
public struct Localee
{
    public LanguageTyp languageTyp;
    [Multiline]
   public string text;
   /* public Sprite image;
    public float lineSpacing;
    public Vector2 linePosition;
    public float lineWidth;
    public int fontSize;*/
}


