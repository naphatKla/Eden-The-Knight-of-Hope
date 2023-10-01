using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class EnemyCombatSystem : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;
    public LayerMask targetLayer;
    public Transform attackPoint;
    public int attackDamage = 40;
    public Vector2 attackArea;
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
        // stun
        if (_enemy.isStun)
        {
            //_nextAttackTime = Time.time + (1f / attackRate);
            _isCharge = false;
        }
        if(_isCharge) return;
        StartCoroutine( Attack(0.4f));
    }

    private IEnumerator Attack(float delay)
    {
        // flip attack point
        if (_enemy.spriteRenderer.flipX)
            _attackPointParent.transform.right =
                new Vector3(-1, 0, 0);
        else
            _attackPointParent.transform.right =
                new Vector3(1, 0, 0);
        

        List<Collider2D> hitTargets =
            Physics2D.OverlapBoxAll(attackPoint.position, attackArea, 0, targetLayer).ToList();
        
        if (hitTargets.Count <= 0) yield break;
        if (!hitTargets.Contains(_enemy.target.GetComponent<Collider2D>())) yield break;

        _enemy.agent.velocity = Vector3.zero;
        
        if(Time.time < _nextAttackTime) yield break;
        
        _animator.SetTrigger("Attack");
        _nextAttackTime = Time.time + (1f / attackRate);
        float timeCount = 0;
        
        while (timeCount < delay)
        {
            _isCharge = true;
            _enemy.agent.velocity = Vector3.zero;
            
            // stun
            if (_enemy.isStun)
            {
                _nextAttackTime = Time.time + (1f / attackRate);
                _isCharge = false;
                yield break;
            }
            
            timeCount += Time.deltaTime;
            yield return null;
        }
        
        hitTargets =  Physics2D.OverlapBoxAll(attackPoint.position,attackArea, 0, targetLayer).ToList();
        if (hitTargets.Count <= 0)
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
        Gizmos.DrawWireCube(attackPoint.position,attackArea);
    }
}
