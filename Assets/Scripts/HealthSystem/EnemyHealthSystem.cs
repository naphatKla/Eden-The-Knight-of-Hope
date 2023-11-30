using System;
using System.Collections;
using EnemyBehavior;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HealthSystem
{
    public class EnemyHealthSystem : HealthSystem
    {
        public GameObject DamageIndicator;
        private Enemy _enemy;
        protected override void Start()
        {
            _enemy = GetComponent<Enemy>();
            base.Start();
        }

        private void Update()
        {
            sliderHpPlayer.gameObject.SetActive(CurrentHp < maxHp);
        }

        /// <summary>
        /// Take damage and stun the enemy if the attacker is the player.
        /// And then set enemy target to the attacker.
        /// </summary>
        /// <param name="damage">Damage taken.</param>
        /// <param name="attacker">The Attacker.</param>
        public override void TakeDamage(float damage, GameObject attacker = null)
        {
            _enemy.Target = attacker;
        
            if (attacker && attacker.CompareTag("Player"))
                StartCoroutine(Stun(0.5f));
            ShowDamageIndicator(damage);
            base.TakeDamage(damage, attacker);
        }
    
        
        /// <summary>
        /// Dead and add point to the player score.
        /// </summary>
        protected override void Dead()
        {
            if(Random.Range(0,101) >= 90)
                GameManager.Instance.AddPoint(10);
        
            base.Dead();
        }

        public void ShowDamageIndicator(float damage)
        {
            var go = Instantiate(DamageIndicator, transform.position, quaternion.identity);
            go.GetComponent<TextMeshPro>().text = damage.ToString("0");
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
