using System;
using PlayerBehavior;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem
{
    public class PlayerCombatSystem : CombatSystem
    {
        [SerializeField] private Slider attackCooldownSlider;
        public static PlayerCombatSystem Instance { get; private set;}
        public float AttackStat {get => attackStat; set => attackStat = value;}
        public float BaseAttackStat {get => baseAttackStat;}
        public AttackPattern CurrentAttackPattern { get => currentAttackPattern; }
        public float CurrentAttackCooldown {get => currentAttackCooldown; set => currentAttackCooldown = value;}
        
        protected override void Start()
        {
            base.Start();
            Instance = this;
        }
        
        /// <summary>
        /// Attack when the player click the mouse.
        /// </summary>
        protected override void AttackHandle()
        {
            float progress = (Time.time - lastAttackTime) / currentAttackCooldown;
            if (double.IsFinite(progress))
            {
                attackCooldownSlider.gameObject.SetActive((progress <= 1));
                attackCooldownSlider.value = progress;
            }
            
            if (UIManager.Instance.isAnyUIOpen) return;
            if (Player.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDash")) return;
            if(!Input.GetMouseButtonDown(0)) return;
            base.AttackHandle();
        }
    }
}
