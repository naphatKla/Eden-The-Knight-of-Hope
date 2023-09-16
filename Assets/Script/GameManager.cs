using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
}
