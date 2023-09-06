using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    [SerializeField] private float Speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Speed;
        playerRigidbody2D.velocity = playerVelocity;
    }
}
