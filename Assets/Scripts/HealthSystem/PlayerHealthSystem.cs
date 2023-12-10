using PlayerBehavior;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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
            Invoke(nameof(DeadPunishment), respawnTime+0.5f);
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

        public void DeadPunishment()
        {
            GameManager.Instance.AddPoint(-200);
        }

        public override void TakeDamage(float damage, GameObject attacker = null)
        {
            if (PlayerBehavior.Player.Instance.IsDash) return;
            PlaySound(takeDamageSounds);
            base.TakeDamage(damage, attacker);
        }

        protected override void ShowDamageIndicator(float damage)
        {
            if (!damageIndicator) return;
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f),0);
            var go = Instantiate(damageIndicator, transform.position + offset, quaternion.identity);
            go.GetComponent<TextMeshPro>().text = $"<color=red>{damage}";
        }
    }
}
