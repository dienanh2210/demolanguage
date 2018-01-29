using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using System.Runtime.InteropServices;


public class Photo : Page {

    public GameObject btBack, btPhoto;
    [SerializeField]
    Text txtBack, txtCameraSwitch;
    bool b = false;



    [DllImport("__Internal")]
　　private static extern void _PlaySystemShutterSound();

　　[DllImport("__Internal")]
　　private static extern void _GetTexture(byte[] textureByte, int length);

    private void OnEnable()
    {
        txtBack.text = ApplicationData.GetLocaleText(LocaleType.ButtonBack);
        txtCameraSwitch.text = ApplicationData.GetLocaleText(LocaleType.ButtonSwitchCamera);
    }

    public void ChangeBonusPage()
    {
        PageManager.Show(PageType.BonusPage);
    }

    void DidImageWriteToAlbum(string errorDescription)
    {
        Handheld.StopActivityIndicator();
    }

    public void TakeShot()
    {

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

        
        string filename = string.Format("screen_{0}x{1}_{2}.png",
                             Screen.width, Screen.height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

        string path = string.Format("{0}/{1}", Application.persistentDataPath, filename);

#if UNITY_ANDROID
        Texture2D screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        screenCapture.Apply();

        yield return new WaitForSeconds(1f);

        if (!b)
            NativeGallery.SaveToGallery(screenCapture, "Yokai", "overwrite.png"); // not overwritten on iOS
        else
            NativeGallery.SaveToGallery(screenCapture, "Yokai", "my img {0}.jpeg");

        b = !b;


#endif

#if UNITY_IOS

        Texture2D screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        screenCapture.Apply();

        byte[] screenshot = screenCapture.EncodeToPNG();

        _GetTexture(screenshot, screenshot.Length);
       
#endif

        btBack.transform.localScale = new Vector3(1, 1, 1);
        btPhoto.transform.localScale = new Vector3(1, 1, 1);
    }

    
}
