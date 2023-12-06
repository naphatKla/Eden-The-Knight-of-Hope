using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinYouLose : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    

    public void Restart()
    {
        StartCoroutine(LoadLevel(1));
    }
    
    public void MainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void Continue()
    {
        StartCoroutine(LoadLevel(2));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
       transition.SetTrigger("Start");
       yield return new WaitForSeconds(transitionTime);
       SceneManager.LoadScene(levelIndex);
    }
}
