using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletDamage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float attackRate;
    private float _nextAttackTime;
    private GameObject _target;
    void Start()
    {
        
    }


    void Update()
    {
        Collider2D[] targetInAttackArea = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius,targetLayerMask);
        
        if (targetInAttackArea.Length <= 0) return;

        List<float> distances = new List<float>();
        foreach (Collider2D col in targetInAttackArea)
            distances.Add(Vector2.Distance(transform.position, col.transform.position));

        _target = targetInAttackArea[distances.IndexOf(distances.Min())].gameObject;
        
        if (Time.time < _nextAttackTime) return;
        Bullet bulletSpawn =  Instantiate(bulletPrefab, bulletSpawnPoint.position,Quaternion.identity).GetComponent<Bullet>();
        bulletSpawn.Init(_target,bulletSpeed,bulletDamage,this);
        
        _nextAttackTime = Time.time + (1f / attackRate);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
