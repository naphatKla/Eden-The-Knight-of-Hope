using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu]
    public class ItemSo : ScriptableObject
    {
        [field: SerializeField] public bool IsStackable { get; set; }
        [field: SerializeField] public int MaxStackSize { get; set; } = 1;
        [field: SerializeField] public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField] public Sprite ItemImage { get; set; }
        [field: SerializeField] public ItemSlotType ItemSlotType { get; set; }
        [field: SerializeField] public int Cost { get; set; }
        public int ID => GetInstanceID();
    }
}