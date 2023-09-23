using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealthSystem : HealthSystem
{
    [SerializeField] private float hpRegenPercentage;
    protected override void Update()
    {
        base.Update();
        
        if(TimeSystem.instance.GetTimeState() != TimeSystem.TimeState.Day) return;
        _currentHp +=  ((hpRegenPercentage / 100) * maxHp) * Time.deltaTime;
    }
}
