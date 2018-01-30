using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class Override : MonoBehaviour
{
    public Text label;

#if !UNITY_EDITOR && UNITY_IOS

  [DllImport("__Internal")]
  static extern string getMessage();

  void Awake()
    { 
        label.text = getMessage(); 
    }
#endif
}