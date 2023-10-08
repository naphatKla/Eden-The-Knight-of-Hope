
namespace Spawner
{
    public class ItemSpawner : Spawner
    {
        /// <summary>
        /// Spawn item one time per day.
        /// </summary>
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
    }
}
