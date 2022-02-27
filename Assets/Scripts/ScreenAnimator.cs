using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScreenTransitionsPro;

public class ScreenAnimator : MonoBehaviour
{
    
    
    void Start()
    {
        //_transition = GetComponent<ScreenTransitionSimple>();

        //or (?)

        //ScreenTransitionSimple transition = GetComponent<ScreenTransitionSimple>();
        //transition.cutoff = 1f;
    }
    
    /*
    public void StartTransition()
    {
        StartCoroutine(AnimateTransition());
    }

    IEnumerator AnimateTransition()
    {
        transition.transitioning = true;
        while (transition.cutoff < 1f)
        {
            transition.cutoff += Time.deltaTime;
            yield return 0;
        }
    }
    */
}
