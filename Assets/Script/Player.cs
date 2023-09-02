using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Declare Variable
    public enum PlayerState
    {
        Idle,
        Walk,
        Sprint,
        Dash
    }
    [Header("Player Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashDelay;
    private Rigidbody2D _playerRigidbody2D;
    public PlayerState _playerState;
    private float _currentSpeed;
    private bool _isDash;
    #endregion

    #region Unity Method
    void Start()
    {
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _playerState = PlayerState.Idle;
    }
    
    void Update()
    {
        MovementHandle();
    }
    
    private IEnumerator Dash()
    {
        float _dashTimeCount = 0;

        while (_dashTimeCount < dashDuration)
        {
            _isDash = true;
            _dashTimeCount += Time.deltaTime;
            _currentSpeed = dashSpeed;
            SetPlayerState(PlayerState.Dash);
            yield return null;
        }
        SetPlayerState(PlayerState.Idle);

        yield return new WaitForSeconds(dashDelay);
        _isDash = false;
    }
    #endregion

    #region Method
    private void MovementHandle()
    {
        WalkHandle();
        SprintHandle();
        DashHandle();
        
        Vector2 playerVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * _currentSpeed;
        _playerRigidbody2D.velocity = playerVelocity;
        
        if(playerVelocity == Vector2.zero) 
            SetPlayerState(PlayerState.Idle);
    }
    
    private void WalkHandle()
    {
        if(CheckPlayerState(PlayerState.Dash)) return;
        
        _currentSpeed = walkSpeed;
        SetPlayerState(PlayerState.Walk);
    }
    
    private void SprintHandle()
    {
        if (CheckPlayerState(PlayerState.Dash)) return;
        if (CheckPlayerState(PlayerState.Idle)) return;
        if (!Input.GetKey(KeyCode.LeftShift)) return;
        
        _currentSpeed = sprintSpeed;
        SetPlayerState(PlayerState.Sprint);
    }
    
    private void DashHandle()
    {
        if (_isDash) return;
        if (CheckPlayerState(PlayerState.Idle)) return;
        if (!Input.GetKeyDown(KeyCode.LeftControl)) return;
        
        StartCoroutine(Dash());
    }
    
    private void SetPlayerState(PlayerState state)
    {
        _playerState = state;
    }

    private bool CheckPlayerState(PlayerState state)
    {
        return _playerState == state;
    }
    #endregion
}
