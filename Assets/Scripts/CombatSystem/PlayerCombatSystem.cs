using UnityEngine;

namespace CombatSystem
{
    public class PlayerCombatSystem : CombatSystem
    {
        protected override void AttackHandle()
        {
            if(!Input.GetMouseButtonDown(0)) return;
            base.AttackHandle();
        }
    }
}
