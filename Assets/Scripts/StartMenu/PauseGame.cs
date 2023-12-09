using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    private bool isGamePaused = false;
    public GameObject YouStop;
    
    /*[SerializeField] private GameObject Continue;
    [SerializeField] private GameObject mainMenu;*/
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    

    void TogglePause()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            YouStop.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            YouStop.SetActive(false);
        }
            
    }

    void ActivatePauseMenu(bool activate)
    {
        if (YouStop != null)
        {
            YouStop.SetActive(activate);
        }
    }
}
