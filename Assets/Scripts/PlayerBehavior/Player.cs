using System;
using System.Collections;
using System.Linq;
using CombatSystem;
using HealthSystem;
using UnityEngine;
/*using UnityEngine.Serialization;*/


namespace PlayerBehavior
{
    public enum PlayerState
    {
        Idle,
        Walk,
        Sprint,
        Dash
    }

    public class Player : MonoBehaviour
    {
        #region Declare Variables

        [Header("Player Movement")] 
        public PlayerState playerState;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCooldown;
        [SerializeField] private KeyCode sprintKey;
        [SerializeField] private KeyCode dashKey;
        [SerializeField] private Transform canvasTransform;

        [Header("Player Stamina")] 
        [SerializeField] private float maxStamina;
        [SerializeField] private float staminaRegenSpeed;
        [SerializeField] private float staminaRegenCooldown;
        [SerializeField] private float sprintStaminaDrain;
        [SerializeField] private float dashStaminaDrain;
        
        [Header("Dash Sound")]
        [SerializeField] private AudioClip[] dashSound;
        
        [Header("UI")]
        [SerializeField] private GameObject miniMapUI;
        [SerializeField] private Camera miniMapCamera;
        [SerializeField] private AudioClip[] openMinimapSounds;

        private bool _isDash;
        private bool _isDashCooldown;
        private bool _dashBuffering;
        private float _currentSpeed;
        private float _currentStamina;
        private float _staminaRegenCurrentCooldown;
        private Animator _animator;
        private Rigidbody2D _playerRigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _firstDashDirection;
        public static Player Instance;
        private static readonly int IsDashAnimation = Animator.StringToHash("IsDash");
        public Animator Animator => _animator;
        public bool IsDash => _isDash;
        public float MaxStamina => maxStamina;
        public float CurrentStamina { get => _currentStamina; set => _currentStamina = value; }

        #endregion

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerRigidbody2D = GetComponent<Rigidbody2D>();
            ResetState();
            Instance = this;
            _currentStamina = maxStamina;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            MovementHandle();
            RegenStaminaHandle();
            MiniMapOpenHandle(miniMapUI);
        }

        private void LateUpdate()
        {
            // Lock the canvas UI rotation.
            canvasTransform.right = Vector3.right;
            if(!_isDash) return;
            var color = _spriteRenderer.color;
            color = new Color(color.r, color.g, color.b, 0.5f);
            _spriteRenderer.color = color;
        }
        
        #region Methods

        /// <summary>
        /// Use for control the player movement system.
        /// </summary>
        private void MovementHandle()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackState_1") ||
                _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackState_2") ||
                _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackState_3") ||
                _animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerHeavyAttack") ||
                PlayerHealthSystem.Instance.isDead ||
                PlayerInteractSystem.Instance.isStopMove)
            {
                if (PlayerInteractSystem.Instance.isStopMove)
                {
                    SetPlayerState(PlayerState.Idle);
                    _animator.SetTrigger(playerState.ToString());
                }
                _playerRigidbody2D.velocity = Vector2.zero;
                if (Input.GetKeyDown(dashKey))
                    _dashBuffering = true;
                    
                return;
            }

            WalkHandle();
            SprintHandle();
            DashHandle();

            Vector2 playerVelocity;

            if (_isDash)
            {
                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                {
                    playerVelocity =  transform.right * _currentSpeed;
                    _playerRigidbody2D.velocity = Vector2.ClampMagnitude(playerVelocity, _currentSpeed);
                }
                else
                {
                    if (_firstDashDirection == Vector2.zero)
                        _firstDashDirection = transform.right;
                    playerVelocity =  _firstDashDirection * _currentSpeed;
                    _playerRigidbody2D.velocity = Vector2.ClampMagnitude(playerVelocity, _currentSpeed);
                }
            }
            else
            {
                playerVelocity =  new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * _currentSpeed;
                _playerRigidbody2D.velocity = Vector2.ClampMagnitude(playerVelocity, _currentSpeed);
            }
            _animator.SetTrigger(playerState.ToString());
            _animator.SetBool(IsDashAnimation, _isDash);

            // flip player horizontal direction
            if (Input.GetAxisRaw("Horizontal") != 0)
                transform.right = Input.GetAxisRaw("Horizontal") < 0 ? Vector2.left : Vector2.right;
        }

        /// <summary>
        /// Walk System 
        /// </summary>
        private void WalkHandle()
        {
            if (CheckPlayerState(PlayerState.Dash)) return;
            _currentSpeed = walkSpeed;
            
            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                SetPlayerState(PlayerState.Idle);
                return;
            }
            
            SetPlayerState(PlayerState.Walk);
        }

        /// <summary>
        /// Sprint System
        /// </summary>
        private void SprintHandle()
        {
            if (CheckPlayerState(PlayerState.Dash) || CheckPlayerState(PlayerState.Idle)) return;
            if (_currentStamina <= 0) return;
            if (!Input.GetKey(sprintKey)) return;
      
            _currentSpeed = sprintSpeed;
            SetPlayerState(PlayerState.Sprint);
            _currentStamina -= sprintStaminaDrain * Time.deltaTime;
        }

        /// <summary>
        /// Use for handle dash system.
        /// </summary>
        private void DashHandle()
        {
            if (_isDash) return;
            if (_isDashCooldown) return;
            if (_currentStamina < dashStaminaDrain)
            {
                _dashBuffering = false;
                return;
            }
            if (!Input.GetKeyDown(dashKey) && !_dashBuffering) return;

            StartCoroutine(Dash());
            _currentStamina -= dashStaminaDrain;
        }

        /// <summary>
        /// Dash behavior for start coroutine in dash system.
        /// </summary>
        private IEnumerator Dash()
        {
            _firstDashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            SoundManager.Instance.RandomPlaySound(dashSound);
            _isDash = true;
            _isDashCooldown = true;
            _currentSpeed = dashSpeed;
            SetPlayerState(PlayerState.Dash);

            yield return new WaitForSeconds(dashDuration);
            _isDash = false;
            _dashBuffering = false;
            SetPlayerState(PlayerState.Idle);
            //transform.right *= -1;
            _spriteRenderer.color = Color.white;
            //PlayerCombatSystem.Instance.CurrentAttackCooldown /= 2;

            yield return new WaitForSeconds(dashCooldown);
            _isDashCooldown = false;
        }
        
        
        /// <summary>
        /// Use for set player state.
        /// </summary>
        /// <param name="state">State that you want to set.</param>
        private void SetPlayerState(PlayerState state)
        {
            playerState = state;
        }

        /// <summary>
        /// Regen stamina when player is not running or dashing.
        /// </summary>
        private void RegenStaminaHandle()
        {
            _currentStamina = Mathf.Clamp(_currentStamina, 0, maxStamina);
            if (CheckPlayerState(PlayerState.Sprint) || CheckPlayerState(PlayerState.Dash))
            {
                _staminaRegenCurrentCooldown = 0f;
                return;
            }

            _staminaRegenCurrentCooldown += Time.deltaTime;
            if (_staminaRegenCurrentCooldown < staminaRegenCooldown) return;
            _currentStamina += staminaRegenSpeed * Time.deltaTime;
            _currentStamina = Mathf.Clamp(_currentStamina, 0, maxStamina);
        }
        
        /// <summary>
        /// Use to check the current player state.
        /// </summary>
        /// <param name="state">State that you want to check.</param>
        /// <returns>Is current state is equal to the input state or not. (True/False)</returns>
        private bool CheckPlayerState(PlayerState state)
        {
            return playerState == state;
        }

        /// <summary>
        /// Reset every behavior to default. ( Use when start / respawn. )
        /// </summary>
        public void ResetState()
        {
            SetPlayerState(PlayerState.Idle);
            _isDash = false;
            _isDashCooldown = false;
            _currentSpeed = walkSpeed;
            _currentStamina = maxStamina;
            PlayerInteractSystem.Instance.isStopMove = false;
            PlayerCombatSystem.Instance.CancelAttacking();
        }

        private void MiniMapOpenHandle(GameObject miniMapOpen)
        {
            if (!Input.GetKeyDown(KeyCode.M)) return;
            miniMapOpen.SetActive(!miniMapOpen.activeSelf);
            miniMapCamera.gameObject.SetActive(miniMapOpen.activeSelf);
            SoundManager.Instance.RandomPlaySound(openMinimapSounds);
        }
        #endregion
    }
}