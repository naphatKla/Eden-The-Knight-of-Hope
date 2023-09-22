using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Vector2 enemySpawnRadius = new Vector2(10f, 10f);
    [SerializeField] private float enemySpawnTime;
    
    public GameObject enemyPrefab;
    
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, enemySpawnTime);
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
        // คำนวณตำแหน่งสุ่มภายใน enemySpawnRadius
        Vector2 spawnPosition = new Vector2(
            UnityEngine.Random.Range(-enemySpawnRadius.x, enemySpawnRadius.x),
            UnityEngine.Random.Range(-enemySpawnRadius.y, enemySpawnRadius.y)
        );

        // สร้างศัตรูที่ตำแหน่งที่คำนวณได้
        Instantiate(enemyPrefab, transform.position + (Vector3)spawnPosition, Quaternion.identity);
    }
    
}
    

