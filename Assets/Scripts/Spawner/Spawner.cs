using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spawner
{
    [System.Serializable]
    public struct PriorityObject<T>
    {
        public T obj;
        public float spawnRate;
    }
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Vector2 spawnArea;
        [SerializeField] private float spawnCooldown;
        [SerializeField] private float spawnOffset;
        [SerializeField] private int maxObject;
        [SerializeField] private List<PriorityObject<GameObject>> objectsToSpawn;
        [SerializeField] private List<GameObject> spawnedObjects = new List<GameObject>();
        private bool _isSpawn;
        private Transform _thisTransform;

        protected virtual void Start()
        {
            _thisTransform = transform;
        }
    
        protected virtual void Update()
        {
            SpawnObjectHandler();
        }
        
        protected virtual void SpawnObjectHandler()
        {
            // Override this method with some logic for spawn object.
            if (transform.childCount >= maxObject) return;
            SpawnItem();
        }

        protected virtual void SpawnItem()
        {
            Vector2 randomPosition = new Vector2(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(-spawnArea.y, spawnArea.y));
            Vector2 spawnPosition = randomPosition + (Vector2)transform.position;
            GameObject pickObject = ProjectExtensions.PickOneFromList(objectsToSpawn).obj;
            
            for (int i = 0; i < transform.childCount; i++)
                spawnedObjects.Add(transform.GetChild(i).gameObject);
            
            bool isOverlap = spawnedObjects.All(obj => Vector2.Distance(obj.transform.position, spawnPosition) < spawnOffset);
            
            for (int i = 0; isOverlap && spawnedObjects.Count > 0; i++)
            {
                randomPosition = new Vector2(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(-spawnArea.y, spawnArea.y));
                spawnPosition = randomPosition + (Vector2)transform.position;
                isOverlap = spawnedObjects.All(obj => Vector2.Distance(obj.transform.position, spawnPosition) < spawnOffset);
                if (i > 10) break;
            }
            
            Instantiate(pickObject, spawnPosition, Quaternion.identity, transform);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x * 2, spawnArea.y * 2, 0f));
        }
    }
}

