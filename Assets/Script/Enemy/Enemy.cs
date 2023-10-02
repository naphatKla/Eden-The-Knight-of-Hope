using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    #region Declare Variable
    public enum EnemyState
    {
        Idle,
        FollowTarget,
        ReturnToSpawn,
        WaitToReturn,
    }

    public enum PriorityTag
    {
        Player,
        NPC,
        Tower,
        Base
    }
    
    [SerializeField] private float viewDistance;
    [SerializeField] private float roamDuration; 
    [SerializeField] private float roamCooldown; 
    [SerializeField] private Vector2 roamArea; 
    [SerializeField] private bool nightMode;
    [SerializeField] private LayerMask playerLayerMask;  
    [SerializeField] private List<PriorityTag> priorityTags;
    [SerializeField] private EnemyState enemyActionState;
    private bool _isStun;
    private List<Collider2D> _targetInDistances;
    private Coroutine _waitToReturnCoroutine;
    private Coroutine _roamAroundCoroutine;
    private Vector2 _spawnPoint;
    private GameObject _target;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    
    public bool NightMode { get => nightMode; set => nightMode = value; }
    public bool IsStun { get => _isStun; set => _isStun = value; }
    public GameObject Target { get => _target; set => _target = value; }
    public NavMeshAgent Agent { get => _agent;}
    public SpriteRenderer SpriteRenderer { get => _spriteRenderer; }
    #endregion
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _agent.updateUpAxis = false;
        _agent.updateRotation = false;
        _spawnPoint = transform.position;
        enemyActionState = EnemyState.Idle;
    }

    void Update()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyHurt")) return;
        if(_isStun) return;

        BehaviorHandle();
        AnimationUpdate();
    }
    
    #region Methods
    
    /// <summary>
    /// Handle enemy behavior.
    /// </summary>
    private void BehaviorHandle()
    {
        _targetInDistances = Physics2D.OverlapCircleAll(transform.position, viewDistance, playerLayerMask).ToList();

        if (_targetInDistances.Count > 0)
            FollowTargetHandle();
        
        else if (CheckState(EnemyState.FollowTarget))
            ReturnToSpawnHandle();
        
        else if ((Vector2)transform.position == _spawnPoint)
            IdleHandle();
        
        else if (nightMode)
            _agent.SetDestination(GameManager.instance.playerBase.position);
    }
    
    /// <summary>
    /// Select target with the first priority tag found in the target list.
    /// </summary>
    private void SelectTarget()
    {
        if (enemyActionState == EnemyState.FollowTarget) return;
        
        // Detect and set target only on the first priority tag found 
        foreach (PriorityTag priorityTag in priorityTags)
        {
            Collider2D target = _targetInDistances.FirstOrDefault(target => target.CompareTag(priorityTag.ToString()));
            if(target.IsUnityNull()) continue;
            _target = target.gameObject;
            break;
        }
    }
    
    /// <summary>
    /// Follow target.
    /// </summary>
    private void FollowTargetHandle()
    {
        SelectTarget();
        SetEnemyState(EnemyState.FollowTarget);
        _agent.SetDestination(_target.transform.position);
    }
    
    /// <summary>
    /// Return to spawn point.
    /// </summary>
    private void ReturnToSpawnHandle()
    {
        SetEnemyState(EnemyState.WaitToReturn);

        if (_target.IsUnityNull() || !_target.gameObject.activeSelf)
        {
            SetEnemyState(EnemyState.ReturnToSpawn);
            _agent.SetDestination(_spawnPoint);
            return;
        }

        _agent.SetDestination(_target.transform.position);

        if (_waitToReturnCoroutine != null) return;
        _waitToReturnCoroutine = StartCoroutine(WaitToReturn(3));
    }
    
    /// <summary>
    /// Idle and roam around spawn point.
    /// </summary>
    private void IdleHandle()
    {
        SetEnemyState(EnemyState.Idle);
        if (_roamAroundCoroutine != null) return;
        _roamAroundCoroutine = StartCoroutine(RoamAround(roamDuration));
    }
    
    /// <summary>
    /// Update enemy animation.
    /// </summary>
    private void AnimationUpdate()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);

        // flip horizontal direction relate with enemy direction
        if (Mathf.Abs(_agent.velocity.x) > 1)
            _spriteRenderer.flipX = _agent.velocity.x < 0f;
    }

    /// <summary>
    /// Set enemy state.
    /// </summary>
    /// <param name="state">State that you want to set.</param>
    public void SetEnemyState(EnemyState state)
    {
        enemyActionState = state;
    }
    
    /// <summary>
    /// Check enemy state.
    /// </summary>
    /// <param name="state">State that you want to check.</param>
    /// <returns></returns>
    public bool CheckState(EnemyState state)
    {
        return state == enemyActionState;
    }
    
    /// <summary>
    /// Wait for a while and return to spawn point.
    /// </summary>
    /// <param name="time">Time to wait.</param>
    /// <returns></returns>
    private IEnumerator WaitToReturn(float time = 0)
    {
        yield return new WaitForSeconds(time);
        enemyActionState = EnemyState.ReturnToSpawn;
    }
    
    /// <summary>
    /// Roam around spawn point.
    /// </summary>
    /// <param name="time">Roam duration.</param>
    /// <returns></returns>
    private IEnumerator RoamAround(float time)
    {
        float timeCount = 0;
        Vector2 randomPos = new Vector2(_spawnPoint.x + Random.Range(roamArea.x, roamArea.y),
            _spawnPoint.y + Random.Range(roamArea.x, roamArea.y));

        yield return new WaitForSeconds(roamCooldown);

        while (timeCount < time)
        {
            timeCount += Time.deltaTime;
            _agent.SetDestination(randomPos);

            if (!CheckState(EnemyState.Idle)) yield break;
            yield return null;
        }
    }
    
    /// <summary>
    ///  Draw gizmos on inspector
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
    #endregion
}
