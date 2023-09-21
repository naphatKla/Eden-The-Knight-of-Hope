using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public Transform playerBase;

    public static GameManager instance;
    void Start()
    {
        instance = this;
    }
    
    void Update()
    {
        if (playerBase == null) SceneManager.LoadScene(0);
    }
}
