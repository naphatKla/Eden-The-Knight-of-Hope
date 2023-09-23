using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Vector2 enemySpawnRadius = new Vector2(10f, 10f);
    [SerializeField] private float enemySpawnTime;
    [SerializeField] private int maxEnemy = 4;
    [SerializeField] private int currentEnemyCount;
    public GameObject enemyPrefab;
    
    void Start()
    {
        currentEnemyCount = 0; // เริ่มต้นจำนวนศัตรูในปัจจุบันที่ 0
        InvokeRepeating("SpawnEnemy", 0.5f, enemySpawnTime);
    }
    
    void Update()
    {
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(enemySpawnRadius.x * 2, enemySpawnRadius.y * 2, 0f));
    }

    private void SpawnEnemy()
    {
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
}
    

