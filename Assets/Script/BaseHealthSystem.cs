using UnityEngine;

public class BaseHealthSystem : HealthSystem
{
    [SerializeField] private float hpRegenPercentage;
    private void Update()
    {
        // regen hp when day time.
        if(TimeSystem.instance.GetTimeState() != TimeState.Day) return;
        Heal((hpRegenPercentage / 100) * maxHp * Time.deltaTime);
    }
}
