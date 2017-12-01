using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoFrame : Page {

    private RawImage image;
    private WebCamTexture cam;
    private AspectRatioFitter arf;

    public void SwitchCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        cam.Stop();
        cam.deviceName = (cam.deviceName == devices[0].name) ? devices[1].name : devices[0].name;
        cam.Play();
        image.texture = cam;
    }

    private void OnEnable()
    {
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
                cam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (cam == null)
        {
            Debug.Log("Unable to find back camera !");
            return;
        }
        
        cam.deviceName = devices[0].name;
        cam.Play();
        image.texture = cam;
    }

    void Update()
    {

        float cwNeeded = -cam.videoRotationAngle;
        if (cam.videoVerticallyMirrored)
        {
            cwNeeded += 180f;
        }

        image.rectTransform.localEulerAngles = new Vector3(0f, 0f, cwNeeded);

        float videoRatio = (float)cam.width / (float)cam.height;
        arf.aspectRatio = videoRatio;

        if (cam.videoVerticallyMirrored)
        {
            image.uvRect = new Rect(1, 0, -1, 1);
        }
        else
        {
            image.uvRect = new Rect(0, 0, 1, 1);
        }
    }

    private void OnDisable()
    {
        if (cam.isPlaying)
        {
            cam.Stop();
        }
    }
}
