using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    //public Animator transition;
    //public float transitionTime = 1f;

    //Camera mainCamera = Camera.main;

    /*void Start()
    {
        transition = GetComponent<ScreenTransitionSimple>();
    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //StartCoroutine(LoadBattle());
            //mainCamera.GetComponent<ScreenAnimator>().StartTransition();
            SceneManager.LoadScene(1);
        }
            
    }

    /*IEnumerator LoadBattle()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(1);

        transition.transitioning = true;
        while (transition.cutoff < 1f)
        {
            transition.cutoff += Time.deltaTime;
            yield return 0;
        }

        SceneManager.LoadScene(1);
    }*/
}
