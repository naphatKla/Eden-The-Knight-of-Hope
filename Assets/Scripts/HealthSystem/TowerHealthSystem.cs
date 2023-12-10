using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        towerHpFill.color = CurrentHp / maxHp > 0.5f
            ? Color.Lerp(Color.green, Color.yellow, (1 - (CurrentHp / maxHp)) * 2)
            : Color.Lerp(Color.yellow, Color.red, (1 - (CurrentHp / (maxHp / 2))));
        Color color = towerHpFill.color;
        color.a = 0.75f;
        towerHpFill.color = color;
    }

    public override void ResetHealth()
    {
        base.ResetHealth();
        sliderHpPlayer.gameObject.SetActive(CurrentHp < maxHp && CurrentHp > 0);
        towerHpFill.color = CurrentHp / maxHp > 0.5f
            ? Color.Lerp(Color.green, Color.yellow, (1 - (CurrentHp / maxHp)) * 2)
            : Color.Lerp(Color.yellow, Color.red, (1 - (CurrentHp / (maxHp / 2))));
        Color color = towerHpFill.color;
        color.a = 0.75f;
        towerHpFill.color = color;
    }

    protected override void ShowDamageIndicator(float damage)
    {
        if (!damageIndicator) return;
        Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f),0);
        var go = Instantiate(damageIndicator, transform.position + offset, quaternion.identity);
        go.GetComponent<TextMeshPro>().text = $"<color=red>{damage}";
    }
}
