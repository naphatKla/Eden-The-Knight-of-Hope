using PlayerBehavior;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem
{
    public class PlayerCombatSystem : CombatSystem
    {
        [SerializeField] private Slider attackCooldownSlider;
        [SerializeField] private Image weaponSlot;
        [SerializeField] private Image weaponSlotCooldown;
        public static PlayerCombatSystem Instance { get; private set;}
        public float AttackStat {get => attackStat; set => attackStat = value;}
        public float BaseAttackStat {get => baseAttackStat;}
        public AttackPattern CurrentAttackPattern { get => currentAttackPattern; }
        public float CurrentAttackCooldown {get => currentAttackCooldown; set => currentAttackCooldown = value;}
        private bool _attackBuffering;
        
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
                weaponSlotCooldown.gameObject.SetActive((weaponSlot.isActiveAndEnabled));
                weaponSlotCooldown.fillAmount = 1-progress;
            }
            
            if (UIManager.Instance.CheckIsAnyUIOpen()) return;
            if (Player.Instance.Animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDash"))
            {
                if (Input.GetMouseButtonDown(0))
                    _attackBuffering = true;
                return;
            }
            if(!Input.GetMouseButtonDown(0) && !_attackBuffering) return;
            base.AttackHandle();
            _attackBuffering = false;
        }
    }
}
