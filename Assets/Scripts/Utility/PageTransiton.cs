using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class PageTransiton : MonoBehaviour {

   
    [SerializeField]
    public Image blackPage;

    Animator animator;

    const float TRANSITION_SEC = 0.5f;

    void Awake ()
    {
        animator = blackPage.GetComponent<Animator> ();
    }

    public void Fade(Action OnTransitionTiming)
    {
        blackPage.enabled = true;
        animator.Play ("Fade");
        StartCoroutine (_DoTransition(OnTransitionTiming));
    }

    IEnumerator _DoTransition (Action OnTransition)
    {
        yield return new WaitForSeconds (TRANSITION_SEC);
        OnTransition ();
    }
}
