using System.Collections;
using EnemyBehavior;
using UnityEngine;

namespace HealthSystem
{
    public class EnemyHealthSystem : HealthSystem
    {
        private Enemy _enemy;
        protected override void Start()
        {
            _enemy = GetComponent<Enemy>();
            base.Start();
        }
    
        public override void TakeDamage(float damage, GameObject attacker)
        {
            _enemy.Target = attacker;
        
            if (attacker.CompareTag("Player"))
                StartCoroutine(Stun(0.5f));
        
            base.TakeDamage(damage);
        }
    
        /// <summary>
        /// Dead and add point to the player score.
        /// </summary>
        protected override void Dead()
        {
            if(Random.Range(0,101) >= 90)
                GameManager.instance.AddPoint(10);
        
            base.Dead();
        }

        /// <summary>
        /// stun the enemy.
        /// </summary>
        /// <param name="stunDuration">Stun duration (sec).</param>
        /// <returns></returns>
        IEnumerator Stun(float stunDuration)
        {
            _enemy.IsStun = true;
            yield return new WaitForSeconds(stunDuration);
            _enemy.IsStun = false;
        }
    }
}
