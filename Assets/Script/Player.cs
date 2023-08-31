using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Plsyer Set")]
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float _currentSpeed;
    private float _dashTimeCount;
    private bool _isDash;



// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _currentSpeed = sprintSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !_isDash)
        {
            _isDash = true;
            _dashTimeCount = 0;
        }

        if (_dashTimeCount < dashDuration)
        {
            _dashTimeCount += Time.deltaTime;
            _currentSpeed = dashSpeed;
        }
        else
        {
            _isDash = false;
        }
        
        
        Vector2 playerVelocity = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")) * _currentSpeed;
        playerRigidbody2D.velocity = playerVelocity;
    }
    
    
}
