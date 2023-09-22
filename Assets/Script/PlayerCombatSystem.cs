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
    public Transform heavyAttackPoint;
    public int attackDamage = 40;
    public int heavyAttackDamage = 60;
    public float attackRange = 0.5f;
    public float heavyAttackRange = 2f;
    public float attackRate = 1f;
    private float _nextAttackTime = 0f;
    private int noOfClick = 0; 
    private float currentCooldown = 0f;
    private GameObject _attackPointParent;
    private GameObject _heavyAttackPointParent;

    private void Start()
    {
        _attackPointParent = Instantiate(attackPoint.gameObject,transform.position,quaternion.identity,transform);
        attackPoint.SetParent(_attackPointParent.transform);
        _heavyAttackPointParent = Instantiate(heavyAttackPoint.gameObject,transform.position,quaternion.identity,transform);
        heavyAttackPoint.SetParent(_heavyAttackPointParent.transform);
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        currentCooldown = currentCooldown - Time.deltaTime;
        if (currentCooldown <= 0 )
        {
            currentCooldown = 0;
            noOfClick = 0;
        }
        if (Input.GetAxisRaw("Horizontal")  != 0)
            _attackPointParent.transform.right = new Vector3(Input.GetAxisRaw("Horizontal") , 0, 0);

        if ((Time.time > _nextAttackTime) & Input.GetKeyDown(KeyCode.Mouse1))
        {
            HeavyAttack();
            _nextAttackTime = Time.time + (1f / attackRate);
        }
        
        if (Time.time < _nextAttackTime || !Input.GetKeyDown(KeyCode.Mouse0)) return;
        Attack();
        noOfClick++;
        currentCooldown = currentCooldown + attackRate+ 0.2f;
        if (currentCooldown > attackRate+0.5f)
        {
            currentCooldown = attackRate + 0.5f;
        }
        _nextAttackTime = Time.time + (1f / attackRate);
    }

    void Attack()
    {
        // flip attack point
        if (noOfClick == 0)
        {
            _animator.SetTrigger("Attack1");
        }
        if (noOfClick == 1)
        {
            _animator.SetTrigger("Attack2");
        }
        if (noOfClick == 2)
        {
            _animator.SetTrigger("Attack3");
            noOfClick = -1;
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayer);
        if (noOfClick != -1)
        {
            foreach (Collider2D enemy in hitEnemies)
                enemy.GetComponent<HealthSystem>().TakeDamage(attackDamage);
        }
        else
        {
            foreach (Collider2D enemy in hitEnemies)
                enemy.GetComponent<HealthSystem>().TakeDamage(attackDamage+(attackDamage/2));
        }
    }

    void HeavyAttack()
    {
        _animator.SetTrigger("HeavyAttack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(heavyAttackPoint.position,heavyAttackRange,enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
            enemy.GetComponent<HealthSystem>().TakeDamage(heavyAttackDamage);
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
            Gizmos.DrawWireSphere(attackPoint.position,attackRange);
            Gizmos.DrawWireSphere(heavyAttackPoint.position,heavyAttackRange);
    }
}
