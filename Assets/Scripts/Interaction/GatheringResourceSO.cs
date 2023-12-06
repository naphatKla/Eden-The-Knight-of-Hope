using System;
using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct GatheringItemDrop
{
    public ItemSo item;
    public Vector2 quantityDrop; // random x - y
}

[CreateAssetMenu]
public class GatheringResourceSO : ScriptableObject
{
    public Sprite[] sprite;
    public Vector2 point;
    public float gatheringTime;
    public List<PriorityObject<GatheringItemDrop>> itemDrop1;
    public List<PriorityObject<GatheringItemDrop>> itemDrop2;
    public List<PriorityObject<GatheringItemDrop>> itemDrop3;
    public List<PriorityObject<GatheringItemDrop>> itemDrop4;
    public List<PriorityObject<GatheringItemDrop>> itemDrop5;
    
    [Header("Sound")]
    public AudioClip[] gatheringSounds;
    public AudioClip[] destroySounds;
}
