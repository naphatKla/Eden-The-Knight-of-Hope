using CombatSystem;
using HealthSystem;
using Inventory;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentItemSO : ItemSo
{
    [Header("Stats Adder")] 
    [SerializeField] private float hp;
    [SerializeField] private float atkPercent;
    [SerializeField] private float atkSpeedPercent;
    
    public void AddStats()
    {
        Debug.LogWarning("Add Stats");
        float baseAttackStat = PlayerCombatSystem.Instance.BaseAttackStat;
        
        PlayerCombatSystem.Instance.AttackStat += (baseAttackStat * (atkPercent/100));
        PlayerCombatSystem.Instance.ReduceCoolDownPercent += (atkSpeedPercent/100);
        PlayerHealthSystem.Instance.maxHp += hp;
        Debug.Log($"atk : {PlayerCombatSystem.Instance.AttackStat}");
        Debug.Log($"atk speed : {PlayerCombatSystem.Instance.CurrentAttackCooldown}");
        Debug.Log($"hp : {PlayerHealthSystem.Instance.maxHp}");
    }
    
    public void RemoveStats()
    {
        Debug.LogWarning("Remove Stats");
        float baseAttackStat = PlayerCombatSystem.Instance.BaseAttackStat;

        PlayerCombatSystem.Instance.AttackStat -=  (baseAttackStat * (atkPercent/100));
        PlayerCombatSystem.Instance.ReduceCoolDownPercent -= (atkSpeedPercent/100);
        PlayerHealthSystem.Instance.maxHp -= hp;
        Debug.Log($"atk : {PlayerCombatSystem.Instance.AttackStat}");
        Debug.Log($"atk speed : {PlayerCombatSystem.Instance.CurrentAttackCooldown}");
        Debug.Log($"hp : {PlayerHealthSystem.Instance.maxHp}");
    }
}

