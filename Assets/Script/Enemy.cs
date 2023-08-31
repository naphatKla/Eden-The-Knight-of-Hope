using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    #region Declare Variable
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float viewDistance;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _spawnPoint;

    #endregion

    #region Unity Method
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spawnPoint = transform.position;
    }
    
    void Update()
    {
        Collider2D viewDistanceHit = Physics2D.OverlapCircle(transform.position, viewDistance);
        
        if (!viewDistanceHit) return;
        FollowTarget(target.transform.position, speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

    #endregion

    public void FollowTarget(Vector3 targetPosition, float speed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        _rigidbody2D.velocity = direction * speed;
    }
}
