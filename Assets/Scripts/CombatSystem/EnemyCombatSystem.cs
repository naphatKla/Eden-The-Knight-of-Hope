using System.Linq;
using EnemyBehavior;
using UnityEngine;

namespace CombatSystem
{
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
            if (targetInAttackArea.All(target => target.gameObject != _enemy.Target)) return;
            
            _enemy.Agent.velocity = Vector2.zero;
            base.AttackHandle();
        }
    }
}
