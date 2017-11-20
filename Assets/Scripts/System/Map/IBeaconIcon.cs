using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBeaconIcon : MonoBehaviour {

    [SerializeField]
    int ibeaconIndex;

    public int IbeaconIndex {
        get {
            return ibeaconIndex;
        }
    }

    SpriteRenderer spriteRenderer;

    SpriteRenderer GetSpriteRenderer ()
    {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer> ();
        }
        return spriteRenderer;
    }

    public void SetIconImage (Sprite image)
    {
        GetSpriteRenderer().sprite = image;
    }

    public void SetActive (bool isActive)
    {
        gameObject.SetActive (isActive);
    }

}
