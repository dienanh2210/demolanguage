using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraAsBackground : MonoBehaviour {
    private RawImage image;
    private WebCamTexture cam;
    private AspectRatioFitter arf;
    private bool camAvailable;

    public void CameraStart ()
    {
        if (cam != null && cam.isPlaying) {
            return;
        }

        arf = GetComponent<AspectRatioFitter> ();
        image = GetComponent<RawImage> ();


        WebCamDevice [] devices = WebCamTexture.devices;

        if (devices.Length == 0) {
            Debug.Log ("No camera detected !");
            camAvailable = false;
            return;
        }

        if (cam == null) {
            for (int i = 0; i < devices.Length; i++) {
                if (!devices [i].isFrontFacing) {
                    cam = new WebCamTexture (devices [i].name, Screen.width/5, Screen.height/5);
                    break;
                }
            }

            if (devices.Length > 0 && cam == null) {
                cam = new WebCamTexture (devices [0].name, Screen.width/5, Screen.height/5);
            }

            if (cam == null) {
                Debug.Log ("Unable to find back camera !");
                return;
            }
        }

        cam.Play ();
        image.texture = cam;
        camAvailable = true;
    }

    void Update(){
        if (cam == null || !cam.isPlaying) {
            return;
        }

        if (cam.width < 100) {
            return;
        }

        Debug.Log (-cam.videoRotationAngle);
        float cwNeeded = -cam.videoRotationAngle;
        if (cam.videoVerticallyMirrored) {
            cwNeeded += 180f;
        }

        image.rectTransform.localEulerAngles = new Vector3 (0f, 0f, cwNeeded);

        float videoRatio = (float)cam.width / (float)cam.height;
        arf.aspectRatio = videoRatio;

        if (cam.videoVerticallyMirrored) {
            image.uvRect = new Rect (1, 0, -1, 1);
        } else {
            image.uvRect = new Rect (0, 0, 1, 1);
        }
	}

    public void CameraStop ()
    {
        cam.Stop ();
    }
}
