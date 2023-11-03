using System.Collections.Generic;
using System.Linq;
using Spawner;
using UnityEngine;

public static class ProjectExtensions
{
    public static void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static PriorityObject<T> RandomPickOne<T>(List<PriorityObject<T>> list)
    {
        float totalSpawnRate = list.Sum(spawnObject => spawnObject.spawnRate);
        float randomSpawnRate = Random.Range(0, totalSpawnRate);
        
        foreach (var obj in list)
        {
            if (randomSpawnRate <= obj.spawnRate) return obj;
            randomSpawnRate -= obj.spawnRate;
        }

        return default;
    }
}
