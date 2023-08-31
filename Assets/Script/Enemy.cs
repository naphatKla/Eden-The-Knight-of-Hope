using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    #region Declare Variable
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask playerLayerMask;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _spawnPoint;
    public EnemyState EnemyActionState;
    #endregion

    #region Unity Method
    void Start()
    {
        EnemyActionState = EnemyState.Idle;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spawnPoint = transform.position;
    }
    
    void Update()
    {
        Collider2D viewDistanceHit = Physics2D.OverlapCircle(transform.position, viewDistance,playerLayerMask);
        
        if (viewDistanceHit)
        {
            SetEnemyState(EnemyState.FollowTarget);
            FollowTarget(target.transform.position, speed);
        }
        else if (EnemyActionState == EnemyState.FollowTarget)
        {
            EnemyActionState = EnemyState.WaitToReturn;
            StartCoroutine(SetEnemyState(EnemyState.ReturnToSpawn, 3));
        }
        else if (EnemyActionState == EnemyState.ReturnToSpawn)
        {
            SetEnemyState(EnemyState.ReturnToSpawn);
            FollowTarget(_spawnPoint, speed * 2);
            
            if (Mathf.Round(transform.position.x) == Mathf.Round(_spawnPoint.x) && Mathf.Round(transform.position.y) == Mathf.Round(_spawnPoint.y))
            {
                SetEnemyState(EnemyState.Idle);
                _rigidbody2D.velocity = Vector3.zero;
            }
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
                FollowTarget(target.transform.position, speed);
            
            yield return null;
        }

        EnemyActionState = state;
    }
    
    #endregion

    public void FollowTarget(Vector3 targetPosition, float speed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        _rigidbody2D.velocity = direction * speed;
    }

    public void SetEnemyState(EnemyState state)
    {
        EnemyActionState = state;
    }
    
    public enum EnemyState
    {
        Idle,
        FollowTarget,
        ReturnToSpawn,
        WaitToReturn
    }
}
