using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerInventoryItem : BaseUIInventoryItem
{
    public override void OnDrop(PointerEventData eventData)
    {
        BaseUIInventoryItem droppedItem = eventData.pointerDrag.GetComponent<BaseUIInventoryItem>();
        if (droppedItem.ParentInventoryData.name == "PlayerEquipment" && !isEmpty)
            if (droppedItem.ItemData.ItemSlotType != ItemData.ItemSlotType) return;
        
        base.OnDrop(eventData);
    }
}
