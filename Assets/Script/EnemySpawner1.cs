using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner2 : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnPoint;
    //[SerializeField] private float enemySpawnRadius = 0.5f;
    [SerializeField] private Vector2  enemySpawnRadius = new Vector2(0.5f, 0.5f);
    [SerializeField] private float enemySpawnTime;
    
    public GameObject enemyPrefab;
    
    void Start()
    {
        Invoke("SpawnEnemy",enemySpawnTime);
    }
    
    void Update()
    {
        
    }
    
    private void OnDrawGizmos()
    {
        Vector2 scale = new Vector2(enemySpawnRadius.x, enemySpawnRadius.y);
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(enemySpawnPoint.position, Quaternion.identity, scale);
        Gizmos.matrix = oldMatrix;
        
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(Vector2.zero, enemySpawnRadius.x,enemySpawnRadius.y);
    }
    
    private void SpawnEnemy()
    {
        // Random position x, y in red area
        Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * enemySpawnRadius;
        Vector2 spawnPosition = new Vector2(randomPosition.x, randomPosition.y);
        // Spawn enemy
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        
        Invoke("SpawnEnemy",enemySpawnTime);
    }
    
    
}
