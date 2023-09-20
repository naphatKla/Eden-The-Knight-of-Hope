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
        base.TakeDamage(damage);
        _enemy.target = attacker;
    }
}
