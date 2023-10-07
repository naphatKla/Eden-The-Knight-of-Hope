using Unity.VisualScripting;
using UnityEngine;

public class EnemyCombatSystem : CombatSystem
{
    private Enemy _enemy;

    protected override void Start()
    {
        _enemy = GetComponent<Enemy>();
        base.Start();
    }

    protected override void AttackHandle()
    {
        if (targetInAttackArea.Count <= 0) return;
        
        if (_enemy.IsStun)
        {
            CancelAttacking();
            return;
        }

        base.AttackHandle();
    }
}
