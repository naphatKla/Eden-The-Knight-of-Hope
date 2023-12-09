using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HealthSystem;
using UnityEngine;

namespace CombatSystem
{
    public enum AttackState
    {
        AttackState0,
        AttackState1,
        AttackState2,
    }

    [System.Serializable]
    public struct AttackPattern
    {
        public float power;
        public float delay;
        public float cooldown;
        public AttackState attackState;
        public AudioClip[] attackSounds;
    }
    public class CombatSystem : MonoBehaviour
    {
        #region Declare Variable
        [Header("Attack Stats & Config")]
        [SerializeField] protected float baseAttackStat;
        [SerializeField] protected Transform attackPoint;
        [SerializeField] protected Vector2 attackArea;
        [SerializeField] protected LayerMask targetLayer;
        protected List<Collider2D> TargetInAttackArea;
        protected float currentAttackCooldown;
        public float ReduceCoolDownPercent { get; set; }
        protected float attackStat;

        [Header("Normal Attack")]
        [SerializeField] protected List<AttackPattern> attackPatterns;
        protected AttackPattern currentAttackPattern;
        [SerializeField] protected AttackState attackState;
        protected Coroutine attackCoroutine;
        protected Animator animator;
        protected float lastAttackTime;

        private HealthSystem.HealthSystem _healthSystem;
        #endregion
    
        protected virtual void Start()
        {
            attackStat = baseAttackStat;
            attackState = AttackState.AttackState0;
            currentAttackPattern = attackPatterns[0];
            animator = GetComponent<Animator>();
            _healthSystem = GetComponent<HealthSystem.HealthSystem>();
            
        }
    
        protected virtual void Update()
        {
            if (_healthSystem.isDead) return;
            TargetInAttackArea = Physics2D.OverlapBoxAll(attackPoint.position, attackArea, 0 , targetLayer).ToList();
            AttackHandle();
        }

        #region Methods
        /// <summary>
        /// use for handle the attack system.
        /// Please override this method with attack condition before call this method.
        /// </summary>
        protected virtual void AttackHandle()
        {
            if (Time.time < lastAttackTime + currentAttackCooldown) return;   // cooldown check.
            if (attackCoroutine != null) return;
      
            attackState = Time.time - lastAttackTime > 2 ? AttackState.AttackState0 : attackState;
            currentAttackPattern = attackPatterns[(int)attackState];
            currentAttackCooldown = currentAttackPattern.cooldown - (currentAttackPattern.cooldown * ReduceCoolDownPercent);
            attackCoroutine = StartCoroutine(Attack(currentAttackPattern.delay));
        }
    
    
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Attack target with delay.
        /// </summary>
        /// <param name="delay">delay time (sec)</param>
        /// <returns></returns>
        protected virtual IEnumerator Attack(float delay)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger(attackState.ToString());
                
            SoundManager.Instance.RandomPlaySound(currentAttackPattern.attackSounds);
            yield return new WaitForSeconds(delay);
            if (_healthSystem.isDead) yield break;
            
            List<HealthSystem.HealthSystem> targetHealthSystems = TargetInAttackArea.Select(target => target.GetComponent<HealthSystem.HealthSystem>()).ToList();

            List<AudioClip> soundList = new List<AudioClip>();
            targetHealthSystems.ForEach(target =>
            {
                target.TakeDamage(currentAttackPattern.power * attackStat, gameObject);
                
                if (target.CompareTag("Player")) return;
                if (target.takeDamageSounds.Length <= 0) return;
                AudioClip takeDamageSound = target.takeDamageSounds[Random.Range(0, target.takeDamageSounds.Length)];
                if (soundList.Contains(takeDamageSound)) return;
                soundList.Add(takeDamageSound);
                target.PlaySound(takeDamageSound);
            });
        
            // Change attack stage to next stage, if attack state is the last state, change to the first state.
            attackState = (int)attackState >= attackPatterns.Count - 1 ? AttackState.AttackState0 : attackState + 1;
            attackCoroutine = null;
        }
    
    
        /// <summary>
        /// Cancel attacking.
        /// </summary>
        public void CancelAttacking()
        {
            if (attackCoroutine == null) return;
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    
    
        /// <summary>
        /// Just drawn gizmos on the inspector.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!attackPoint) return;
            Gizmos.DrawWireCube(attackPoint.position,attackArea);
        }
        #endregion
    }
}