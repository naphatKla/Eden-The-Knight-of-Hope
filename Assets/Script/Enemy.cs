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
    [SerializeField] private List<PriorityTag> priorityTags;
    [SerializeField] private float roamDuration;
    [SerializeField] private float roamCooldown;
    [SerializeField] private Vector2 roamArea;
    private NavMeshAgent _agent;
    private Rigidbody2D _rigidbody2D;
    private GameObject _target;
    private Vector2 _spawnPoint;
    private bool _isWait;
    private bool _isRoam;
    private float _roamCooldownCounter;
    public EnemyState enemyActionState;
    #endregion

    #region Unity Method
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _isWait = false;
        _spawnPoint = transform.position;
        enemyActionState = EnemyState.Idle;
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
        else if (enemyActionState == EnemyState.FollowTarget)
        {
            
            SetEnemyState(EnemyState.WaitToReturn);
        }
        else if ((Vector2)transform.position == _spawnPoint)
        {
            SetEnemyState(EnemyState.Idle);
        }
        
        PlayAction(enemyActionState);
        Debug.Log(enemyActionState);
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
        enemyActionState = state;
    }

    private IEnumerator RoamAround(float time)
    {
        float _timeCount = 0;
        Vector2 randomPos = new Vector2(_spawnPoint.x + Random.Range(roamArea.x, roamArea.y), _spawnPoint.y + Random.Range(roamArea.x, roamArea.y));
        
        while (_timeCount < time)
        {
            _isRoam = true;
            _agent.SetDestination(randomPos);
            _timeCount += Time.deltaTime;
            if (enemyActionState != EnemyState.Idle)
                yield break;
            yield return null;
        }

        _roamCooldownCounter = 0;
        _isRoam = false;
    }
    #endregion

    #region Method
    public void SetEnemyState(EnemyState state)
    {
        enemyActionState = state;
    }

    public void PlayAction(EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
            {
                _roamCooldownCounter += Time.deltaTime;
                if(_roamCooldownCounter < roamCooldown) return;
                if (_isRoam) return;
                StartCoroutine(RoamAround(roamDuration));
                break;
            }
            case EnemyState.FollowTarget:
            {
                _agent.SetDestination(_target.transform.position);
                break;
            }
            case EnemyState.ReturnToSpawn:
            {
                _agent.SetDestination(_spawnPoint);
                break;
            }
            case EnemyState.WaitToReturn:
            {
                _agent.SetDestination(_target.transform.position);
                
                if(!_isWait)
                    StartCoroutine(SetEnemyState(EnemyState.ReturnToSpawn, 3));
                break;
            }
        }
    }
    #endregion
}
