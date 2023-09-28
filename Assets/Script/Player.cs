using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = System.Numerics.Vector3;

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
    public PlayerState playerState;
    private Rigidbody2D _playerRigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private float _currentSpeed;
    private bool _isDash;
    private bool _isRunning;
    public static Player instance;

    #endregion

    #region Unity Method

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerState = PlayerState.Idle;
        instance = this;
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackState_1") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackState_2") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackState_3") ||
            _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerHeavyAttack") )
        {
            _playerRigidbody2D.velocity = Vector2.zero;
            return;
        }
      
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

        _animator.SetTrigger(playerState.ToString());

        // flip player horizontal direction
        if (Input.GetAxisRaw("Horizontal") != 0)
            _spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") < 0;
    }

    private void WalkHandle()
    {
        if (CheckPlayerState(PlayerState.Dash)) return;

        _currentSpeed = walkSpeed;

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            SetPlayerState(PlayerState.Idle);
        else
            SetPlayerState(PlayerState.Walk);
    }

    private void SprintHandle()
    {
        if (CheckPlayerState(PlayerState.Dash)) return;
        if (CheckPlayerState(PlayerState.Idle))
        {
            //_isRunning = false;
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isRunning = !_isRunning;
        }

        if (_isRunning)
        {
            _currentSpeed = sprintSpeed;
            SetPlayerState(PlayerState.Sprint);
        }
    }

    private void DashHandle()
    {
        if (_isDash) return;
        if (CheckPlayerState(PlayerState.Idle)) return;
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        StartCoroutine(Dash());
    }

    private void SetPlayerState(PlayerState state)
    {
        playerState = state;
    }

    private bool CheckPlayerState(PlayerState state)
    {
        return playerState == state;
    }

    public void ResetState()
    {
        SetPlayerState(PlayerState.Idle);
        _isDash = false;
        _currentSpeed = walkSpeed;
    }

    #endregion
}
