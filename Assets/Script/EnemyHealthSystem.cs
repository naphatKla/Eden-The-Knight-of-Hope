using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    private Enemy _enemy;
    protected override void Start()
    {
        base.Start();
        _enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        _enemy.target = attacker;
        base.TakeDamage(damage);
    }

    protected override void Dead()
    {
        if(Random.Range(0,101) >= 90)
            GameManager.instance.AddPoint(10);
        base.Dead();
    }
}
