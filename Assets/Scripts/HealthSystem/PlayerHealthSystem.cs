using PlayerBehavior;
using UnityEngine;

namespace HealthSystem
{
    public class PlayerHealthSystem : HealthSystem
    {
        [SerializeField] private float respawnTime;
        public static PlayerHealthSystem Instance;

        protected override void Start()
        {
            base.Start();
            Instance = this;
        }

        /// <summary>
        /// Player Dead and Respawn after a few seconds.
        /// </summary>
        protected override void Dead()
        {
            Invoke(nameof(Respawn),respawnTime);
            base.Dead();
        }

        /// <summary>
        /// Respawn the player.
        /// </summary>
        private void Respawn()
        {
            gameObject.SetActive(true);
            spriteRenderer.color = Color.white;
            ResetHealth();
            Player.Instance.ResetState();
            transform.position = GameManager.Instance.spawnPoint;
        }

        public override void TakeDamage(float damage, GameObject attacker = null)
        {
            if (PlayerBehavior.Player.Instance.IsDash) return; 
            base.TakeDamage(damage, attacker);
        }
    }
}
