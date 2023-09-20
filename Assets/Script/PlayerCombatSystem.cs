using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCombatSystem : MonoBehaviour
{
    private Animator _animator;
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public int attackDamage = 40;
    public Vector2 attackArea;
    public float attackRate = 2f;
    private float _nextAttackTime = 0f;
    private GameObject _attackPointParent;

    private void Start()
    {
        _attackPointParent = Instantiate(attackPoint.gameObject,transform.position,quaternion.identity,transform);
        attackPoint.SetParent(_attackPointParent.transform);
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal")  != 0)
            _attackPointParent.transform.right = new Vector3(Input.GetAxisRaw("Horizontal") , 0, 0);
        
        if (Time.time < _nextAttackTime || !Input.GetKeyDown(KeyCode.Mouse0)) return;
        
        Attack();
        _nextAttackTime = Time.time + (1f / attackRate);
    }

    void Attack()
    {
        _animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position,attackArea,0,enemyLayer);
        
        foreach (Collider2D enemy in hitEnemies)
            enemy.GetComponent<EnemyHealthSystem>().TakeDamage(attackDamage,gameObject);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireCube(attackPoint.position,attackArea);
    }
}
