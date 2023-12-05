using HealthSystem;
using Interaction;
using PlayerBehavior;
using UnityEngine;

public class Catheral : InteractableObject
{
    //[SerializeField] private float healAmount;
    [SerializeField] private int cost;
    
    protected override void InteractHandler()
    {
        prompt = GameManager.Instance.totalPoint >= cost ? $"<b>[ E ] Full heal</b>\n<color=green>Cost: {cost}$</color>" : $"<b>[ E ] Full heal</b>\n<color=red>Cost: {cost}$</color>";
        interactionTextUI.text = prompt;
        if (GameManager.Instance.totalPoint < cost) return;
        base.InteractHandler();
    }

    protected override void InteractAction()
    {
        PlayerHealthSystem.Instance.Heal(PlayerHealthSystem.Instance.maxHp);
        Player.Instance.CurrentStamina = Player.Instance.MaxStamina;
        GameManager.Instance.AddPoint(-cost);
    }
}
