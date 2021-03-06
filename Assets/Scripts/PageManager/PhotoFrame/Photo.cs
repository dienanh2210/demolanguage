﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Linq;

public class Photo : Page
{

    #region Declare
    public GameObject btBack, btPhoto, DialogTwitter;
    public Text txtShareTwitterTitle, txtYes, txtNo , txtBack, txtSwitch;
    bool b = false;
    static Texture2D screenCapture;

    [DllImport("__Internal")]
    private static extern void _PlaySystemShutterSound();

    [DllImport("__Internal")]
    private static extern void _GetTexture(byte[] textureByte, int length);

    #endregion

   

    #region DELEGATE_EVENT_LISTENER
    void ScreenshotSaved()
    {
#if UNITY_IPHONE || UNITY_IPAD
        byte[] dataToSave = screenCapture.EncodeToPNG();

        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");

        File.WriteAllBytes(destination, dataToSave);

        GeneralSharingiOSBridge.ShareTextWithImage(destination, "");
#endif
    }
    #endregion


    #region Utilities

    private void OnEnable()
    {
        txtBack.text = ApplicationData.GetLocaleText(LocaleType.ButtonBack);
        txtSwitch.text = ApplicationData.GetLocaleText(LocaleType.ButtonSwitchCamera);
        txtShareTwitterTitle.text = ApplicationData.GetLocaleText(LocaleType.ShareTwitterTitle);
        txtYes.text = ApplicationData.GetLocaleText(LocaleType.ButtonYes);
        txtNo.text = ApplicationData.GetLocaleText(LocaleType.ButtonNo);

        txtShareTwitterTitle.font = ChangeFont();
        txtYes.font = ChangeFont();
        txtNo.font = ChangeFont();
        txtBack.font = ChangeFont();
        txtSwitch.font = ChangeFont();

        txtBack.text = ApplicationData.GetLocaleText(LocaleType.ButtonBack);
        txtBack.fontSize = ApplicationData.SetFontSize(LocaleType.ButtonBack);

        txtSwitch.text = ApplicationData.GetLocaleText(LocaleType.ButtonSwitchCamera);
        txtSwitch.fontSize = ApplicationData.SetFontSize(LocaleType.ButtonSwitchCamera);

        txtShareTwitterTitle.text = ApplicationData.GetLocaleText(LocaleType.ShareTwitterTitle);
        txtShareTwitterTitle.fontSize = ApplicationData.SetFontSize(LocaleType.ShareTwitterTitle);

        txtYes.text = ApplicationData.GetLocaleText(LocaleType.ButtonYes);
        txtYes.fontSize = ApplicationData.SetFontSize(LocaleType.ButtonYes);
        txtNo.text = ApplicationData.GetLocaleText(LocaleType.ButtonNo);
        txtNo.fontSize = ApplicationData.SetFontSize(LocaleType.ButtonNo);

    }

    public void ChangeBonusPage()
    {
        PageManager.Show(PageType.BonusPage);
    }
    #endregion

    #region TakePhoto
    void DidImageWriteToAlbum(string errorDescription)
    {
        Handheld.StopActivityIndicator();
    }

    public void TakeShot()
    {
        if (screenCapture != null)
        {
            screenCapture = null;
        }

#if UNITY_IOS

		_PlaySystemShutterSound();
		//Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.Gray);
		//Handheld.StartActivityIndicator();
		StartCoroutine(TakeHiResShot());

#endif
#if UNITY_ANDROID
        StartCoroutine(TakeHiResShot());
#endif
    }

    IEnumerator TakeHiResShot()
    {
        btBack.transform.localScale = new Vector3(0, 0, 0);
        btPhoto.transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForEndOfFrame();


#if UNITY_ANDROID
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        screenCapture.Apply();

        NativeGallery.SaveToGallery(screenCapture, "Yokai", "my img {0}.jpeg");
        


#endif

#if UNITY_IOS

		screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		screenCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
		screenCapture.Apply();

		byte[] screenshot = screenCapture.EncodeToPNG();

		_GetTexture(screenshot, screenshot.Length);

#endif

        btBack.transform.localScale = new Vector3(1, 1, 1);
        btPhoto.transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(0.5f);
        DialogTwitter.SetActive(true);
    }
    #endregion

    #region Twitter

    public void ShareTwitter()
    {
#if UNITY_IOS
        DialogTwitter.SetActive(false);
        ScreenshotSaved();

#endif

#if UNITY_ANDROID
        SaveAndShare();
#endif

    }

    void SaveAndShare()
    {
        if (screenCapture != null)
        {
            try
            {
                DialogTwitter.SetActive(false);
                byte[] dataToSave = screenCapture.EncodeToPNG();
                string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
                File.WriteAllBytes(destination, dataToSave);

                AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
                intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);

                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
                intentObject.Call<AndroidJavaObject>("setType", "image/png");
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "");
                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "");
                currentActivity.Call("startActivity", jChooser);

            }
            catch (Exception e)
            {
                DebugConsole.Log(e.Message);
            }
           
        }
        else
        {
            DebugConsole.Log("Error !");
        }

    }

    #endregion
}



