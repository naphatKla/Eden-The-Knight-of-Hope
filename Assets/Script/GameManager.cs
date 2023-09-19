using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int TotalPoint = 0;
    public Transform player;
    public Transform tower;

    public static GameManager instance;
    void Start()
    {
        instance = this;
    }
    
    void Update()
    {
        
    }
    
    // Add Point
    public void AddPoint(int n)
    {
        TotalPoint += n;
    }
}
