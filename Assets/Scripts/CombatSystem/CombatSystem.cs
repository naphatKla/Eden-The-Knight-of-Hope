using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private AttackState attackState;
        private Coroutine _attackCoroutine;
        private Animator _animator;
        protected float lastAttackTime;
        #endregion
    
        protected virtual void Start()
        {
            attackStat = baseAttackStat;
            attackState = AttackState.AttackState0;
            currentAttackPattern = attackPatterns[0];
            _animator = GetComponent<Animator>();
        }
    
        protected virtual void Update()
        {
            TargetInAttackArea = Physics2D.OverlapBoxAll(attackPoint.position, attackArea, 0, targetLayer).ToList();
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
            if (_attackCoroutine != null) return;
      
            attackState = Time.time - lastAttackTime > 2 ? AttackState.AttackState0 : attackState;
            currentAttackPattern = attackPatterns[(int)attackState];
            currentAttackCooldown = currentAttackPattern.cooldown - (currentAttackPattern.cooldown * ReduceCoolDownPercent);
            _attackCoroutine = StartCoroutine(Attack(currentAttackPattern.delay));
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
            _animator.SetTrigger(attackState.ToString());
        
            yield return new WaitForSeconds(delay);
        
            List<HealthSystem.HealthSystem> targetHealthSystems = TargetInAttackArea.Select(target => target.GetComponent<HealthSystem.HealthSystem>()).ToList();
            targetHealthSystems.ForEach(target => target.TakeDamage(currentAttackPattern.power * attackStat,gameObject));
        
            // Change attack stage to next stage, if attack state is the last state, change to the first state.
            attackState = (int)attackState >= attackPatterns.Count - 1 ? AttackState.AttackState0 : attackState + 1;
            _attackCoroutine = null;
        }
    
    
        /// <summary>
        /// Cancel attacking.
        /// </summary>
        protected void CancelAttacking()
        {
            if (_attackCoroutine == null) return;
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
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