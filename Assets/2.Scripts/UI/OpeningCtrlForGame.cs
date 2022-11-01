using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningCtrlForGame : KitaujiGameDesignClub.GameFramework.UI.OpeningCtrl
{
   

    public CanvasGroup WelcomeCanvasGroup;
    

    public override void Start()
    {
        base.Start();
        StartCoroutine(WelcomeFadeIn());
    }



    IEnumerator WelcomeFadeIn()
    {
        WelcomeCanvasGroup.alpha = 0f;

        yield return new WaitForSeconds(1.2f);
        while (true)
        {
            WelcomeCanvasGroup.alpha = Mathf.Lerp(WelcomeCanvasGroup.alpha, 1f, 0.51f * Time.deltaTime);
            if(WelcomeCanvasGroup.alpha >= 0.99f) yield break;
            else yield return new WaitForEndOfFrame();
            
        }
    }
    

  

 
}
