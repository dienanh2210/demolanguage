using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoFrame : Page {

    private RawImage image;
    private WebCamTexture Tex;
    WebCamDevice FrontCam, BackCam , CurrentCam;
    private AspectRatioFitter arf;
    bool front = false;

    public void SwitchCamera()
    {
        if (CurrentCam.name == FrontCam.name)
        {
            front = true;
            CurrentCam = BackCam;
            Tex.Stop();
            Tex = new WebCamTexture(CurrentCam.name, Screen.width, Screen.height, 30);
            Tex.Play();
            image.texture = Tex;
            

        }
        else
        {
            front = false;
            CurrentCam = FrontCam;
            Tex.Stop();
            Tex = new WebCamTexture(CurrentCam.name, Screen.width, Screen.height, 30);
            Tex.Play();
            image.texture = Tex;
        }
    }



    private void OnEnable()
    {
        front = false;
        arf = GetComponent<AspectRatioFitter>();
        image = GetComponent<RawImage>();


        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected !");

            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                FrontCam = devices[i];
                CurrentCam = devices[i];
                Tex = new WebCamTexture(CurrentCam.name, Screen.width, Screen.height, 30);
            }
            else
            {
                BackCam = devices[i];
            }
        }
        Tex.Play();
        image.texture = Tex;
    }

    void Flip(bool front)
    {
        float cwNeeded = -Tex.videoRotationAngle;
        if (Tex.videoVerticallyMirrored)
        {
            cwNeeded += 180f;
        }

        if (front)
        {
            image.rectTransform.localEulerAngles = new Vector3(0f, 180f, cwNeeded);
        }
        else
        {
            image.rectTransform.localEulerAngles = new Vector3(0f, 0f, cwNeeded);
        }

        float videoRatio = (float)Tex.width / (float)Tex.height;
        arf.aspectRatio = videoRatio;

        if (Tex.videoVerticallyMirrored)
        {
            image.uvRect = new Rect(1, 0, -1, 1);
        }
        else
        {
            image.uvRect = new Rect(0, 0, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        Flip(front);
    }
   
    private void OnDisable()
    {
        if (Tex.isPlaying)
        {
            Tex.Stop();
        }
    }
}
