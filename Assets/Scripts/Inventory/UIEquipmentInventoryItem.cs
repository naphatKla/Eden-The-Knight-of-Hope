using Inventory;
using UnityEngine.EventSystems;

public enum ItemSlotType
{
    Weapon,
    Armor,
    QuickSlot,
    Other
}

public class UIEquipmentInventoryItem : UIInventoryItem
{
    public override void OnDrop(PointerEventData eventData)
    {
        UIInventoryItem droppedItem = eventData.pointerDrag.GetComponent<UIInventoryItem>();
        if(droppedItem.itemSlotType != itemSlotType) return;
        base.OnDrop(eventData);
    }
}
