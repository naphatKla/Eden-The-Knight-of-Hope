using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator _animator;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public int attackDamage = 40;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time < nextAttackTime || !Input.GetKeyDown(KeyCode.Mouse0)) return;
        
        Attack();
        nextAttackTime = Time.time + (1f / attackRate);
    }

    void Attack()
    {
        _animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
        
        /*foreach (Collider2D enemy in hitEnemies)
            enemy.GetComponent<Enemy01>().TakeDamage(attackDamage);*/
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}
