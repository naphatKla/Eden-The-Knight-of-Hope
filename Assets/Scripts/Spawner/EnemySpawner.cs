using System.Linq;
using EnemyBehavior;
using UnityEngine;


namespace Spawner
{
    [System.Serializable]
    public struct NightModeSpawn
    {
        public int day;
        public float spawnCoolDown;
    }
    public class EnemySpawner : Spawner
    {
        #region Declare Variables
        [Space] [SerializeField] private bool nightMode;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private NightModeSpawn[] nightModeSpawnRates;
        private bool _isReduceRate;
        #endregion
        
        protected override void Start()
        {
            if (nightMode)
            {
                TimeSystem.Instance.OnNewDay += ReduceSpawnRateOnTheNewDay;
                spawnCooldown = nightModeSpawnRates.FirstOrDefault(rate => rate.day == 0).spawnCoolDown;
            }
        }

        #region Methods
        /// <summary>
        /// Normal mode : Spawn enemy if player is not in the area.
        /// Night mode : spawn enemy only at night
        /// </summary>
        protected override void SpawnObjectHandler()
        {
            bool isPlayerInArea = Physics2D.OverlapBoxNonAlloc(transform.position, spawnArea * 2, 0, new Collider2D[1], playerMask) > 0;
            
            if (nightMode)
            {
                if (TimeSystem.Instance.timeState != TimeState.Night) return;
                base.SpawnObjectHandler();
                return;
            }

            if (isPlayerInArea)
            {
                NextSpawnTime = Time.time + spawnCooldown;
                return;
            }
            base.SpawnObjectHandler();
        }

        
        /// <summary>
        /// If night mode is on, spawn enemy with night mode.
        /// </summary>
        protected override void SpawnObject()
        {
            base.SpawnObject();
            
            if (!nightMode) return;
            LastSpawnedObject.GetComponent<Enemy>().NightMode = true;
        }

        
        /// <summary>
        /// Reduce spawn rate on the new day.
        /// </summary>
        private void ReduceSpawnRateOnTheNewDay()
        {
            foreach (var rate in nightModeSpawnRates)
            {
                if (rate.day != TimeSystem.Instance.day) continue;
                spawnCooldown = rate.spawnCoolDown;
                break;
            }
        }
        #endregion
    }
}
    

