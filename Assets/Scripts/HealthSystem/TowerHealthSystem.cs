
using System;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthSystem : HealthSystem.HealthSystem
{
    public Action OnTowerDamaged;
    public Image towerHpFill;

    public override void TakeDamage(float damage, GameObject attacker = null)
    {
        base.TakeDamage(damage, attacker);
        towerHpFill.color = CurrentHp / maxHp > 0.5f
            ? Color.Lerp(Color.green, Color.yellow, (1 - (CurrentHp / maxHp)) * 2)
            : Color.Lerp(Color.yellow, Color.red, (1 - (CurrentHp / (maxHp / 2))));
        Color color = towerHpFill.color;
        color.a = 0.75f;
        towerHpFill.color = color;
        if (this)
            OnTowerDamaged?.Invoke();
    }
}
