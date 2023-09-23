using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyCombatSystem : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;
    public LayerMask targetLayer;
    public Transform attackPoint;
    public int attackDamage = 40;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    private float _nextAttackTime = 0f;
    private GameObject _attackPointParent;
    private bool _isCharge;
    private void Start()
    {
        _attackPointParent = Instantiate(attackPoint.gameObject,transform.position,quaternion.identity,transform);
        attackPoint.SetParent(_attackPointParent.transform);
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyHurt"))
        {
            _nextAttackTime = Time.time + (1f / attackRate);
            _isCharge = false;
        }
        if(_isCharge) return;
        StartCoroutine( Attack(0.5f));
    }

    private IEnumerator Attack(float delay)
    {
        // flip attack point
        if (_enemy.agent.velocity.magnitude != 0)
            _attackPointParent.transform.right =
                new Vector3(_enemy.agent.velocity.x / (Mathf.Abs(_enemy.agent.velocity.x) + 0.01f), 0, 0);
        
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,targetLayer);

        if (hitTargets.Length <= 0) yield break;
        _enemy.agent.velocity = Vector3.zero;
        
        if(Time.time < _nextAttackTime) yield break;
        
        _animator.SetTrigger("Attack");
        _nextAttackTime = Time.time + (1f / attackRate);
        float timeCount = 0;
        
        while (timeCount < delay)
        {
            _isCharge = true;
            _enemy.agent.velocity = Vector3.zero;
            
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyHurt"))
            {
                _nextAttackTime = Time.time + (1f / attackRate);
                _isCharge = false;
                yield break;
            }

            timeCount += Time.deltaTime;
            yield return null;
        }
        
        hitTargets = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,targetLayer);
        if (hitTargets.Length <= 0)
        {
            _isCharge = false;
            yield break;
        }

        foreach (Collider2D target in hitTargets) 
            target.GetComponent<HealthSystem>().TakeDamage(attackDamage);
        
        _isCharge = false;
        _nextAttackTime = Time.time + (1f / attackRate);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}
