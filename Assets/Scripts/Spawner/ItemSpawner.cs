using UnityEngine;

namespace Spawner
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private Vector2 itemSpawnArea = new Vector2(10f, 10f);
        [SerializeField] private int maxItem = 4;
        [SerializeField] private int currentItemCount;
        private bool _isSpawn;
        public GameObject itemPrefab;
    
        void Start()
        {
            currentItemCount = 0;
        }
    
        void Update()
        {
            currentItemCount = transform.childCount;
        
            if(currentItemCount >= maxItem) return;
            if (TimeSystem.instance.GetTimeState() == TimeState.Night)
            {
                _isSpawn = false;
                return;
            }
            if(_isSpawn) return;

            for (int i = 0; i < maxItem - currentItemCount; i++)
            {
                SpawnItem();
            }

            _isSpawn = true;

        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(itemSpawnArea.x * 2, itemSpawnArea.y * 2, 0f));
        }

        private void SpawnItem()
        {
            Vector2 spawnPosition = new Vector2(
                UnityEngine.Random.Range(-itemSpawnArea.x, itemSpawnArea.x),
                UnityEngine.Random.Range(-itemSpawnArea.y, itemSpawnArea.y)
            );

            GameObject newEnemy = Instantiate(itemPrefab, transform.position + (Vector3)spawnPosition,
                Quaternion.identity, transform);
        }
    }
}
