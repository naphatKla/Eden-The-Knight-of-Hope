
using System;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthSystem : HealthSystem.HealthSystem
{
    public Action OnTowerDamaged;
    public Image towerHpFill;

    protected override void Start()
    {
        base.Start();
        sliderHpPlayer.gameObject.SetActive(CurrentHp < maxHp && CurrentHp > 0);
    }
    public override void TakeDamage(float damage, GameObject attacker = null)
    {
        base.TakeDamage(damage, attacker);
        sliderHpPlayer.gameObject.SetActive(CurrentHp < maxHp && CurrentHp > 0);
        towerHpFill.color = CurrentHp / maxHp > 0.5f
            ? Color.Lerp(Color.green, Color.yellow, (1 - (CurrentHp / maxHp)) * 2)
            : Color.Lerp(Color.yellow, Color.red, (1 - (CurrentHp / (maxHp / 2))));
        Color color = towerHpFill.color;
        color.a = 0.75f;
        towerHpFill.color = color;
        if (this)
            OnTowerDamaged?.Invoke();
    }

    public override void Heal(float healPoint)
    {
        base.Heal(healPoint);
        sliderHpPlayer.gameObject.SetActive(CurrentHp < maxHp && CurrentHp > 0);
    }
}
