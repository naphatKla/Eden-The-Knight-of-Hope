using UnityEngine;

namespace HealthSystem
{
    public class PlayerHealthSystem : HealthSystem
    {
        [SerializeField] private float respawnTime;
    
        /// <summary>
        /// Player Dead and Respawn after a few seconds.
        /// </summary>
        protected override void Dead()
        {
            Invoke(nameof(Respawn),respawnTime);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Respawn the player.
        /// </summary>
        private void Respawn()
        {
            gameObject.SetActive(true);
            ResetHealth();
            transform.position = GameManager.Instance.spawnPoint;
        }
    }
}
