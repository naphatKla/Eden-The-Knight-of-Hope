using CombatSystem;
using HealthSystem;
using Inventory;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class EquipmentItemSO : ItemSo
{
    [Header("Stats Adder")] 
    [SerializeField] private float hp;
    [SerializeField] private float atkPercent;
    [SerializeField] private float atkSpeedPercent;
    
    public void AddStats()
    {
        float baseAttackStat = PlayerCombatSystem.Instance.BaseAttackStat;
        
        PlayerCombatSystem.Instance.AttackStat = baseAttackStat + (baseAttackStat * (atkPercent/100));
        PlayerCombatSystem.Instance.ReduceCoolDownPercent = (atkSpeedPercent/100);
        PlayerHealthSystem.Instance.maxHp += hp;
    }
    
    public void RemoveStats()
    {
        float baseAttackStat = PlayerCombatSystem.Instance.BaseAttackStat;

        PlayerCombatSystem.Instance.AttackStat = baseAttackStat;
        PlayerCombatSystem.Instance.ReduceCoolDownPercent = 0;
        PlayerHealthSystem.Instance.maxHp -= hp;
    }
}

