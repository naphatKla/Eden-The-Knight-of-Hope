
using System;
using UnityEngine;

public class TowerHealthSystem : HealthSystem.HealthSystem
{
    public Action OnTowerDamaged;

    public override void TakeDamage(float damage, GameObject attacker = null)
    {
        OnTowerDamaged?.Invoke();
        base.TakeDamage(damage, attacker);
    }
}
