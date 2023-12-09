using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct PriorityObject<T>
{
    public T obj;
    public float spawnRate;
}

namespace Spawner
{
    public enum SpawnMode
    {
        Loop,
        Trigger
    }
    public class Spawner : MonoBehaviour
    {
        #region Declare Variables
        [SerializeField] protected Vector2 spawnArea;
        [SerializeField] protected float spawnCooldown;
        [SerializeField] private float spawnOffset;
        [SerializeField] private int maxObject;
        [SerializeField] protected SpawnMode spawnMode;
        [SerializeField] private List<PriorityObject<GameObject>> objectsToSpawn;
        [SerializeField] private Color gizmosColor;
        protected bool IsSpawningComplete;
        protected GameObject LastSpawnedObject;
        protected float NextSpawnTime;
        #endregion
        
        protected virtual void Start() {}
        protected virtual void Update()
        {
            SpawnObjectHandler();
        }

        #region Methods
        // ReSharper disable Unity.PerformanceAnalysis
        protected virtual void SpawnObjectHandler()
        {
            // Override this method with some logic for spawn object.
            if (spawnMode == SpawnMode.Trigger && IsSpawningComplete) return;
            if (transform.childCount >= maxObject)
            {
                IsSpawningComplete = true;
                return;
            }
            if (Time.time < NextSpawnTime) return;
            SpawnObject();
        }

        protected virtual void SpawnObject(GameObject objToSpawn = null)
        {
            
            Vector2 randomPosition = new Vector2(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(-spawnArea.y, spawnArea.y));
            Vector2 spawnPosition = randomPosition + (Vector2)transform.position;
            GameObject pickObject =  objToSpawn? objToSpawn : ProjectExtensions.RandomPickOne(objectsToSpawn).obj;
            List<GameObject> spawnedObjects = new List<GameObject>();
            
            for (int i = 0; i < transform.childCount; i++)
                spawnedObjects.Add(transform.GetChild(i).gameObject);
            
            bool isOverlap = spawnedObjects.Any(obj => Vector2.Distance(obj.transform.position, spawnPosition) < spawnOffset);
            for (int i = 0; isOverlap; i++)
            {
                if (spawnedObjects.Count <= 0) break;
                randomPosition = new Vector2(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(-spawnArea.y, spawnArea.y));
                spawnPosition = randomPosition + (Vector2)transform.position;
                isOverlap = spawnedObjects.Any(obj => Vector2.Distance(obj.transform.position, spawnPosition) < spawnOffset);
                if (i > 25) break;
            }
            
            LastSpawnedObject = Instantiate(pickObject, spawnPosition, Quaternion.identity, transform);
            NextSpawnTime = Time.time + spawnCooldown;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(gizmosColor.r, gizmosColor.g, gizmosColor.b, gizmosColor.a);
            Gizmos.DrawWireCube(transform.position, spawnArea * 2);
        }
        #endregion
    }
}

