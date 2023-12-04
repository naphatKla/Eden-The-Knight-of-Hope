using System.Collections.Generic;
using Interaction;
using UnityEngine;

namespace Spawner
{
    public class ItemSpawner : Spawner
    {
        [SerializeField] private List<PriorityObject<GatheringResourceSO>> itemsDataToSpawn;
        /// <summary>
        /// Spawn item one time per day.
        /// </summary>
        ///
        protected override void SpawnObjectHandler()
        {
            if (TimeSystem.Instance.timeState == TimeState.Night)
            {
                // reset spawning operation.
                IsSpawningComplete = false; 
                return;
            }
            base.SpawnObjectHandler();
        }
        
        protected override void SpawnObject(GameObject objToSpawn = null)
        {
            base.SpawnObject();
            GatheringResourceSO itemToSpawn = ProjectExtensions.RandomPickOne(itemsDataToSpawn).obj;
            LastSpawnedObject.GetComponent<GatheringResource>().SetData(itemToSpawn);
        }
    }
}
