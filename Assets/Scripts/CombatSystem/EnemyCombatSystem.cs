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

        /// <summary>
        /// Attack when the target is in attack area.
        /// </summary>
        protected override void AttackHandle()
        {
            if (TargetInAttackArea.Count <= 0) return;
            if (_enemy.IsStun)
            {
                CancelAttacking();
                return;
            }
            
            // if the target is not in attack area, cancel attacking and follow the target.
            if (TargetInAttackArea.All(target => target.gameObject != _enemy.Target)) return;
            
            _enemy.Agent.velocity = Vector2.zero;
            base.AttackHandle();
        }
    }
}
