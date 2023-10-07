using System.Collections;
using EnemyBehavior;
using UnityEngine;

namespace Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Vector2 enemySpawnRadius = new Vector2(10f, 10f);
        [SerializeField] private float enemySpawnTime;
        [SerializeField] private int maxEnemy = 4;
        [SerializeField] private int currentEnemyCount;
        [SerializeField] private bool nightMode;
        [SerializeField] private LayerMask playerMask;
        bool isReduceRate = false;
        public GameObject enemyPrefab;
    
        void Start()
        {
            currentEnemyCount = 0; // เริ่มต้นจำนวนศัตรูในปัจจุบันที่ 0
            StartCoroutine(SpawnEnemy());
        }
    
        void Update()
        {
            bool isNight = TimeSystem.instance.GetTimeState() == TimeState.Night;
        
            if (!isNight)
            {
                isReduceRate = false;
            }

            switch (TimeSystem.instance.day)
            {
                case 0:
                    enemySpawnTime = 7;
                    break;
                case 1:
                    enemySpawnTime = 5.5f;
                    break;
                case 2:
                    enemySpawnTime = 4.5f;
                    break;
                case 3:
                    enemySpawnTime = 3.5f;
                    break;
            }
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(enemySpawnRadius.x * 2, enemySpawnRadius.y * 2, 0f));
        }

        private IEnumerator SpawnEnemy()
        {
            yield return new WaitForSeconds(3);

            while (true)
            {
                bool isNight = TimeSystem.instance.GetTimeState() == TimeState.Night;
                if (nightMode)
                {
                    if (!isNight)
                    {
                        yield return null;
                        continue;
                    }

                    // ตรวจสอบว่ายังไม่เกิน MaxEnemy ก่อนที่จะสร้างศัตรูใหม่
                    if (currentEnemyCount < maxEnemy)
                    {
                        // คำนวณตำแหน่งสุ่มภายใน enemySpawnRadius
                        Vector2 spawnPosition = new Vector2(
                            UnityEngine.Random.Range(-enemySpawnRadius.x, enemySpawnRadius.x),
                            UnityEngine.Random.Range(-enemySpawnRadius.y, enemySpawnRadius.y)
                        );
            
                        // สร้างศัตรูที่ตำแหน่งที่คำนวณได้
                        Enemy newEnemy = Instantiate(enemyPrefab, transform.position + (Vector3)spawnPosition, Quaternion.identity,transform).GetComponent<Enemy>();
                        newEnemy.NightMode = true;
                    }
        
                    // update จำนวนศัตรู
                    currentEnemyCount = transform.childCount;
                }
                else
                {
                    Collider2D[] col = Physics2D.OverlapBoxAll(transform.position, new Vector2(enemySpawnRadius.x * 2, enemySpawnRadius.y * 2),
                        0, playerMask);

                    if (col.Length > 0)
                    {
                        yield return null;
                        continue;
                    }
                
                    // ตรวจสอบว่ายังไม่เกิน MaxEnemy ก่อนที่จะสร้างศัตรูใหม่
                    if (currentEnemyCount < maxEnemy)
                    {
                        // คำนวณตำแหน่งสุ่มภายใน enemySpawnRadius
                        Vector2 spawnPosition = new Vector2(
                            UnityEngine.Random.Range(-enemySpawnRadius.x, enemySpawnRadius.x),
                            UnityEngine.Random.Range(-enemySpawnRadius.y, enemySpawnRadius.y)
                        );
            
                        // สร้างศัตรูที่ตำแหน่งที่คำนวณได้
                        GameObject newEnemy = Instantiate(enemyPrefab, transform.position + (Vector3)spawnPosition, Quaternion.identity,transform);
                    }
        
                    // update จำนวนศัตรู
                    currentEnemyCount = transform.childCount;
                }

                yield return new WaitForSeconds(enemySpawnTime);
            }
        }
    }
}
    

