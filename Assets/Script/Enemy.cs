using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    #region Declare Variable
    public enum EnemyState
    {
        Idle,
        FollowTarget,
        ReturnToSpawn,
        WaitToReturn
    }
    public enum PriorityTag
    {
        Player,
        NPC,
        Tower
    }
    
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<PriorityTag> priorityTags;
    private GameObject _target;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _spawnPoint;
    private bool _isWait;
    private float _idleTimeCounter;
    private bool _isRoam;
    public EnemyState EnemyActionState;
    #endregion

    #region Unity Method
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _isWait = false;
        _spawnPoint = transform.position;
        EnemyActionState = EnemyState.Idle;
    }
    
    void Update()
    {
        Collider2D[] targetInDistances = Physics2D.OverlapCircleAll(transform.position, viewDistance,playerLayerMask);
 
        if (targetInDistances.Length > 0)
        {
            
            // set target to the first priority tag
            for (int i = 0; i < priorityTags.Count; i++)
            {
                foreach (Collider2D col in targetInDistances)
                {
                    if (!col.gameObject.CompareTag(priorityTags[i].ToString())) continue;
                    
                    _target = col.gameObject;
                    i += priorityTags.Count; // break out loop
                    break;
                }
            }
            SetEnemyState(EnemyState.FollowTarget);
        }
        else if (EnemyActionState == EnemyState.FollowTarget)
        {
            
            SetEnemyState(EnemyState.WaitToReturn);
        }
        else if ((Vector2)transform.position == _spawnPoint)
        {
            SetEnemyState(EnemyState.Idle);
        }
        
        PlayAction(EnemyActionState);
        Debug.Log(EnemyActionState);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

    private IEnumerator SetEnemyState(EnemyState state, float time = 0)
    {
        float timeCount = 0;

        while (timeCount < time)
        {
            timeCount += Time.deltaTime;
            _isWait = true;
            yield return null;
        }

        _isWait = false;
        EnemyActionState = state;
    }

    private IEnumerator RoamAround(float time)
    {
        float timeCounter = 0;
        Vector2 randomPos = new Vector2(_spawnPoint.x + Random.Range(-4, 4), _spawnPoint.y + Random.Range(-4, 4));
        
        while (timeCounter < time)
        {
            _isRoam = true;
            agent.SetDestination(randomPos);
            timeCounter += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Rome Done");
        _isRoam = false;
    }
    #endregion

    #region Method
    public void FollowTarget(Vector3 targetPosition, float speed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        _rigidbody2D.velocity = direction * speed;
    }

    public void SetEnemyState(EnemyState state)
    {
        EnemyActionState = state;
    }

    public void PlayAction(EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
            {
                _idleTimeCounter += Time.deltaTime;
                if((int)_idleTimeCounter % 5 != 0) return;
                if (Random.Range(0, 100) < 0) return;
                if (_isRoam) return;
                StartCoroutine(RoamAround(3));
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
                
                if(!_isWait)
                    StartCoroutine(SetEnemyState(EnemyState.ReturnToSpawn, 3));
                break;
            }
        }
    }
    #endregion
}
