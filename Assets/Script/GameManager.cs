using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public int totalPoint = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
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
    
    // Add Point
    public void AddPoint(int n)
    {
        totalPoint += n;
        scoreText.text = $"Score: {totalPoint}";
    }
}
