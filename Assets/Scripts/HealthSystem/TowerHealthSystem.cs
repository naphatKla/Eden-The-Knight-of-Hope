
using System;
using UnityEngine;

public class TowerHealthSystem : HealthSystem.HealthSystem
{
    public Action OnTowerDamaged;

    public override void TakeDamage(float damage, GameObject attacker = null)
    {
        base.TakeDamage(damage, attacker);
        if (this)
            OnTowerDamaged?.Invoke();
    }
}
