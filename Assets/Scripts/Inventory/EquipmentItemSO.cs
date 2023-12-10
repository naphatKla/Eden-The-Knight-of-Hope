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
    [SerializeField] private float atk;
    [SerializeField] private float atkSpeedPercent;
    [Header("For Usable Item(Quick Slot) Only")]
    public float coolDown;
    public AudioClip[] useSounds;
    
    public void AddStats()
    {
        PlayerCombatSystem.Instance.AttackStat += atk;
        PlayerCombatSystem.Instance.ReduceCoolDownPercent += (atkSpeedPercent/100);
        PlayerHealthSystem.Instance.Heal(hp);
        Player.Instance.CurrentStamina += stamina;
        PlayerHealthSystem.Instance.maxHp += maxHp;
        SoundManager.Instance.RandomPlaySound(useSounds);
    }
    
    public void RemoveStats()
    {
        PlayerCombatSystem.Instance.AttackStat -= atk;
        PlayerCombatSystem.Instance.ReduceCoolDownPercent -= (atkSpeedPercent/100);
        PlayerHealthSystem.Instance.maxHp -= maxHp;
        PlayerHealthSystem.Instance.Heal(-hp);
        Player.Instance.CurrentStamina -= stamina;
    }
}

