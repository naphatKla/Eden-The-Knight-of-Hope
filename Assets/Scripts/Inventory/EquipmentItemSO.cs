using CombatSystem;
using HealthSystem;
using Inventory;
using PlayerBehavior;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class EquipmentItemSO : ItemSo
{
    [FormerlySerializedAs("hp")]
    [Header("Stats Adder")] 
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private float stamina;
    [SerializeField] private float atkPercent;
    [SerializeField] private float atkSpeedPercent;
    [Header("For Usable Item(Quick Slot) Only")]
    public float coolDown;
    
    public void AddStats()
    {
        Debug.LogWarning("Add Stats");
        float baseAttackStat = PlayerCombatSystem.Instance.BaseAttackStat;
        
        PlayerCombatSystem.Instance.AttackStat += (baseAttackStat * (atkPercent/100));
        PlayerCombatSystem.Instance.ReduceCoolDownPercent += (atkSpeedPercent/100);
        PlayerHealthSystem.Instance.Heal(hp);
        Player.Instance.CurrentStamina += stamina;
        PlayerHealthSystem.Instance.maxHp += maxHp;
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
        PlayerHealthSystem.Instance.maxHp -= maxHp;
        PlayerHealthSystem.Instance.Heal(-hp);
        Player.Instance.CurrentStamina -= stamina;
        Debug.Log($"atk : {PlayerCombatSystem.Instance.AttackStat}");
        Debug.Log($"atk speed : {PlayerCombatSystem.Instance.CurrentAttackCooldown}");
        Debug.Log($"hp : {PlayerHealthSystem.Instance.maxHp}");
    }
}

