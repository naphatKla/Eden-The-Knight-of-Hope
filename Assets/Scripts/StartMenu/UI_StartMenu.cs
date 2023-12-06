using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_StartMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    [SerializeField] private AudioClip backgroundMusic;

    private void Start()
    {
        SoundManager.Instance.PlayBackGroundMusic(backgroundMusic);
    }

    public void PlayButton()
    {
        StartCoroutine(LoadLevel(1));
    }
    
    public void CreditsButton()
    {
        StartCoroutine(LoadLevel(2));
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
       transition.SetTrigger("Start");
       yield return new WaitForSeconds(transitionTime);
       SceneManager.LoadScene(levelIndex);
    }
}
