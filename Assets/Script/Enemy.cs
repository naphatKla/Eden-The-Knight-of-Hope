using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

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
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private NavMeshAgent agent;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _spawnPoint;
    public EnemyState EnemyActionState;
    #endregion

    #region Unity Method
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _spawnPoint = transform.position;
        EnemyActionState = EnemyState.Idle;
    }
    
    void Update()
    {
        Collider2D viewDistanceHit = Physics2D.OverlapCircle(transform.position, viewDistance,playerLayerMask);
        
        if (viewDistanceHit)
        {
            SetEnemyState(EnemyState.FollowTarget);
            agent.SetDestination(target.transform.position);
        }
        else if (EnemyActionState == EnemyState.FollowTarget)
        {
            EnemyActionState = EnemyState.WaitToReturn;
            StartCoroutine(SetEnemyState(EnemyState.ReturnToSpawn, 3));
        }
        else if (EnemyActionState == EnemyState.ReturnToSpawn)
        {
            SetEnemyState(EnemyState.ReturnToSpawn);
            agent.SetDestination(_spawnPoint);
        }

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
            
            if(EnemyActionState.Equals(EnemyState.WaitToReturn))
                agent.SetDestination(target.transform.position);
            
            yield return null;
        }

        EnemyActionState = state;
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
                
                break;
            }
            case EnemyState.FollowTarget:
            {
                
                break;
            }
            case EnemyState.ReturnToSpawn:
            {
                
                break;
            }
            case EnemyState.WaitToReturn:
            {
                
                break;
            }
        }
    }
    #endregion
}
