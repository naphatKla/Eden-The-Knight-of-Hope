using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
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
        FocusOnTower,
    }

    public enum PriorityTag
    {
        Player,
        NPC,
        Tower
    }
    
    [SerializeField] private float viewDistance; 
    [SerializeField] private LayerMask playerLayerMask;  
    [SerializeField] private List<PriorityTag> priorityTags; 
    [SerializeField] private float roamDuration; 
    [SerializeField] private float roamCooldown; 
    [SerializeField] private Vector2 roamArea; 
    public NavMeshAgent agent;
    public bool nightMode;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private GameObject _target;
    private Vector2 _spawnPoint;
    private bool _isWait;
    private bool _isRoam;
    public EnemyState enemyActionState;

    #endregion
    
    
    #region Unity Method
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _isWait = false;
        _spawnPoint = transform.position;
        enemyActionState = EnemyState.Idle;
    }

    void Update()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyHurt")) return;

        Collider2D[] targetInDistances = Physics2D.OverlapCircleAll(transform.position, viewDistance, playerLayerMask);

        if (targetInDistances.Length > 0)
        {
            // Detect targets only on the first priority tag found
            for (int i = 0; i < priorityTags.Count; i++)
            {
                foreach (Collider2D col in targetInDistances)
                {
                    if (!col.gameObject.CompareTag(priorityTags[i].ToString())) continue;

                    _target = col.gameObject;
                    i += priorityTags.Count; // break out the loop
                    break;
                }
            }

            SetEnemyState(EnemyState.FollowTarget);
        }
        else if (enemyActionState == EnemyState.FollowTarget)
        {
            SetEnemyState(EnemyState.WaitToReturn);
        }
        else if ((Vector2)transform.position == _spawnPoint)
        {
            SetEnemyState(EnemyState.Idle);
        }
        else if (nightMode)
        {
            SetEnemyState(EnemyState.FocusOnTower);
        }

        PlayAction(enemyActionState);
        _animator.SetFloat("Speed",agent.velocity.magnitude);
        
        // flip horizontal direction relate with enemy direction
        if (agent.velocity.x != 0)
            _spriteRenderer.flipX = agent.velocity.x < 0;

    }

    
    /// <summary>
    ///  Draw gizmos on inspector
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

    
    /// <summary>
    ///  Wait for a second and set enemy state to ReturnToSpawn.
    /// </summary>
    private IEnumerator WaitToReturn(float time = 0)
    {
        _isWait = true;
        yield return new WaitForSeconds(time);
        _isWait = false;
        enemyActionState = EnemyState.ReturnToSpawn;
    }
    
    
    /// <summary>
    ///  Roam an enemy around enemy's spawn point.
    /// </summary>
    private IEnumerator RoamAround(float time)
    {
        float _timeCount = 0;
        Vector2 randomPos = new Vector2(_spawnPoint.x + Random.Range(roamArea.x, roamArea.y),
            _spawnPoint.y + Random.Range(roamArea.x, roamArea.y));
        _isRoam = true;

        yield return new WaitForSeconds(roamCooldown);

        while (_timeCount < time)
        {
            _isRoam = true;
            _timeCount += Time.deltaTime;
            agent.SetDestination(randomPos);

            if (enemyActionState != EnemyState.Idle)
            {
                _isRoam = false;
                yield break;
            }

            yield return null;
        }

        _isRoam = false;
    }
    #endregion

    
    #region Method
    
    /// <summary>
    ///  Set enemy state.
    /// </summary>
    public void SetEnemyState(EnemyState state)
    {
        enemyActionState = state;
    }

    
    /// <summary>
    ///  Play enemy action and behavior follow enemy state.
    /// </summary>
    public void PlayAction(EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
            {
                if (_isRoam) return;
                StartCoroutine(RoamAround(roamDuration));
                break;
            }
            case EnemyState.FollowTarget:
            {
                agent.SetDestination(_target.transform.position);
                break;
            }
            case EnemyState.ReturnToSpawn:
            {
                agent.SetDestination(_spawnPoint);
                break;
            }
            case EnemyState.WaitToReturn:
            {
                agent.SetDestination(_target.transform.position);
                
                if (!_isWait)
                    StartCoroutine(WaitToReturn(3));
                break;
            }
            case EnemyState.FocusOnTower:
            {
                agent.SetDestination(GameManager.instance.tower.position);
                break;
            }
        }
    }
    #endregion
}
