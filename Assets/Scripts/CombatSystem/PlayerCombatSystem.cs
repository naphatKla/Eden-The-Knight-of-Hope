using PlayerBehavior;
using UnityEngine;

namespace CombatSystem
{
    public class PlayerCombatSystem : CombatSystem
    {
        /// <summary>
        /// Attack when the player click the mouse.
        /// </summary>
        protected override void AttackHandle()
        {
            if (Player.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDash")) return;
            if(!Input.GetMouseButtonDown(0)) return;
            base.AttackHandle();
        }
    }
}
